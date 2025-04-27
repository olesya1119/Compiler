using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.ViewModel
{/*
    using Compiler.Analysis;
    using Compiler.Model;
    using Compiler.Views;
    using global::Compiler.Analysis;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
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
                //TaskWindow Window = new TaskWindow();
                //Window.Show();
                string exeFolder = AppDomain.CurrentDomain.BaseDirectory;
                string wordFilePath = Path.Combine(exeFolder, "Files", "Постановка задачи.docx"); // "Docs" — подпапка

                if (File.Exists(wordFilePath))
                {
                    try
                    {
                        // Открываем файл в Word (или программе по умолчанию)
                        Process.Start(new ProcessStartInfo(wordFilePath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось открыть файл: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Файл не найден!");
                }
            }

            /// <summary> Обработчик события для пункта "Грамматика" </summary>
            private void ShowGrammar(object parameter)
            {
                //GramWindow window = new GramWindow();
                //window.Show();
                string exeFolder = AppDomain.CurrentDomain.BaseDirectory;
                string wordFilePath = Path.Combine(exeFolder, "Files", "Грамматика.docx"); // "Docs" — подпапка

                if (File.Exists(wordFilePath))
                {
                    try
                    {
                        // Открываем файл в Word (или программе по умолчанию)
                        Process.Start(new ProcessStartInfo(wordFilePath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось открыть файл: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Файл не найден!");
                }
            }

            /// <summary> Обработчик события для классификации грамматики </summary>
            private void ShowGrammarClassification(object parameter)
            {
                //GrammarClassificationWindow window = new GrammarClassificationWindow();
                //window.Show();
                string exeFolder = AppDomain.CurrentDomain.BaseDirectory;
                string wordFilePath = Path.Combine(exeFolder, "Files", "Классификация грамматики.docx"); // "Docs" — подпапка

                if (File.Exists(wordFilePath))
                {
                    try
                    {
                        // Открываем файл в Word (или программе по умолчанию)
                        Process.Start(new ProcessStartInfo(wordFilePath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось открыть файл: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Файл не найден!");
                }
            }

            /// <summary> Обработчик события для метода анализа </summary>
            private void ShowAnalysisMethod(object parameter)
            {
                //AnalysisMethod window = new AnalysisMethod();
                //window.Show();
                string exeFolder = AppDomain.CurrentDomain.BaseDirectory;
                string wordFilePath = Path.Combine(exeFolder, "Files", "Метод анализа.docx"); // "Docs" — подпапка

                if (File.Exists(wordFilePath))
                {
                    try
                    {
                        // Открываем файл в Word (или программе по умолчанию)
                        Process.Start(new ProcessStartInfo(wordFilePath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось открыть файл: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Файл не найден!");
                }

            }

            /// <summary> Обработчик события для диагностики и нейтрализации ошибок </summary>
            private void ShowErrorDiagnostics(object parameter)
            {
                //ErrorHandlingWindow window = new ErrorHandlingWindow();
                //window.Show();
                string exeFolder = AppDomain.CurrentDomain.BaseDirectory;
                string wordFilePath = Path.Combine(exeFolder, "Files", "Диагностика и нейтрализация ошибок.docx"); // "Docs" — подпапка

                if (File.Exists(wordFilePath))
                {
                    try
                    {
                        // Открываем файл в Word (или программе по умолчанию)
                        Process.Start(new ProcessStartInfo(wordFilePath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось открыть файл: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Файл не найден!");
                }
            }

            /// <summary> Обработчик события для тестового примера </summary>
            private void ShowTestExample(object parameter)
            {
               
                //if (DocumentsVM.SelectedDocument == null)
                //{
                //    DocumentsVM.NewDocument(1);
                //}

                DocumentsVM.SelectedDocument.TextContent = "func calc(a, b, c int){\r\n\treturn a * (b - c)\r\n};";

                //GoParserExamples window = new GoParserExamples();
                //window.Show();

                // Путь к файлу (относительно папки с программой)
                string exeFolder = AppDomain.CurrentDomain.BaseDirectory;
                string wordFilePath = Path.Combine(exeFolder, "Files", "Тестовые примеры.docx"); // "Docs" — подпапка

                if (File.Exists(wordFilePath))
                {
                    try
                    {
                        // Открываем файл в Word (или программе по умолчанию)
                        Process.Start(new ProcessStartInfo(wordFilePath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось открыть файл: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Файл не найден!");
                }
            }

            /// <summary> Обработчик события для списка литературы </summary>
            private void ShowLiteratureList(object parameter)
            {
                //ReferencesWindow window = new ReferencesWindow();
                //window.Show();

                string exeFolder = AppDomain.CurrentDomain.BaseDirectory;
                string wordFilePath = Path.Combine(exeFolder, "Files", "Список литературы.docx"); // "Docs" — подпапка

                if (File.Exists(wordFilePath))
                {
                    try
                    {
                        // Открываем файл в Word (или программе по умолчанию)
                        Process.Start(new ProcessStartInfo(wordFilePath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось открыть файл: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Файл не найден!");
                }
            }

            /// <summary> Обработчик события для исходного кода программы </summary>
            private void ShowSourceCode(object parameter)
            {

                //MessageBox.Show("Листинг программной части разработанного синтаксического анализатора создания функции языка Go представлен в приложении Б.");
                string exeFolder = AppDomain.CurrentDomain.BaseDirectory;
                string wordFilePath = Path.Combine(exeFolder, "Files", "Листинг программы.docx"); // "Docs" — подпапка

                if (File.Exists(wordFilePath))
                {
                    try
                    {
                        // Открываем файл в Word (или программе по умолчанию)
                        Process.Start(new ProcessStartInfo(wordFilePath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось открыть файл: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Файл не найден!");
                }
            }


            /// <summary> Обработчик события для запуска программы </summary>
            private void StartExecution(object parameter)
            {
                if (DocumentsVM.SelectedDocument != null)
                {
                    Parser parser = new Parser(DocumentsVM.SelectedDocument.TextContent);
                    DocumentsVM.SelectedErrors.Clear();

                    foreach (var e in parser.Parse())
                    {
                        DocumentsVM.AddError(e.Line, e.Column, e.Message);

                    }
                    OnPropertyChanged(nameof(Errors));
                }
            }
        }
    }*/

}
