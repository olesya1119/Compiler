using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Compiler.ViewModel
{

    // Часть класса для обработки команд горячих клавиш
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


        private void InitShortcutCommand() {
            ExitApplicationCommand = new RelayCommand(ExitApplication);
            UndoCommand = new RelayCommand(ExecuteUndo);
            RedoCommand = new RelayCommand(ExecuteRedo);
            CutCommand = new RelayCommand(ExecuteCut);
            CopyCommand = new RelayCommand(ExecuteCopy);
            PasteCommand = new RelayCommand(ExecutePaste);
            DeleteCommand = new RelayCommand(ExecuteDelete);
            SelectAllCommand = new RelayCommand(ExecuteSelectAll);
        }

        private void ExecuteUndo(object parameter) => SelectedRichTextBox()?.Undo();
        private void ExecuteRedo(object parameter) => SelectedRichTextBox()?.Redo();
        private void ExecuteCut(object parameter) => SelectedRichTextBox()?.Cut();
        private void ExecuteCopy(object parameter) => SelectedRichTextBox()?.Copy();
        private void ExecutePaste(object parameter) => SelectedRichTextBox()?.Paste();
        private void ExecuteSelectAll(object parameter) => SelectedRichTextBox()?.SelectAll();

        private void ExecuteDelete(object parameter)
        {
            var richTextBox = SelectedRichTextBox();
            if (richTextBox != null)
            {
                EditingCommands.Delete.Execute(null, richTextBox);
            }
        }

        /// <summary> Обработчик события для выхода из программы </summary>
        private void ExitApplication(object parameter) => Application.Current.Shutdown();

    }
}
