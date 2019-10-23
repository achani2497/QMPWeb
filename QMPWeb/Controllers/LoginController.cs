using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QueMePongo;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QMPWeb.Controllers
{
    public class LoginController : Controller
    {
        //DB db = new DB();
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public void login(IFormCollection form)
        {

            ViewResult vista = View("~/Views/Home/Index.cshtml");

            //Usuario user = db.usuarios.Where(user => user.usuario == form["user"]).Select(user => user).Single();

            //vista.ViewData.Add("usuario", user);

        }
    }
}
