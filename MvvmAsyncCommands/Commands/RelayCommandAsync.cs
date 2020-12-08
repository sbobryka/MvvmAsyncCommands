using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MvvmAsyncCommands.Commands
{
    internal class RelayCommandAsync : BaseCommand
    {
        private bool isExecuting;
        private Func<object, Task> execute;
        private Func<object, bool> canExecute;

        public override bool CanExecute(object parameter)
        {
            return !isExecuting && (canExecute?.Invoke(parameter) ?? true);
        }

        public async override void Execute(object parameter)
        {
            isExecuting = true;
            await execute(parameter);
            isExecuting = false;
            CommandManager.InvalidateRequerySuggested();
        }

        public RelayCommandAsync(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }
    }
}
