using Compiler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;


namespace Compiler.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        /* Команды работы с текстом которые полностью реализовать в DocumentsVM*/
        public DocumentsViewModel DocumentsVM { get; private set; }  
        public ICommand NewDocumentCommand { get; }
        public ICommand OpenDocumentCommand { get; }
        public ICommand SaveDocumentCommand { get; }
        public ICommand CloseDocumentCommand { get; }
        public ICommand SaveDocumentAsCommand { get; }

        public ICommand ExitApplicationCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
        public ICommand CutCommand { get; }
        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SelectAllCommand { get; }

        public ICommand ShowTaskCommand { get; }
        public ICommand ShowGrammarCommand { get; }
        public ICommand ShowGrammarClassificationCommand { get; }
        public ICommand ShowAnalysisMethodCommand { get; }
        public ICommand ShowErrorDiagnosticsCommand { get; }
        public ICommand ShowTestExampleCommand { get; }
        public ICommand ShowLiteratureListCommand { get; }
        public ICommand ShowSourceCodeCommand { get; }
        public ICommand StartExecutionCommand { get; }
        public ICommand ShowHelpCommand { get; }
        public ICommand ShowAboutCommand { get; }

        public MainViewModel()
        {
            DocumentsVM = new DocumentsViewModel();
            NewDocumentCommand = DocumentsVM.NewDocumentCommand;
            OpenDocumentCommand = DocumentsVM.OpenDocumentCommand;
            SaveDocumentCommand = DocumentsVM.SaveDocumentCommand;
            SaveDocumentAsCommand = DocumentsVM.SaveDocumentAsCommand;
            CloseDocumentCommand = DocumentsVM.CloseDocumentCommand;

            ExitApplicationCommand = new RelayCommand(ExitApplication);
            UndoCommand = new RelayCommand(ExecuteUndo);
            RedoCommand = new RelayCommand(ExecuteRedo);
            CutCommand = new RelayCommand(ExecuteCut);
            CopyCommand = new RelayCommand(ExecuteCopy);
            PasteCommand = new RelayCommand(ExecutePaste);
            DeleteCommand = new RelayCommand(ExecuteDelete);
            SelectAllCommand = new RelayCommand(ExecuteSelectAll);


            ShowTaskCommand = new RelayCommand(ShowTaskDefinition);
            ShowGrammarCommand = new RelayCommand(ShowGrammar);
            ShowGrammarClassificationCommand = new RelayCommand(ShowGrammarClassification);
            ShowAnalysisMethodCommand = new RelayCommand(ShowAnalysisMethod);
            ShowErrorDiagnosticsCommand = new RelayCommand(ShowErrorDiagnostics);
            ShowTestExampleCommand = new RelayCommand(ShowTestExample);
            ShowLiteratureListCommand = new RelayCommand(ShowLiteratureList);
            ShowSourceCodeCommand = new RelayCommand(ShowSourceCode);
            StartExecutionCommand = new RelayCommand(StartExecution);
            ShowHelpCommand = new RelayCommand(ShowHelp);
            ShowAboutCommand = new RelayCommand(ShowAbout);


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


        private RichTextBox SelectedRichTextBox() => DocumentsVM.SelectedDocument?.Editor;

        /// <summary> Обработчик события для выхода из программы </summary>
        private void ExitApplication(object parameter)
        {
            Application.Current.Shutdown();
        }

        

        /// <summary> Обработчик события для пункта "Постановка задачи" </summary>
        private void ShowTaskDefinition(object parameter)
        {
            MessageBox.Show("13 - Постановка задачи");
        }

        /// <summary> Обработчик события для пункта "Грамматика" </summary>
        private void ShowGrammar(object parameter)
        {
            MessageBox.Show("14 - Грамматика");
        }

        /// <summary> Обработчик события для классификации грамматики </summary>
        private void ShowGrammarClassification(object parameter)
        {
            MessageBox.Show("15 - Классификация грамматики");
        }

        /// <summary> Обработчик события для метода анализа </summary>
        private void ShowAnalysisMethod(object parameter)
        {
            MessageBox.Show("16 - Метод анализа");
        }

        /// <summary> Обработчик события для диагностики и нейтрализации ошибок </summary>
        private void ShowErrorDiagnostics(object parameter)
        {
            MessageBox.Show("17 - Диагностика и нейтрализация ошибок");
        }

        /// <summary> Обработчик события для тестового примера </summary>
        private void ShowTestExample(object parameter)
        {
            MessageBox.Show("18 - Тестовый пример");
        }

        /// <summary> Обработчик события для списка литературы </summary>
        private void ShowLiteratureList(object parameter)
        {
            MessageBox.Show("19 - Список литературы");
        }

        /// <summary> Обработчик события для исходного кода программы </summary>
        private void ShowSourceCode(object parameter)
        {
            MessageBox.Show("20 - Исходный код программы");
        }

        /// <summary> Обработчик события для запуска программы </summary>
        private void StartExecution(object parameter)
        {
            MessageBox.Show("21 - Пуск");
        }

        /// <summary> Обработчик события для вызова справки </summary>
        private void ShowHelp(object parameter)
        {
            MessageBox.Show("22 - Вызов справки");
        }

        /// <summary> Обработчик события для информации о программе </summary>
        private void ShowAbout(object parameter)
        {
            MessageBox.Show("23 - О программе");
        }

    }
}
