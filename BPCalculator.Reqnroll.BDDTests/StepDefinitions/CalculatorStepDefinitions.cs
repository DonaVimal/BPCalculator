using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BPCalculator.Reqnroll.BDDTests.StepDefinitions
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        protected BloodPressure _bp;
        protected Exception _caughtException;
        protected string _result;

        public CalculatorStepDefinitions()
        {
            _bp = new BloodPressure();
        }

        [Given("my systolic value is {int}")]
        public void GivenMySystolicValueIs(int systolic)
        {
            _bp.Systolic = systolic;
        }

        [Given("my diastolic value is {int}")]
        public void GivenMyDiastolicValueIs(int diastolic)
        {
            _bp.Diastolic = diastolic;
        }

        [When("I evaluate the BP category")]
        public void WhenIEvaluateTheBPCategory()
        {
            try
            {
                _result = _bp.Category.ToString();
            }
            catch (Exception ex)
            {
                _caughtException = ex;
            }
        }

        [Then("the result should be {string}")]
        public void ThenTheResultShouldBe(string expectedResult)
        {
            // 1. Ensure no exception occurred during the 'When' step
            if (_caughtException != null)
            {
                Assert.Fail($"Expected result '{expectedResult}', but an exception was thrown: {_caughtException.Message}");
            }

            // 2. Compare the actual result with the expected result from the Feature file
            // Note: Using Assert.AreEqual (MSTest) based on your stack trace.
            Assert.AreEqual(expectedResult, _result, "The calculated blood pressure category is incorrect.");
        }

        [Then("an invalid reading error should be thrown")]
        public void ThenAnInvalidReadingErrorShouldBeThrown()
        {
            // Assert that the 'When' step caught an exception
            Assert.IsNotNull(_caughtException, "Expected an invalid reading error, but no exception was thrown.");
        }
        [When("I validate the BP input range")]
        public void WhenIValidateTheBPInputRange()
        {
            try
            {
                // Accessing the Category property triggers the validation logic in the BloodPressure class
                var category = _bp.Category;
            }
            catch (Exception ex)
            {
                _caughtException = ex;
            }
        }

        [Then("the validation should fail with {string}")]
        public void ThenTheValidationShouldFailWith(string expectedMessage)
        {
            // 1. Ensure an error occurred
            Assert.IsNotNull(_caughtException, "Expected a validation error, but no exception was thrown.");

            // 2. Verify the error message matches the feature file example
            // We use AreEqual to check for the specific message (e.g., "Invalid Systolic Value")
            Assert.AreEqual(expectedMessage, _caughtException.Message);
        }
    }
}
