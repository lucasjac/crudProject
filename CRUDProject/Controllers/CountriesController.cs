using Microsoft.AspNetCore.Mvc;

namespace CRUDProject.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        [Route("UpdloadFromExcel")]
        public IActionResult UploadFromExcel()
        {
            return View();
        }

        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select an xlsx file.";
                return View();
            }

            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Please select an xlsx file.";
                return View();
            }
        }
    }
}
