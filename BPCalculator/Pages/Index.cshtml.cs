using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

// page model

namespace BPCalculator.Pages
{
    public class BloodPressureModel : PageModel
    {
        [BindProperty]                              // bound on POST
        public BloodPressure BP { get; set; }

        // setup initial data
        public void OnGet()
        {
            BP = new BloodPressure() { Systolic = 100, Diastolic = 60 };
        }

        // POST, validate
        public IActionResult OnPost()
        {
            // If invalid range, return to page and display default model validation
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Calculate category
            var category = BP.Category;

            // Store category for UI
            ViewData["Category"] = category.ToString();

            // New Feature: Add guidance message
            ViewData["Guidance"] = BpGuidance.GuidanceFor(category);

            // Store last reading in session
            HttpContext.Session.SetString("LastBP", $"{BP.Systolic}/{BP.Diastolic} — {category}");

            ViewData["LastBP"] = HttpContext.Session.GetString("LastBP");

            // extra validation
            if (!(BP.Systolic > BP.Diastolic))
            {
                ModelState.AddModelError("", "Systolic must be greater than Diastolic");
            }
            return Page();
        }
    }
}