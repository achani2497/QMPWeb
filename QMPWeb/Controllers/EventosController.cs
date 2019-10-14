using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QMPWeb.Models;

namespace QMPWeb.Controllers
{
    public class EventosController : Controller
    {
        public IActionResult Index()
        {
            return View("Eventos");
        }
    }
}
