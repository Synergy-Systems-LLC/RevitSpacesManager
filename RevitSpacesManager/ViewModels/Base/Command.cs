using RevitSpacesManager.Models;
using System;
using System.Windows.Input;

namespace RevitSpacesManager.ViewModels
{
    internal abstract class Command : ICommand
    {
        public abstract IModel Model { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);
    }
}
