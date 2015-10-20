using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace CRUDAlunos.Util {
    public class DelegateCommand : ICommand {
        private readonly Action _action;
        public event System.EventHandler CanExecuteChanged;

        public DelegateCommand(Action action) {
            _action = action;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            if (parameter == null) {
                _action();
            }
        }

    }
}
