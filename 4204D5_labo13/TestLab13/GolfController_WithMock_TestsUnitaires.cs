using Labo13.Controllers;
using Labo13.Data;
using Labo13.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLab13
{
    public class GolfController_WithMock_TestsUnitaires
    {
        private DbContextOptions<Labo13Context> _mockDbContextOptions;
        private Labo13Context _mockDbContext;
        private GolfController ctrl;

        public GolfController_WithMock_TestsUnitaires()
        {
            _mockDbContextOptions = new DbContextOptionsBuilder<Labo13Context>().UseInMemoryDatabase("TestDbContext").Options;
            _mockDbContext = new Labo13Context(_mockDbContextOptions);

            List<ScoreTrou> scoreTrous = new List<ScoreTrou>();
            scoreTrous.Add(new ScoreTrou { ScoreTrouId = 1, Score = -1, Terme = "birdie", GolfeurId = 1, DateTrou = new DateTime() });
            scoreTrous.Add(new ScoreTrou { ScoreTrouId = 2, Score = -2, Terme = "eagle", GolfeurId = 2, DateTrou = new DateTime() });
            _mockDbContext.AddRangeAsync(scoreTrous);
            _mockDbContext.SaveChanges();

            ctrl = new GolfController(_mockDbContext);
        }

        [Fact]
        public async Task ScoresTrou_ReturnView_OK_Async()
        {
            // ARRANGE
            _mockDbContext.Database.EnsureCreated();
            // ACT
            var result = await ctrl.ScoresTrou();
            var viewResult = result as ViewResult;
            // ASSERT
            Assert.NotNull(viewResult);
            Assert.IsType<ViewResult>(viewResult);

            var model = viewResult.ViewData.Model as List<ScoreTrou>;
            Assert.NotNull(model);
            Assert.IsType<List<ScoreTrou>>(viewResult.Model);
            Assert.Equal(2, model.Count);
            Assert.IsType<ScoreTrou>(model[0]);
        }
    }
}
