using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QMPWeb.Controllers
{
    public class PrendasController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(int idUsuario)
        {
            if(idUsuario != 0){

                ViewBag.Id = idUsuario;

                return View("Prendas");
            } else {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
