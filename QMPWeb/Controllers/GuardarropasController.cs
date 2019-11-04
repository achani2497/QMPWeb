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

        public IActionResult Index(int id, int? error)
        {

            var userRepository = new UsuarioRepository();

            DB db = new DB();

            List<guardarropaXusuarioRepository> guardarropasParciales = db.guardarropaXusuarioRepositories.FromSqlRaw($"Select * From guardarropaxusuario Where id_usuario = '{id}'").ToList();
            ViewBag.Guardarropas = guardarropasParciales;

            if(error != null){
                Error err = db.errores.FromSqlRaw($"Select * From errores Where id_error = '{error}'").FirstOrDefault();
                ViewBag.ErrorDeComparticion = err.descripcion;
            }

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
            guardarropaRepo.Insert(guardarropa, db, idUsuario);

            TempData["SuccessMessage"] = "Guardarropa "+guardarropa.nombreGuardarropas+" creado con exito!";

            return RedirectToAction("Index", "Guardarropas", new {id = idUsuario});

        }

        public IActionResult EliminarGuardarropa(int idGuardarropa, int idUsuario)
        {

            DB db = new DB();
            
            GuardarropaRepository guardarropaRepo = new GuardarropaRepository();

            Guardarropa guardarropa = db.guardarropas.FromSqlRaw($"Select * From guardarropas Where id_guardarropa = '{idGuardarropa}'").FirstOrDefault();

            if(guardarropa.id_duenio == idUsuario){//Compruebo que el que quiera eliminar sea el dueño

                // UTILIZO UNA SQLRAW PORQUE SINO NI PUEDO ELIMINAR VARIOS REGISTROS DE LA TABLA guardarropaxusuario
                db.Database.ExecuteSqlRaw($"delete from guardarropaxusuario Where id_guardarropa = '{idGuardarropa}'");

            } else {
                db.Remove(db.guardarropaXusuarioRepositories.Single(gxu => gxu.id_guardarropa == idGuardarropa && gxu.id_usuario == idUsuario));
                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Guardarropa "+ guardarropa.nombreGuardarropas +" eliminado!";
            return RedirectToAction("Index", "Guardarropas", new {id = idUsuario});

        }

        [HttpPost]
        public IActionResult CompartirGuardarropa(IFormCollection form){
            DB db = new DB();
            ViewResult vista = View("Guardarropas");

            Usuario usuarioDuenio = db.usuarios.FromSqlRaw($"Select * From usuarios Where id_usuario = '{form["idUsuarioDuenio"]}'").FirstOrDefault();
            Usuario usuarioParaCompartir = db.usuarios.FromSqlRaw($"Select * From usuarios Where usuario = '{form["nombreUsuarioACompartir"]}'").FirstOrDefault();
            Guardarropa guardarropaOriginal = db.guardarropas.FromSqlRaw($"Select * From guardarropas Where id_guardarropa = '{form["idGuardarropaACompartir"]}' and id_duenio = '{form["idUsuarioDuenio"]}'").FirstOrDefault();
            guardarropaXusuarioRepository gxuR = new guardarropaXusuarioRepository(); 
            guardarropaXusuarioRepository gxuRParaConsulta = new guardarropaXusuarioRepository(); 

            if(usuarioParaCompartir != null){//Compruebo que exista el usuario
                gxuRParaConsulta = db.guardarropaXusuarioRepositories.FromSqlRaw($"Select * from guardarropaxusuario Where id_guardarropa = '{guardarropaOriginal.id_guardarropa}' and id_usuario = '{usuarioParaCompartir.id_usuario}'").FirstOrDefault();
                if(gxuRParaConsulta == null){//Si fuese != null significa que ya le compartio el guardarropa a ese usuario
                    if(usuarioDuenio.tipoDeUsuario == usuarioParaCompartir.tipoDeUsuario || usuarioDuenio.tipoDeUsuario == 0){//Compruebo que el nivel de usuario deje compartir
                        gxuR.id_guardarropa = guardarropaOriginal.id_guardarropa;
                        gxuR.id_usuario = usuarioParaCompartir.id_usuario;
                        gxuR.nombreGuardarropa = guardarropaOriginal.nombreGuardarropas;

                        db.guardarropaXusuarioRepositories.Add(gxuR);

                        db.SaveChanges();

                        TempData["SuccessMessage"] = "Guardarropa compartido con "+ usuarioParaCompartir.usuario +" :D !";

                        return RedirectToAction("Index", "Guardarropas", new {id = form["idUsuarioDuenio"]});

                    } else {
                        
                        TempData["ErrorMessage"] = "No se puede compartir el guardarropas con el usuario " + form["nombreUsuarioACompartir"] + " porque es de un tipo de usuario inferior al tuyo!";

                        return RedirectToAction("Index", "Guardarropas", new {id = form["idUsuarioDuenio"]});

                    }
                } else {//En caso de que ya le compartió el guardarropas

                    TempData["ErrorMessage"] = "Ya compartiste el guardarropa "+ guardarropaOriginal.nombreGuardarropas +" con el usuario " + form["nombreUsuarioACompartir"] + "!";

                    return RedirectToAction("Index", "Guardarropas", new {id = form["idUsuarioDuenio"]});

                }
            } else { //Mensaje de error por si no existe el usuario

                TempData["ErrorMessage"] = "El usuario " + form["nombreUsuarioACompartir"] + " no existe!";

                return RedirectToAction("Index", "Guardarropas", new {id = form["idUsuarioDuenio"]});

            }

        }
        
        [HttpPost]
        public IActionResult EditarGuardarropa(IFormCollection form){

            DB db = new DB();

            Guardarropa guardarropaParaActualizar = db.guardarropas.FromSqlRaw($"Select * From guardarropas Where id_guardarropa = '{form["idGuardarropa"]}'").AsNoTracking().FirstOrDefault(); 
            guardarropaXusuarioRepository gxuParaActualizar = db.guardarropaXusuarioRepositories.FromSqlRaw($"Select * From guardarropaxusuario Where id_guardarropa = '{form["idGuardarropa"]}'").AsNoTracking().FirstOrDefault();
            
            if(guardarropaParaActualizar.id_duenio == Convert.ToInt32(form["idUsuario"])){

                guardarropaXusuarioRepository gxuUpdateado = new guardarropaXusuarioRepository();
                    gxuUpdateado.guardarropaXusuario_id = gxuParaActualizar.guardarropaXusuario_id;
                    gxuUpdateado.id_guardarropa = gxuParaActualizar.id_guardarropa;
                    gxuUpdateado.id_usuario = gxuParaActualizar.id_usuario;
                    gxuUpdateado.nombreGuardarropa = form["nuevoNombreGuardarropa"];

                Guardarropa guardarropaUpdateado = new Guardarropa();
                    guardarropaUpdateado.id_duenio = guardarropaParaActualizar.id_duenio;
                    guardarropaUpdateado.id_guardarropa = guardarropaParaActualizar.id_guardarropa ;
                    guardarropaUpdateado.nombreGuardarropas = gxuUpdateado.nombreGuardarropa;

                db.guardarropas.Update(guardarropaUpdateado);

                db.Database.ExecuteSqlRaw($"update guardarropaxusuario set nombreguardarropa = '{form["nuevoNombreGuardarropa"]}' Where id_guardarropa = '{form["idGuardarropa"]}'");

                db.SaveChanges();

                TempData["SuccessMessage"] = "Modificaste el nombre del guardarropa '" + guardarropaParaActualizar.nombreGuardarropas + "' a '" + form["nuevoNombreGuardarropa"] + "' con exito :D !";

                return RedirectToAction("Index", "Guardarropas", new {id = form["idUsuario"]});

            } else {

                TempData["ErrorMessage"] = "No podes editar el guardarropa " + guardarropaParaActualizar.nombreGuardarropas + " porque no sos el dueño";

                return RedirectToAction("Index", "Guardarropas", new {id = form["idUsuario"]});

            }


        }

        public class JsonGuardarropa
        {
            public int idUsuario { get; set; }
            public string nombreGuardarropa { get; set; }
        }
    }
}
