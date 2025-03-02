using Compiler.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;


namespace Compiler.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {

        ///<summary> ViewModel для работы с документами </summary>
        public DocumentsViewModel DocumentsVM { get; private set; }

        ///<summary> Список ошибок в текщем документе </summary>
        public ObservableCollection<ErrorModel> Errors => DocumentsVM.SelectedErrors;

        ///<summary> Выбранный RichTextBox </summary>
        private RichTextBox SelectedRichTextBox() => DocumentsVM.SelectedDocument?.Editor;

        public MainViewModel()
        {
            DocumentsVM = new DocumentsViewModel();

            // Подписываемся на изменение выбранного документа (обновляем список ошибок)
            DocumentsVM.PropertyChanged += (object sender, PropertyChangedEventArgs e) => {
                if (e.PropertyName == nameof(DocumentsVM.SelectedDocument))
                {
                    OnPropertyChanged(nameof(Errors)); // Теперь DataGrid обновится
                }
            };

            //Инцилизируем все команды
            InitAnalysisCommand();
            InitShortcutCommand();
            InitTextEditingCommand();
            InitInfoCommand();
        }

        
    }
}
