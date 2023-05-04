using Labo13.Controllers;
using Labo13.Data;
using Labo13.Models;
using Labo13.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            // ARRANGE
            using Labo13Context context = Fixture.CreateContext();
            GolfController controller = new GolfController(context);

            // ACT
            IActionResult actionResult = await controller.Index();

            //ASSERT
            Assert.NotNull(actionResult);

            ViewResult viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.NotNull(viewResult);

            List<VwDetailsScoreGolfeur> model = Assert.IsAssignableFrom<List<VwDetailsScoreGolfeur>>(viewResult.Model);
            Assert.NotNull(model);
            Assert.Equal(5, model.Count);

        }

        [Fact]
        public async Task DeleteScrore_OK_Async()
        {
            // ARRANGE
            using Labo13Context context = Fixture.CreateContext();
            GolfController controller = new GolfController(context);
            int count = context.ScoreTrous.Count();

            // ACT
            await context.ScoreTrous.AddAsync(
                new ScoreTrou { 
                    DateTrou = DateTime.Now,
                    Score = 0,
                    GolfeurId = 1,
                    Terme = "birdie"
                });
            await context.SaveChangesAsync();
            IActionResult actionResult = await controller.DeleteScoreTrou((await context.ScoreTrous.OrderBy(s => s.ScoreTrouId).LastAsync()).ScoreTrouId);

            //ASSERT
            Assert.NotNull(actionResult);

            RedirectToActionResult objResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.NotNull(objResult);
            Assert.Equal("Index", objResult.ActionName);

            Assert.Equal(count, context.ScoreTrous.Count());

        }

        [Fact]
        public async Task CreateScoreTrou_OK_async()
        {
            // ARRANGE
            using Labo13Context context = Fixture.CreateContext();
            GolfController controller = new GolfController(context);
            int count = context.ScoreTrous.Count();

            CreerScoreTrouVM cstvm = new CreerScoreTrouVM
            {
                NomGolfeur = "Tiger Woods",
                Score = -2,
            };

            // ACT
            var result = await controller.CreateScoreTrou(cstvm);
            var tiger = await context.Golfeurs.FirstOrDefaultAsync(s => s.GolfeurId == 1);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(count + 1, context.ScoreTrous.Count());

            Assert.NotNull(tiger);
            Assert.Equal("Tiger Woods", tiger.Nom);
            Assert.Equal(tiger.ScoreTotal, -2);
            Assert.Equal(6, tiger.NbTrous);
        }

    }
}
