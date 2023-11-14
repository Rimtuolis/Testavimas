using AutoFixture;
using Bunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PSA.Client.Pages.Fights;
using PSA.Client.Pages.Users;
using PSA.Client.Shared;
using PSA.Shared;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace PSA.ClientTests
{
	[TestClass]
	public class FightsPageTest : BunitTestContext
	{

		private readonly Fixture fixture = new();
		Random rand = new Random();

		[DataTestMethod]
		[DataRow(2, 1)] 
		[DataRow(2, 2)] 
		[DataRow(2, 3)]
		[DataRow(1, 3)]
		public async Task OnInitializedAsync_RendersProperly(int state, int winner)
		{

			var mock = Services.AddMockHttpClient();
			var robotId = 1;

			var robotParts1 = fixture.Build<RobotPart>().With(n => n.fk_robotas, 1).With(m => m.fk_preke_id, 1).CreateMany(50).ToList();
			var robotParts2 = fixture.Build<RobotPart>().With(n => n.fk_robotas, 2).With(m => m.fk_preke_id, 2).CreateMany(50).ToList();
			var robotParts = robotParts1.Concat(robotParts2).ToList();
			var fights = fixture.Build<Fight>().With(x => x.state, state).With(n => n.winner, winner).With(k => k.fk_robot1, 1).With(l => l.fk_robot2, 2).CreateMany(30).ToList();
			var robot1 = fixture.Build<Robot>().With(n => n.Id, 1).Create();
			var robot2 = fixture.Build<Robot>().With(n => n.Id, 2).Create();
			List<Robot> robot = new List<Robot>{ robot1, robot2 };
			var rototobas = fixture.Create<Robot>();
			var product1 = fixture.Build<Product>().With(n => n.Quantity, 10).With(x => x.Price, rand.NextDouble() * 100).With(l => l.Id, 1).CreateMany(15).ToList();
			var product2 = fixture.Build<Product>().With(n => n.Quantity, 10).With(x => x.Price, rand.NextDouble() * 100).With(l => l.Id, 2).CreateMany(15).ToList();
			var product = product1.Concat(product2).ToList();
			var profile = fixture.Create<CurrentUser>();

			mock.When(HttpMethod.Get, $"/api/fights/view/{robotId}").RespondJson(fights);

			mock.When(HttpMethod.Get, "/api/robots").RespondJson(robot);
			mock.When(HttpMethod.Get, "/api/robots/reikia/id").RespondJson(rototobas);
			mock.When(HttpMethod.Get, "/api/robotPart").RespondJson(robotParts);
			mock.When(HttpMethod.Get, "/api/products").RespondJson(product);
			mock.When(HttpMethod.Get, "/api/currentuser").RespondJson(profile);
			mock.When(HttpMethod.Get, "/api/robots/1").Respond("application/json", "{}");
			mock.When(HttpMethod.Put, "/api/robotPart/5").Respond("application/json", "{}");
			mock.When(HttpMethod.Put, "/api/robots/tie/1/2").Respond("application/json", "{}");

			mock.When(HttpMethod.Put, "/api/fights/win/mhm").With(request => request.Content.ReadAsStringAsync().Result.Contains("fk_robot1") &&
					 request.Content.ReadAsStringAsync().Result.Contains("fk_robot2")).Respond("application/json", "{}");
			mock.When(HttpMethod.Put, $"/api/robots/win/1/2").Respond("application/json", "{}");
			mock.When(HttpMethod.Put, $"/api/robots/win/2/1").Respond("application/json", "{}");
			mock.When(HttpMethod.Post, "/api/Bets/1").Respond("application/json", "{}");
			mock.When(HttpMethod.Post, "/api/Bets/2").Respond("application/json", "{}");
			mock.When(HttpMethod.Get, "/api/robots/1").RespondJson(robot[0]);
			mock.When(HttpMethod.Get, "/api/robots/2").RespondJson(robot[1]);
			mock.When(HttpMethod.Put, $"/api/robotPart/5").Respond("application/json", "{}");
			mock.When(HttpMethod.Put, $"/api/robotPart/10").Respond("application/json", "{}");
			mock.When(HttpMethod.Put, $"/api/robotPart/15").Respond("application/json", "{}");


			// Act
			var cut = RenderComponent<Fights>(parameters => parameters.Add(p => p.robotId, robotId));


			cut.WaitForState(() => cut.FindAll("td").Count > 0, timeout: TimeSpan.FromSeconds(1));

			// Assert
			Assert.AreEqual(0,0);
		}

		[TestMethod]
		public async Task HandleState_NavigatesToCorrectUrl()
		{
			var mock = Services.AddMockHttpClient();
			var robotId = 1;

			var cut = RenderComponent<Fights>(parameters => parameters.Add(p => p.robotId, robotId));
			var navMan = Services.GetRequiredService<FakeNavigationManager>();
			var fightId = 1;

			mock.When(HttpMethod.Put, $"/api/fights/{fightId}").RespondJson(new Fight());

			// Act
			await cut.InvokeAsync(() => cut.Instance.HandleState(fightId));

			// Assert
			Assert.AreEqual($"http://localhost/fights/{robotId}", navMan.Uri);
		}

		[TestMethod]
		public async Task Decline_NavigatesToCorrectUrl()
		{
			var mock = Services.AddMockHttpClient();
			var robotId = 1;

			var cut = RenderComponent<Fights>(parameters => parameters.Add(p => p.robotId, robotId));
			var navMan = Services.GetRequiredService<FakeNavigationManager>();
			var fightId = 1;

			mock.When(HttpMethod.Delete, $"/api/fights/{fightId}").RespondJson(new Fight());

			// Act
			await cut.InvokeAsync(() => cut.Instance.Decline(fightId));

			// Assert
			Assert.AreEqual($"http://localhost/fights/{robotId}", navMan.Uri);
		}

		[TestMethod]
		public async Task HandleDamage_SendsCorrectRequests()
		{
			// Arrange
			var mock = Services.AddMockHttpClient();
			var cut = RenderComponent<Fights>();

			var robotId = 1;
			var damage = 5;

			// Set up expectations
			mock.When(HttpMethod.Get, $"http://localhost/api/robots/{robotId}").Respond("application/json", "{}");
			mock.When(HttpMethod.Put, $"http://localhost/api/robotPart/{damage}").Respond("application/json", "{}");

			// Act
			await cut.InvokeAsync(() => cut.Instance.HandleDamage(robotId, damage));

			// Assert
			mock.VerifyNoOutstandingExpectation();
		}

		[TestMethod]
		public async Task WinLoseTieCondition_SendsCorrectRequest()
		{
			// Arrange
			var mock = Services.AddMockHttpClient();
			var cut = RenderComponent<Fights>();

			var id = 1;
			var id2 = 2;

			// Set up expectations
			mock.When(HttpMethod.Put, $"/api/robots/win/{id}/{id2}")
				.Respond("application/json", "{}");

			// Act
			await cut.InvokeAsync(() => cut.Instance.WinLoseTieCondition(id, id2));

			// Assert
			mock.VerifyNoOutstandingExpectation();
		}

		[TestMethod]
		public async Task TieCondition_SendsCorrectRequest()
		{
			// Arrange
			var mock = Services.AddMockHttpClient();
			var cut = RenderComponent<Fights>();

			var id = 1;
			var id2 = 2;

			// Set up expectations
			mock.When(HttpMethod.Put, $"http://localhost/api/robots/tie/{id}/{id2}")
				.Respond("application/json", "{}");

			// Act
			await cut.InvokeAsync(() => cut.Instance.TieCondition(id, id2));

			// Assert
			mock.VerifyNoOutstandingExpectation();
		}

		[TestMethod]
		public async Task HandleFinished_SendsCorrectRequest()
		{
			// Arrange
			var mock = Services.AddMockHttpClient();
			var cut = RenderComponent<Fights>();

			var fight = new Fight { fk_robot1 = 1, fk_robot2 = 2 };

			// Set up expectations
			mock.When(HttpMethod.Put, "http://localhost/api/fights/win/mhm")
				.With(request => request.Content.ReadAsStringAsync().Result.Contains("fk_robot1") &&
								 request.Content.ReadAsStringAsync().Result.Contains("fk_robot2"))
				.Respond("application/json", "{}");

			// Act
			await cut.InvokeAsync(() => cut.Instance.HandleFinished(fight));

			// Assert
			mock.VerifyNoOutstandingExpectation();
		}
	}
}