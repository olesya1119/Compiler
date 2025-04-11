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

        public bool FindLparen { get; set; } = false;
        public bool FindFunc { get; set; } = false;

        public int LparenPos { get; set; }
        public int FuncPos { get; set; }

        private int endPos = 0;


        public List<ErrorEntry> Parse()
        {
            BeforeParse();
            Func();
            return _errors;
        }


        private void BeforeParse()
        {
            _tokens.Add(new Token(CODE.DELIMITER, " ", _tokens[_tokens.Count - 1].Line, _tokens[_tokens.Count - 1].EndColumn + 1, _tokens[_tokens.Count - 1].EndColumn + 1, _tokens[_tokens.Count - 1].EndIndex + 1, _tokens[_tokens.Count - 1].EndIndex + 1));
            while (Index < _tokens.Count - 1 && Token.Code != CODE.LPAREN)
            {
                Index++;
            }
            if (Token.Code == CODE.LPAREN)
            {
                FindLparen = true;
                LparenPos = Index;
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

            if (LparenPos > 10 || LparenPos < FuncPos)
            {
                FindLparen = false;
            }

            if (FindLparen) endPos = LparenPos;
            else endPos = _tokens.Count;
            Index = 0;

            NextPosition = endPos + 1;
        }

        private void Func()
        {
            if (!FindFunc)
            {
                AddError($"Ожидалось: ключевое слово func.");
                Index = 1;

            }
            else
            {
                if (FuncPos != 0)
                {
                    AddError($"Ожидалось: ключевое слово func.");
                }

                Index = FuncPos + 1;
                if (Token.Code != CODE.DELIMITER)
                {
                    AddError($"Ожидалось: пробел после func.");
                }
                Index++;
            }

            FuncName();
        }

        private void FuncName()
        {
            SkipSpace();
            string errorValue;
            Condition condition = () => Token.Code != CODE.IDENTIFIER;
            Token startToken = Token;

            // Проверяем имя функции
            errorValue = CollectError(Index, endPos, true, condition);
            if (Token.Code != CODE.IDENTIFIER)
            {
                AddError($"Ожидалось: идентификатор (имя функции).", 0,startToken);
            }
            else
            {
                if (errorValue != "") AddError($"Лишняя последовательность символов.", 0, startToken);
            }

            AfterFuncName();
        }

        private void AfterFuncName()
        {
            Index++;
            SkipSpace();
            string errorValue;
            Token startToken = Token;
            if (FindLparen)
            {
                errorValue = CollectError(Index, endPos);
                if (errorValue != "") AddError($"Лишняя последовательность символов.", 0, startToken);
            }
            else
            {
                AddError($"Ожидалось: ( после имени функции.", 1);
                NextPosition = Index;
            }

            //AddError($"ГОЛОВА: {_tokens[NextPosition]}");

        }
    }
}
