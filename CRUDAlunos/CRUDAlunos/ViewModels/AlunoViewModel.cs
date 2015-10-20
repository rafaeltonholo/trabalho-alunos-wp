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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CRUDAlunos.ViewModels {
    public class AlunoViewModel : BaseViewModel<AlunoView, Aluno> {

        #region Fields

        private DelegateCommand _viewAddCommand;
        private DelegateCommand _addCommand;
        private DelegateCommand _deleteCommand;
        private DelegateCommand _fotoCommand;
        private AlunoView _currentModel;
        private MediaCapture _cameraChooser;

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

        public async void CameraCapturePhoto() {
            //CameraCaptureUI
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
                return _addCommand ?? (_addCommand = new DelegateCommand(() => {
                    if (VMItem.Id == 0) {
                        Application.Add(VMItem);
                        MessageDialog dialog = new MessageDialog("Registro cadastrado com sucesso!", "Sucesso");
                        dialog.ShowAsync();
                    } else {
                        Application.Update(VMItem);
                        MessageDialog dialog = new MessageDialog("Registro atualizado com sucesso!", "Sucesso");
                        dialog.ShowAsync();
                    }
                    Page.Frame.Navigate(typeof(MainPage));
                }));
            }
        }

        public DelegateCommand DeleteCommand {
            get {
                return _deleteCommand ?? (_deleteCommand = new DelegateCommand(() => {
                    Application.Remove(VMItem);
                    //Navigation.Item = null;
                    MessageDialog dialog = new MessageDialog("Registro excluído com sucesso", "Sucesso");
                    dialog.ShowAsync();
                    Page.Frame.Navigate(typeof(MainPage));
                }));
            }
        }

        public DelegateCommand FotoCommand {
            get {
                return _fotoCommand ?? (_fotoCommand = new DelegateCommand(() => {
                    MessageDialog dialog = new MessageDialog("Foto", "Sucesso");
                    dialog.ShowAsync();
                }));
            }
        }

        #endregion

    }
}
