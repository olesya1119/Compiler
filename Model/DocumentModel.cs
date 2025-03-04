using ICSharpCode.AvalonEdit;
using System.Collections.ObjectModel;
using System.IO;

namespace Compiler.Model
{
    /// <summary> Модель, представляющая документ </summary>
    public class DocumentModel : BaseModel
    {
        private FileInfo _fileInfo;  // Используем FileInfo для работы с файлом
        private string _textContent;
        private TextEditor _editor;
        private ObservableCollection<ErrorModel> _errors = new ObservableCollection<ErrorModel>();

        /// <summary> Информация о файле </summary>
        public FileInfo FileInfo
        {
            get => _fileInfo;
            set { _fileInfo = value; OnPropertyChanged(); }
        }

        /// <summary> Название файла </summary>
        public string FileName
        {
            get => _fileInfo?.Name;
            set
            {
                if (_fileInfo != null && _fileInfo.Name != value)
                {
                    // Обновляем имя файла в _fileInfo
                    _fileInfo = new FileInfo(Path.Combine(_fileInfo.DirectoryName, value));
                    OnPropertyChanged();
                }
            }
        }

        /// <summary> Путь к файлу </summary>
        public string FilePath
        {
            get => _fileInfo?.FullName;
            set
            {
                if (_fileInfo != null && _fileInfo.FullName != value)
                {
                    // Обновляем путь в _fileInfo
                    _fileInfo = new FileInfo(value);
                    OnPropertyChanged();
                }
            }
        }

        /// <summary> Внутреннее содержание документа </summary>
        public string TextContent
        {
            get => _textContent;
            set
            {
                if (_textContent != value)
                {
                    _textContent = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary> AvalonEdit TextEditor для документа </summary>
        public TextEditor Editor
        {
            get => _editor;
            set
            {
                _editor = value;
                OnPropertyChanged();
                if (_editor != null)
                {
                    _editor.TextChanged += (s, e) => TextContent = _editor.Text;
                }
            }
        }

        /// <summary> Список ошибок в документе </summary>
        public ObservableCollection<ErrorModel> Errors
        {
            get => _errors;
            set { _errors = value; OnPropertyChanged(); }
        }

        /// <summary> Добавить ошибку в документ </summary>
        public void AddError(int line, int column, string message)
        {
            Errors.Add(new ErrorModel(Errors.Count + 1, FileName, line, column, message));
            OnPropertyChanged(nameof(Errors));
        }

        /// <summary> Конструктор модели документа </summary>
        public DocumentModel(string filePath)
        {
            FileInfo = new FileInfo(filePath);
        }

        /// <summary> Загрузка текста из файла </summary>
        public void LoadTextFromFile()
        {
            if (FileInfo.Exists)
            {
                TextContent = File.ReadAllText(FileInfo.FullName);
            }
        }

        /// <summary> Сохранение текста в файл </summary>
        public void SaveTextToFile()
        {
            if (FileInfo.Exists)
            {
                File.WriteAllText(FileInfo.FullName, TextContent);
            }
        }
    }
}
