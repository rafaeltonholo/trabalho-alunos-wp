using CRUDAlunos.Domain.Entities.Base;
using System.Collections.Generic;

namespace CRUDAlunos.Aplicacao.Interfaces.Base {
    public interface IBaseApplication<TView, TEntity>
        where TView : class
        where TEntity : BaseEntity {

        void Add(TView entidade);
        void Update(TView entidade);
        void Remove(TView entidade);
        TView FindById(int id);
        IEnumerable<TView> FindAll();
    }
}
