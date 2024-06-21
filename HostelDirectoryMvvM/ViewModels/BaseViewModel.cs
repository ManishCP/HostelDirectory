using HostelDirectoryMvvM.Commands;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace HostelDirectoryMvvM.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private readonly Messenger _messenger = Messenger.Instance;

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Message Property

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(nameof(Message)); }
        }

        #endregion

        #region RelayCommand Implementation

        protected RelayCommand CreateCommand(Action execute)
        {
            return new RelayCommand(execute);
        }

        protected RelayCommand CreateCommand(Action<object> execute)
        {
            return new RelayCommand(execute);
        }

        #endregion

        #region Messenger Handling

        protected void SubscribeToMessenger<TMessage>(Action<TMessage> action)
        {
            _messenger.Subscribe(action);
        }

        protected void PublishMessage<TMessage>(TMessage message)
        {
            _messenger.Publish(message);
        }

        #endregion
    }
}
