using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ASJ.Controllers
{
    public class PDFController : Controller
    {
        public IActionResult Index(int instrumentId, int organizationId, int year)
        {
            return View();

        }
    }
}