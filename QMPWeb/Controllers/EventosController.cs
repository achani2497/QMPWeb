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
        public IActionResult Index(int idUsuario)
        {
               if(idUsuario != 0){

                ViewBag.Id = idUsuario;

                return View("Eventos");
            } else {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
