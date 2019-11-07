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
    public class GuardarropasController : Controller
    {

        public IActionResult Index(int id)
        {
            var userRepository = new UsuarioRepository();
            guardarropaXusuarioRepository guardarropaDAO = new guardarropaXusuarioRepository();

            List<guardarropaXusuarioRepository> guardarropasParciales = guardarropaDAO.listarGuardarropasDeUsuario(id);
            ViewBag.Guardarropas = guardarropasParciales;

            ViewBag.Id = id;

            return View("Guardarropas");
        }

        [HttpPost]
        public IActionResult CrearGuardarropa(IFormCollection form){

            DB db = new DB();
            Guardarropa guardarropa = new Guardarropa();

            int idUsuario = Convert.ToInt32(form["idUsuario"]);

            guardarropa.nombreGuardarropas = form["nombreGuardarropa"];
            guardarropa.id_duenio = idUsuario;

            GuardarropaRepository guardarropaRepo = new GuardarropaRepository();
            guardarropaRepo.Create(guardarropa, db, idUsuario);

            TempData["SuccessMessage"] = "Guardarropa "+guardarropa.nombreGuardarropas+" creado con exito!";

            return RedirectToAction("Index", "Guardarropas", new {id = idUsuario});

        }

        public IActionResult EliminarGuardarropa(int idGuardarropa, int idUsuario)
        {

            GuardarropaRepository guardarropaDAO = new GuardarropaRepository();

            var mensaje = guardarropaDAO.Delete(idGuardarropa, idUsuario);

            TempData["SuccessMessage"] = mensaje;
            return RedirectToAction("Index", "Guardarropas", new {id = idUsuario});

        }

        [HttpPost]
        public IActionResult CompartirGuardarropa(IFormCollection form){

            //Hago estos pasamanos asquerosos porque no me deja parsear directamente
            String nombreDeUsuarioACompartir = form["nombreUsuarioACompartir"];
            String idUsuarioDuenioString = form["idUsuarioDuenio"];
            String idGuardarropaACompartirString = form["idGuardarropaACompartir"];
            int idUsuarioDuenio = Convert.ToInt32(idUsuarioDuenioString);
            int idGuardarropaACompartir = Convert.ToInt32(idGuardarropaACompartirString);

            UsuarioRepository userDAO = new UsuarioRepository();
            GuardarropaRepository guardarropaDAO = new GuardarropaRepository();

            Usuario usuarioDuenio = userDAO.BuscarUsuarioPorId(idUsuarioDuenio);
            Usuario usuarioParaCompartir = userDAO.BuscarUsuarioPorUsername(nombreDeUsuarioACompartir);

            Guardarropa guardarropaParaCompartir = guardarropaDAO.buscarGuardarropaPorIdYPorDuenio(idGuardarropaACompartir, idUsuarioDuenio);
            
            int respuesta = usuarioDuenio.compartirGuardarropa(guardarropaParaCompartir, usuarioParaCompartir);

            switch(respuesta){
                case 0:

                    TempData["SuccessMessage"] = "Guardarropa compartido con "+ usuarioParaCompartir.usuario +" :D !";

                    return RedirectToAction("Index", "Guardarropas", new {id = idUsuarioDuenio});

                case 1:

                    TempData["ErrorMessage"] = "No se puede compartir el guardarropas con el usuario " + nombreDeUsuarioACompartir + " porque es de un tipo de usuario inferior al tuyo!";

                    return RedirectToAction("Index", "Guardarropas", new {id = idUsuarioDuenio});

                case 2:

                    TempData["ErrorMessage"] = "Ya compartiste el guardarropa "+ guardarropaParaCompartir.nombreGuardarropas +" con el usuario " + nombreDeUsuarioACompartir + "!";

                    return RedirectToAction("Index", "Guardarropas", new {id = idUsuarioDuenio});

                case 3:

                    TempData["ErrorMessage"] = "El usuario " + nombreDeUsuarioACompartir + " no existe!";

                    return RedirectToAction("Index", "Guardarropas", new {id = idUsuarioDuenio});

                default:

                    return RedirectToAction("Index", "Login");

            }

        }
        
        [HttpPost]
        public IActionResult EditarGuardarropa(IFormCollection form){

            GuardarropaRepository guardarropaDAO = new GuardarropaRepository();

            String idGuardarropaString = form["idGuardarropa"];
            String idUsuarioString = form["idUsuario"];
            int idGuardarropa = Convert.ToInt32(idGuardarropaString);
            int idUsuario = Convert.ToInt32(idUsuarioString);

            String nuevoNombreGuardarropa = form["nuevoNombreGuardarropa"];
            String nombreViejoGuardarropa = form["nombreViejoGuardarropa"];

            
            if(guardarropaDAO.TryUpdate(idGuardarropa, idUsuario, nuevoNombreGuardarropa)){

                TempData["SuccessMessage"] = "Modificaste el nombre del guardarropa '" + nombreViejoGuardarropa + "' a '" + nuevoNombreGuardarropa + "' con exito :D !";

                return RedirectToAction("Index", "Guardarropas", new {id = idUsuario});

            } else {

                TempData["ErrorMessage"] = "No podes editar el guardarropa " + nombreViejoGuardarropa + " porque no sos el dueño";

                return RedirectToAction("Index", "Guardarropas", new {id = idUsuario});

            }

        }

        public class JsonGuardarropa
        {
            public int idUsuario { get; set; }
            public string nombreGuardarropa { get; set; }
        }
    }
}
