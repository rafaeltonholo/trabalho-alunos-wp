using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace CRUDAlunos.ViewModels.Base {
    public interface IViewModel<T> : INotifyPropertyChanged
        where T : class, new() {
        T VMItem { get; set; }
        Page Page { get; set; }
        ObservableCollection<T> Items { get; set; }
        void LoadData();
        void ReloadData();
    }
}
