using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AngleSharp;
using AutoFixture;
using Blazorise;
using Bunit;
using Bunit.TestDoubles;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PSA.Client.Pages.Gambling;
using PSA.Client.Pages.Fights;
using PSA.Client.Pages.Tournaments;
using PSA.Shared;
using RichardSzalay.MockHttp;
using static System.Net.WebRequestMethods;
using TestContext = Bunit.TestContext;
using BlackJack = PSA.Client.Pages.Gambling.BlackJack;

namespace PSA.ClientTests
{
    [TestClass()]
    public class BlackJackTests : BunitTestContext
    {
        public static Fixture _fixture = new();

        [TestMethod]
        public void PlayingTests_boolFalse()
        {
            // Arrange

            using var ctx = new TestContext();
            var mock = ctx.Services.AddMockHttpClient();
            var tempBool = false;
            var tempCurrent = _fixture.Build<CurrentUser?>().With(x => x.Id, 1).Create();
            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            mock.When($"/api/blackjack/gamestate").RespondJson(tempBool);

            var cut = RenderComponent<BlackJack>();

            cut.WaitForState(() => cut.FindAll("td").Count > 0);
            // Act - Simulate form submission
            var result = cut.Instance.Playing();
            Assert.IsNotNull(result, "error");
        }
    }
}
