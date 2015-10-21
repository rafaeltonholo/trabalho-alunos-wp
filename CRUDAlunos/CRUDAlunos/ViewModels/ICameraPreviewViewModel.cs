using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CRUDAlunos.ViewModels {
    public interface ICameraPreviewViewModel : INotifyPropertyChanged {
        CaptureElement CaptureElement { get; set; }
        void RaisedPropertyChanged<Type>(Expression<Func<Type>> expression);
        event EventHandler<PhotoTakedEventArgs> PhotoTaked;
    }

    public class PhotoTakedEventArgs : EventArgs {

        public byte[] ImageSource { get; private set; }

        public PhotoTakedEventArgs(byte[] imageSource) {
            ImageSource = imageSource;
        }
    }
}