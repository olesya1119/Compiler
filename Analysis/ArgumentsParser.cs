using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Analysis
{
    enum ArgumentsStatus
    {
        FIND_RPAREN, // Нашли )
        FIND_LBRACE, // Нашли {
        FIND_RETURN, // Нашли return
        FIND_END,  // Нашли ;
        NOT_FIND // Ничего не нашли
    }


    /// <summary>
    /// Класс для парсинга аргументов.
    /// Помимо ошибок непосредственно в аргументах он выделяет так же и ошибки в оконцовке аргументов. 
    /// </summary>
    class ArgumentsParser : BaseParser
    {
        public ArgumentsParser(List<Token> tokens, int pos, string text) : base(tokens, pos, text) { }

        public ArgumentsStatus Status { get; set; }
        public int EndPos { get; set; }
        public bool FindAgrsType { get; set; } = false;


        public List<ErrorEntry> Parse()
        {
            if (_tokens.Count == 0) return _errors;
            FindArgumentsEnd();
            ParseArguments();
            return _errors;
        }


        /// <summary> Функция, которая ищет конец наших аргументов </summary>
        private void FindArgumentsEnd()
        {
            Status = ArgumentsStatus.NOT_FIND;
            Index++;
            EndPos = Index;

            // Постараемся найти конец наших аргументов. Сначала без ошибок - )
            while (Token.Code != CODE.RPAREN && IsNotEndList && Token.Code != CODE.LBRACE && Token.Code != CODE.RETURN && Token.Code != CODE.END)
            {
                Index++;
            }
            if (Token.Code == CODE.RPAREN)
            {
                EndPos = Index;
                Status = ArgumentsStatus.FIND_RPAREN;
                return;
            }
            Index = 0;

            //Если не вышло ищем {
            while (IsNotEndList && Token.Code != CODE.LBRACE)
            {
                Index++;
            }
            if (Token.Code == CODE.LBRACE)
            {
                EndPos = Index;
                Status = ArgumentsStatus.FIND_LBRACE;
                return;
            }
            Index = 0;



            //Если не вышло ищем return
            while (IsNotEndList && Token.Code != CODE.RETURN)
            {
                Index++;
            }
            if (Token.Code == CODE.RETURN)
            {
                EndPos = Index;
                Status = ArgumentsStatus.FIND_RETURN;
                return;
            }
            Index = 0;

            while (IsNotEndList && Token.Code != CODE.END)
            {
                Index++;
            }
            if (Token.Code == CODE.END)
            {
                EndPos = Index;
                Status = ArgumentsStatus.FIND_END;
                return;
            }
            Index = 0;
            EndPos = _tokens.Count - 1;
        }



        private void ParseArguments()
        {
            CODE lastCode = CODE.COMMA;
            string errorValue;
            Index = 0;
            Token startErrorToken;

            while (Index < EndPos && !Type())
            {
                errorValue = "";
                SkipSpace();
                // Значит что мы сейчас ждем индификатор
                if (lastCode == CODE.COMMA)
                {
                    if (Token.Code == CODE.IDENTIFIER)
                    {
                        lastCode = CODE.IDENTIFIER;
                        Index++;
                        continue;
                    }

                    else
                    {
                        startErrorToken = Token;
                        errorValue = CollectError(Index, EndPos, true, () => !Type() && Token.Code != CODE.IDENTIFIER && Token.Code != CODE.COMMA);
                        if (errorValue != "")
                        {
                            AddError($"Ожидался индификатор, а нашлось '{errorValue}'.", startErrorToken);
                        }
                        else
                        {
                            AddError($"Ожидался индификатор, а нашлось '{Token.TokenValue}'.");
                        }

                        if (Type())
                        {
                            FindAgrsType = true;
                            break;
                        }
                        else if (Token.Code == CODE.IDENTIFIER)
                        {
                            lastCode = CODE.IDENTIFIER;
                            Index++;
                        }
                        else if (Token.Code == CODE.COMMA)
                        {
                            lastCode = CODE.COMMA;
                            Index++;
                        }
                    }

                }

                else if (lastCode == CODE.IDENTIFIER)
                {
                    if (Token.Code == CODE.COMMA)
                    {
                        lastCode = CODE.COMMA;
                        Index++;
                        continue;
                    }
                    if (Type())
                    {
                        FindAgrsType = true;
                        Index++;
                        break;
                    }

                    else
                    {
                        startErrorToken = Token;
                        errorValue = CollectError(Index, EndPos, true, () => !Type() && Token.Code != CODE.IDENTIFIER && Token.Code != CODE.COMMA);
                        if (errorValue != "")
                        {
                            AddError($"Ожидалась запятая или тип, а нашлось '{errorValue}'.", startErrorToken);
                        }
                        else
                        {
                            AddError($"Ожидалась запятая или тип, а нашлось '{Token.TokenValue}'.");
                        }

                        if (Type())
                        {
                            FindAgrsType = true;
                            Index++;
                            break;
                        }
                        else if (Token.Code == CODE.IDENTIFIER)
                        {
                            lastCode = CODE.IDENTIFIER;
                            Index++;
                        }
                        else if (Token.Code == CODE.COMMA)
                        {
                            lastCode = CODE.COMMA;
                            Index++;
                        }
                    }
                }
            }

            if (FindAgrsType && Index < EndPos - 1)
            {
                startErrorToken = Token;
                errorValue = CollectError(Index, EndPos);
                if (errorValue != "")
                {
                    AddError($"Неожиданное значение после типа аргументов '{errorValue}'.", startErrorToken);
                }
                else
                {
                    AddError($"Неожиданное значение после типа аргументов '{Token.TokenValue}'.");
                }
            }

            else if (!FindAgrsType)
            {
                Index=EndPos;
                errorValue = CollectError(Index, EndPos);
                startErrorToken = Token;
                AddError($"Ожидался тип аргументов, а нашлось '{Token.TokenValue}'." , startErrorToken);
                if (errorValue != "")
                {
                    AddError($"Ожидался тип аргументов, а нашлось '{errorValue}'.");
                }
                else
                {
                    AddError($"Ожидался тип аргументов, а нашлось '{Token.TokenValue}.");
                }
            }


            // Проверяем конец и пытаемся найти переход на выражение

            Index = EndPos;
            if (Status != ArgumentsStatus.FIND_RPAREN)
            {
                AddError("Ожидалось )");
            }

            Index = EndPos + 1;
            NextPosition = Index;

            if (Status == ArgumentsStatus.FIND_RPAREN)
            {
                while (Token.Code != CODE.LBRACE && IsNotEndList)
                {
                    Index++;
                }
                if (Token.Code != CODE.LBRACE)
                {
                    Index = EndPos + 1;
                    AddError("Ожидалось {");
                    Index++;
                }
                else
                {
                    if (Index != EndPos + 1)
                    {
                        errorValue = CollectError(Index, EndPos + 1);
                        if (errorValue != "")
                        {
                            AddError($"Неожиданное значение после ) '{errorValue}'.");
                        }
                        else
                        {
                            AddError($"Неожиданное значение после ) '{Token.TokenValue}'.");
                        }
                    }
                    Index++;
                }


            }

            if (Status == ArgumentsStatus.FIND_LBRACE || Status == ArgumentsStatus.FIND_RPAREN)
            {
                int startPos = Index;
                while (Token.Code != CODE.RETURN && IsNotEndList)
                {
                    Index++;
                }
                if (Token.Code != CODE.RETURN)
                {
                    Index = EndPos + 2;
                    AddError("Ожидалось return");
                    errorValue = CollectError(Index, _tokens.Count, true, () => Token.Code != CODE.IDENTIFIER);
                    if (errorValue != "")
                    {
                        AddError($"Ожидалось return, а нашлось '{errorValue}'.");
                    }
                    else
                    {
                        AddError($"Ожидалось return, а нашлось '{Token.TokenValue}'.");
                    }

                    NextPosition = Index;
                    return;
                }
                else
                {
                    if (Index != startPos)
                    {
                        errorValue = CollectError(startPos, Index, false);
                        if (errorValue != "")
                        {
                            AddError($"Неожиданное значение перед return '{errorValue}'.");
                        }
                        else
                        {
                            AddError($"Неожиданное значение перед return '{Token.TokenValue}'.");
                        }
                    }
                    NextPosition = Index + 1;
                }
            }

            if (Status == ArgumentsStatus.FIND_RETURN)
            {
                NextPosition = EndPos + 1;
            }

            if (Status == ArgumentsStatus.FIND_END)
            {
                NextPosition = EndPos + 1;
            }

            if (Status == ArgumentsStatus.NOT_FIND)
            {
                Index = EndPos;
                AddError("Ожидалось ;");
            }

            Console.WriteLine("Аргументы: " + Tokens[NextPosition].ToString());

        }
    }
}
