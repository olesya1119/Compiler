using Compiler.Model;
using Compiler.ViewModel;
using ICSharpCode.AvalonEdit;
using System.Windows;
using System.Windows.Controls;

namespace Compiler
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            // Этот метод сработает, когда TabItem будет загружен
            if (sender is ContentPresenter contentPresenter)
            {
                if (contentPresenter.Content is DocumentModel doc)
                {
                    // Проверяем, не создан ли уже TextEditor
                    if (doc.Editor == null)
                    {
                        // Динамически создаем новый TextEditor
                        var textEditor = new TextEditor
                        {
                            FontFamily = new System.Windows.Media.FontFamily("Consolas"),
                            FontSize = 14,
                            ShowLineNumbers = true,
                            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
                        };

                        // Привязываем TextEditor к модели
                        doc.Editor = textEditor;

                        // Синхронизируем начальный текст с редактором
                        textEditor.Text = doc.TextContent;

                        // Добавляем TextEditor в контейнер, который находится в ContentTemplate
                        var editorContainer = contentPresenter.FindName("EditorContainer") as Grid;
                        if (editorContainer != null)
                        {
                            editorContainer.Children.Clear();
                            editorContainer.Children.Add(textEditor);
                        }

                        // Привязываем событие TextChanged, чтобы синхронизировать изменения
                        textEditor.TextChanged += (s, args) =>
                        {
                            doc.TextContent = textEditor.Text;
                        };
                    }
                }
            }
        }
    }
}
