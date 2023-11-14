using AutoFixture;
using Bunit;
using Bunit.TestDoubles;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.Extensions.DependencyInjection;
using PSA.Client;
using PSA.Client.Pages.Bets;
using PSA.Client.Pages.Robot;
using PSA.Shared;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.ClientTests
{
    [TestClass()]
    public class ManualTest : BunitTestContext
    {
        public static Fixture _fixture = new();

        [TestMethod]
        public void AddToCartRobotIsNull()
        {
            //Arrange
            var mock = Services.AddMockHttpClient();
            var testUser = _fixture.Create<CurrentUser>();
            mock.When("/api/builder").RespondJson((Robot)null);
            mock.When("/api/currentuser").RespondJson(testUser);

            var cut = RenderComponent<Manual>();

            //Act
            cut.WaitForState(() => cut.FindAll("p").Count > 0);

            //Assert

        }
        [TestMethod]
        public async Task AddToCartTest()
        {
            //Arrange
            var mock = Services.AddMockHttpClient();
            var navMan = Services.GetRequiredService<FakeNavigationManager>();
            var tempParts = _fixture.Create<Fight>();
            var testUser = _fixture.Create<CurrentUser>();
            var testRightLeg = _fixture.Build<Product>().With(x => x.Category, 2).With(z => z.Price, 15).With(c => c.Quantity, 2).Create();
            var testHead = _fixture.Build<Product>().With(x => x.Category, 3).With(y => y.Connection, testRightLeg.Connection).With(z => z.Price, 15).With(c => c.Quantity, 2).Create();
            var testLeftArm = _fixture.Build<Product>().With(x => x.Category, 4).With(y => y.Connection, testRightLeg.Connection).With(z => z.Price, 15).With(c => c.Quantity, 2).Create();
            var testRightArm = _fixture.Build<Product>().With(x => x.Category, 5).With(y => y.Connection, testRightLeg.Connection).With(z => z.Price, 15).With(c => c.Quantity, 2).Create();
            var testLeftLeg = _fixture.Build<Product>().With(x => x.Category, 6).With(y => y.Connection, testRightLeg.Connection).With(z => z.Price, 15).With(c => c.Quantity, 2).Create();
            var testBody = _fixture.Build<Product>().With(x => x.Category, 7).With(y => y.Connection, testRightLeg.Connection).With(z => z.Price, 15).With(c => c.Quantity, 2).Create();
            var testRobot = _fixture.Build<Robot>().With(y => y.RightLeg, testRightLeg.Id)
                .With(y => y.Head, testHead.Id)
                .With(y => y.LeftArm, testLeftArm.Id)
                .With(y => y.RightArm, testRightArm.Id)
                .With(y => y.LeftLeg, testLeftLeg.Id)
                .With(y => y.Body, testBody.Id)
                .Create();
            mock.When("/api/builder").RespondJson(testRobot);
            mock.When("/api/currentuser").RespondJson(testUser);
            mock.When($"/api/robots/parts/{testRobot.RightLeg}").RespondJson(testRightLeg);
            mock.When($"/api/robots/parts/{testRobot.Head}").RespondJson(testHead);
            mock.When($"/api/robots/parts/{testRobot.LeftArm}").RespondJson(testLeftArm);
            mock.When($"/api/robots/parts/{testRobot.RightArm}").RespondJson(testRightArm);
            mock.When($"/api/robots/parts/{testRobot.LeftLeg}").RespondJson(testLeftLeg);
            mock.When($"/api/robots/parts/{testRobot.Body}").RespondJson(testBody);
            mock.When(HttpMethod.Post, "/api/robots/").RespondJson(testRobot);
            mock.When(HttpMethod.Post, "/api/cart/add").RespondJson(testBody);
            var cut = RenderComponent<Manual>();


            //Act
            cut.WaitForState(() => cut.FindAll("a.btn").Count > 0, timeout: TimeSpan.FromSeconds(1));
            cut.Find("a.btn").Click();

            //Assert
            cut.WaitForAssertion(() => Assert.AreEqual("http://localhost/cart/", navMan.Uri));
          
        }

    }
}
