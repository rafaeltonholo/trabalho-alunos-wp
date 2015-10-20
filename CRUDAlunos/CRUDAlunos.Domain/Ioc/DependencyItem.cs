using System;

namespace CRUDAlunos.Ioc {
    public class DependencyItem {
        public Type Class { get; set; }
        public Type Interface { get; set; }
        public DependencyType Type { get; set; }
        public object Instance { get; set; }
    }
}
