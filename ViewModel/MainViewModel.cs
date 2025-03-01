using Compiler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Compiler.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public DocumentsViewModel DocumentsVM { get; }

        public ICommand NewDocumentCommand => DocumentsVM.NewDocumentCommand;
        public ICommand OpenDocumentCommand => DocumentsVM.OpenDocumentCommand;
        public ICommand SaveDocumentCommand => DocumentsVM.SaveDocumentCommand;
        public ICommand CloseDocumentCommand => DocumentsVM.CloseDocumentCommand;


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

            ExitApplicationCommand = new RelayCommand(ExitApplication);
            UndoCommand = new RelayCommand(UndoAction);
            RedoCommand = new RelayCommand(RedoAction);
            CutCommand = new RelayCommand(CutText);
            CopyCommand = new RelayCommand(CopyText);
            PasteCommand = new RelayCommand(PasteText);
            DeleteCommand = new RelayCommand(DeleteText);
            SelectAllCommand = new RelayCommand(SelectAllText);
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


        /// <summary> Обработчик события для выхода из программы </summary>
        private void ExitApplication(object parameter)
        {
            MessageBox.Show("5 - Выход");
        }

        /// <summary> Обработчик события для отмены действия </summary>
        private void UndoAction(object parameter)
        {
            MessageBox.Show("6 - Отменить");
        }

        /// <summary> Обработчик события для повтора действия </summary>
        private void RedoAction(object parameter)
        {
            MessageBox.Show("7 - Повторить");
        }

        /// <summary> Обработчик события для вырезания текста </summary>
        private void CutText(object parameter)
        {
            MessageBox.Show("8 - Вырезать");
        }

        /// <summary> Обработчик события для копирования текста </summary>
        private void CopyText(object parameter)
        {
            MessageBox.Show("9 - Копировать");
        }

        /// <summary> Обработчик события для вставки текста </summary>
        private void PasteText(object parameter)
        {
            MessageBox.Show("10 - Вставить");
        }

        /// <summary> Обработчик события для удаления текста </summary>
        private void DeleteText(object parameter)
        {
            MessageBox.Show("11 - Удалить");
        }

        /// <summary> Обработчик события для выделения всего текста </summary>
        private void SelectAllText(object parameter)
        {
            MessageBox.Show("12 - Выделить все");
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
