using CRUDAlunos.Aplicacao.Interfaces.Base;
using CRUDAlunos.Domain.Entities.Base;
using CRUDAlunos.Domain.Ioc;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Windows.UI.Xaml.Controls;

namespace CRUDAlunos.ViewModels.Base {
    public class BaseViewModel<T, Y> : IViewModel<T>
        where T : class, new()
        where Y : BaseEntity {

        #region Fields

        private T _viewModelItem;
        private ObservableCollection<T> _items;
        private Page _page;

        #endregion

        #region Properties

        public T VMItem {
            get { return _viewModelItem; }
            set {
                _viewModelItem = value;
                RaisedPropertyChanged(() => VMItem);
            }
        }

        public Page Page {
            get {
                return this._page;
            }
            set {
                this._page = value;
            }
        }

        public IBaseApplication<T, Y> Application { get; private set; }

        public ObservableCollection<T> Items {
            get { return _items; }
            set {
                _items = value;
                RaisedPropertyChanged(() => Items);
            }
        }

        #endregion

        #region Constructor

        public BaseViewModel() {
            RegisterEvents();
            Items = new ObservableCollection<T>();
            VMItem = new T();
            Application = DependencyCore.Instance.GetInstance<IBaseApplication<T, Y>>();
        }

        #endregion


        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        public void RegisterEvents() {
            PropertyChanged += (sender, e) => { };
        }

        public virtual void LoadData() { }

        public virtual void ReloadData() { }

        protected void RaisedPropertyChanged<Type>(Expression<Func<Type>> expression) {
            var member = expression.Body as MemberExpression;
            var pInfo = member.Member as PropertyInfo;

            if (pInfo != null)
                PropertyChanged(this, new PropertyChangedEventArgs(pInfo.Name));
        }

        #endregion
    }
}