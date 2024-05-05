using System.Reflection.Metadata;
using System.Windows;
using FileEncryptor.Infrastructure.Commands.Base;

namespace FileEncryptor.Infrastructure.Commands
{
    internal class CloseWindowCommand : Command
    {
        protected override bool CanExecute(object parameter) => (parameter as Window ?? App.FocusedWindow ?? App.ActivedWindow) != null;

        protected override void Execute(object parameter) =>
            (parameter as Window ?? App.FocusedWindow ?? App.ActivedWindow)?.Close();
    }
}
