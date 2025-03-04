using System.Collections.ObjectModel;
using ICSharpCode.AvalonEdit;

namespace Compiler.Model
{
    public class DocumentModel : BaseModel
    {
        private string _fileName;
        private string _filePath;
        private string _textContent;
        private ObservableCollection<ErrorModel> _errors = new ObservableCollection<ErrorModel>();

        public string FileName
        {
            get => _fileName;
            set { _fileName = value; OnPropertyChanged(); }
        }

        public string FilePath
        {
            get => _filePath;
            set { _filePath = value; OnPropertyChanged(); }
        }

        public string TextContent
        {
            get => _textContent;
            set { _textContent = value; OnPropertyChanged(); }
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