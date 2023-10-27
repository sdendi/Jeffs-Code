

using Alba;
using DemoApi.Services;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace DemoApi.ContractTests.Temperatures;
public class ConversionTests
{

	[Fact]
	public async Task CanConvertFromFToC()
	{

		// Given
		var expectedReponse = new ConversionResponse(212, 100);

		var host = await AlbaHost.For<Program>();

		var response = await host.Scenario(api =>
		{
			api.Get.Url("/temperatures/farenheit/212/celcius");
			api.StatusCodeShouldBeOk();
		});

		var message = response.ReadAsJson<ConversionResponse>();

		Assert.NotNull(message);

		Assert.Equal(expectedReponse, message);


	}
	[Fact]
	public async Task CanConvertFromCToF()
	{

		// Given
		var expectedReponse = new ConversionWithFeeResponse(212, 100, 0);

		var host = await AlbaHost.For<Program>(config =>
		{
			config.ConfigureServices(sp =>
			{
				var fakeFeeThing = Substitute.For<ICalculateFees>();

				sp.AddScoped<ICalculateFees>(s => fakeFeeThing);
			});
		});

		var response = await host.Scenario(api =>
		{
			api.Get.Url("/temperatures/celcius/100/farenheit");
			api.StatusCodeShouldBeOk();
		});

		var message = response.ReadAsJson<ConversionWithFeeResponse>();

		Assert.NotNull(message);

		Assert.Equal(expectedReponse, message);


	}
}
