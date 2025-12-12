using Microsoft.Playwright;
using Microsoft.Playwright.MSTest; // <--- Important for MSTest base classes
using Microsoft.VisualStudio.TestTools.UnitTesting; // <--- Standard MSTest namespace
using System.Threading.Tasks;

namespace BPCalculator.E2ETests
{
    [TestClass]
    public class BPCalculatorTests : PageTest
    {
        // Remove the 'const' string and replace with this property:
        private string AppUrl => Environment.GetEnvironmentVariable("AppUrl")
                                 ?? "http://localhost:5038"; // Fallback for local testing        

        [TestInitialize]
        public async Task Setup()
        {
            // Navigate to the app before every test
            await Page.GotoAsync(AppUrl);
        }

        [TestMethod]
        public async Task Page_Load_HasCorrectTitle()
        {
            await Expect(Page).ToHaveTitleAsync("BP Category Calculator - BPCalculator");
            await Expect(Page.Locator("h4")).ToHaveTextAsync("BP Category Calculator");
        }

        [TestMethod]
        public async Task Calculate_HighBloodPressure_ReturnsHighCategory()
        {
            // Arrange
            await Page.FillAsync("#BP_Systolic", "150");
            await Page.FillAsync("#BP_Diastolic", "95");

            // Act
            await Page.ClickAsync("input[value='Submit']");

            // Assert
            var alertBox = Page.Locator(".alert-info");
            await Expect(alertBox).ToBeVisibleAsync();
            await Expect(alertBox).ToContainTextAsync("High");
            await Expect(alertBox).ToContainTextAsync("High BP: seek medical advice");
        }

        [TestMethod]
        public async Task Calculate_PreHighBloodPressure_ReturnsPreHighCategory()
        {
            // Arrange
            await Page.FillAsync("#BP_Systolic", "130");
            await Page.FillAsync("#BP_Diastolic", "85");

            // Act
            await Page.ClickAsync("input[value='Submit']");

            // Assert
            var alertBox = Page.Locator(".alert-info");
            await Expect(alertBox).ToContainTextAsync("Pre-High");
        }

        [TestMethod]
        public async Task Calculate_IdealBloodPressure_ReturnsIdealCategory()
        {
            // Arrange
            await Page.FillAsync("#BP_Systolic", "110");
            await Page.FillAsync("#BP_Diastolic", "70");

            // Act
            await Page.ClickAsync("input[value='Submit']");

            // Assert
            var alertBox = Page.Locator(".alert-info");
            await Expect(alertBox).ToContainTextAsync("Ideal");
        }

        [TestMethod]
        public async Task Calculate_LowBloodPressure_ReturnsLowCategory()
        {
            // Arrange
            await Page.FillAsync("#BP_Systolic", "80");
            await Page.FillAsync("#BP_Diastolic", "50");

            // Act
            await Page.ClickAsync("input[value='Submit']");

            // Assert
            var alertBox = Page.Locator(".alert-info");
            await Expect(alertBox).ToContainTextAsync("Low");
        }

        [TestMethod]
        public async Task Validation_InputAboveMax_ShowsRangeError()
        {
            // Arrange
            await Page.FillAsync("#BP_Systolic", "200");
            await Page.FillAsync("#BP_Diastolic", "80");

            // Act
            await Page.ClickAsync("input[value='Submit']");

            // Assert
            var errorSpan = Page.Locator("span[data-valmsg-for='BP.Systolic']");
            await Expect(errorSpan).ToContainTextAsync("Invalid Systolic Value");
        }

        [TestMethod]
        public async Task Validation_InputBelowMin_ShowsRangeError()
        {
            // Arrange
            await Page.FillAsync("#BP_Systolic", "120");
            await Page.FillAsync("#BP_Diastolic", "30");

            // Act
            await Page.ClickAsync("input[value='Submit']");

            // Assert
            var errorSpan = Page.Locator("span[data-valmsg-for='BP.Diastolic']");
            await Expect(errorSpan).ToContainTextAsync("Invalid Diastolic Value");
        }
    }
}
