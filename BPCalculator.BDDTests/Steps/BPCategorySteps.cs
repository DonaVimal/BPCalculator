using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reqnroll;

namespace BPCalculator.BDDTests.Steps
{
    [Binding]
    public class BPCategorySteps : BPCommonSteps
    {
        [Then("the result should be \"(.*)\"")]
        public void ThenTheResultShouldBe(string expectedCategory)
        {
            Assert.AreEqual(expectedCategory, _result);
        }
    }
}
