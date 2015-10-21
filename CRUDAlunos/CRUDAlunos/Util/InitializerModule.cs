using CRUDAlunos.Aplicacao.Interfaces;
using CRUDAlunos.Aplicacao.Interfaces.Base;
using CRUDAlunos.Aplicacao.ViewObjects;
using CRUDAlunos.Data;
using CRUDAlunos.Data.Interfaces;
using CRUDAlunos.Data.Repository;
using CRUDAlunos.Domain.Entities;
using CRUDAlunos.Domain.Interfaces.Repository.Base;
using CRUDAlunos.ViewModels;
using CRUDAlunos.ViewModels.Base;
//using Ninject.Modules;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
//using Ninject;
using CRUDAlunos.Domain.Ioc;
using CRUDAlunos.Data.Util;
using AutoMapper;
using CRUDAlunos.CrossCutting.Mapping;

namespace CRUDAlunos.Util {    
    public class InitializerModule {
        public static void Initialize() {
            RegisterMappings();
            RegisterDependencies();
        }

        private static void RegisterMappings() {
            Mapper.Initialize((configuration) => {
                configuration.AddProfile<EntityToViewModel>();
                configuration.AddProfile<ViewModelToEntity>();
                configuration.AddProfile<DataToEntity>();
                configuration.AddProfile<EntityToData>();
            });
        }

        private static void RegisterDependencies() {
            //NinjectKernel.Instance.InitializeKernel(new StandardKernel(new InitializerModule()));
            DependencyCore.Instance.AddBinding<ISQLitePlatform, SQLitePlatformWinRT>();
            DependencyCore.Instance.AddBinding<ISQLite, WPSQLite>();

            DependencyCore.Instance.AddBinding<IBaseApplication<AlunoView, Aluno>, AlunoApplication>();
            DependencyCore.Instance.AddBinding<IBaseRepository<Aluno>, AlunoRepository>();

            DependencyCore.Instance.AddBinding<IViewModel<AlunoView>, AlunoViewModel>();
            DependencyCore.Instance.AddBinding<ICameraPreviewViewModel, CameraPreviewViewModel>();

            DBHelper.CreateDatabase();
        }

        /*public override void Load() {
            NinjectKernel.Instance.AddBinding<ISQLitePlatform, SQLitePlatformWinRT>();
            NinjectKernel.Instance.AddBinding<ISQLite, WPSQLite>();

            NinjectKernel.Instance.AddBinding<IBaseApplication<AlunoView, Aluno>, AlunoApplication>();
            NinjectKernel.Instance.AddBinding<IBaseRepository<Aluno>, AlunoRepository>();

            NinjectKernel.Instance.AddBinding<IViewModel<AlunoView>, AlunoViewModel>();
        }*/
    }
}
