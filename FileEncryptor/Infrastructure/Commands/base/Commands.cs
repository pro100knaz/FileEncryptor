using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileEncryptor.Infrastructure.Commands.Base
{
    public abstract class Command : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }


        private bool _Executeable = true;

        public bool Executeable
        {
            get => _Executeable;
            set
            {
                if (_Executeable == value) return;
                _Executeable = value;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        bool ICommand.CanExecute(object parameter) => _Executeable && CanExecute(parameter);

        void ICommand.Execute(object parameter)
        {
            if(CanExecute(parameter))
                Execute(parameter);
        }

        protected virtual bool CanExecute(object parameter) => true;

        protected abstract void Execute(object parameter);

    }
}
