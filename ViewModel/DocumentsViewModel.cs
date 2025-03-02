using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Microsoft.Win32;
using Compiler.Model;
using System.Windows.Documents;

namespace Compiler.ViewModel
{
    /// <summary> ViewModel для работы с документами </summary>
    public class DocumentsViewModel : BaseViewModel
    {
        private DocumentModel _selectedDocument;

        /// <summary> Список открытых документов </summary>
        public ObservableCollection<DocumentModel> OpenDocuments { get; set; } = new ObservableCollection<DocumentModel>();
        
        /// <summary> Текущий документ </summary>
        public DocumentModel SelectedDocument
        {
            get => _selectedDocument;
            set
            {
                _selectedDocument = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedErrors)); // Обновляем ошибки при смене документа
            }
        }

        /// <summary> Список ошибкок в текущем документе </summary>
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

                var doc = new DocumentModel(Path.GetFileName(openFileDialog.FileName)) // Только имя файла
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

            if (string.IsNullOrEmpty(SelectedDocument.FilePath))
            {
                SaveDocumentAs(parameter); // Если у документа нет пути, вызываем "Сохранить как..."
            }
            else
            {
                // Сохраняем файл по известному пути
                TextRange textRange = new TextRange(SelectedDocument.Document.ContentStart, SelectedDocument.Document.ContentEnd);
                File.WriteAllText(SelectedDocument.FilePath, textRange.Text);
            }
        }

        private void SaveDocumentAs(object parameter)
        {
            if (SelectedDocument == null) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Текстовые файлы|*.txt|Все файлы|*.*" };
            if (saveFileDialog.ShowDialog() == true)
            {
                TextRange textRange = new TextRange(SelectedDocument.Document.ContentStart, SelectedDocument.Document.ContentEnd);
                File.WriteAllText(saveFileDialog.FileName, textRange.Text);
                SelectedDocument.FilePath = saveFileDialog.FileName; // Запоминаем путь файла
                SelectedDocument.FileName = Path.GetFileName(saveFileDialog.FileName); // Обновляем название вкладки
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
