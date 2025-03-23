using Compiler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Analysis
{
    public class ErrorEntry
    {
        public string Message { get; set; }
        public Token Token { get; set; }

        public ErrorEntry(string message, Token token)
        {
            Message = message;
            this.Token = token;
        }
    }
    public class Parser
    {
        private List<Token> _tokens;
        private int _currentTokenIndex;
        private List<ErrorEntry> _errors;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _currentTokenIndex = 0;
            _errors = new List<ErrorEntry>();
        }

        public void Parse()
        {
            RemoveUnknownSymbols();
            FUNC();
        }

        private void RemoveUnknownSymbols()
        {
            int i = 0;
            while (i < _tokens.Count)
            {
                if (_tokens[i].Code == CODE.ERROR)
                {
                    // Начало последовательности неизвестных символов
                    int startIndex = i;
                    string unknownSequence = _tokens[i].TokenValue;
                    i++;

                    // Собираем все подряд идущие неизвестные символы
                    while (i < _tokens.Count && _tokens[i].Code == CODE.ERROR)
                    {
                        unknownSequence += _tokens[i].TokenValue;
                        i++;
                    }

                    // Добавляем одну ошибку для всей последовательности
                    AddError($"Неизвестная последовательность символов: '{unknownSequence}' (Line: {_tokens[startIndex].Line}, Column: {_tokens[startIndex].StartColumn})");


                    // Удаляем все токены из последовательности
                    _tokens.RemoveRange(startIndex, i - startIndex);
                    i = startIndex; // Возвращаемся к текущей позиции, так как список изменился
                }
                else
                {
                    i++;
                }
            }
        }

        public List<ErrorEntry> Errors => _errors;

        private Token CurrentToken => _currentTokenIndex < _tokens.Count ? _tokens[_currentTokenIndex] : null;

        private void MoveNext()
        {
            _currentTokenIndex++;
        }

        private void AddError(string message)
        {
            _errors.Add(new ErrorEntry($"Ошибка: {message}", CurrentToken));
        }

        private void Match(CODE expectedCode)
        {
            if (CurrentToken != null && CurrentToken.Code == expectedCode)
            {
                MoveNext();
            }
            else
            {
                AddError($"Ожидалось {expectedCode}, но найдено {CurrentToken?.Code}");
                MoveNext(); // Пропускаем текущий токен и продолжаем
            }
        }

        private void FUNC()
        {
            if (CurrentToken != null && CurrentToken.Code == CODE.FUNC)
            {
                Match(CODE.FUNC);
                SPACE_AFTER_FUNC();
            }
            else
            {
                AddError("Ожидалось ключевое слово 'func'");
                SkipUntil(new[] { CODE.LBRACE, CODE.RBRACE });
            }
        }

        private void SPACE_AFTER_FUNC()
        {
            if (CurrentToken != null && CurrentToken.Code == CODE.DELIMITER)
            {
                Match(CODE.DELIMITER);
                FUNC_NAME();
            }
            else
            {
                AddError("Ожидался пробел после 'func'");
                SkipUntil(new[] { CODE.LPAREN, CODE.LBRACE });
            }
        }

        private void FUNC_NAME()
        {
            if (CurrentToken != null && CurrentToken.Code == CODE.IDENTIFIER)
            {
                Match(CODE.IDENTIFIER);
                AFTER_FUNC_NAME();
            }
            else
            {
                AddError("Ожидался идентификатор имени функции");
                SkipUntil(new[] { CODE.LPAREN, CODE.LBRACE });
            }
        }

        private void AFTER_FUNC_NAME()
        {
            if (CurrentToken != null && CurrentToken.Code == CODE.LPAREN)
            {
                Match(CODE.LPAREN);
                ARGUMENTS();
            }
            else
            {
                AddError("Ожидалось '(' после имени функции");
                SkipUntil(new[] { CODE.LBRACE });
            }
        }

        private void ARGUMENTS()
        {
            if (CurrentToken != null && CurrentToken.Code != CODE.RPAREN)
            {
                PARAMS_LIST();
            }
            ARGUMENTS_END();
        }

        private void PARAMS_LIST()
        {
            PARAMS_WITH_TYPE();
            PARAMS_LIST_TAIL();
        }

        private void PARAMS_LIST_TAIL()
        {
            if (CurrentToken != null && CurrentToken.Code == CODE.COMMA)
            {
                Match(CODE.COMMA);
                PARAMS_LIST();
            }
        }

        private void PARAMS_WITH_TYPE()
        {
            PARAMS();
            if (CurrentToken != null && CurrentToken.Code == CODE.DELIMITER)
            {
                Match(CODE.DELIMITER);
                TYPE();
            }
            else
            {
                AddError("Ожидался пробел после параметра");
                SkipUntil(new[] { CODE.COMMA, CODE.RPAREN });
            }
        }

        private void PARAMS()
        {
            if (CurrentToken != null && CurrentToken.Code == CODE.IDENTIFIER)
            {
                Match(CODE.IDENTIFIER);
                PARAMS_TAIL();
            }
            else
            {
                AddError("Ожидался идентификатор параметра");
                SkipUntil(new[] { CODE.COMMA, CODE.RPAREN });
            }
        }

        private void PARAMS_TAIL()
        {
            if (CurrentToken != null && CurrentToken.Code == CODE.COMMA)
            {
                Match(CODE.COMMA);
                PARAMS();
            }
        }

        private void ARGUMENTS_END()
        {
            if (CurrentToken != null && CurrentToken.Code == CODE.RPAREN)
            {
                Match(CODE.RPAREN);
                RETURN_TYPE();
            }
            else
            {
                AddError("Ожидалось ')' после аргументов");
                SkipUntil(new[] { CODE.LBRACE });
            }
        }

        private void RETURN_TYPE()
        {
            TYPE();
            AFTER_RETURN_TYPE();
        }

        private void AFTER_RETURN_TYPE()
        {
            if (CurrentToken != null && CurrentToken.Code == CODE.LBRACE)
            {
                Match(CODE.LBRACE);
                FUNC_BODY();
            }
            else
            {
                AddError("Ожидалось '{' после типа возврата");
                SkipUntil(new[] { CODE.RBRACE });
            }
        }

        private void FUNC_BODY()
        {
            if (CurrentToken != null && CurrentToken.Code == CODE.RETURN)
            {
                Match(CODE.RETURN);
                RETURN_VALUE();
            }
            else
            {
                AddError("Ожидалось ключевое слово 'return'");
                SkipUntil(new[] { CODE.RBRACE });
            }
        }

        private void RETURN_VALUE()
        {
            if (CurrentToken != null && CurrentToken.Code == CODE.DELIMITER)
            {
                Match(CODE.DELIMITER);
                EXPRESSION_AFTER_RETURN();
            }
            else
            {
                AddError("Ожидался пробел после 'return'");
                SkipUntil(new[] { CODE.RBRACE });
            }
        }

        private void EXPRESSION_AFTER_RETURN()
        {
            EXPRESSION();
            END_FUNC_BODY();
        }

        private void END_FUNC_BODY()
        {
            if (CurrentToken != null && CurrentToken.Code == CODE.RBRACE)
            {
                Match(CODE.RBRACE);
                END();
            }
            else
            {
                AddError("Ожидалось '}' в конце тела функции");
                SkipUntil(new[] { CODE.END });
            }
        }

        private void END()
        {
            if (CurrentToken != null && CurrentToken.Code == CODE.END)
            {
                Match(CODE.END);
            }
            else
            {
                AddError("Ожидалось ';' в конце функции");
            }
        }

        private void TYPE()
        {
            if (CurrentToken != null && (CurrentToken.Code == CODE.INT || CurrentToken.Code == CODE.UINT || CurrentToken.Code == CODE.FLOAT32 || CurrentToken.Code == CODE.FLOAT64))
            {
                Match(CurrentToken.Code);
            }
            else
            {
                AddError("Ожидался тип (int, uint, float32, float64)");
                SkipUntil(new[] { CODE.LBRACE, CODE.RPAREN });
            }
        }

        private void EXPRESSION()
        {
            TERM();
            A();
        }

        private void A()
        {
            if (CurrentToken != null && (CurrentToken.Code == CODE.PLUS || CurrentToken.Code == CODE.MINUS))
            {
                Match(CurrentToken.Code);
                TERM();
                A();
            }
        }

        private void TERM()
        {
            OPERAND();
            B();
        }

        private void B()
        {
            if (CurrentToken != null && (CurrentToken.Code == CODE.MULTIPLY || CurrentToken.Code == CODE.DIVIDE))
            {
                Match(CurrentToken.Code);
                OPERAND();
                B();
            }
        }

        private void OPERAND()
        {
            if (CurrentToken != null && CurrentToken.Code == CODE.IDENTIFIER)
            {
                Match(CODE.IDENTIFIER);
            }
            else if (CurrentToken != null && CurrentToken.Code == CODE.UNSIGNED_INT)
            {
                Match(CODE.UNSIGNED_INT);
            }
            else if (CurrentToken != null && CurrentToken.Code == CODE.LPAREN)
            {
                Match(CODE.LPAREN);
                EXPRESSION();
                Match(CODE.RPAREN);
            }
            else
            {
                AddError("Ожидался операнд (идентификатор, число или выражение в скобках)");
                SkipUntil(new[] { CODE.PLUS, CODE.MINUS, CODE.MULTIPLY, CODE.DIVIDE, CODE.RPAREN, CODE.RBRACE });
            }
        }

        private void SkipUntil(CODE[] stopTokens)
        {
            while (CurrentToken != null && !Array.Exists(stopTokens, token => token == CurrentToken.Code))
            {
                MoveNext();
            }
        }

    }
}