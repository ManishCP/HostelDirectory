using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HostelDirectoryMvvM.Commands
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action DoWork; //Delegate

        public RelayCommand(Action work)
        {
            DoWork = work;
            
        }

        public bool CanExecute(object parameter)
        {
            return true;  //if true user will be able to interact if not it will be disabled
        }

        public void Execute(object parameter)
        {
            DoWork();
        }
    }
}
