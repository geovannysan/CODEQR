using System.Data.SQLite;

namespace NEWCODES.Infraestructura
{
    public class AdoContext
    {
        public SQLiteConnection Connection { get; private set; }

        public AdoContext()
        {
            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CodeEvente.db");
            string connectionString = $"Data Source={dbPath};Version=3;Journal Mode=WAL;";
            Connection = new SQLiteConnection(connectionString);
            Connection.Open();
        }

        public SQLiteCommand CreateCommand(string query)
        {
            var cmd = Connection.CreateCommand();
            cmd.CommandText = query;
            return cmd;
        }

        // Este método se llama automáticamente al usar "using"
        public void Dispose()
        {
            Connection?.Close();
            Connection?.Dispose();
        }
    }
}
