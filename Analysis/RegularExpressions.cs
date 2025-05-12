using Compiler.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Compiler.Analysis
{
    public class ErrorEntry
    {
        public string Message { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public ErrorEntry(string message, int line, int column)
        {
            Message = message;
            Line = line;
            Column = column;
        }
    }

    class RegularExpressions
    {
        private string Text { get; set; }
        DocumentsViewModel DocumentsViewModel { get; set; }

        public RegularExpressions(DocumentsViewModel documentsViewModel)
        {
            Text = documentsViewModel.SelectedDocument.TextContent;
            DocumentsViewModel = documentsViewModel;
            documentsViewModel.SelectedErrors.Clear();
        }

        public void Parse()
        {
            // Блок 1: Числа, начинающиеся на 1
            string pattern1 = @"\b1\d*(?:\.\d+)?";
            ParsePattern(pattern1, "Блок 1. «{0}»");

            // Блок 2: ФИО на русском
            string pattern2 = @"\b[А-ЯЁ][а-яё]+(?:-[А-ЯЁ][а-яё]+)?\s[А-ЯЁ][а-яё]+\s[А-ЯЁ][а-яё]+\b";
            ParsePattern(pattern2, "Блок 2. «{0}»");

            // Блок 3: Base64 
            string pattern3 = @"(?:[A-Za-z0-9+/]{4})+(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?";
            ParsePattern(pattern3, "Блок 3. «{0}»");
        }

        private void ParsePattern(string pattern, string messageFormat)
        {
            string[] lines = Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                var matches = Regex.Matches(lines[lineNumber], pattern);
                foreach (Match match in matches)
                {
                    // Проверяем, что найденная строка не пустая 
                    if (!string.IsNullOrEmpty(match.Value))
                    {
                        DocumentsViewModel.AddError(
                            lineNumber + 1,
                            match.Index + 1,
                            string.Format(messageFormat, match.Value)
                        );
                    }
                }
            }
        }
    }
}
