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
        int TypePos { get; set; }

        // Местоположение различных важных токенов
        bool FindRparen{ get; set; } = false;
        bool FindLbrace { get; set; } = false;
        bool FindReturn { get; set; } = false;
        bool FindEnd { get; set; }=false;

        int RparenPos { get; set; }
        int LbracePos { get; set; }
        int ReturnPos { get; set; }
        int ENDPos { get; set; }


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
            while (IsNotEndList && Token.Code != CODE.RPAREN && Token.Code != CODE.LBRACE && Token.Code != CODE.RETURN && Token.Code != CODE.END)
            {
                Index++;
            }
            if (Token.Code == CODE.RPAREN)
            {
                EndPos = Index;
                Status = ArgumentsStatus.FIND_RPAREN;
                FindRparen = true;
                RparenPos = Index;

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
                FindLbrace = true;
                LbracePos = Index;
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
                FindReturn = true;
                ReturnPos = Index;
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
                FindEnd = true;
                ENDPos = Index;
            }
            
            if(FindRparen) EndPos = RparenPos;
            else if (FindLbrace) EndPos = LbracePos;
            else if (FindReturn) EndPos = ReturnPos;
            else if (FindEnd) EndPos = EndPos;
            else EndPos = _tokens.Count - 1;
        }

        // Проверка аргументов
        private void ParseArguments()
        {
            // Начитаем перебирать аргументы
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
                        AddError($"Ожидалось: индификатор.", 0, startErrorToken);

                        if (Type())
                        {
                            FindAgrsType = true;
                            TypePos = Index;
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
                        TypePos = Index;
                        Index++;
                        break;
                    }

                    else
                    {
                        startErrorToken = Token;
                        errorValue = CollectError(Index, EndPos, true, () => !Type() && Token.Code != CODE.IDENTIFIER && Token.Code != CODE.COMMA);
                        AddError($"Ожидалось: запятая или тип.", 0, startErrorToken);

                        if (Type())
                        {
                            FindAgrsType = true;
                            TypePos = Index;
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

            if (lastCode != CODE.IDENTIFIER) AddError($"Ожидалось: индификатор.", 0, _tokens[Index]);
            NextPosition = Index + 1;

            if (IsNotEndList && Type())
            {
                FindAgrsType = true;
                TypePos = Index;
                Index++;
            }

            if (FindAgrsType && TypePos < EndPos - 1)
            {
                SkipSpace();
                startErrorToken = _tokens[TypePos + 1];
                errorValue = CollectError(Index, EndPos);
                AddError($"Лишняя последовательность символов.", 0, startErrorToken);
            }

            else if (!FindAgrsType)
            {
                Index = EndPos;
                SkipSpace();
                errorValue = CollectError(Index, EndPos);
                startErrorToken = Token;
                AddError($"Ожидалось: тип аргументов.", 1, startErrorToken);
            }

            EndArguments(); 
        }

        // Проверка конца агрументов
        private void EndArguments()
        {
            Token startErrorToken;
            string errorValue;
            // Проверяем конец и пытаемся найти переход на выражение
            
            NextPosition = Index + 1;

            if (FindReturn) NextPosition = ReturnPos + 1;
            else if (FindLbrace) NextPosition = LbracePos + 1;
            else if (FindRparen) NextPosition = RparenPos + 1;
            else NextPosition = Index + 1;

            


            if (!FindRparen) AddError("Ожидалось: ).", 3, _tokens[FindAgrsType ? TypePos + 1 : Index]);
        
            if (!FindLbrace && !FindRparen) AddError("Ожидалось: {.", 2, _tokens[FindAgrsType ? TypePos + 2 : Index]);
            else if (EndPos + 1 < _tokens.Count && !FindLbrace) AddError("Ожидалось: {.", 2, _tokens[FindAgrsType ? EndPos + 1 : Index + 1]);

            if (!FindReturn)
            {
                int buf = NextPosition;
                if (!FindLbrace && !FindRparen && FindAgrsType) buf = TypePos + 1;

                Index = buf;

                if (IsNotEndList) AddError("Ожидалось: ключевое слово return.", 3, _tokens[Index]);
                Index++;
                NextPosition = Index;
            }

            if (!FindEnd)
            {
             
                AddError("Ожидалось: ;", 0, _tokens[_tokens.Count - 1]);
            }

            if (FindRparen && FindLbrace && LbracePos - RparenPos != 1) AddError("Лишняя последовательность символов.", 0, _tokens[RparenPos + 1]);

            if (FindLbrace && FindReturn && ReturnPos - LbracePos != 1) AddError("Лишняя последовательность символов.", 0, _tokens[LbracePos + 1]);

            if (NextPosition >= _tokens.Count) NextPosition = _tokens.Count - 1;
            //AddError("Аргументы: " + Tokens[NextPosition].ToString(), 0, Tokens[NextPosition]);


            /*Index = EndPos;
            if (Status != ArgumentsStatus.FIND_RPAREN)
            {
                AddError("Ожидалось )");
            }

            Index = EndPos + 1;
            NextPosition = Index;

            if (Status == ArgumentsStatus.FIND_RPAREN)
            {
                while (IsNotEndList && Token.Code != CODE.LBRACE)
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
                        startErrorToken = Token;
                        errorValue = CollectError(Index, EndPos + 1);
                        if (errorValue != "")
                        {
                            AddError($"Неожиданное значение после ) '{errorValue}'.", 0, startErrorToken);
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
                    startErrorToken = Token;
                    errorValue = CollectError(Index, _tokens.Count, true, () => Token.Code != CODE.IDENTIFIER);
                    if (errorValue != "")
                    {
                        AddError($"Ожидалось return, а нашлось '{errorValue}'.", 0, startErrorToken);
                    }
                    else
                    {
                        AddError($"Ожидалось return, а нашлось '{Token.TokenValue}'.");
                    }

                    NextPosition = Index + 1;
                    return;
                }
                else
                {
                    if (Index != startPos)
                    {
                        startErrorToken = Token;
                        errorValue = CollectError(startPos, Index, false);
                        if (errorValue != "")
                        {
                            AddError($"Неожиданное значение перед return '{errorValue}'.", 0, startErrorToken);
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

            AddError("Аргументы: " + Tokens[NextPosition].ToString(), 0, Tokens[NextPosition]);*/

        }

            
    }
}
