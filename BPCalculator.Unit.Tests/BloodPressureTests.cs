using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using BPCalculator;

namespace BPCalculator.Unit.Tests
{
    [TestClass]
    public class BloodPressureTests
    {
        // ==========================================
        // EXISTING TESTS (Kept as provided)
        // ==========================================

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
            void Hook(string name, IDictionary<string, string> props)
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

        // ==========================================
        // NEW TESTS ADDED FOR CODE COVERAGE
        // ==========================================

        [TestMethod]
        public void CalculateCategory_PreHigh_TracksTelemetry()
        {
            // Covers the "PreHigh" telemetry block
            var bp = new BloodPressure { Systolic = 130, Diastolic = 85 };
            bool called = false;

            void Hook(string name, IDictionary<string, string> props)
            {
                called = true;
                Assert.AreEqual("PreHigh", props["result"]);
            }

            var cat = bp.CalculateCategory(Hook);
            Assert.AreEqual(BPCategory.PreHigh, cat);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void CalculateCategory_Ideal_TracksTelemetry()
        {
            // Covers the "Ideal" telemetry block
            var bp = new BloodPressure { Systolic = 110, Diastolic = 70 };
            bool called = false;

            void Hook(string name, IDictionary<string, string> props)
            {
                called = true;
                Assert.AreEqual("Ideal", props["result"]);
            }

            var cat = bp.CalculateCategory(Hook);
            Assert.AreEqual(BPCategory.Ideal, cat);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void CalculateCategory_Low_TracksTelemetry()
        {
            // Covers the "Low" telemetry block
            var bp = new BloodPressure { Systolic = 80, Diastolic = 50 };
            bool called = false;

            void Hook(string name, IDictionary<string, string> props)
            {
                called = true;
                Assert.AreEqual("Low", props["result"]);
            }

            var cat = bp.CalculateCategory(Hook);
            Assert.AreEqual(BPCategory.Low, cat);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void CalculateCategory_InvalidLogic_TracksTelemetryBeforeException()
        {
            // Covers the "InvalidReading" telemetry block (Screenshot 1)
            var bp = new BloodPressure { Systolic = 80, Diastolic = 90 }; // Invalid because Systolic < Diastolic
            bool called = false;

            void Hook(string name, IDictionary<string, string> props)
            {
                called = true;
                Assert.AreEqual("InvalidReading", props["result"]);
                Assert.AreEqual("80", props["systolic"]);
                Assert.AreEqual("90", props["diastolic"]);
            }

            // We use Assert.ThrowsException here instead of [ExpectedException] 
            // so we can verify the 'called' variable afterwards.
            Assert.ThrowsException<InvalidOperationException>(() => bp.CalculateCategory(Hook));

            // This asserts that the telemetry code ran BEFORE the exception was thrown
            Assert.IsTrue(called, "Telemetry hook should have been called before exception");
        }
    }
}