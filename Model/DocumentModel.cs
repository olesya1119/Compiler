using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Compiler.Model
{
    public class DocumentModel : INotifyPropertyChanged
    {
        private string _fileName;
        private FlowDocument _document;
        private string _filePath;
        private RichTextBox _editor;

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

        public FlowDocument Document
        {
            get => _document;
            set { _document = value; OnPropertyChanged(); }
        }

        public RichTextBox Editor
        {
            get => _editor;
            set { _editor = value; OnPropertyChanged(); }
        }

        public DocumentModel(string fileName)
        {
            FileName = fileName;
            Document = new FlowDocument(new Paragraph(new Run("")));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

