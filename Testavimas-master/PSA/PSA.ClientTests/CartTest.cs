using Moq;
using PSA.Client.Pages.Orders;
using PSA.ClientTests;
using PSA.Shared;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PSA.ClientTests
{
	[TestClass]
	public class CartPageTests : BunitTestContext
	{
		[TestMethod]
		public async Task OnInitializedAsync_LoadsProductsAndCheckout()
		{
			// Arrange
			var mock = Services.AddMockHttpClient();
			mock.When(HttpMethod.Get, "/api/cart").RespondJson(new Product[] { new Product { Id = 1, Price = 10.0, Quantity = 2 } });
			var navManager = Services.GetRequiredService<FakeNavigationManager>();

			mock.When(HttpMethod.Post, "/api/orders").Respond(HttpStatusCode.OK);

			var cut = RenderComponent<Cart>();

			cut.WaitForState(() => cut.FindAll("button.checkout_btn_1").Count > 0);
			cut.Find("button.checkout_btn_1").Click();

			cut.WaitForAssertion(() => Assert.AreEqual("http://localhost/orders/vieworders", navManager.Uri), timeout: TimeSpan.FromSeconds(1));
			mock.VerifyNoOutstandingExpectation();
		}
		[TestMethod]
		public async Task OnInitializedAsync_LoadsProductsAndRemoveAll()
		{
			// Arrange
			var mock = Services.AddMockHttpClient();
			mock.When(HttpMethod.Get, "/api/cart").RespondJson(new Product[] { new Product { Id = 1, Price = 10.0, Quantity = 2 }, new Product { Id = 2, Price = 20.0, Quantity = 4 } });
			var navManager = Services.GetRequiredService<FakeNavigationManager>();

			mock.When(HttpMethod.Post, "/api/cart/removeall").Respond(HttpStatusCode.OK);

			var cut = RenderComponent<Cart>();

			cut.WaitForState(() => cut.FindAll("button.removeAll").Count > 0);
			cut.Find("button.removeAll").Click();

			cut.WaitForAssertion(() => Assert.AreEqual("http://localhost/cart", navManager.Uri), timeout: TimeSpan.FromSeconds(1));
			mock.VerifyNoOutstandingExpectation();
		}
		[TestMethod]
		public async Task OnInitializedAsync_LoadsProductsAndRemoveOne()
		{
			// Arrange
			var mock = Services.AddMockHttpClient();
			mock.When(HttpMethod.Get, "/api/cart").RespondJson(new Product[] { new Product { Id = 1, Price = 10.0, Quantity = 2 }, new Product { Id = 2, Price = 20.0, Quantity = 4 } });
			var navManager = Services.GetRequiredService<FakeNavigationManager>();

			mock.When(HttpMethod.Post, "/api/cart/remove").Respond(HttpStatusCode.OK);

			var cut = RenderComponent<Cart>();

			cut.WaitForState(() => cut.FindAll("button.removeOne").Count > 0);
			cut.Find("button.removeOne").Click();

			cut.WaitForAssertion(() => Assert.AreEqual("http://localhost/cart", navManager.Uri), timeout: TimeSpan.FromSeconds(1));
			mock.VerifyNoOutstandingExpectation();
		}

	}
}
