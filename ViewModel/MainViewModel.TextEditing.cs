using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Compiler.ViewModel
{
    // Часть класса для работы с документом
    public partial class MainViewModel
    {
        public ICommand NewDocumentCommand { get; private set; }
        public ICommand OpenDocumentCommand { get; private set; }
        public ICommand SaveDocumentCommand { get; private set; }
        public ICommand CloseDocumentCommand { get; private set; }
        public ICommand SaveDocumentAsCommand { get; private set; }

        private void InitTextEditingCommand()
        {
            NewDocumentCommand = DocumentsVM.NewDocumentCommand;
            OpenDocumentCommand = DocumentsVM.OpenDocumentCommand;
            SaveDocumentCommand = DocumentsVM.SaveDocumentCommand;
            SaveDocumentAsCommand = DocumentsVM.SaveDocumentAsCommand;
            CloseDocumentCommand = DocumentsVM.CloseDocumentCommand;
        }

    }
}
