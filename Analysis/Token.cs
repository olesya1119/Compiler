using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Compiler.Analysis
{
    public class Token
    {
        private CODE _code;
        private string _token;
        private int _line;
        private int _startColumn;
        private int _endColumn;

        public Token(CODE code, string token, int line, int startColumn, int endColumn)
        {
            _code = code;
            _token = token;
            _line = line;
            _startColumn = startColumn;
            _endColumn = endColumn;
        }

        public CODE Code => _code;
        public string TokenValue => _token;
        public int Line => _line;
        public int StartColumn => _startColumn;
        public int EndColumn => _endColumn;

    }
}
