using Moq;
using PSA.Client.Pages.Fights;
using PSA.Shared;
using RichardSzalay.MockHttp;
using System;
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
	public class BetOnFightsTest : BunitTestContext
	{
		private TestContext testContext;

		[TestMethod]
		public async Task OnInitializedAsync_RendersProperly()
		{
			var mock = Services.AddMockHttpClient();

			Fight fight = new Fight(DateTime.Now, 1, 1, 1, 1, 1);
			var robot1 = new Robot { Nickname = "Robot1", Id = 1, Wins = 10, Losses = 5, Draws = 2 };
			var robot2 = new Robot { Nickname = "Robot2", Id = 2, Wins = 8, Losses = 3, Draws = 1 };

			mock.When(HttpMethod.Get, $"/api/Fights/{fight.id}").RespondJson(fight);
			mock.When(HttpMethod.Get, $"/api/Robots/{fight.fk_robot1}").RespondJson(robot1);
			mock.When(HttpMethod.Get, $"/api/Robots/{fight.fk_robot2}").RespondJson(robot2);

			var cut = RenderComponent<BetOnFight>(parameters => parameters.Add(p => p.fightId, fight.id));

			var expectedMarkup = $@"
            <tr>
                <td>{robot1.Nickname}</td>
                <td>{robot1.Wins}</td>
                <td>{robot1.Losses}</td>
                <td>{robot1.Draws}</td>
                <td></td>
            </tr>

        ";
			expectedMarkup += expectedMarkup;

			cut.WaitForAssertion(() => cut.FindAll("tr").SkipWhile(n => n.TextContent.Contains("Nickname")).MarkupMatches(expectedMarkup));


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
		public async Task OnRender_IfRobotsAreNull_DisplayParagraph()
		{
			var mock = Services.AddMockHttpClient();

			Fight? fight = null;

			mock.When(HttpMethod.Get, $"/api/Fights/1").RespondJson(new Fight());
			var cut = RenderComponent<BetOnFight>(parameters => parameters.Add(p => p.fightId, 1));

			cut.WaitForState( () => cut.FindAll("p").Count > 0);

			cut.FindAll("p").MarkupMatches("<p>Doomed statymai</p>");
		}
	}
}


//MOCK, STUB -- 