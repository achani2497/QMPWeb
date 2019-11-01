using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueMePongo;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QMPWeb.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult login(IFormCollection form)
        {
            DB db = new DB();

            String nombreUsuario = form["usuario"];

            Usuario user = db.usuarios.FromSqlRaw($"Select * From Usuarios Where usuario ='{nombreUsuario}'").FirstOrDefault();

            if(user != null && user.contrasenia == form["contrasenia"]){
                ViewResult vista = View("~/Views/Home/Index.cshtml");
                ViewBag.Id = user.id_usuario;
                ViewBag.NombreUsuario = user.usuario;

                return vista;

            } else {
                ViewResult vista = View();
                vista.ViewData.Add("mensajeDeError", "Usuario o Contraseña incorrectos");

                return vista;

            }

        }
    }
}
