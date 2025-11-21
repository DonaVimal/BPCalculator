using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BPCalculator.BDDTests.Steps
{
    [Binding]
    public class BPRangeValidationSteps : BPCommonSteps
    {
        private IList<ValidationResult> _validationErrors = new List<ValidationResult>();

        [When("I validate the BP input range")]
        public void WhenIValidateTheBPInputRange()
        {
            Validator.TryValidateObject(_bp, new ValidationContext(_bp), _validationErrors, true);
        }

        [Then("the validation should fail with \"(.*)\"")]
        public void ThenTheValidationShouldFailWith(string message)
        {
            Assert.IsTrue(_validationErrors.Any());
            Assert.IsTrue(_validationErrors.Any(v => v.ErrorMessage == message));
        }
    }
}
