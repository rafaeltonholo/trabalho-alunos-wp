using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDAlunos.Data.DataObjects.Base {
    public class BaseData {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public DateTime DtCriacao { get; set; }
    }
}
