using CRUDAlunos.Domain.Interfaces.Repository.Base;
using System;
using System.Collections.Generic;
using CRUDAlunos.Data.Interfaces;
using CRUDAlunos.Domain.Entities.Base;
using CRUDAlunos.Domain;
using CRUDAlunos.Domain.Ioc;

namespace CRUDAlunos.Data.Repository {
    public class BaseRepository<T> : IBaseRepository<T> where T : class {

        public ISQLite Context { get; private set; }

        public BaseRepository() {
            Context = DependencyCore.Instance.GetInstance<ISQLite>();
        }

        public virtual void Add(T entidade) {
            Context.Connection.Insert(entidade);
        }

        public virtual IEnumerable<T> FindAll() {
            return Context.Connection.Table<T>();
        }

        public virtual T FindById(int id) {
            return Context.Connection.Find<T>(id);
        }

        public virtual void Remove(T entidade) {
            Context.Connection.Delete(entidade);
        }

        public virtual void Update(T entidade) {
            Context.Connection.Update(entidade);
        }
    }
}
