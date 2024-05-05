using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileEncryptor.Infrastructure.Command
{
    internal class LambdaCommand : Command
    {
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;



        public LambdaCommand(Action Execute, Func<bool> CanExecute = null) 
            : this(p=> Execute(), CanExecute is null ? (Func<object,bool>)null : p=> CanExecute())
        {
            
        }

        public LambdaCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            _CanExecute = CanExecute;
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
        }
        protected override bool CanExecute(object? parameter)
        {
            return _CanExecute?.Invoke(parameter) ?? true;
        }
        protected override void Execute(object? parameter)
        {
            _Execute(parameter);
        }
    }
}
