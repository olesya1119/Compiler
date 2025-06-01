using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Analysis
{
    /// <summary>  Коды токенов </summary>
    public enum CODE
    {
        ERROR, // Ошибка
        IDENTIFIER, // Идентификатор
        DELIMITER, // Разделитель (пробел)
        PLUS, // +
        MINUS, // -
        MULTIPLY, // *
        DIVIDE, // /
        POWER, // ^ 
        LPAREN, // (
        RPAREN, // )
        UNSIGNED_INT, // Беззнаковое целое число
    }

    /// <summary> Класс токена </summary>
    public class Token
    {
        public CODE Code { get; set; }
        private string _token;
        private int _line;
        private int _startColumn;
        private int _endColumn;

        public Token(CODE code, string token, int line, int startColumn, int endColumn, int startIndex, int endIndex)
        {
            Code = code;
            _token = token;
            _line = line;
            _startColumn = startColumn;
            _endColumn = endColumn;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public string TokenValue => _token;
        public int Line => _line;
        public int StartColumn => _startColumn;
        public int EndColumn => _endColumn;

        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public override string ToString()
        {
            return $"Code: {Enum.GetName(typeof(CODE), Code),-12} | Token: {_token,-15}";
        }
    }
}
