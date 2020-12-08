using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MvvmAsyncCommands.Commands
{
    internal class RelayCommandAsync : BaseCommand
    {
        public bool isExecuting { get; private set; }
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
        private Func<object, Task> execute;
        private Func<object, bool> canExecute;

        public bool IsCancellationRequested
        {
            get => cancellationToken.IsCancellationRequested;
        }

        public override bool CanExecute(object parameter)
        {
            return !isExecuting && (canExecute?.Invoke(parameter) ?? true);
        }

        public async override void Execute(object parameter)
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            if (cancellationToken.IsCancellationRequested)
            {
                isExecuting = false;
                CommandManager.InvalidateRequerySuggested();
                return;
            }

            isExecuting = true;
            await execute(parameter);

            isExecuting = false;
            CommandManager.InvalidateRequerySuggested();
        }

        public void StopExecute()
        {
            cancellationTokenSource.Cancel();
        }

        public RelayCommandAsync(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }
    }
}
