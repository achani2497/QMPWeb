using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueMePongo;
using queMePongo.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QMPWeb.Controllers
{
    public class PrendasController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(int id)
        {
            guardarropaXusuarioRepository guardarropaDAO = new guardarropaXusuarioRepository();

            List<guardarropaXusuarioRepository> guardarropasParciales = guardarropaDAO.listarGuardarropasDeUsuario(id);

            guardarropaXprendaRepository prendasDAO = new guardarropaXprendaRepository();

            List<guardarropaXprendaRepository> prendasParciales = new List<guardarropaXprendaRepository>();

            foreach(guardarropaXusuarioRepository guarda in guardarropasParciales)
            {
                prendasParciales.AddRange(prendasDAO.listarPrendasDeGuardarropa(guarda.id_guardarropa)); 
            }
            
            ViewBag.Prendas = prendasParciales;

            ViewBag.Id = id;

            return View("Prendas");
        }

        /*
        [HttpPost]
        public IActionResult CrearPrenda(IFormCollection form)
        {

            DB db = new DB();
            Prenda prenda = new Prenda();

            int idUsuario = Convert.ToInt32(form["idUsuario"]);

            prenda.tipoPrenda = form["tipoDePrenda"];
            prenda.colorPrincipal = form["colorPrincipal"];
            prenda.colorSecundario = form["colorSecundario"];
            prenda.Tela = form["TipoDeTela"];
            prenda.Guardarropas = form["guardarropas"];

            PrendaRepository prendaRepo = new PrendaRepository();
            prendaRepo.Create(prenda, db, idUsuario);

            TempData["SuccessMessage"] = "Prenda creada con exito!";

            return RedirectToAction("Index", "Prendas", new { id = idUsuario });

        }*/
    }
}
