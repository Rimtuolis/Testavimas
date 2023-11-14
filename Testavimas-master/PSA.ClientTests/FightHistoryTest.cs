using AutoFixture;
using Moq;
using PSA.Client.Pages.Fights;
using PSA.Shared;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
	public class FightHistoryTest : BunitTestContext
	{
		private readonly Fixture fixture = new();

		[TestMethod]
		public async Task OnInitializedAsync_RendersProperly()
		{
			var mock = Services.AddMockHttpClient();

			var robots = fixture.CreateMany<Robot>(30).ToList();
			robots.Add(new Robot("", 0, 0, 0, 1, 1));
			robots.Add(new Robot("", 0, 2, 0, 1, 1));
			robots.Add(new Robot("", 2, 8, 0, 1, 1));
			mock.When(HttpMethod.Get, $"/api/robots/get/getAll").RespondJson(robots);


			var cut = RenderComponent<FightsHistory>();


			cut.WaitForState(() => cut.FindAll("button").Count > 0);

			cut.Find("button.editPartsButton").Click();
			cut.Find("button.editPartsButton").Click();

		}

		[TestMethod]
		public async Task HandleValidSubmit_NavigatesToTournaments()
		{
			// Arrange
			var mock = Services.AddMockHttpClient();
			var cut = RenderComponent<BetOnFight>(parameters => parameters.Add(p => p.fightId, 1));
			var navMan = Services.GetRequiredService<FakeNavigationManager>();
			mock.When(HttpMethod.Post, "/api/Bets").RespondJson(new Bet());

			// Act - Simulate form submission
			await cut.InvokeAsync(() => cut.Instance.HandleValidSubmit());

			// Assert - Verify navigation
			Assert.AreEqual("http://localhost/Tournaments", navMan.Uri);
		}

		[TestMethod]
		public async Task ShowsRobotStatistics_Async()
		{
			// Arrange
			var mock = Services.AddMockHttpClient();

			var robots = new List<Robot> { new Robot { Id = 1, Nickname = "Robot1", Wins = 2, Losses = 1, Draws = 0 } };
			mock.When(HttpMethod.Get, $"/api/robots/get/getAll").RespondJson(robots);
			var cut = RenderComponent<FightsHistory>();


			// Act
			cut.WaitForState(() => cut.FindAll("tbody td").Count > 0);

			// Assert
			cut.FindAll("tbody td").MarkupMatches(
				"<td>1</td>\r\n<td>Robot1</td>\r\n<td>2</td>\r\n<td>1</td>\r\n<td>0</td>\r\n<td>Very good</td>");
		}
	}
}
