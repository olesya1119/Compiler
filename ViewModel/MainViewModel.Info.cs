using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Compiler.ViewModel
{
    // Часть класса для работы с информацией (помощь, о программе и т.д.)

    
    public partial class MainViewModel 
    {
        public ICommand ShowHelpCommand { get; private set; }
        public ICommand ShowAboutCommand { get; private set; }

        private void InitInfoCommand()
        {
            ShowHelpCommand = new RelayCommand(ShowHelp);
            ShowAboutCommand = new RelayCommand(ShowAbout);
        }
        
        /// <summary> Обработчик события для вызова справки </summary>
        private void ShowHelp(object parameter)
        {
            MessageBox.Show("Вызов справки");
        }

        /// <summary> Обработчик события для информации о программе </summary>
        private void ShowAbout(object parameter)
        {
            MessageBox.Show("О программе");
        }
    }


}
