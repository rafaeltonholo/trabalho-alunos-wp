using System;
using System.Linq;

namespace CRUDAlunos.Ioc {
    public class DependencyManager {
        #region Fields

        private static DependencyCollection _collection;

        #endregion

        #region Constructor

        static DependencyManager() {
            _collection = new DependencyCollection();
        }

        //public DependencyManager()
        //{
        //    _collection = new DependencyCollection();
        //}

        #endregion

        #region Methods

        public static void Register<T>(Type classType, DependencyType type = DependencyType.Transient, object instance = null) {
            _collection.Add(new DependencyItem {
                Class = classType,
                Interface = typeof(T),
                Type = type,
                Instance = instance
            });
        }

        public static T Get<T>(params object[] parameters) {
            var item = _collection.Items.FirstOrDefault(c => c.Interface == typeof(T));

            if (item != null) {
                switch (item.Type) {
                    case DependencyType.Transient:

                        if (parameters == null || !parameters.Any()) {
                            var currentInstance = Activator.CreateInstance(item.Class);
                            return (T)currentInstance;
                        } else {
                            var currentInstance = Activator.CreateInstance(item.Class, parameters);
                            return (T)currentInstance;
                        }

                    case DependencyType.Singleton:

                        if (parameters == null || !parameters.Any()) {
                            if (item.Instance == null) {
                                var singletonInstance = Activator.CreateInstance(item.Class);
                                item.Instance = singletonInstance;
                            }
                            return (T)item.Instance;
                        } else {
                            if (item.Instance == null) {
                                var singletonInstance = Activator.CreateInstance(item.Class, parameters);
                                item.Instance = singletonInstance;
                            }
                            return (T)item.Instance;
                        }

                    default:
                        break;
                }
            }
            return default(T);
        }

        #endregion
    }
}
