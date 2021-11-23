using System;
using System.Windows.Input;

namespace WpfTestApp.ViewModelBase
{

    public class DelegateParameterCommand : ICommand
    {
        #region test  
        //void DelegateMethod<T>(Action<T> action, int count, T paramater)
        //{
        //    for (int i = 0; i < count; i++)
        //    {
        //        action(paramater);
        //    }
        //}

        //void DelegateMethod<T>(Action<T> action, T paramater)
        //{
        //    int count = 5;
        //    for (int i = 0; i < count; i++)
        //    {
        //        action(paramater);
        //    }
        //}

        //private readonly Action<T> _action ;

        //public DelegateCommand(Action<T> action)
        //{
        //    _action = action;
        //}

        #endregion
        private readonly Action<object> _action;

        public DelegateParameterCommand(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

#pragma warning disable 67
        public event EventHandler CanExecuteChanged { add { } remove { } }
#pragma warning restore 67
    }


    public class DelegateCommand : ICommand
    {

        private readonly Action _action;

        public DelegateCommand(Action action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
