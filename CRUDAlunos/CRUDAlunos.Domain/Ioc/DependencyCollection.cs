using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CRUDAlunos.Ioc {
    public class DependencyCollection {
        #region Fields

        private List<DependencyItem> _items;

        #endregion

        #region Properties

        public DependencyItem this[Type type] {
            get {
                return _items.FirstOrDefault(c => c.Interface == type);
            }
        }

        public ReadOnlyCollection<DependencyItem> Items {
            get {
                return new ReadOnlyCollection<DependencyItem>(_items);
            }
        }

        #endregion

        #region Constructor

        public DependencyCollection() {
            _items = new List<DependencyItem>();
        }

        #endregion

        #region Methods

        public void Add(DependencyItem item) {
            _items.Add(item);
        }

        #endregion
    }
}
