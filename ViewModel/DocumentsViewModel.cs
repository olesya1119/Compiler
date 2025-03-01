using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Microsoft.Win32;
using Compiler.Model;
using System.Windows.Documents;

namespace Compiler.ViewModel
{
    public class DocumentsViewModel : BaseViewModel
    {
        public ObservableCollection<DocumentModel> OpenDocuments { get; set; } = new ObservableCollection<DocumentModel>();
        private DocumentModel _selectedDocument;

        public DocumentModel SelectedDocument
        {
            get => _selectedDocument;
            set { _selectedDocument = value; OnPropertyChanged(); }
        }

        public ICommand NewDocumentCommand { get; }
        public ICommand OpenDocumentCommand { get; }
        public ICommand SaveDocumentCommand { get; }
        public ICommand CloseDocumentCommand { get; }

        public DocumentsViewModel()
        {
            NewDocumentCommand = new RelayCommand(NewDocument);
            OpenDocumentCommand = new RelayCommand(OpenDocument);
            SaveDocumentCommand = new RelayCommand(SaveDocument);
            CloseDocumentCommand = new RelayCommand(CloseDocument);
        }

        private void NewDocument(object parameter)
        {
            var newDoc = new DocumentModel("Новый документ");
            OpenDocuments.Add(newDoc);
            SelectedDocument = newDoc;
        }

        private void OpenDocument(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Текстовые файлы|*.txt|Все файлы|*.*" };
            if (openFileDialog.ShowDialog() == true)
            {
                string content = File.ReadAllText(openFileDialog.FileName);
                var doc = new DocumentModel(openFileDialog.FileName)
                {
                    Document = new FlowDocument(new Paragraph(new Run(content)))
                };
                OpenDocuments.Add(doc);
                SelectedDocument = doc;
            }
        }

        private void SaveDocument(object parameter)
        {
            if (SelectedDocument == null) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Текстовые файлы|*.txt|Все файлы|*.*" };
            if (saveFileDialog.ShowDialog() == true)
            {
                TextRange textRange = new TextRange(SelectedDocument.Document.ContentStart, SelectedDocument.Document.ContentEnd);
                File.WriteAllText(saveFileDialog.FileName, textRange.Text);
                SelectedDocument.FileName = saveFileDialog.FileName;
            }
        }

        private void CloseDocument(object parameter)
        {
            if (parameter is DocumentModel doc)
            {
                OpenDocuments.Remove(doc);
                if (OpenDocuments.Count > 0)
                    SelectedDocument = OpenDocuments[0];
            }
        }
    }
}
