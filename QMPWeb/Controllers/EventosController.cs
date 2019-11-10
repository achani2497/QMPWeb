using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueMePongo;
using queMePongo.Repositories;

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

        
        [HttpPost]
        public IActionResult CrearEvento(IFormCollection form)
        {

            DB db = new DB();
            Evento evento = new Evento();

            int idUsuario = Convert.ToInt32(form["idUsuario"]);

            evento.descripcion = form["despripcionEvento"];
            evento.fechaInicioPrendas = Convert.ToDateTime(form["idFechaIni"]);
            evento.fechaFinPrendas = Convert.ToDateTime(form["idFechaFin"]);
            evento.fechaNotificacion = evento.fechaInicioPrendas.AddHours(-1 * Convert.ToInt32(form["idRecord"])); //CHEQUEAR
            evento.id_usuario = idUsuario;
            evento.lugar = form["idLugar"];
            // AGREGAR USER?

            EventoRepository eventoRepo = new EventoRepository();
            eventoRepo.Insert(evento, db);

            TempData["SuccessMessage"] = "Evento creado con exito!";

            return RedirectToAction("Index", "Eventos", new { id = idUsuario });

        }

        public IActionResult EliminarEvento(int idEvento, int idUsuario)
        {

            EventoRepository eventoDAO = new EventoRepository();

            eventoDAO.Delete(idEvento);

            TempData["SuccessMessage"] = "Evento eliminado";
            return RedirectToAction("Index", "Evento", new { id = idUsuario });

        }

        /*
        [HttpPost]
        public IActionResult EditarGuardarropa(IFormCollection form)
        {


        }
        */

    }
}
