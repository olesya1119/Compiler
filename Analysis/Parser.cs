using Compiler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Analysis
{
    enum ErrorType { DELETE, REPLACE, PUSH}

    class Parser
    {
        /*
        private List<Token> tokens;
        private int i = 0;
        private List<ErrorModel> errors;

        private Token token => tokens[i];

        public Parser(List<Token> tokens) {
            this.tokens = tokens;
            errors = new List<ErrorModel>();
            Func();
        }

        private void Func(){
            if (token.Code == CODE.FUNC) SpaceAfterFunc();

            else 
            { 
                
            }
        }


        private void SpaceAfterFunc()
        {
            i++;
            if (token.Code == CODE.DELIMITER) FuncName();
 
            else
            {

            }
        }

        private void FuncName()
        {
            i++;
            if (token.Code == CODE.IDENTIFIER) AfterFuncName();

            else
            {

            }
        }

        private void AfterFuncName()
        {
            i++; 
            if (token.Code == CODE.LPAREN) Arguments();

            else
            {

            }
        }

        private void Arguments()
        {
            i++;
            int 
            if (ParamsList())
            {
                ArgumentsEnd();
            }
            else
            {
                ArgumentsEnd();
            }
        }

        private bool ParamsList()
        {
            return false
        }

        private void ArgumentsEnd()
        {

        }*/

        private List<Token> tokens;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }



        /// <summary> 1) FUNC -> "func" SPACE_AFTER_FUNC </summary>
        private List<ErrorModel> Func(int pos, List<ErrorModel> errors)
        {
            if (isAtEnd(pos)) return errors;

            // Все окей
            if (tokens[pos].Code == CODE.FUNC) return SpaceAfterFunc(pos + 1, errors);

            // Все не окей, пробуем 3 способа нейтрализации
            return minErrorsList(
                Func(pos + 1, AddError(errors, GetError(pos, tokens[pos], ErrorType.DELETE))),
                SpaceAfterFunc(pos + 1, AddError(errors, GetError(pos, tokens[pos], ErrorType.PUSH))),
                SpaceAfterFunc(pos, AddError(errors, GetError(pos, tokens[pos], ErrorType.PUSH)))
                );
        }

        /// <summary> 2) SPACE_AFTER_FUN -> " " FUNC_NAME </summary>
        private List<ErrorModel> SpaceAfterFunc(int pos, List<ErrorModel> errors)
        {
            if (isAtEnd(pos)) return errors;

            // Все окей
            if (tokens[pos].Code == CODE.DELIMITER) return SpaceAfterFunc(pos + 1, errors);

            // Все не окей, пробуем 3 способа нейтрализации
            return minErrorsList(
                SpaceAfterFunc(pos + 1, AddError(errors, GetError(pos, tokens[pos], ErrorType.DELETE))),
                FuncName(pos + 1, AddError(errors, GetError(pos, tokens[pos], ErrorType.PUSH))),
                FuncName(pos, AddError(errors, GetError(pos, tokens[pos], ErrorType.PUSH)))
                );
        }

        /// <summary> 3) FUNC_NAME -> IDENTIFIER AFTER_FUNC_NAME </summary>
        private List<ErrorModel> FuncName(int pos, List<ErrorModel> errors)
        {
            if (isAtEnd(pos)) return errors;

            // Все окей
            if (tokens[pos].Code == CODE.IDENTIFIER) return AfterFuncName(pos + 1, errors);

            // Все не окей, пробуем 3 способа нейтрализации
            return minErrorsList(
                FuncName(pos + 1, AddError(errors, GetError(pos, tokens[pos], ErrorType.DELETE))),
                AfterFuncName(pos + 1, AddError(errors, GetError(pos, tokens[pos], ErrorType.PUSH))),
                AfterFuncName(pos, AddError(errors, GetError(pos, tokens[pos], ErrorType.PUSH)))
                );
        }

        /// <summary> 4) AFTER_FUNC_NAME -> "(" ARGUMENTS </summary>
        private List<ErrorModel> AfterFuncName(int pos, List<ErrorModel> errors)
        {
            if (isAtEnd(pos)) return errors;

            // Все окей
            if (tokens[pos].Code == CODE.LPAREN) return Arguments(pos + 1, errors);

            // Все не окей, пробуем 3 способа нейтрализации
            return minErrorsList(
                AfterFuncName(pos + 1, AddError(errors, GetError(pos, tokens[pos], ErrorType.DELETE))),
                Arguments(pos + 1, AddError(errors, GetError(pos, tokens[pos], ErrorType.PUSH))),
                Arguments(pos, AddError(errors, GetError(pos, tokens[pos], ErrorType.PUSH)))
                );
        }


        /// <summary> 5) ARGUMENTS -> PARAMS_LIST ARGUMENTS_END> | ARGUMENTS_END </summary>
        private List<ErrorModel> Arguments(int pos, List<ErrorModel> errors)
        {

        }




        private List<ErrorModel> AddError(List<ErrorModel> errors, ErrorModel newError)
        {
            List<ErrorModel> newErrors = new List<ErrorModel>();
            foreach (ErrorModel error in errors)
            {
                newErrors.Add(error);
            }
            newErrors.Add(newError);
            return newErrors;
        }



        private bool isAtEnd(int currentPosition)
        {
            return currentPosition >= tokens.Count;
        }

        private List<ErrorModel> minErrorsList(List<ErrorModel> e1, List<ErrorModel> e2, List<ErrorModel> e3)
        {
            if (e1.Count <= e2.Count && e1.Count <= e3.Count) return e1;
            if (e2.Count <= e3.Count) return e2;
            return e3;
        }


        public static ErrorModel GetError(int pos, Token token, ErrorType errorType)
        {
            return new ErrorModel(1, "123", token.Line, token.StartColumn, token.TokenValue);
        }


    }
}