using System.Windows;
using Compiler.ViewModel;
using ICSharpCode.AvalonEdit;

namespace Compiler
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextEditor_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextEditor editor && DataContext is ViewModel.MainViewModel vm)
            {
                var doc = vm.DocumentsVM.SelectedDocument;
                if (doc != null)
                {
                    doc.Editor = editor; // Связываем редактор с моделью
                }
            }
        }

        private void TextEditor_TextChanged(object sender, System.EventArgs e)
        {
            if (sender is TextEditor editor && DataContext is MainViewModel vm)
            {
                var doc = vm.DocumentsVM.SelectedDocument;
                if (doc != null && editor.Text != doc.TextContent)
                {
                    doc.TextContent = editor.Text; // Обновляем модель
                }
            }
        }
    }
}