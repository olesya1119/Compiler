using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Analysis
{
    /// <summary>
    /// Является главным парсером. По сути просто вызывает сканер, удаляет ERROR и вызывает другие парсеры.
    /// </summary>
    public class Parser
    {
        private List<Token> _tokens;
        private List<ErrorEntry> _errors;
        protected int _index;
        protected string _text;

        protected int Index { get { return _index; } set { _index = value; } }
        protected Token Token { get { return _tokens[Index]; } }

        public Parser(string text)
        {
            _text = text;
            _errors = new List<ErrorEntry>() { };
            _index = 0;
        }


        public List<ErrorEntry> Parse()
        {
            var scaner = new Scaner();
            _tokens = scaner.Scan(_text);
            RemoveUnknownSymbols();
            var funcHeadParser = new FuncHeadParser(_tokens, 0, _text);
            try
            {
                funcHeadParser.Parse();
            }
            //catch { }
            finally
            {
                AddErrorsList(funcHeadParser.Errors);
            }


            var argumentsParser = new ArgumentsParser(funcHeadParser.Tokens, funcHeadParser.NextPosition, _text);
            try
            {
                argumentsParser.Parse();
            }
            //catch { }
            finally
            {
                AddErrorsList(argumentsParser.Errors);
            }

            var expressionParser = new ExpressionParser(argumentsParser.Tokens, argumentsParser.NextPosition, _text);
            try
            {
                expressionParser.Parse();
            }
            //catch { }
            finally
            {
                AddErrorsList(expressionParser.Errors);
            }

            SortErrors();
            foreach (var er in _errors)
            {
                Console.WriteLine(er);
            }

            Console.WriteLine("\n\nТОКЕНЫ:");
            foreach (var token in _tokens)
            {
                Console.WriteLine(token);
            }


            return _errors;
        }


        private void SortErrors()
        {
            _errors = _errors.OrderBy(e => e.Line).ThenBy(e => e.Column).ToList();
        }


        // Обработаем все невалидные токены и удалим их
        private void RemoveUnknownSymbols()
        {
            int i = 0;
            while (i < _tokens.Count)
            {
                if (_tokens[i].Code == CODE.ERROR)
                {
                    int startIndex = i;
                    string unknownSequence = _tokens[i].TokenValue;
                    i++;

                    while (i < _tokens.Count && _tokens[i].Code == CODE.ERROR)
                    {
                        unknownSequence += _tokens[i].TokenValue;
                        i++;
                    }

                    AddError($"Неожиданная последовательность символов: '{unknownSequence}'", _tokens[startIndex]);
                    _tokens.RemoveRange(startIndex, i - startIndex);
                    i = startIndex;
                }
                else
                {
                    i++;
                }
            }

            i = 0;
            // Следующий этап - склеиваем ошибочные токены и числа. Возможно мы найдем индификатор?
            while (i < _tokens.Count - 1)
            {
                if (_tokens[i].Code == CODE.IDENTIFIER && _tokens[i + 1].Code == CODE.IDENTIFIER ||
                    _tokens[i].Code == CODE.IDENTIFIER && _tokens[i + 1].Code == CODE.UNSIGNED_INT)
                {
                    var token = new Token(CODE.IDENTIFIER, _tokens[i].TokenValue + _tokens[i + 1].TokenValue,
                        _tokens[i].Line, _tokens[i].StartColumn, _tokens[i + 1].EndColumn);

                    _tokens.RemoveAt(i);
                    _tokens.RemoveAt(i);
                    _tokens.Insert(i, token);
                    continue;

                }

                if (_tokens[i].Code == CODE.UNSIGNED_INT && _tokens[i + 1].Code == CODE.UNSIGNED_INT)
                {
                    var token = new Token(CODE.UNSIGNED_INT, _tokens[i].TokenValue + _tokens[i + 1].TokenValue,
                        _tokens[i].Line, _tokens[i].StartColumn, _tokens[i + 1].EndColumn);

                    _tokens.RemoveAt(i);
                    _tokens.RemoveAt(i);
                    _tokens.Insert(i, token);
                    continue;

                }

                if (isWord(_tokens[i].Code))
                {
                    switch (_tokens[i].TokenValue)
                    {
                        case "func":
                            _tokens[i].Code = CODE.FUNC; break;
                        case "int":
                            _tokens[i].Code = CODE.INT; break;
                        case "uint":
                            _tokens[i].Code = CODE.UINT; break;
                        case "float32":
                            _tokens[i].Code = CODE.FLOAT32; break;
                        case "float64":
                            _tokens[i].Code = CODE.FLOAT64; break;
                        case "return":
                            _tokens[i].Code = CODE.RETURN; break;
                    }
                }
                i++;
            }
        }


        ///<summary> Добавление ошибок в список ошибок </summary>
        protected void AddError(string message, Token token = null)
        {
            _errors.Add(new ErrorEntry(message, token ?? Token));
        }

        private void AddErrorsList(List<ErrorEntry> ers)
        {
            foreach (var er in ers)
            {
                _errors.Add(er);
            }
        }

        private bool isWord(CODE code)
        {
            if (code == CODE.FUNC || code == CODE.INT || code == CODE.IDENTIFIER || code == CODE.UINT ||
                code == CODE.FLOAT32 || code == CODE.FLOAT64 || code == CODE.RETURN) return true;
            return false;
        }
    }
}
