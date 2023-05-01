using Labo13.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLab13
{
    public class BdTestFixture
    {
        private const string ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Lab13Test;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";
        private static bool _databaseInitialized;

        public Labo13Context CreateContext()
        {
            return new Labo13Context(new DbContextOptionsBuilder<Labo13Context>().UseSqlServer(ConnectionString).Options);
        }
    }
}
