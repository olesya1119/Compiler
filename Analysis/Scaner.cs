using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Analysis
{
    public enum CODE
    {
        ERROR, // Ошибка
        INT, // int
        UINT, // uint
        FLOAT32, // float32
        FLOAT64, // float64
        FUNC, // func
        RETURN, // return
        IDENTIFIER, // Идентификатор
        DELIMITER, // Разделитель (пробел)
        PLUS, // +
        MINUS, // -
        MULTIPLY, // *
        DIVIDE, // /
        LBRACE, // {
        RBRACE, // }
        LPAREN, // (
        RPAREN, // )
        COMMA, // ,
        UNSIGNED_INT, // Беззнаковое целое число
        END // ; (конец оператора)
    }



    class Scaner
    {
        Dictionary<string, CODE> keyWords = new Dictionary<string, CODE>()
        {
            { "int", CODE.INT },
            { "uint", CODE.UINT },
            { "float32", CODE.FLOAT32 },
            { "float64", CODE.FLOAT64 },
            { "func", CODE.FUNC },
            { "return", CODE.RETURN }
        };

        public List<Token> Parse(string text)
        {
            var tokens = new List<Token>();
            int line = 1, startColumn = 1, endColumn = 1;


            for (int i = 0; i < text.Length; i++)
            {
                var token = "";

                // 1 - letter
                if (isLetter(text[i]))
                {
                    // Читаем все символы
                    token += text[i];
                    i++;
                    startColumn = i;
                    while (isLetter(text[i]) || isDigit(text[i]))
                    {
                        token += text[i];
                        i++;
                    }
                    endColumn = i;

                    // Создаем токен
                    if (keyWords.ContainsKey(token))
                    {
                        tokens.Add(new Token(keyWords[token], token, line, startColumn, endColumn));
                    }
                    else
                    {
                        tokens.Add(new Token(CODE.IDENTIFIER, token, line, startColumn, endColumn));
                    }

                    // Если следующий символ - разделитель, добавляем
                    if (isDelimiter(text[i]))
                    {
                        tokens.Add(new Token(CODE.DELIMITER, " ", line, endColumn + 1, endColumn + 1));
                    }
                    else i--; // Иначе там что-то другое
                    continue;
                }

                // 12 - digit
                if (isDigit(text[i]))
                {
                    // Читаем все символы
                    token += text[i];
                    i++;
                    startColumn = i;
                    while (isDigit(text[i]))
                    {
                        token += text[i];
                        i++;
                    }
                    endColumn = i;

                    // Создаем токен
                    tokens.Add(new Token(CODE.UNSIGNED_INT, token, line, startColumn, endColumn));


                    // Если следующий символ - разделитель, добавляем
                    if (isDelimiter(text[i]))
                    {
                        tokens.Add(new Token(CODE.DELIMITER, " ", line, endColumn + 1, endColumn + 1));
                    }
                    else i--; // Иначе там что-то другое
                    continue;
                }

                // Всё остальное (что-то односимвольное)
                switch (text[i])
                {
                    case '+': tokens.Add(new Token(CODE.PLUS, "+", line, i + 1, i + 1)); break;
                    case '-': tokens.Add(new Token(CODE.MINUS, "-", line, i + 1, i + 1)); break;
                    case '*': tokens.Add(new Token(CODE.MULTIPLY, "*", line, i + 1, i + 1)); break;
                    case '/': tokens.Add(new Token(CODE.DIVIDE, "/", line, i + 1, i + 1)); break;
                    case '{': tokens.Add(new Token(CODE.LBRACE, "{", line, i + 1, i + 1)); break;
                    case '}': tokens.Add(new Token(CODE.RBRACE, "}", line, i + 1, i + 1)); break;
                    case '(': tokens.Add(new Token(CODE.LPAREN, "(", line, i + 1, i + 1)); break;
                    case ')': tokens.Add(new Token(CODE.RPAREN, ")", line, i + 1, i + 1)); break;
                    case ',': tokens.Add(new Token(CODE.COMMA, ",", line, i + 1, i + 1)); break;
                    case ';': tokens.Add(new Token(CODE.END, ";", line, i + 1, i + 1)); break;
                    case '\n': case '\r': line++; break;
                    case ' ': break;
                    default: tokens.Add(new Token(CODE.ERROR, text[i].ToString(), line, i + 1, i + 2)); break;
                }
            }
            RemoveLeadingTrailingSpaces(tokens);

            return tokens;
        }



        private bool isLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c == '_');
        }

        private bool isDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool isDelimiter(char c)
        {
            return c == ' ' || c == '\n' || c == '\t' || c == '\r';
        }

        private void RemoveLeadingTrailingSpaces(List<Token> tokens)
        {
            if (tokens.Count < 2) return;

            HashSet<CODE> notIgnore = new HashSet<CODE>()
            {
                CODE.RETURN,
                CODE.FUNC,
                CODE.INT,
                CODE.UINT,
                CODE.FLOAT32,
                CODE.FLOAT64,
                CODE.FUNC,
                CODE.RETURN,
                CODE.IDENTIFIER,
                CODE.UNSIGNED_INT
            };

            for (int i = 0; i < tokens.Count - 2; i++)
            {
                if (!(notIgnore.Contains(tokens[i].Code) && notIgnore.Contains(tokens[i+2].Code) && tokens[i + 1].Code == CODE.DELIMITER))
                {
                    if (tokens[i + 1].Code == CODE.DELIMITER) tokens.RemoveAt(i + 1);

                }
            }

            if (tokens[tokens.Count - 1].Code == CODE.DELIMITER)
            {
                tokens.RemoveAt(tokens.Count - 1);
            }

            if (tokens[tokens.Count - 2].Code == CODE.DELIMITER)
            {
                tokens.RemoveAt(tokens.Count - 2);
            }
        }
    }
}
