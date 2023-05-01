using Labo13.Controllers;
using Labo13.Data;
using Labo13.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLab13
{
    public class GolfController_TestsUnitaires
    {
        [Fact]
        public async Task ScoresTrouOKAsync()
        {
            // ARRANGE
            Mock<Labo13Context> mockDbContext = new Mock<Labo13Context>();
            List<ScoreTrou> scoreTrous = new List<ScoreTrou>()
            {
                new ScoreTrou()
                {
                    ScoreTrouId = 1,
                    GolfeurId = 1,
                    Score = -1,
                    DateTrou = new DateTime(),
                    Terme = "birdie"
                },
                new ScoreTrou()
                {
                    ScoreTrouId = 2,
                    GolfeurId = 2,
                    Score = -2,
                    DateTrou = new DateTime(),
                    Terme = "Eagle"
                }
            };
            mockDbContext.Setup(x => x.ScoreTrous).ReturnsDbSet(scoreTrous);

            var ctrl = new GolfController(mockDbContext.Object);

            // ACT

            var result = await ctrl.ScoresTrou();

            // ASSERT
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            List<ScoreTrou> model = Assert.IsType<List<ScoreTrou>>(viewResult.Model);
            Assert.Equal(2, model.Count);
            Assert.Equal(1, model[0].ScoreTrouId);
            Assert.Equal(1, model[0].GolfeurId);
            Assert.IsType<ScoreTrou>(model[0]);
        }

        [Fact]
        public async Task DeleteScoreNotOkAsync()
        {
            // ARRANGE
            Mock<Labo13Context> mockDbContext = new Mock<Labo13Context>();
            List<ScoreTrou> scoreTrous = new List<ScoreTrou>()
            {
                new ScoreTrou()
                {
                    ScoreTrouId = 1,
                    GolfeurId = 1,
                    Score = -1,
                    DateTrou = new DateTime(),
                    Terme = "birdie"
                },
                new ScoreTrou()
                {
                    ScoreTrouId = 2,
                    GolfeurId = 2,
                    Score = -2,
                    DateTrou = new DateTime(),
                    Terme = "Eagle"
                }
            };
            mockDbContext.Setup(x => x.ScoreTrous).ReturnsDbSet(scoreTrous);

            var ctrl = new GolfController(mockDbContext.Object);

            // ACT
            var result = await ctrl.DeleteScoreTrou(5);
            RedirectToActionResult objResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ScoresTrou", objResult.ActionName);

        }
    }
}
