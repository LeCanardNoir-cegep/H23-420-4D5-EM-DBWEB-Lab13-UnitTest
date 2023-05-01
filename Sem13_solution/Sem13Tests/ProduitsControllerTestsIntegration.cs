using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sem13.Controllers;
using Sem13.Data;
using Sem13.Models;
using Sem13.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sem13Test
{
    public class ProduitsControllerTestsIntegration : IClassFixture<BDTestFixture>
    {
        private BDTestFixture Fixture { get; }

        public ProduitsControllerTestsIntegration(BDTestFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public async Task Index_VerifierSiDixProduitsObtenus()
        {
            using Sem13Context context = Fixture.CreateContext();
            ProduitsController controller = new ProduitsController(context);


            IActionResult actionResult = await controller.Index();
            ViewResult viewResult = Assert.IsType<ViewResult>(actionResult);
            List<Produit> model = Assert.IsAssignableFrom<List<Produit>>(viewResult.Model);
            Assert.Equal(10, model.Count);
        }

        [Fact]
        public async Task CreatePost_InsertionProduitValide()
        {
            CreationProduitVM cpvm = new CreationProduitVM()
            {
                Nom = "Divan 2 places", PrixEntier = 499, PrixDecimal = 99,
                QteStock = 3, EstDiscontinue = false, Categorie = "Meuble"
            };

            using Sem13Context context = Fixture.CreateContext();

            context.Database.BeginTransaction();

            ProduitsController controller = new ProduitsController(context);
            IActionResult resultat = await controller.Create(cpvm);

            context.ChangeTracker.Clear();

            Assert.Equal(11, await context.Produits.CountAsync());

            //context.Database.RollbackTransaction();
        }

        [Fact]
        public async Task IndexAvecAutorisation_VerifierRedirection()
        {
            WebApplicationFactory<Program> application = GetApp();
            //Sem13Context context = Fixture.CreateContext();

            using IServiceScope services = application.Services.CreateScope();
            HttpClient client = application.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            //ProduitsController controller = new ProduitsController(context);

            HttpResponseMessage reponse = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/Produits/IndexAvecAutorisation"));

            Assert.Equal(HttpStatusCode.Redirect, reponse.StatusCode);
        }

        [Fact]
        public async Task IndexAvecAutorisation_VerifierDixProduits()
        {
            WebApplicationFactory<Program> application = GetApp();

            using IServiceScope services = application.Services.CreateScope();
            HttpClient client = application.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/Utilisateurs/Connexion");
            FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"Pseudonyme", "max" }, 
                {"MotDePasse", "Salut1!"} 
            });
            request.Content = content;
            
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Found)
            {
                string? authCookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
                if(authCookie != null)
                {
                    client.DefaultRequestHeaders.Add("Cookie", authCookie);
                    HttpResponseMessage resultat = await client.GetAsync("/Produits/IndexAvecAutorisation");

                    Assert.True(resultat.IsSuccessStatusCode);
                }
            }
        }

        private WebApplicationFactory<Program> GetApp()
        {
            return new WebApplicationFactory<Program>();
        }
    }
}
