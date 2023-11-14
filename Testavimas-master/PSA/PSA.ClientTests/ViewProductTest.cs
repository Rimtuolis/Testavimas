using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoFixture;
using Bunit;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PSA.Client.Pages.Fights;
using PSA.Client.Pages.Products;
using PSA.Client.Pages.Users;
using PSA.Shared;
using RichardSzalay.MockHttp;
using static System.Net.WebRequestMethods;

namespace PSA.ClientTests
{
	[TestClass]
	public class ViewProductTests : BunitTestContext
	{
		private readonly Fixture fixture = new();
		[TestMethod]
		public async Task RendersProductDetails()
		{
			// Arrange
			int productId = 1;
			var mock = Services.AddMockHttpClient();

			var fakeProduct = new Product
			{
				Id = productId,
				Name = "Test Product",
				Description = "This is a test product.",
				Price = 99.99,
				Material = 1,
				Category = 1,
				Connection = 1,
				Attack = 80,
				Defense = 70,
				Speed = 90
			};

			mock.When(HttpMethod.Get, $"/api/products/{productId}").RespondJson(fakeProduct);
			mock.When(HttpMethod.Get, $"/api/currentuser").RespondJson(new CurrentUser());

			var cut = RenderComponent<ViewProduct>(parameters => parameters.Add(p => p.id, "1"));

			// Act
			cut.WaitForAssertion(() => cut.Markup.Contains("Test Product"));
			cut.WaitForAssertion(() => cut.Markup.Contains("This is a test product."));
			// Add more assertions for other product details
		}

		[TestMethod]
		public async Task ShowsAddToCartButtonForLoggedInUser()
		{
			// Arrange
			var mockHttp = Services.AddMockHttpClient();

			var fakeProfile = new CurrentUser
			{
				LoggedIn = true,
				UserLevel = AccessLevelType.CLIENT
			};

			mockHttp.When(HttpMethod.Get, "/api/currentuser").RespondJson(fakeProfile);

			var fakeProduct = new Product
			{
				Id = 1,
				Name = "Test Product",
				Description = "This is a test product.",
				Price = 99.99,
				Material = 1,
				Category = 1,
				Connection = 1,
				Attack = 80,
				Defense = 70,
				Speed = 90
			};


			mockHttp.When(HttpMethod.Get, $"/api/products/1")
				.RespondJson(fakeProduct);

			var cut = RenderComponent<ViewProduct>(parameters => parameters
				.Add(p => p.id, "1"));

			cut.WaitForState(() => cut.FindAll("button.btn_3").Count > 0);
			//cut.WaitForAssertion(() => cut.Find("button.btn_3").TextContent.Contains("Add to cart"));
		}
	}
}
