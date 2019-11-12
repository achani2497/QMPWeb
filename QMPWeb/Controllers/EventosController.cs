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
                
                var userRepository = new UsuarioRepository();
                DB db = new DB();

                ViewBag.Eventos = db.eventos.FromSqlRaw($"Select * From eventos Where id_usuario = '{idUsuario}'").ToList();

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

            int idUser = Convert.ToInt32(form["idUsuario"]);

            evento.descripcion = form["despripcionEvento"];
            evento.fechaInicioPrendas = Convert.ToDateTime(form["idFechaIni"]);
            evento.fechaFinPrendas = Convert.ToDateTime(form["idFechaFin"]);
            evento.fechaNotificacion = evento.fechaInicioPrendas.AddHours(-1 * Convert.ToInt32(form["idRecord"])); //CHEQUEAR
            evento.id_usuario = idUser;
            evento.lugar = form["idLugar"];
            evento.tipoEvento = 1; //------------------------------------------TIPO DE EVENTO????????????????????

            EventoRepository eventoRepo = new EventoRepository();
            eventoRepo.Insert(evento, db);

            TempData["SuccessMessage"] = "Evento creado con exito! :D";

            return RedirectToAction("Index", "Eventos", new { idUsuario = idUser });

        }

        public IActionResult EliminarEvento(IFormCollection form)
        {

            string idEventoString = form["idEvento"];
            string idUsuarioString = form["idUsuario"];
            int idEvento = Convert.ToInt32(idEventoString);
            int idUser = Convert.ToInt32(idUsuarioString);

            EventoRepository eventoDAO = new EventoRepository();

            eventoDAO.Delete(idEvento);

            TempData["SuccessMessage"] = "Evento eliminado! :D";
            return RedirectToAction("Index", "Eventos", new { idUsuario = idUser });

        }

        /*
        [HttpPost]
        public IActionResult EditarGuardarropa(IFormCollection form)
        {


        }
        */

    }
}
