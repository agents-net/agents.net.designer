#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Windows.Input;

namespace Agents.Net.Designer.ViewModel
{
    public class RelayCommand : ICommand    
    {    
        private readonly Action<object> execute;    
        private readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged;
     
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)    
        {    
            this.execute = execute;    
            this.canExecute = canExecute;    
        }    
     
        public bool CanExecute(object parameter)    
        {    
            return canExecute == null || canExecute(parameter);    
        }    
     
        public void Execute(object parameter)    
        {    
            execute(parameter);    
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }  
}
