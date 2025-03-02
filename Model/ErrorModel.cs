using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Model
{
    public class ErrorModel
    {
        public int Number { get; set; } // Номер ошибки
        public string FileName { get; set; } // Название файла
        public int Line { get; set; } // Номер строки
        public int Column { get; set; } // Столбец
        public string Message { get; set; } // Текст ошибки

        public ErrorModel(int number, string fileName, int line, int column, string message)
        {
            Number = number;
            FileName = fileName;
            Line = line;
            Column = column;
            Message = message;
        }
    }

}
