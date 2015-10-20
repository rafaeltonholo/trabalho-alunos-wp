using CRUDAlunos.Domain.Entities.Base;

namespace CRUDAlunos.Domain.Entities {
    public class Aluno : BaseEntity {
        public int Matricula { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public int Idade { get; set; }
        public string NomePai { get; set; }
        public string NomeMae { get; set; }
        public int Nota { get; set; }
        public int PercentualFaltas { get; set; }
        public byte[] Foto { get; set; }
    }
}
