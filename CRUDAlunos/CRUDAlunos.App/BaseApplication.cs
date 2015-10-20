using AutoMapper;
using CRUDAlunos.Aplicacao.Interfaces.Base;
using CRUDAlunos.Domain.Entities.Base;
using CRUDAlunos.Domain.Interfaces.Repository.Base;
using System.Collections.Generic;
using System.Linq;
using CRUDAlunos.Domain.Ioc;

namespace CRUDAlunos.Aplicacao {
    public class BaseApplication<TView, TEntity> : IBaseApplication<TView, TEntity>
        where TView : class
        where TEntity : BaseEntity {

        #region Properties

        public IBaseRepository<TEntity> Repository { get; private set; }

        #endregion

        #region Constructor

        public BaseApplication() {
            Repository = DependencyCore.Instance.GetInstance<IBaseRepository<TEntity>>();
        }

        #endregion

        #region Methods
        public virtual void Add(TView entidade) {
            var map = Mapper.Map<TView, TEntity>(entidade);
            Repository.Add(map);
        }

        public virtual void Update(TView entidade) {
            var map = Mapper.Map<TView, TEntity>(entidade);
            Repository.Update(map);
        }

        public virtual void Remove(TView entidade) {
            var map = Mapper.Map<TView, TEntity>(entidade);
            Repository.Remove(map);
        }

        public TView FindById(int id) {
            var map = Repository.FindById(id);
            return Mapper.Map<TEntity, TView>(map);
        }

        public IEnumerable<TView> FindAll() {
            var map = Repository.FindAll();
            return Mapper.Map<List<TEntity>, List<TView>>(map.ToList());
        }
        #endregion
    }
}
