#pragma warning disable CS0067

using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace WT.UI.DetailMessage.ViewModel
{
    internal class DetailMessageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Title { get; set; }

        public string Message { get; set; }

        public string Detail { get; set; }

        public ICommand OkCmd => new RelayCommand<Window>(p => p.Close());
    }
}
