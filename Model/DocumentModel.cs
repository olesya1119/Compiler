using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Compiler.Model
{
    /// <summary> Модель представляющая документ </summary>
    public class DocumentModel : BaseModel
    {
     
        private string _fileName; 
        private string _filePath; 
        private FlowDocument _document;  
        private RichTextBox _editor;
        private ObservableCollection<ErrorModel> _errors = new ObservableCollection<ErrorModel>();

        /// <summary> Название файла </summary>
        public string FileName
        {
            get => _fileName;
            set { _fileName = value; OnPropertyChanged(); }
        }

        /// <summary> Путь к файлу </summary>
        public string FilePath
        {
            get => _filePath;
            set { _filePath = value; OnPropertyChanged(); }
        }

        /// <summary> Документ </summary>
        public FlowDocument Document
        {
            get => _document;
            set { _document = value; OnPropertyChanged(); }
        }

        /// <summary> RichTextBox в которым открыт этот документ </summary>
        public RichTextBox Editor
        {
            get => _editor;
            set { _editor = value; OnPropertyChanged(); }
        }

        /// <summary> Список ошибок </summary>
        public ObservableCollection<ErrorModel> Errors
        {
            get => _errors;
            set { _errors = value; OnPropertyChanged(); }
        }

        /// <summary> Добавление новой ошибки. </summary>
        public void AddError(int line, int column, string message)
        {
            Errors.Add(new ErrorModel(_errors.Count + 1, _fileName, line, column, message));
            OnPropertyChanged(nameof(Errors));
        }



        /// <summary> Модель представляющая документ. </summary>
        public DocumentModel(string fileName)
        {
            FileName = fileName;
            Document = new FlowDocument(new Paragraph(new Run("")));
        }

        
    }
}
