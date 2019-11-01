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

            DB db = new DB();

            List<guardarropaXusuarioRepository> guardarropasParciales = db.guardarropaXusuarioRepositories.FromSqlRaw($"Select * From guardarropaxusuario Where id_usuario = '{id}'").ToList();
            ViewBag.Guardarropas = guardarropasParciales;

            ViewBag.Id = id;

            return View("Guardarropas");
        }

        [HttpPost]
        public JsonResult crearGuardarropa([FromBody] JsonGuardarropa jsonGuardarropa){

            DB db = new DB();
            Guardarropa guardarropa = new Guardarropa();

            int idUsuario = jsonGuardarropa.idUsuario;

            guardarropa.nombreGuardarropas = jsonGuardarropa.nombreGuardarropa;
            guardarropa.id_duenio = idUsuario;

            GuardarropaRepository guardarropaRepo = new GuardarropaRepository();
            guardarropaRepo.Insert(guardarropa, db, idUsuario);

            return Json("'Success: true'");

        }

        public IActionResult EliminarGuardarropa(int idGuardarropa, int idUsuario)
        {

            DB db = new DB();
            
            GuardarropaRepository guardarropaRepo = new GuardarropaRepository();

            Guardarropa guardarropa = db.guardarropas.FromSqlRaw($"Select * From guardarropas Where id_guardarropa = '{idGuardarropa}'").FirstOrDefault();

            if(guardarropa.id_duenio == idUsuario){
                db.Remove(db.guardarropaXusuarioRepositories.Single(gxu => gxu.id_guardarropa == idGuardarropa));
                db.SaveChanges();
            } else {
                db.Remove(db.guardarropaXusuarioRepositories.Single(gxu => gxu.id_guardarropa == idGuardarropa));
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Guardarropas", new {id = idUsuario});

        }

        public class JsonGuardarropa
        {
            public int idUsuario { get; set; }
            public string nombreGuardarropa { get; set; }
        }
    }
}
