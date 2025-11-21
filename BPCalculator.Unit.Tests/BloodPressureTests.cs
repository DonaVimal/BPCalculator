

namespace BPCalculator.Unit.Tests
{
    [TestClass]
    public class BloodPressureTests
    {
        [TestMethod]
        public void Category_LowBloodPressure()
        {
            var bp = new BloodPressure { Systolic = 80, Diastolic = 50 };
            Assert.AreEqual(BPCategory.Low, bp.Category);
        }
        [TestMethod]
        public void Category_IdealBloodPressure()
        {
            var bp = new BloodPressure { Systolic = 110, Diastolic = 70 };
            Assert.AreEqual(BPCategory.Ideal, bp.Category);
        }
        [TestMethod]
        public void Category_PreHighBloodPressure()
        {
            var bp = new BloodPressure { Systolic = 130, Diastolic = 85 };
            Assert.AreEqual(BPCategory.PreHigh, bp.Category);
        }
        [TestMethod]
        public void Category_HighBloodPressure()
        {
            var bp = new BloodPressure { Systolic = 150, Diastolic = 95 };
            Assert.AreEqual(BPCategory.High, bp.Category);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Category_InvalidReading_ThrowsException()
        {
            var bp = new BloodPressure { Systolic = 80, Diastolic = 90 };
            var category = bp.Category; // This should throw
        }
        [TestMethod]
        public void IsValidReading_Valid()
        {
            var bp = new BloodPressure { Systolic = 120, Diastolic = 80 };
            Assert.IsTrue(bp.IsValidReading);
        }
        [TestMethod]
        public void IsValidReading_Invalid()
        {
            var bp = new BloodPressure { Systolic = 80, Diastolic = 90 };
            Assert.IsFalse(bp.IsValidReading);
        }
        [TestMethod]
        public void Validate_OutOfRange_SystolicTooLow()
        {
            var bp = new BloodPressure { Systolic = 60, Diastolic = 70 };
            var ok = bp.Validate(out IList<string> messages);
            Assert.IsFalse(ok);
            Assert.IsTrue(messages.Contains("Invalid Systolic Value"));
        }

        [TestMethod]
        public void Validate_OutOfRange_DiastolicTooHigh()
        {
            var bp = new BloodPressure { Systolic = 120, Diastolic = 150 };
            var ok = bp.Validate(out IList<string> messages);
            Assert.IsFalse(ok);
            Assert.IsTrue(messages.Contains("Invalid Diastolic Value"));
        }

        [TestMethod]
        public void CalculateCategory_TracksTelemetryWhenHookProvided()
        {
            var bp = new BloodPressure { Systolic = 150, Diastolic = 95 };
            bool called = false;
            void Hook(string name, System.Collections.Generic.IDictionary<string, string> props)
            {
                called = true;
                Assert.AreEqual("BP_Calc", name);
                Assert.AreEqual("150", props["systolic"]);
                Assert.AreEqual("95", props["diastolic"]);
                Assert.AreEqual("High", props["result"]);
            }

            var cat = bp.CalculateCategory(Hook);
            Assert.AreEqual(BPCategory.High, cat);
            Assert.IsTrue(called);
        }
    }
}

