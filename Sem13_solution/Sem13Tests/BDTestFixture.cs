using Microsoft.EntityFrameworkCore;
using Sem13.Data;
using Sem13.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sem13Test
{
    public class BDTestFixture
    {
        private const string ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Sem13Tests;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";

        public Sem13Context CreateContext()
        {
            return new Sem13Context(new DbContextOptionsBuilder<Sem13Context>().UseSqlServer(ConnectionString).Options);
        }
    }
}
