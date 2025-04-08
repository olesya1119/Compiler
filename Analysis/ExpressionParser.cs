using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Analysis
{
    class ExpressionParser : BaseParser
    {
        enum StatuExp { OPERTION, OPERAND, LPAREN, RPAREN }
        public ExpressionParser(List<Token> tokens, int pos, string text) : base(tokens, pos, text)
        {
        }

        public List<ErrorEntry> Parse()
        {
            SkipSpace();
            Console.WriteLine(Token);
            EXPRESSION();

            return _errors;
        }


        public bool Operation()
        {
            if (Token.Code == CODE.PLUS || Token.Code == CODE.MINUS || Token.Code == CODE.MULTIPLY || Token.Code == CODE.DIVIDE) return true;
            else return false;
        }



        public void EXPRESSION()
        {
            int countLPAREN = 0;
            int countRPAREN = 0;
            int ENDPos = 0;
            int RBRACEPos = 0;
            int endPos = 0;

            bool findEND = false;
            bool findRBRACE = false;
            while (IsNotEndList && Token.Code != CODE.RBRACE && Token.Code != CODE.END)
            {
                Index++;
            }
            if (IsNotEndList && Token.Code == CODE.RBRACE)
            {
                findRBRACE = true;
                RBRACEPos = Index;

            }
            Index = 0;

            while (IsNotEndList && Token.Code != CODE.END)
            {
                Index++;
            }
            if (Token.Code == CODE.END)
            {
                findEND = true;
                ENDPos = Index;

            }
            Index = 0;

            if (findRBRACE) endPos = RBRACEPos;
            else if (findEND) endPos = ENDPos;
            else endPos = _tokens.Count;


            StatuExp status = StatuExp.OPERTION;

            while (Index < endPos)
            {
                SkipSpace();
                if (Index >= endPos) break;
                if (status == StatuExp.OPERTION)
                {
                    if (Token.Code == CODE.LPAREN)
                    {
                        countLPAREN++;
                        status = StatuExp.LPAREN;
                        Index++;
                        continue;
                    }
                    else if (Token.Code == CODE.IDENTIFIER || Token.Code == CODE.UNSIGNED_INT)
                    {
                        status = StatuExp.OPERAND;
                        Index++;
                        continue;
                    }
                    else
                    {
                        string errorValue = "";
                        while (Index < endPos && !Operation() && Token.Code != CODE.LPAREN && Token.Code != CODE.RPAREN && Token.Code != CODE.IDENTIFIER && Token.Code != CODE.UNSIGNED_INT)
                        {
                            errorValue += Token.TokenValue;
                            Index++;
                        }
                        if (errorValue != "") AddError($"Ожидался операнд или ( а нашлось {errorValue}");
                        else AddError($"Ожидался операнд или ( а нашлось {Token.TokenValue}");
                        if (Operation()) { status = StatuExp.OPERTION; Index++; continue; }
                        if (Token.Code == CODE.LPAREN) { status = StatuExp.LPAREN; Index++; countLPAREN++; continue; }
                        if (Token.Code == CODE.RPAREN) { status = StatuExp.RPAREN; Index++; countRPAREN++; continue; }
                        if (Token.Code == CODE.IDENTIFIER || Token.Code == CODE.UNSIGNED_INT) { status = StatuExp.OPERAND; Index++; continue; }
                    }
                }

                else if (status == StatuExp.LPAREN)
                {
                    if (Token.Code == CODE.IDENTIFIER || Token.Code == CODE.UNSIGNED_INT)
                    {
                        status = StatuExp.OPERAND;
                        Index++;
                        continue;
                    }
                    else
                    {
                        string errorValue = "";
                        while (Index < endPos && !Operation() && Token.Code != CODE.LPAREN && Token.Code != CODE.RPAREN && Token.Code != CODE.IDENTIFIER && Token.Code != CODE.UNSIGNED_INT)
                        {
                            errorValue += Token.TokenValue;
                            Index++;
                        }
                        if (errorValue != "") AddError($"Ожидался операнд, а нашлось {errorValue}");
                        else AddError($"Ожидался операнд, а нашлось {Token.TokenValue}");
                        if (Operation()) { status = StatuExp.OPERTION; Index++; continue; }
                        if (Token.Code == CODE.LPAREN) { status = StatuExp.LPAREN; Index++; countLPAREN++; continue; }
                        if (Token.Code == CODE.RPAREN) { status = StatuExp.RPAREN; Index++; countRPAREN++; continue; }
                        if (Token.Code == CODE.IDENTIFIER || Token.Code == CODE.UNSIGNED_INT) { status = StatuExp.OPERAND; Index++; continue; }
                    }
                }

                else if (status == StatuExp.OPERAND)
                {
                    if (Operation())
                    {
                        status = StatuExp.OPERTION;
                        Index++;
                        continue;
                    }
                    else if (Token.Code == CODE.RPAREN)
                    {
                        status = StatuExp.RPAREN;
                        countRPAREN++;
                        Index++;
                        continue;
                    }
                    else
                    {
                        string errorValue = "";
                        while (Index < endPos && !Operation() && Token.Code != CODE.LPAREN && Token.Code != CODE.RPAREN && Token.Code != CODE.IDENTIFIER && Token.Code != CODE.UNSIGNED_INT)
                        {
                            errorValue += Token.TokenValue;
                            Index++;
                        }
                        if (errorValue != "") AddError($"Ожидался операнд или ), а нашлось {errorValue}");
                        else AddError($"Ожидался операнд или ), а нашлось {Token.TokenValue}");

                        if (Operation()) { status = StatuExp.OPERTION; Index++; continue; }
                        if (Token.Code == CODE.LPAREN) { status = StatuExp.LPAREN; Index++; countLPAREN++; continue; }
                        if (Token.Code == CODE.RPAREN) { status = StatuExp.RPAREN; Index++; countRPAREN++; continue; }
                        if (Token.Code == CODE.IDENTIFIER || Token.Code == CODE.UNSIGNED_INT) { status = StatuExp.OPERAND; Index++; continue; }
                    }
                }

                else
                {
                    if (Operation())
                    {
                        status = StatuExp.OPERTION;
                        Index++;
                        continue;
                    }
                    else
                    {
                        string errorValue = "";
                        while (Index < endPos && !Operation() && Token.Code != CODE.LPAREN && Token.Code != CODE.RPAREN && Token.Code != CODE.IDENTIFIER && Token.Code != CODE.UNSIGNED_INT)
                        {
                            errorValue += Token.TokenValue;
                            Index++;
                        }
                        if (errorValue != "") AddError($"Ожидалась арифметическая операция, а нашлось {errorValue}");
                        else AddError($"Ожидалась арифметическая операция, а нашлось {Token.TokenValue}");

                        if (Operation()) { status = StatuExp.OPERTION; Index++; continue; }
                        if (Token.Code == CODE.LPAREN) { status = StatuExp.LPAREN; Index++; countLPAREN++; continue; }
                        if (Token.Code == CODE.RPAREN) { status = StatuExp.RPAREN; Index++; countRPAREN++; continue; }
                        if (Token.Code == CODE.IDENTIFIER || Token.Code == CODE.UNSIGNED_INT) { status = StatuExp.OPERAND; Index++; continue; }
                    }

                }
            }

            if (status != StatuExp.OPERAND && status != StatuExp.RPAREN)
            {
                AddError($"Ожидался операнд или )", 0, _tokens[Index]);
            }

            if (countLPAREN > countRPAREN)
            {
                AddError($"Ожидалось )");
            }

            if (countRPAREN > countLPAREN)
            {
                int _ = Index;
                Index = 0;
                AddError($"Ожидалось (");
                Index = _;
            }

            if (!findRBRACE)
            {
                AddError("Ожидалось }");
                return;
            }
            if (!findEND) 
            {
                AddError("Ожидалось ;");
                return;
            }

            if (ENDPos - RBRACEPos != 1)
            {
                AddError($"Неожиданная последовательность" , 0, _tokens[RBRACEPos  + 1]);
            } 

        }
    }
}
