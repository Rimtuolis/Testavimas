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
using PSA.Client.Shared;
using PSA.Client.Pages.Bets;


namespace PSA.ClientTests
{
    [TestClass]
    public class MainHeaderTests : BunitTestContext
    {
        public static Fixture _fixture = new();
        [TestMethod]
        public async Task OnLogoutClicked_NavigatesToCorrectUrl()
        {
            using var ctx = new TestContext();
            var mock = Services.AddMockHttpClient();
            var tempBool = false;
            var navMan = Services.GetRequiredService<FakeNavigationManager>();

            var tempCurrent = _fixture.Build<CurrentUser?>().With(x => x.Id, 1).Create();
            mock.When($"/api/auth/logout").RespondJson(tempCurrent);
            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            var cut = RenderComponent<MainHeader>();

            //cut.WaitForState(() => cut.FindAll("ul").Count > 0);
            cut.WaitForState(() => cut.FindAll("li").Count > 0);
            //cut.WaitForState(() => cut.FindAll("a").Count > 0);

            var result = cut.Instance.OnLogoutClicked();
            Assert.AreEqual("http://localhost/", navMan.Uri);
        }
        [TestMethod]
        public async Task OpenAddBalancePage_NavigatesToCorrectUrl()
        {
            using var ctx = new TestContext();
            var mock = Services.AddMockHttpClient();
            var tempBool = false;
            var navMan = Services.GetRequiredService<FakeNavigationManager>();

            var tempCurrent = _fixture.Build<CurrentUser?>().With(x => x.Id, 1).Create();
            mock.When($"/api/currentuser").RespondJson(tempCurrent);
            var cut = RenderComponent<MainHeader>();

            cut.WaitForState(() => cut.FindAll("li").Count > 0);

            cut.Instance.OpenAddBalancePage();
            Assert.AreEqual("http://localhost/profiles/balance", navMan.Uri);

        }
    }
}
