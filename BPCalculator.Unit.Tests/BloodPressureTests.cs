using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using BPCalculator;

namespace BPCalculator.Unit.Tests
{
    [TestClass]
    public class BloodPressureTests
    {
        // ... [Existing tests from previous steps] ...

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
            var category = bp.Category;
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

        // --- NEW TESTS FOR FULL COVERAGE ---

        [TestMethod]
        public void CalculateCategory_TracksTelemetryWhenHookProvided()
        {
            var bp = new BloodPressure { Systolic = 150, Diastolic = 95 };
            bool called = false;
            void Hook(string name, IDictionary<string, string> props)
            {
                called = true;
                Assert.AreEqual("BP_Calc", name);
                Assert.AreEqual("High", props["result"]);
            }

            var cat = bp.CalculateCategory(Hook);
            Assert.AreEqual(BPCategory.High, cat);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void CalculateCategory_PreHigh_TracksTelemetry()
        {
            var bp = new BloodPressure { Systolic = 130, Diastolic = 85 };
            bool called = false;
            void Hook(string name, IDictionary<string, string> props) => called = true;

            bp.CalculateCategory(Hook);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void CalculateCategory_Ideal_TracksTelemetry()
        {
            var bp = new BloodPressure { Systolic = 110, Diastolic = 70 };
            bool called = false;
            void Hook(string name, IDictionary<string, string> props) => called = true;

            bp.CalculateCategory(Hook);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void CalculateCategory_Low_TracksTelemetry()
        {
            var bp = new BloodPressure { Systolic = 80, Diastolic = 50 };
            bool called = false;
            void Hook(string name, IDictionary<string, string> props) => called = true;

            bp.CalculateCategory(Hook);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void CalculateCategory_InvalidLogic_TracksTelemetryBeforeException()
        {
            // Covers logic error telemetry (Systolic < Diastolic)
            var bp = new BloodPressure { Systolic = 80, Diastolic = 90 };
            bool called = false;
            void Hook(string name, IDictionary<string, string> props) => called = true;

            Assert.ThrowsException<InvalidOperationException>(() => bp.CalculateCategory(Hook));
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void CalculateCategory_InvalidRange_ThrowsArgumentException()
        {
            // Covers the Data Annotation validation block (if !isValid)
            var bp = new BloodPressure { Systolic = 60, Diastolic = 80 }; // 60 is < 70 (Min)

            var ex = Assert.ThrowsException<ArgumentException>(() => bp.CalculateCategory());
            Assert.AreEqual("Invalid Systolic Value", ex.Message);
        }
    }
}