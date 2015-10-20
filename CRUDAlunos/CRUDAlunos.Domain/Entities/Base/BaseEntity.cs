using System;

namespace CRUDAlunos.Domain.Entities.Base {
    public class BaseEntity {
        public int Id { get; set; }
        public DateTime DtCriacao { get; set; }
    }
}
