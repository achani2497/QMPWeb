using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QueMePongo;
using queMePongo.Repositories;

namespace QMPWeb.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly IConfiguration config;

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Usuario user)
        {
            var helper = new Helper();

            helper.crearUsuario(user.usuario, user.contrasenia);

            return RedirectToAction("Index", "Home");

        }

        public class JsonUsuario
        {
            public string nombreUsuario { get; set; }
            public string contrasenia { get; set; }
        }

    }
}
