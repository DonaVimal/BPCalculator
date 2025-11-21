using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;
using System;

namespace BPCalculator.BDDTests.Steps
{
    [Binding]
    public class BPInvalidReadingSteps : BPCommonSteps
    {
        [Then("an invalid reading error should be thrown")]
        public void ThenAnInvalidReadingErrorShouldBeThrown()
        {
            Assert.IsNotNull(_caughtException);
            Assert.IsInstanceOfType(_caughtException, typeof(InvalidOperationException));
            Assert.AreEqual("Systolic pressure must be higher than diastolic pressure.",
                            _caughtException.Message);
        }
    }
}
