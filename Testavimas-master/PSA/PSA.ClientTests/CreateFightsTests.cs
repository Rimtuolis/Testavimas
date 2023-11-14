using Microsoft.JSInterop;
using Moq;
using PSA.Client.Pages.Fights;
using PSA.Shared;
using RichardSzalay.MockHttp;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HttpMethod = System.Net.Http.HttpMethod;
using TestContext = Bunit.TestContext;

namespace PSA.ClientTests
{
	/// <summary>
	/// These tests are written entirely in C#.
	/// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
	/// </summary>
	[TestClass]
	public class CreateFightsTests : BunitTestContext
	{
		private TestContext testContext;

		[TestMethod]
		public async Task OnInitializedAsync_RendersProperly()
		{
			// Arrange
			var mock = Services.AddMockHttpClient();
			mock.When(HttpMethod.Get, "/api/currentuser").RespondJson(new CurrentUser());
			mock.When(HttpMethod.Get, "/api/robots/component/mySelf").RespondJson(new Robot[] { new Robot() });
			mock.When(HttpMethod.Get, "/api/robots/component/getOponent").RespondJson(new Robot[] { new Robot() });

			// Act
			var cut = RenderComponent<CreateFight>();

			// Assert
			cut.WaitForAssertion(() => cut.FindAll("option").Count.Equals(4));
			cut.WaitForAssertion(() => cut.FindAll("button").Count.Equals(1));
		}
		[TestMethod]
		public async Task HandleValidSubmit_DateInPast_ShowsAlert()
		{
			// Arrange
			var mock = Services.AddMockHttpClient();

			var jsMock = new Mock<IJSRuntime>();
			JSInterop.Mode = JSRuntimeMode.Loose;
			var plannedInvocation = JSInterop.SetupVoid("alert");
			mock.When(HttpMethod.Get, "/api/currentuser").RespondJson(new CurrentUser());
			mock.When(HttpMethod.Get, "/api/robots/component/mySelf").RespondJson(new Robot[] { new Robot() });
			mock.When(HttpMethod.Get, "/api/robots/component/getOponent").RespondJson(new Robot[] { new Robot() });


			var cut = RenderComponent<CreateFight>();

			await cut.InvokeAsync(() => cut.Instance.HandleValidSubmit());

			// Assert
			JSInterop.VerifyInvoke("alert", "Enter upcoming date not past.");
		}
		[TestMethod]
		public async Task HandleValidSubmit_NavigatesToFights()
		{
			var jsMock = new Mock<IJSRuntime>();
			JSInterop.Mode = JSRuntimeMode.Loose;
			var plannedInvocation = JSInterop.SetupVoid("alert");

			var mock = Services.AddMockHttpClient();
			var navMan = Services.GetRequiredService<FakeNavigationManager>();
			mock.When(HttpMethod.Get, "/api/currentuser").RespondJson(new CurrentUser());
			mock.When(HttpMethod.Get, "/api/robots/component/mySelf").RespondJson(new Robot[] { new Robot() });
			mock.When(HttpMethod.Get, "/api/robots/component/getOponent").RespondJson(new Robot[] { new Robot() });
			mock.When(HttpMethod.Post, "/api/fights").Respond(HttpStatusCode.OK);

			var cut = RenderComponent<CreateFight>();
			await cut.InvokeAsync(() => cut.Instance.HandleValidSubmit());
			// Assert
			Assert.AreEqual("http://localhost/fights/0", navMan.Uri);
			//Assert.AreEqual(1, cut.Instance.Fight.fk_robot1);
		}
	}
}
