using Compiler.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Compiler.ViewModel
{
    // Часть класса для работы с анализатором
    public partial class MainViewModel 
    {

        public ICommand ShowTaskCommand { get; private set; }
        public ICommand ShowGrammarCommand { get; private set; }
        public ICommand ShowGrammarClassificationCommand { get; private set; }
        public ICommand ShowAnalysisMethodCommand { get; private set; }
        public ICommand ShowErrorDiagnosticsCommand { get; private set; }
        public ICommand ShowTestExampleCommand { get; private set; }
        public ICommand ShowLiteratureListCommand { get; private set; }
        public ICommand ShowSourceCodeCommand { get; private set; }
        public ICommand StartExecutionCommand { get; private set; }


        private void InitAnalysisCommand()
        {
            ShowTaskCommand = new RelayCommand(ShowTaskDefinition);
            ShowGrammarCommand = new RelayCommand(ShowGrammar);
            ShowGrammarClassificationCommand = new RelayCommand(ShowGrammarClassification);
            ShowAnalysisMethodCommand = new RelayCommand(ShowAnalysisMethod);
            ShowErrorDiagnosticsCommand = new RelayCommand(ShowErrorDiagnostics);
            ShowTestExampleCommand = new RelayCommand(ShowTestExample);
            ShowLiteratureListCommand = new RelayCommand(ShowLiteratureList);
            ShowSourceCodeCommand = new RelayCommand(ShowSourceCode);
            StartExecutionCommand = new RelayCommand(StartExecution);
        }

        /// <summary> Обработчик события для пункта "Постановка задачи" </summary>
        private void ShowTaskDefinition(object parameter)
        {
            MessageBox.Show("Постановка задачи");
        }

        /// <summary> Обработчик события для пункта "Грамматика" </summary>
        private void ShowGrammar(object parameter)
        {
            MessageBox.Show("Грамматика");
        }

        /// <summary> Обработчик события для классификации грамматики </summary>
        private void ShowGrammarClassification(object parameter)
        {
            MessageBox.Show("Классификация грамматики");
        }

        /// <summary> Обработчик события для метода анализа </summary>
        private void ShowAnalysisMethod(object parameter)
        {
            MessageBox.Show("Метод анализа");
        }

        /// <summary> Обработчик события для диагностики и нейтрализации ошибок </summary>
        private void ShowErrorDiagnostics(object parameter)
        {
            MessageBox.Show("Диагностика и нейтрализация ошибок");
        }

        /// <summary> Обработчик события для тестового примера </summary>
        private void ShowTestExample(object parameter)
        {
            MessageBox.Show("Тестовый пример");
        }

        /// <summary> Обработчик события для списка литературы </summary>
        private void ShowLiteratureList(object parameter)
        {
            MessageBox.Show("Список литературы");
        }

        /// <summary> Обработчик события для исходного кода программы </summary>
        private void ShowSourceCode(object parameter)
        {
            MessageBox.Show("Исходный код программы");
        }

        /// <summary> Обработчик события для запуска программы </summary>
        private void StartExecution(object parameter)
        {
            if (DocumentsVM.SelectedDocument != null)
            {
                Scaner scaner = new Scaner();
                DocumentsVM.SelectedErrors.Clear();
                var result = scaner.Parse(DocumentsVM.SelectedDocument.TextContent);
                foreach (var e in result)
                {
                    DocumentsVM.AddError(e.Line, e.StartColumn, e.ToString());
                }
                OnPropertyChanged(nameof(Errors));
            }
        }
    }
}
