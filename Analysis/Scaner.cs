﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Analysis
{
    public class Scaner
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

        public List<Token> Scan(string text)
        {
            var tokens = new List<Token>();
            int line = 1, currentColumn = 0; // Текущая позиция в строке
            int absoluteIndex = 0; // Абсолютный индекс в исходном тексте

            text = text.Replace("\t", " ");
            text = text.Replace("\r", " ");

            for (int i = 0; i < text.Length; i++, absoluteIndex++)
            {
                var token = "";
                int startColumn = currentColumn + 1; // Начало токена
                int startIndex = absoluteIndex;     // Начальный индекс в исходном тексте

                // Пропускаем пробелы и новые строки
                if (text[i] == ' ')
                {
                    currentColumn++;
                    continue;
                }
                else if (text[i] == '\n' || text[i] == '\r')
                {
                    line++;
                    currentColumn = 0;
                    continue;
                }

                // 1 - letter
                if (isLetter(text[i]))
                {
                    token += text[i];
                    currentColumn++;
                    int j = i + 1;
                    while (j < text.Length && (isLetter(text[j]) || isDigit(text[j])))
                    {
                        token += text[j];
                        j++;
                        currentColumn++;
                        absoluteIndex++;
                    }
                    int endColumn = currentColumn;
                    int endIndex = absoluteIndex;
                    i = j - 1;

                    // Создаём токен
                    if (keyWords.ContainsKey(token))
                    {
                        tokens.Add(new Token(keyWords[token], token, line, startColumn, endColumn, startIndex, endIndex));
                    }
                    else
                    {
                        tokens.Add(new Token(CODE.IDENTIFIER, token, line, startColumn, endColumn, startIndex, endIndex));
                    }

                    // Если следующий символ - разделитель, добавляем
                    if (i + 1 < text.Length && isDelimiter(text[i + 1]))
                    {
                        tokens.Add(new Token(CODE.DELIMITER, " ", line, endColumn + 1, endColumn + 1, endIndex + 1, endIndex + 1));
                    }
                    continue;
                }

                // 2 - digit
                if (isDigit(text[i]))
                {
                    token += text[i];
                    currentColumn++;
                    int j = i + 1;
                    while (j < text.Length && isDigit(text[j]))
                    {
                        token += text[j];
                        j++;
                        currentColumn++;
                        absoluteIndex++;
                    }
                    int endColumn = currentColumn;
                    int endIndex = absoluteIndex;
                    i = j - 1;

                    // Создаём токен
                    tokens.Add(new Token(CODE.UNSIGNED_INT, token, line, startColumn, endColumn, startIndex, endIndex));

                    // Если следующий символ - разделитель, добавляем
                    if (i + 1 < text.Length && isDelimiter(text[i + 1]))
                    {
                        tokens.Add(new Token(CODE.DELIMITER, " ", line, endColumn + 1, endColumn + 1, endIndex + 1, endIndex + 1));
                    }
                    continue;
                }

                // Всё остальное (односимвольные токены)
                currentColumn++;
                int singleCharColumn = currentColumn;
                int endCharIndex = absoluteIndex;
                switch (text[i])
                {
                    case '+': tokens.Add(new Token(CODE.PLUS, "+", line, singleCharColumn, singleCharColumn, startIndex, endCharIndex)); break;
                    case '-': tokens.Add(new Token(CODE.MINUS, "-", line, singleCharColumn, singleCharColumn, startIndex, endCharIndex)); break;
                    case '*': tokens.Add(new Token(CODE.MULTIPLY, "*", line, singleCharColumn, singleCharColumn, startIndex, endCharIndex)); break;
                    case '/': tokens.Add(new Token(CODE.DIVIDE, "/", line, singleCharColumn, singleCharColumn, startIndex, endCharIndex)); break;
                    case '{': tokens.Add(new Token(CODE.LBRACE, "{", line, singleCharColumn, singleCharColumn, startIndex, endCharIndex)); break;
                    case '}': tokens.Add(new Token(CODE.RBRACE, "}", line, singleCharColumn, singleCharColumn, startIndex, endCharIndex)); break;
                    case '(': tokens.Add(new Token(CODE.LPAREN, "(", line, singleCharColumn, singleCharColumn, startIndex, endCharIndex)); break;
                    case ')': tokens.Add(new Token(CODE.RPAREN, ")", line, singleCharColumn, singleCharColumn, startIndex, endCharIndex)); break;
                    case ',': tokens.Add(new Token(CODE.COMMA, ",", line, singleCharColumn, singleCharColumn, startIndex, endCharIndex)); break;
                    case ';': tokens.Add(new Token(CODE.END, ";", line, singleCharColumn, singleCharColumn, startIndex, endCharIndex)); break;
                    default:
                        tokens.Add(new Token(CODE.ERROR, text[i].ToString(), line, singleCharColumn, singleCharColumn, startIndex, endCharIndex));
                        break;
                }
            }

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
    }
}
