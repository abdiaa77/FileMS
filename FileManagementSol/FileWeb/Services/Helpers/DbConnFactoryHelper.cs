using MySql.Data.MySqlClient;

namespace FileWeb.Services.Helpers
{
    public class DbConnFactoryHelper
    {
        private readonly string _connString;
        private readonly IConfiguration _config;
        public DbConnFactoryHelper(IConfiguration configu)
        {
            _config = configu;
            _connString = _config.GetValue<string>("ConnectionStrings:fileDbConnStr") ??
                                            throw new ApplicationException("Couldn't establish a connection to the db Server"); ;
        }

        public MySqlConnection Create()
        {
            return new MySqlConnection(_connString);
        }

    }
}
