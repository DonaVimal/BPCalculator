using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BPCalculator
{
    // BP categories
    public enum BPCategory
    {
        [Display(Name = "Low Blood Pressure")] Low,
        [Display(Name = "Ideal Blood Pressure")] Ideal,
        [Display(Name = "Pre-High Blood Pressure")] PreHigh,
        [Display(Name = "High Blood Pressure")] High
    };

    public class BloodPressure
    {
        public const int SystolicMin = 70;
        public const int SystolicMax = 190;
        public const int DiastolicMin = 40;
        public const int DiastolicMax = 100;

        [Range(SystolicMin, SystolicMax, ErrorMessage = "Invalid Systolic Value")]
        public int Systolic { get; set; }                       // mmHG

        [Range(DiastolicMin, DiastolicMax, ErrorMessage = "Invalid Diastolic Value")]
        public int Diastolic { get; set; }                      // mmHG

        public bool IsValidReading => Systolic > Diastolic;

        public BPCategory Category => CalculateCategory();

        public BPCategory CalculateCategory(Action<string, IDictionary<string, string>> telemetryHook = null)
        {
            // Basic validation: systolic must be higher than diastolic
            if (!IsValidReading)
            {
                telemetryHook?.Invoke("BP_Calc", new Dictionary<string, string>
                {
                    ["systolic"] = Systolic.ToString(),
                    ["diastolic"] = Diastolic.ToString(),
                    ["result"] = "InvalidReading"
                });
                throw new InvalidOperationException("Systolic pressure must be higher than diastolic pressure.");
            }

            // Ranges (lower limits inclusive)
            if (Systolic >= 140 || Diastolic >= 90)
            {
                telemetryHook?.Invoke("BP_Calc", new Dictionary<string, string>
                {
                    ["systolic"] = Systolic.ToString(),
                    ["diastolic"] = Diastolic.ToString(),
                    ["result"] = "High"
                });
                return BPCategory.High;
            }

            if (Systolic >= 120 || Diastolic >= 80)
            {
                telemetryHook?.Invoke("BP_Calc", new Dictionary<string, string>
                {
                    ["systolic"] = Systolic.ToString(),
                    ["diastolic"] = Diastolic.ToString(),
                    ["result"] = "PreHigh"
                });
                return BPCategory.PreHigh;
            }

            if (Systolic >= 90 || Diastolic >= 60)
            {
                telemetryHook?.Invoke("BP_Calc", new Dictionary<string, string>
                {
                    ["systolic"] = Systolic.ToString(),
                    ["diastolic"] = Diastolic.ToString(),
                    ["result"] = "Ideal"
                });
                return BPCategory.Ideal;
            }

            // Low
            telemetryHook?.Invoke("BP_Calc", new Dictionary<string, string>
            {
                ["systolic"] = Systolic.ToString(),
                ["diastolic"] = Diastolic.ToString(),
                ["result"] = "Low"
            });
            return BPCategory.Low;
        }

        // New: Validate input ranges using DataAnnotations. Returns validation messages.
        public bool Validate(out IList<string> validationMessages)
        {
            var context = new ValidationContext(this);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(this, context, results, validateAllProperties: true);

            validationMessages = new List<string>();
            foreach (var r in results)
            {
                validationMessages.Add(r.ErrorMessage);
            }

            return isValid;
        }
    }
}
