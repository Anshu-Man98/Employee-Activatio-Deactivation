using System;
using EmployeeDeactivation.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDeactivation.Controllers
{

    public class PdfController : Controller
    {
        private readonly IPdfDataOperation _pdfDataOperation;

        public PdfController(IPdfDataOperation pdfDataOperation)
        {
            _pdfDataOperation = pdfDataOperation;
        }
        [HttpGet]
        [Route("Pdf/CreateDeactivationPdf")]
        public IActionResult CreateDeactivationPdf(string gId)
        {
           return Json("data:application/pdf;base64," + Convert.ToBase64String(_pdfDataOperation.FillDeactivationPdfForm(gId)));
        }

        [HttpGet]
        [Route("Pdf/CreateActivationPdf")]
        public IActionResult CreateActivationPdf(string gId)
        {
            return Json("data:application/pdf;base64," + Convert.ToBase64String(_pdfDataOperation.FillActivationPdfForm(gId)));
        }
        
    }
}
