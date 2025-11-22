namespace Morsley.UK.DarkBootstrap.UI.IntegrationTests;

public class MessagePageTests
{
	private IWebDriver _driver;

    private const string SendMessageFormId = "send-message-form";
    private const string NameInputId = "name-input";
    private const string SendMessageButtonId = "send-message-button";

    // Validation Classes...
    private const string FormNeedsValidationClass = "needs-validation";
    private const string FormWasValidatedClass = "was-validated";

    [SetUp]
	public void Setup()
	{
		var options = new ChromeOptions();
		//options.AddArgument("--headless=new");
		_driver = new ChromeDriver(options);
	}

	[TearDown]
	public void TearDown()
	{
		_driver?.Quit();
		_driver?.Dispose();
	}

	[Test]
	public void MessagePage_CanLoadAndClickSend()
	{
		// Arrange...
			
		// Navigate to the root of the site that hosts the message page
		_driver.Navigate().GoToUrl(TestHostFixture.MessageUrl);

		// Ensure the form exists
		var form = _driver.FindElement(By.Id(SendMessageFormId));
		form.ShouldNotBeNull();

		// Ensure the form has only a single 'needs-validation' class
		var beforeClasses = form.GetAttribute("class");
        beforeClasses.ShouldNotBeNull();
        beforeClasses.ShouldContain(FormNeedsValidationClass);

		// Find and click the Send button
		var sendButton = _driver.FindElement(By.Id(SendMessageButtonId));
		sendButton.ShouldNotBeNull();
		sendButton.Click();

        // Ensure the form has only a single 'needs-validation' class
        var afterClasses = form.GetAttribute("class");
        afterClasses.ShouldNotBeNull();
        afterClasses.ShouldContain(FormNeedsValidationClass);
        afterClasses.ShouldContain(FormWasValidatedClass);

        // At this stage we just assert that we were able to click without errors.
        // You can extend this with assertions against success messages or navigation.

    }
}