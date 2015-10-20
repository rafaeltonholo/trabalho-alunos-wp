using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace CRUDAlunos.Aplicacao.ViewObjects {
    public class BaseView : INotifyPropertyChanged { 
        public int Id { get; set; }

        #region Constructor

        public BaseView() {
            PropertyChanged += (s, e) => { };
        }

        #endregion

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisedPropertyChanged<T>(Expression<Func<T>> expression) {
            var member = expression.Body as MemberExpression;
            var pInfo = member.Member as PropertyInfo;

            if (pInfo != null)
                PropertyChanged(this, new PropertyChangedEventArgs(pInfo.Name));
        }

        #endregion
    }
}
