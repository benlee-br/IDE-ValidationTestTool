using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace WpfTestApp.ViewModelBase
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void OnPropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> action)
        {
            var expr = (MemberExpression)action.Body;
            OnPropertyChanged(expr.Member.Name);
        }
        protected async Task DispatchAsync(Action action)
        {
            var taskCompletionSource = new TaskCompletionSource<object>();
            var callback = new SendOrPostCallback(delegate (object state)
            {
                try
                {
                    action.Invoke();
                    taskCompletionSource.SetResult(Type.Missing);
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetException(ex);
                }
            });

            await taskCompletionSource.Task;
        }
        protected async Task DispatchAsync(Func<Task> func)
        {
            var taskCompletionSource = new TaskCompletionSource<object>();
            var callback = new SendOrPostCallback(async delegate (object state)
            {
                try
                {
                    await func.Invoke();
                    taskCompletionSource.SetResult(Type.Missing);
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetException(ex);
                }
            });
            //SynchronizationContext.Post(callback, null);
            await taskCompletionSource.Task;
        }
    }
}
