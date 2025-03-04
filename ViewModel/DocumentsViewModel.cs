using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Microsoft.Win32;
using Compiler.Model;
using ICSharpCode.AvalonEdit;

namespace Compiler.ViewModel
{
    public class DocumentsViewModel : BaseViewModel
    {
        private DocumentModel _selectedDocument;

        public ObservableCollection<DocumentModel> OpenDocuments { get; set; } = new ObservableCollection<DocumentModel>();

        private TextEditor _editor;

        public DocumentModel SelectedDocument
        {
            get => _selectedDocument;
            set
            {
                _selectedDocument = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedErrors));
            }
        }

        public ObservableCollection<ErrorModel> SelectedErrors => SelectedDocument?.Errors;

        public ICommand NewDocumentCommand { get; }
        public ICommand OpenDocumentCommand { get; }
        public ICommand SaveDocumentCommand { get; }
        public ICommand SaveDocumentAsCommand { get; }
        public ICommand CloseDocumentCommand { get; }

        public DocumentsViewModel()
        {
            NewDocumentCommand = new RelayCommand(NewDocument);
            OpenDocumentCommand = new RelayCommand(OpenDocument);
            SaveDocumentCommand = new RelayCommand(SaveDocument);
            SaveDocumentAsCommand = new RelayCommand(SaveDocumentAs);
            CloseDocumentCommand = new RelayCommand(CloseDocument);
        }

        public TextEditor Editor
        {
            get => _editor;
            set { _editor = value; OnPropertyChanged(); }
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
                var doc = new DocumentModel(Path.GetFileName(openFileDialog.FileName))
                {
                    TextContent = content
                };
                OpenDocuments.Add(doc);
                SelectedDocument = doc;
            }
        }

        private void SaveDocument(object parameter)
        {
            
            if (SelectedDocument == null) return;
            SelectedDocument.Status = false;

            if (string.IsNullOrEmpty(SelectedDocument.FilePath))
            {
                SaveDocumentAs(parameter);
            }
            else
            {
                File.WriteAllText(SelectedDocument.FilePath, SelectedDocument.TextContent);
            }
        }

        private void SaveDocumentAs(object parameter)
        {
            if (SelectedDocument == null) return;
            SelectedDocument.Status = false;

            SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Текстовые файлы|*.txt|Все файлы|*.*" };
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, SelectedDocument.TextContent);
                SelectedDocument.FilePath = saveFileDialog.FileName;
                SelectedDocument.FileName = Path.GetFileName(saveFileDialog.FileName);
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

        public void AddError(int line, int column, string message)
        {
            if (SelectedDocument != null)
            {
                SelectedDocument.AddError(line, column, message);
                OnPropertyChanged(nameof(SelectedErrors));
            }
        }
    }
}