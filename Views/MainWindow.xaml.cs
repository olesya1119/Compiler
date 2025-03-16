using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Compiler.Views;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace Compiler
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Загружаем и регистрируем подсветку Go из ресурса
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Compiler.GoMode.xshd"))
            {
                if (stream != null)
                {
                    using (var reader = new XmlTextReader(stream))
                    {
                        var highlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                        HighlightingManager.Instance.RegisterHighlighting("Go", new[] { ".go" }, highlighting);
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить GoMode.xshd. Проверь путь к ресурсу.");
                }
            }
        }

        private void TextEditor_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextEditor editor && DataContext is ViewModel.MainViewModel vm)
            {
                var doc = vm.DocumentsVM.SelectedDocument;
                if (doc != null)
                {
                    vm.DocumentsVM.Editor = editor;
                    editor.Text = doc.TextContent;
                    editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("Go"); // Используем Go
                }
            }
        }

        private void TextEditor_TextChanged(object sender, System.EventArgs e)
        {
            if (sender is TextEditor editor && DataContext is ViewModel.MainViewModel vm)
            {
                var doc = vm.DocumentsVM.SelectedDocument;
                if (doc != null)
                {
                    doc.TextContent = editor.Text;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is ViewModel.MainViewModel vm && vm.DocumentsVM.SelectedDocument != null && vm.DocumentsVM.SelectedDocument.Status) // если документ изменен
            {
                var confirmWindow = new ConfirmExitWindow();
                confirmWindow.Owner = this; 
                confirmWindow.ShowDialog();

                if (confirmWindow.SaveChanges)
                {
                    vm.SaveDocumentCommand.Execute(null); // Сохраняем документ
                }
                else
                {
                    // Если не нужно сохранять, просто закрываем
                    e.Cancel = false;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModel.MainViewModel vm && vm.DocumentsVM.SelectedDocument != null && vm.DocumentsVM.SelectedDocument.Status) // если документ изменен
            {
                var confirmWindow = new ConfirmExitWindow();
                confirmWindow.Owner = this;
                confirmWindow.ShowDialog();

                if (confirmWindow.SaveChanges)
                {
                    vm.SaveDocumentCommand.Execute(null); // Сохраняем документ
                }
            }

        }
    }

    
}
