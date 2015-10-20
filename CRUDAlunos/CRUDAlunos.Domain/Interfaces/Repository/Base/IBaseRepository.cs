using CRUDAlunos.Domain.Entities.Base;
using System.Collections.Generic;

namespace CRUDAlunos.Domain.Interfaces.Repository.Base {
    public interface IBaseRepository<T> where T : class {
        void Add(T entidade);
        void Update(T entidade);
        void Remove(T entidade);
        T FindById(int id);
        IEnumerable<T> FindAll();
    }
}
