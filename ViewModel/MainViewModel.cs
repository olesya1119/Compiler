using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Compiler.Model;
using ICSharpCode.AvalonEdit;

namespace Compiler.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        public DocumentsViewModel DocumentsVM { get; private set; }

        public ObservableCollection<ErrorModel> Errors => DocumentsVM.SelectedErrors;

        private TextEditor SelectedTextEditor() => DocumentsVM.SelectedDocument?.Editor;

        public MainViewModel()
        {
            DocumentsVM = new DocumentsViewModel();

            DocumentsVM.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(DocumentsVM.SelectedDocument))
                {
                    OnPropertyChanged(nameof(Errors));
                }
            };

            InitAnalysisCommand();
            InitShortcutCommand();
            InitTextEditingCommand();
            InitInfoCommand();
        }

    }
}