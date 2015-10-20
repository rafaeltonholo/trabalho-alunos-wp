using CRUDAlunos.Data.Interfaces;
using CRUDAlunos.Domain.Ioc;
using SQLite.Net.Interop;

namespace CRUDAlunos.Data {
    public class WPSQLite : ISQLite {
        public SQLite.Net.SQLiteConnection Connection {
            get {
                var local = "DBCliente.db3";
                return new SQLite.Net.SQLiteConnection(DependencyCore.Instance.GetInstance<ISQLitePlatform>(), local);
            }
        }
    }
}
