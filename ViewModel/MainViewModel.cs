using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Compiler.Model;
using Compiler.Views;
using ICSharpCode.AvalonEdit;

namespace Compiler.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        public DocumentsViewModel DocumentsVM { get; private set; }
        private string _callStack;

        public ObservableCollection<ErrorModel> Errors => DocumentsVM.SelectedErrors;
        public ObservableCollection<ErrorModel> Lexems => DocumentsVM.SelectedLexems;
        public string CallStack
        {
            get => _callStack;
            set { _callStack = value; OnPropertyChanged(); }
        }

        private TextEditor SelectedTextEditor() => DocumentsVM.Editor;

        public MainViewModel()
        {
            DocumentsVM = new DocumentsViewModel();

            DocumentsVM.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(DocumentsVM.SelectedDocument))
                {
                    OnPropertyChanged(nameof(Errors));
                    OnPropertyChanged(nameof(Lexems));
                }
            };

            InitAnalysisCommand();
            InitShortcutCommand();
            InitTextEditingCommand();
            InitInfoCommand();
        }

        

    }
}