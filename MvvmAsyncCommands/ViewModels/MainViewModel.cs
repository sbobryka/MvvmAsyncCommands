using MvvmAsyncCommands.Commands;
using MvvmAsyncCommands.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MvvmAsyncCommands.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        Operations operations;

        #region CurrentValue

        private double currentValue;

        public double CurrentValue
        {
            get => currentValue;
            set => Set(ref currentValue, value);
        }

        #endregion

        #region MaxValue

        private double maxValue;

        public double MaxValue
        {
            get => maxValue;
            set => Set(ref maxValue, value);
        }

        #endregion

        #region StartCommandAsync

        public RelayCommandAsync StartCommandAsync { get; }

        private async Task OnStartCommandAsyncExecuted(object property)
        {
            for (double i = CurrentValue; i < MaxValue; i++)
            {
                if (StartCommandAsync.IsCancellationRequested) return;

                await Task.Run(() => operations.SomeOperation());

                CurrentValue = i;
            }
        }

        private bool CanStartCommandAsyncExecute(object property)
        {
            return CurrentValue < MaxValue ? true : false;
        }

        #endregion

        #region CancelCommand

        public ICommand CancelCommand { get; }

        private void OnCancelCommandExecuted(object property)
        {
            StartCommandAsync.StopExecute();
        }

        private bool CanCancelCommandExecute(object property)
        {
            return StartCommandAsync.isExecuting;
        }

        #endregion

        public MainViewModel()
        {
            CurrentValue = 0;
            MaxValue = 1000;

            StartCommandAsync = new RelayCommandAsync(OnStartCommandAsyncExecuted, CanStartCommandAsyncExecute);
            CancelCommand = new RelayCommand(OnCancelCommandExecuted, CanCancelCommandExecute);

            operations = new Operations();
        }
    }
}
