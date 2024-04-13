using System.Windows.Input;

namespace CupSystem.Helper
{
    public class RelayCommand(Action _execute, Func<bool>? _canExecute = null) : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter = null)
            => _canExecute is null || _canExecute();

        public void Execute(object? parameter = null)
            => _execute();
    }

    public class RelayCommand<T>(Action<T> _execute, Func<T, bool>? _canExecute = null) : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) 
            => _canExecute is null || _canExecute((T)parameter!);

        public void Execute(object? parameter)
            => _execute((T)parameter!);
    }
}
