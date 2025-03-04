using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Compiler.Model;

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

        /// <summary> Список ошибок в текущем документе </summary>
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
            var newDoc = new DocumentModel("Новый документ.txt");
            OpenDocuments.Add(newDoc);
            SelectedDocument = newDoc;

            // Очистка текста
            SelectedDocument.TextContent = string.Empty;

            if (SelectedDocument.Editor != null)
            {
                SelectedDocument.Editor.Text = string.Empty;
            }
        }

        private void OpenDocument(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Текстовые файлы|*.txt|Все файлы|*.*" };
            if (openFileDialog.ShowDialog() == true)
            {
                var doc = new DocumentModel(openFileDialog.FileName);
                doc.LoadTextFromFile(); // Загружаем содержимое файла в модель
                OpenDocuments.Add(doc);
                SelectedDocument = doc;

                if (SelectedDocument.Editor != null)
                {
                    SelectedDocument.Editor.Text = doc.TextContent; // Синхронизируем текст редактора с моделью
                }
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
                SelectedDocument.SaveTextToFile(); // Сохраняем текст
            }
        }

        private void SaveDocumentAs(object parameter)
        {
            if (SelectedDocument == null) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Текстовые файлы|*.txt|Все файлы|*.*" };
            if (saveFileDialog.ShowDialog() == true)
            {
                SelectedDocument.FileInfo = new FileInfo(saveFileDialog.FileName);
                SelectedDocument.SaveTextToFile(); // Сохраняем текст
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
