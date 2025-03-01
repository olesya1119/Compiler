using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Documents;

namespace Compiler.Model
{
    public class DocumentModel : INotifyPropertyChanged
    {
        private string _fileName;
        private FlowDocument _document;

        public string FileName
        {
            get => _fileName;
            set { _fileName = value; OnPropertyChanged(); }
        }

        public FlowDocument Document
        {
            get => _document;
            set { _document = value; OnPropertyChanged(); }
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

