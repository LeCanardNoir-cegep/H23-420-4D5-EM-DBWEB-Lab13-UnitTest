using Labo13.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLab13
{
    public class GolfController_TestsIntegration : IClassFixture<BdTestFixture>
    {
        private BdTestFixture Fixture { get; }

        public GolfController_TestsIntegration(BdTestFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public async Task Index_OK_Async()
        {

        }

    }
}
