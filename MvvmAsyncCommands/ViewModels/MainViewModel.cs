using MvvmAsyncCommands.Commands;
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
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

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

        public ICommand StartCommandAsync { get; }

        private async Task OnStartCommandAsyncExecuted(object property)
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            for (int i = 0; i < 100; i++)
            {
                if (cancellationToken.IsCancellationRequested) return;

                CurrentValue = await Task.Run(() => IncreaseValue(CurrentValue));
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
            cancellationTokenSource.Cancel();
        } 

        #endregion

        #region IncreaseValue

        private double IncreaseValue(double value)
        {
            Thread.Sleep(100);
            return ++value;
        }

        #endregion

        public MainViewModel()
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            CurrentValue = 0;
            MaxValue = 1000;

            StartCommandAsync = new RelayCommandAsync(OnStartCommandAsyncExecuted, cancellationToken, CanStartCommandAsyncExecute);
            CancelCommand = new RelayCommand(OnCancelCommandExecuted);
        }
    }
}
