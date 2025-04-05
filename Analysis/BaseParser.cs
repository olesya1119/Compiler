using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Analysis
{
    /// <summary> Класс для представления ошибок </summary>
    public class ErrorEntry
    {
        public string Message { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public ErrorEntry(string message, Token token)
        {
            Message = message;
            Line = token.Line;
            Column = token.StartColumn;
        }

        public override string ToString()
        {
            return Message + " | " + Line.ToString() + " | " + Column.ToString();
        }
    }

    /// <summary> Делегат для проверки условия </summary>
    public delegate bool Condition();


    /// <summary> Базовый класс для всех парсеров, кроме главного </summary>
    class BaseParser
    {
        // Используются только в парсере
        protected List<Token> _tokens;
        protected int _index;
        protected List<ErrorEntry> _errors;
        protected string _text;

        protected int Index { get { return _index; } set { _index = value; } }
        protected Token Token { get { return _tokens[Index]; } }

        protected bool IsNotEndList => _index < _tokens.Count - 1;


        // Используются другими парсерами, как результат работы. 
        public int NextPosition { get; protected set; }
        public List<Token> Tokens { get { return _tokens; } }
        public List<ErrorEntry> Errors { get { return _errors; } }


        // Мы не смотрим уже на просмотренные токены и не меняем изначальный список
        // Убираем токены с прошлого парсера
        public BaseParser(List<Token> tokens, int pos, string text)
        {
            _text = text;
            _tokens = new List<Token>() { };
            _errors = new List<ErrorEntry>() { };
            _index = 0;
            // Скопируем только нужные нам токены
            for (int i = 0; i < tokens.Count; i++)
            {
                if (i >= pos)
                {
                    _tokens.Add(tokens[i]);
                }
            }

            _text = text;
        }


        ///<summary> Добавление ошибок в список ошибок </summary>
        protected void AddError(string message, Token token = null)
        {
            _errors.Add(new ErrorEntry(message, token ?? Token));
        }

        ///<summary> Сбор ошибок </summary>
        protected string CollectError(int startPos, int endPos, bool moveIndex = true, Condition condition = null)
        {
            // Устанавливаем condition по умолчанию (всегда true)
            condition = condition ?? (() => true);

            SkipSpace();
            string errorValue = "";
            Token startToken = _tokens[startPos], endToken;
            bool flag = false;

            for (int i = startPos; i < endPos && condition(); i++)
            {
                flag = true;
                if (moveIndex) Index++;
                endToken = _tokens[i];
                errorValue += _tokens[i].TokenValue;
            }

            // TODO: Тут нужно явно доработать. Пока что не трогаю то нужно делать срез со строки.
            // А еще аналогично организовать лексические ошибки
            if (flag)
            {

            }


            return errorValue;
        }


        // Вспомогательный функционал

        ///<summary> Пропуск пробелов </summary>
        protected void SkipSpace()
        {
            if (Token.Code == CODE.DELIMITER) Index++;
        }

        ///<summary> Проверяет, что заданный токен является Типом </summary>
        protected bool Type(Token token)
        {
            if (token.Code == CODE.INT || token.Code == CODE.UINT || token.Code == CODE.FLOAT32 || token.Code == CODE.FLOAT64) return true;
            else return false;
        }

        ///<summary> Проверяет, что текущий токен является Типом </summary>
        protected bool Type()
        {
            if (Token.Code == CODE.INT || Token.Code == CODE.UINT || Token.Code == CODE.FLOAT32 || Token.Code == CODE.FLOAT64) return true;
            else return false;
        }
    }
}
