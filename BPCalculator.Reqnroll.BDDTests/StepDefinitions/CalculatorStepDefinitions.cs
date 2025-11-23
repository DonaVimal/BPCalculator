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
    }
}
