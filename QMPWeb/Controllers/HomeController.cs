using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueMePongo;

namespace QMPWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(int? idUsuario)
        {

            if(idUsuario != null){
                DB db = new DB();

                Usuario usuario = db.usuarios.FromSqlRaw($"Select * From usuarios Where id_usuario = '{idUsuario}'").FirstOrDefault();

                ViewBag.Id = usuario.id_usuario;
                ViewBag.NombreUsuario = usuario.usuario;

                return View();

            } else {

                return RedirectToAction("Index", "Login");

            }

        }
    }
}
