using HostelDirectoryMvvM.Commands;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace HostelDirectoryMvvM.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Message Property

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(nameof(Message)); }
        }

        #endregion

        #region RelayCommand Implementation

        protected RelayCommand CreateCommand(Action execute)
        {
            return new RelayCommand(execute);
        }

        #endregion

        #region Messenger Handling

        protected void SubscribeToMessenger<TMessage>(Action<TMessage> action)
        {
            Messenger.Subscribe(action);
        }

        protected void PublishMessage<TMessage>(TMessage message)
        {
            Messenger.Publish(message);
        }

        #endregion

    }
}
