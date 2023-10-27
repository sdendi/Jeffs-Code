
using Alba;
using DemoApi.Services;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace DemoApi.ContractTests.Todos;
public class GettingTodos
{

	[Fact]
	public async Task CanGetATodo()
	{
		var host = await AlbaHost.For<Program>();

		await host.Scenario(api =>
		{
			api.Get.Url("/todo-list/e9df7a6a-bdb7-422c-9dcc-b5bdb0bcd4b0");
			api.StatusCodeShouldBeOk();
		});



	}
	[Fact]
	public async Task IfNoTodoFourOhFourIsReturned()
	{
		var host = await AlbaHost.For<Program>();
		await host.Scenario(api =>
		{
			api.Get.Url("/todo-list/a30390fe-39a2-49a3-aa63-2708415ac675");
			api.StatusCodeShouldBe(404);

		});
	}
	[Fact(Skip = "Just a Demo")]
	public async Task WhatIfTheDatabaseIsDown()
	{
		var host = await AlbaHost.For<Program>(c =>
		{
			c.ConfigureServices(service =>
			{
				var fakeIDocumentSession = Substitute.For<IDocumentSession>();
				fakeIDocumentSession.When(d => d.Query<TodoItemCreated>()).Throw<ArgumentException>();
				service.AddScoped<IDocumentSession>(sp => fakeIDocumentSession);
			});
		});
		await host.Scenario(api =>
		{
			api.Get.Url("/todo-list/a30390fe-39a2-49a3-aa63-2708415ac675");
		});
	}

}
