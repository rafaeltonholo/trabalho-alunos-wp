using CRUDAlunos.Ioc;
using Ninject;
using System;

namespace CRUDAlunos.Domain.Ioc {
    public class DependencyCore {
        private IKernel _kernel;
        private static DependencyCore instance;

        public static DependencyCore Instance {
            get {
                if (instance == null) {
                    instance = new DependencyCore();
                }

                return instance;
            }
        }

        private DependencyCore() { }

        public void InitializeKernel(IKernel kernel) {
            if(this._kernel != null) {
                throw new Exception("O Kernel já foi inicializado");
            }

            this._kernel = kernel;
        }

        public void AddBinding<TInterface, TImplementation>() where TImplementation : TInterface {
            //_kernel.Bind<TInterface>().To<TImplementation>();
            DependencyManager.Register<TInterface>(typeof(TImplementation));
        }

        public T GetInstance<T>() {
            return DependencyManager.Get<T>();
            //return _kernel.Get<T>();
        }

        public T GetSingletonInstance<T>() {
            return DependencyManager.Get<T>(DependencyType.Singleton);
        }
    }
}
