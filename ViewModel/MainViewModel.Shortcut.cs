using System.Windows.Input;
using ICSharpCode.AvalonEdit;

namespace Compiler.ViewModel
{
    public partial class MainViewModel
    {
        public ICommand ExitApplicationCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand CutCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public ICommand PasteCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SelectAllCommand { get; private set; }

        private void InitShortcutCommand()
        {
            ExitApplicationCommand = new RelayCommand(ExitApplication);
            UndoCommand = new RelayCommand(ExecuteUndo);
            RedoCommand = new RelayCommand(ExecuteRedo);
            CutCommand = new RelayCommand(ExecuteCut);
            CopyCommand = new RelayCommand(ExecuteCopy);
            PasteCommand = new RelayCommand(ExecutePaste);
            DeleteCommand = new RelayCommand(ExecuteDelete);
            SelectAllCommand = new RelayCommand(ExecuteSelectAll);
        }

        private void ExecuteUndo(object parameter)
        {
            var editor = SelectedTextEditor();
            if (editor != null && editor.CanUndo)
            {
                editor.Undo();
            }
        }

        private void ExecuteRedo(object parameter)
        {
            var editor = SelectedTextEditor();
            if (editor != null && editor.CanRedo)
            {
                editor.Redo();
            }
        }

        private void ExecuteCut(object parameter)
        {
            var editor = SelectedTextEditor();
            if (editor != null && editor.SelectionLength > 0)
            {
                editor.Cut();
            }
        }

        private void ExecuteCopy(object parameter)
        {
            var editor = SelectedTextEditor();
            if (editor != null && editor.SelectionLength > 0)
            {
                editor.Copy();
            }
        }

        private void ExecutePaste(object parameter)
        {
            var editor = SelectedTextEditor();
            if (editor != null)
            {
                editor.Paste();
            }
        }

        private void ExecuteDelete(object parameter)
        {
            var editor = SelectedTextEditor();
            if (editor != null && editor.SelectionLength > 0)
            {
                editor.SelectedText = ""; // Удаляем выделенный текст
            }
        }

        private void ExecuteSelectAll(object parameter)
        {
            var editor = SelectedTextEditor();
            if (editor != null)
            {
                editor.SelectAll();
            }
        }

        /// <summary> Обработчик события для выхода из программы </summary>
        private void ExitApplication(object parameter)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}