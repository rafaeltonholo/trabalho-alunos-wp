using CRUDAlunos.Data.DataObjects.Base;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDAlunos.Data.DataObjects {
    [Table("Aluno")]
    public class AlunoData : BaseData {
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
