namespace StringCalculator;
public class StringCalculatorInterationTests
{
	private readonly StringCalculator _calculator;
	private readonly ILogger _logger;
	private readonly IWebService _webService;
	public StringCalculatorInterationTests()
	{
		_logger = Substitute.For<ILogger>();
		_webService = Substitute.For<IWebService>();
		_calculator = new StringCalculator(_logger, _webService);
	}

	[Theory]
	[InlineData("1")]
	[InlineData("2")]
	[InlineData("1,3")]
	public void WritesToLogger(string numbers)
	{
		// when 
		var result = _calculator.Add(numbers);

		// then
		// Does "1" get written to the logger's write method?
		_logger.Received().Write(result.ToString());
		_webService.DidNotReceive().NofityOfLoggingFailure();
	}

	[Fact]
	public void SendsMessageToApiIfLoggerFails()
	{
		_logger.When(m => m.Write(Arg.Any<string>()))
			.Do(x => throw new LoggingException());
		// Stub - just provides canned answers.
		//_logger.When(b => b.Write("999")).Throws<LoggingException>();
		_calculator.Add("999");


		_webService.Received(1).NofityOfLoggingFailure();

	}

}

// Mock is when we use a test double to record interactions
// and we verify it was used properly.
// Mocks are the "assert" "then" they pass or fail the test.
