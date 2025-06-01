using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Analysis
{
    class ExpressionParser : BaseParser
    {
        private int _indentLevel = 0;
        private string callStack = "";

        public ExpressionParser(List<Token> tokens, int pos, string text) : base(tokens, pos, text)
        {
        }

        public string Parse()
        {
            SkipSpace();
            CleanParens();
            DebugPrint("E ", 0, _tokens.Count);
            E(0, _tokens.Count);

            return callStack;
        }

        public void CleanParens()
        {
            Index = 0;
            Stack<int> parenStack = new Stack<int>();
            List<int> unmatchedRight = new List<int>();
            List<int> unmatchedLeft = new List<int>();

            // Первый проход: находим все непарные скобки
            for (; Index < _tokens.Count; Index++)
            {
                if (Token.Code == CODE.LPAREN)
                {
                    parenStack.Push(Index);
                }
                else if (Token.Code == CODE.RPAREN)
                {
                    if (parenStack.Count > 0)
                    {
                        parenStack.Pop();
                    }
                    else
                    {
                        unmatchedRight.Add(Index);
                    }
                }
            }

            // Оставшиеся в стеке - это непарные левые скобки
            unmatchedLeft = parenStack.ToList();

            int rmCount = 0;
            // Удаляем непарные правые скобки (с начала к концу)
            foreach (int index in unmatchedRight.OrderByDescending(i => i))
            {
                AddError("Правая круглая скобка ) не имеет пары.", 0, _tokens[index - rmCount]);
                _tokens.RemoveAt(index - rmCount);
                rmCount++;
            }

            // Удаляем непарные левые скобки (с конца к началу)
            foreach (int index in unmatchedLeft.OrderByDescending(i => i))
            {
                AddError("Левая круглая скобка ( не имеет пары.", 0, _tokens[index - rmCount]);
                _tokens.RemoveAt(index - rmCount);
            }

            Index = 0;
        }



        public void E(int startPos, int endPos)
        {
            _indentLevel++;

            int countRparen = 0;
            for (Index = endPos - 1; Index > startPos; Index--)
            {
                if (Token.Code == CODE.RPAREN) countRparen++;
                else if (Token.Code == CODE.LPAREN) countRparen--;

                if (countRparen == 0 && (Token.Code == CODE.PLUS || Token.Code == CODE.MINUS))
                {
                    int bufIndex = Index; 
                    DebugPrint("E ", startPos, Index);
                    E(startPos, Index);

                    Index = bufIndex;

                    DebugPrint("T ", Index + 1, endPos);
                    T(Index + 1, endPos);

                    _indentLevel--;
                    return;
                }
            }

            DebugPrint("T ", startPos, endPos);
            T(startPos, endPos);

            _indentLevel--;

        }

        public void T(int startPos, int endPos)
        {
            _indentLevel++;

            int countRparen = 0;
            for (Index = endPos - 1; Index > startPos; Index--)
            {
                if (Token.Code == CODE.RPAREN) countRparen++;
                else if (Token.Code == CODE.LPAREN) countRparen--;

                if (countRparen == 0 && (Token.Code == CODE.MULTIPLY || Token.Code == CODE.DIVIDE))
                {
                    int bufIndex = Index;

                    DebugPrint("T ", startPos, Index);
                    T(startPos, Index);

                    Index = bufIndex;

                    DebugPrint("F ", Index + 1, endPos);
                    F(Index + 1, endPos);

                    _indentLevel--;
                    return;
                }
            }

            DebugPrint("F ", startPos, endPos);
            F(startPos, endPos);

            _indentLevel--;

        }

        public void F(int startPos, int endPos)
        {
            _indentLevel++;

            int countRparen = 0;

            for (Index = startPos; Index < endPos; Index++)
            {
                if (Token.Code == CODE.RPAREN) countRparen++;
                else if (Token.Code == CODE.LPAREN) countRparen--;

                if (countRparen == 0 && _tokens[Index].Code == CODE.POWER)
                {
                    int bufIndex = Index;

                    DebugPrint("V ", startPos, Index);
                    V(startPos, Index);

                    Index = bufIndex;

                    DebugPrint("F ", Index + 1, endPos);
                    F(Index + 1, endPos);

                    _indentLevel--;
                    return;
                }
            }

            DebugPrint("V ", startPos, endPos);
            V(startPos, endPos);

            _indentLevel--;

        }

        public void V(int startPos, int endPos)
        {
            _indentLevel++;

            Index = startPos;
            if (startPos == endPos) return;

            if (Token.Code == CODE.UNSIGNED_INT || Token.Code == CODE.IDENTIFIER)
            {
                Index++;

                if (Index != endPos)
                {
                    AddError("Неожиданная последовательность символов", 0, Token);
                }

            }
            else if (Token.Code == CODE.LPAREN)
            {
                Index++;
                for (; Index < endPos && Token.Code != CODE.RPAREN; Index++) ;

                if (Token.Code == CODE.RPAREN)
                {
                    if (Index < endPos - 1)
                    {
                        AddError("Неожиданная последовательность символов", 0, _tokens[Index + 1]);
                    }
                    DebugPrint("E ", startPos + 1, Index);
                    E(startPos + 1, Index);
                }
            }
            else if (endPos < startPos)
            {
                AddError("ОПА");
            }
            _indentLevel--;
        }

        public void DebugPrint(string mess, int startPos, int endPos = -1)
        {
            string indent = new string(' ', _indentLevel * 2); 
            callStack += indent + mess + ": ";
            Console.Write(indent + mess + ": ");

            if (endPos == -1)
            {
                Console.WriteLine($"{_tokens[startPos].TokenValue}");
                callStack += $"{_tokens[startPos].TokenValue}\n";
            }
            else
            {
                string buf = "";
                for (int i = startPos; i < endPos; i++)
                {
                    buf += _tokens[i].TokenValue + " ";
                }
                Console.WriteLine($"{buf}");
                callStack += $"{buf}";
                if (buf == "") {
                    callStack += $"ε";
                }
                callStack += $"\n";
            }
        }
    }
}






