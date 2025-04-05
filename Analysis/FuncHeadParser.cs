using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Analysis
{
    class FuncHeadParser : BaseParser
    {
        public FuncHeadParser(List<Token> tokens, int pos, string text) : base(tokens, pos, text)
        {
        }

        public bool FindLPAREN { get; set; } = false;
        public bool FindFunc { get; set; } = false;

        public int LPARENPos { get; set; }
        public int FuncPos { get; set; }

        public List<ErrorEntry> Parse()
        {
            while (Index < _tokens.Count - 1 && Token.Code != CODE.LPAREN)
            {
                Index++;
            }
            if (Token.Code == CODE.LPAREN)
            {
                FindLPAREN = true;
                LPARENPos = Index;
            }
            Index = 0;

            while (Index < _tokens.Count - 1 && Token.Code != CODE.FUNC)
            {
                Index++;
            }
            if (Token.Code == CODE.FUNC)
            {
                FindFunc = true;
                FuncPos = Index;

            }
            Index = 0;

            Func();
            return _errors;
        }


        private void Func()
        {
            Condition condition = () => Token.Code != CODE.IDENTIFIER;
            string errorValue;

            if (!FindFunc)
            {
                AddError($"Ожидалось ключевое слово func");
                Index = 1;

            }
            else
            {
                if (FuncPos != 0)
                {
                    AddError($"Ожидалось ключевое слово func");
                }

                Index = FuncPos + 1;
                if (Token.Code != CODE.DELIMITER)
                {
                    AddError($"Ожидался пробел после func");
                }
                Index++;
            }


            if (FindLPAREN)
            {
                errorValue = CollectError(Index, LPARENPos, true, condition);

                if (Token.Code != CODE.IDENTIFIER)
                {
                    if (errorValue != "") AddError($"Ожидалось имя функции, а нашлось {errorValue}");
                }
                else
                {
                    if (errorValue != "") AddError($"Неожиданная последовательность символов {errorValue}");
                    Index++;

                    errorValue = CollectError(Index, LPARENPos - 1, true, condition);

                    if (errorValue != "")
                    {
                        if (Token.Code == CODE.IDENTIFIER)
                        {
                            Index--;
                            AddError($"Ожидалось (");
                        }
                        else AddError($"Неожиданные символы после имени функции {errorValue}");
                    }
                }
            }

            else
            {
                errorValue = CollectError(Index, _tokens.Count, true, condition);

                if (Token.Code != CODE.IDENTIFIER)
                {
                    if (errorValue != "") AddError($"Ожидалось имя функции, а нашлось {errorValue}");
                }
                AddError($"Ожидалось (");
            }
            NextPosition = Index + 1;
            Console.WriteLine("Голова: " + Tokens[NextPosition].ToString());
        }
    }
}
