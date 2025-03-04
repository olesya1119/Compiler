using System.Collections.ObjectModel;
using ICSharpCode.AvalonEdit;

namespace Compiler.Model
{
    public class DocumentModel : BaseModel
    {
        private string _fileName;
        private string _filePath;
        private string _textContent;
        private bool _status;
        private ObservableCollection<ErrorModel> _errors = new ObservableCollection<ErrorModel>();

        public string FileName
        {
            get
            {
                if (_status)
                {
                    return "* " + _fileName;
                }
                return _fileName;
            }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FileName)); 
                }
            }
        }

        public string FilePath
        {
            get => _filePath;  
            set { _filePath = value; OnPropertyChanged(); }
        }

        

        public bool Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                    OnPropertyChanged(nameof(FileName)); // Обновляем имя файла, когда статус меняется
                }
            }
        }

        public string TextContent
        {
            get => _textContent;
            set
            {
                if (_textContent != value)
                {
                    _textContent = value;
                    Status = true; // Документ изменен
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FileName)); // Обновляем название файла
                }
            }
        }


        public ObservableCollection<ErrorModel> Errors
        {
            get => _errors;
            set { _errors = value; OnPropertyChanged(); }
        }

        public void AddError(int line, int column, string message)
        {
            Errors.Add(new ErrorModel(_errors.Count + 1, _fileName, line, column, message));
            OnPropertyChanged(nameof(Errors));
        }

        public DocumentModel(string fileName)
        {
            FileName = fileName;
            TextContent = "";
        }
    }
}