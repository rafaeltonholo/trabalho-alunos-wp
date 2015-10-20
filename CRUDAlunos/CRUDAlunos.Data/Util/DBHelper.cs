using CRUDAlunos.Data.DataObjects;

namespace CRUDAlunos.Data.Util {
    public static class DBHelper {
        public static void CreateDatabase() {
            var context = new WPSQLite();

            context.Connection.CreateTable(typeof(AlunoData));
        }
    }
}
