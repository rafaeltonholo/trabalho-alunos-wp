using CRUDAlunos.Aplicacao.ViewObjects;
using CRUDAlunos.Domain.Entities;
using CRUDAlunos.Util;
using CRUDAlunos.ViewModels.Base;
using CRUDAlunos.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Media.Capture;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace CRUDAlunos.ViewModels {
    public class AlunoViewModel : BaseViewModel<AlunoView, Aluno> {

        #region Fields

        private DelegateCommand _viewAddCommand;
        private DelegateCommand _addCommand;
        private DelegateCommand _deleteCommand;
        private DelegateCommand _fotoCommand;
        private AlunoView _currentModel;
        private BitmapImage _alunoBitmap;

        #endregion

        #region Propertieds

        public AlunoView CurrentModel {
            get { return _currentModel; }
            set {
                _currentModel = value;

                if (_currentModel != null) {
                    Page.Frame.Navigate(typeof(AlunoEditView), _currentModel);
                }

                RaisedPropertyChanged(() => CurrentModel);
            }
        }

        public Visibility CanDelete { get { return VMItem.Id == 0 ? Visibility.Collapsed : Visibility.Visible; } }

        public BitmapImage AlunoBitmap {
            get {
                if(_alunoBitmap == null) {
                    AlunoBitmap = ConvertByteArrayToBitMap(VMItem.Foto);
                }

                return _alunoBitmap;
            }
            set {
                _alunoBitmap = value;
                RaisedPropertyChanged(() => AlunoBitmap);
            }
        }

        #endregion

        #region Constructor

        public AlunoViewModel() : base() {
            LoadData();
        }

        public AlunoViewModel(Page page) {
            Page = page;
        }

        #endregion

        #region Methods

        public override void LoadData() {
            Items = new ObservableCollection<AlunoView>(Application.FindAll());
            this.CurrentModel = null;
        }

        public override void ReloadData() {
            this.LoadData();
        }
        #endregion

        #region Commands

        public DelegateCommand ViewAddCommand {
            get {
                return _viewAddCommand ?? (_viewAddCommand = new DelegateCommand(() => {
                    Page.Frame.Navigate(typeof(AlunoEditView));
                }));
            }
        }

        public DelegateCommand AddCommand {
            get {
                return _addCommand ?? (_addCommand = new DelegateCommand(async () => {
                    if (VMItem.Id == 0) {
                        Application.Add(VMItem);
                        MessageDialog dialog = new MessageDialog("Registro cadastrado com sucesso!", "Sucesso");
                        await dialog.ShowAsync();
                    } else {
                        Application.Update(VMItem);
                        MessageDialog dialog = new MessageDialog("Registro atualizado com sucesso!", "Sucesso");
                        await dialog.ShowAsync();
                    }
                    Page.Frame.Navigate(typeof(MainPage));
                }));
            }
        }

        public DelegateCommand DeleteCommand {
            get {
                return _deleteCommand ?? (_deleteCommand = new DelegateCommand(async () => {
                    Application.Remove(VMItem);
                    MessageDialog dialog = new MessageDialog("Registro excluído com sucesso", "Sucesso");
                    await dialog.ShowAsync();
                    Page.Frame.Navigate(typeof(MainPage));
                }));
            }
        }

        public DelegateCommand FotoCommand {
            get {
                return _fotoCommand ?? (_fotoCommand = new DelegateCommand(() => {
                    EventHandler<PhotoTakedEventArgs> photoTakedEvent = new EventHandler<PhotoTakedEventArgs>(AlunoViewModel_PhotoTaked);
                    Page.Frame.Navigate(typeof(CameraPreview), photoTakedEvent);
                }));
            }
        }

        private void AlunoViewModel_PhotoTaked(object sender, PhotoTakedEventArgs e) {
            this.VMItem.Foto = e.ImageSource;

            AlunoBitmap = ConvertByteArrayToBitMap(e.ImageSource);
        }

        private BitmapImage ConvertByteArrayToBitMap(byte[] byteArray) {
            BitmapImage bitmap = null;
            if (byteArray.Length > 0) {
                using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream()) {
                    using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0))) {
                        writer.WriteBytes(byteArray);
                        writer.StoreAsync().GetResults();
                    }

                    bitmap = new BitmapImage();
                    bitmap.SetSource(ms);
                }
            }

            return bitmap;
        }

        #endregion

    }
}
