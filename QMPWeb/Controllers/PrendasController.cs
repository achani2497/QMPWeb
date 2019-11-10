using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QueMePongo;
using queMePongo.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QMPWeb.Controllers
{
    public class PrendasController : Controller
    {
        private readonly IHostingEnvironment hostingEnviroment;
        public PrendasController(IHostingEnvironment ihe){
            hostingEnviroment = ihe;
        }

        public IActionResult Index(int idUsuario)
        {
            if(idUsuario != 0){

                guardarropaXusuarioRepository guardarropaDAO = new guardarropaXusuarioRepository();

                List<guardarropaXusuarioRepository> guardarropasParciales = guardarropaDAO.listarGuardarropasDeUsuario(idUsuario);

                guardarropaXprendaRepository prendasDAO = new guardarropaXprendaRepository();

                List<guardarropaXprendaRepository> prendasParciales = new List<guardarropaXprendaRepository>();

                foreach(guardarropaXusuarioRepository guarda in guardarropasParciales)
                {
                    prendasParciales.AddRange(prendasDAO.listarPrendasDeGuardarropa(guarda.id_guardarropa)); 
                }
            
                ViewBag.Prendas = prendasParciales;

                ViewBag.Id = idUsuario;

                ViewBag.PrendasDelUsuario = TraerPrendasDelUsuario(idUsuario);

                return View("Prendas");

            } else {

                return RedirectToAction("Index", "Login");

            }
        }

        [HttpPost]
        public IActionResult Create(IFormCollection form, IFormFile imagenDePrenda){

            DB db = new DB();

            Prenda prendaNueva = new Prenda();
            PrendaRepository prendaDAO = new PrendaRepository();

            prendaNueva.colorPrincipal = form["colorPrincipal"];
            prendaNueva.colorSecundario = form["colorSecundario"];

            string idTelaString = form["tipoDeTela"];
            string idGuardarropaString = form["idGuardarropa"];
            string idTipoPrendaString = form["tipoDePrenda"];
            string idUsuarioString = form["idUsuario"];

            int idGuardarropa = Convert.ToInt32(idGuardarropaString);
            int idTela = Convert.ToInt32(idTelaString);
            int idTipoPrenda = Convert.ToInt32(idTipoPrendaString);
            int idUsuario = Convert.ToInt32(idUsuarioString);
            
            prendaNueva.id_tela = idTela;
            prendaNueva.tipoPrenda = idTipoPrenda;
            prendaNueva.id_duenio = idUsuario;

            var imagen = imagenDePrenda;
            
            var nombreUnico = GenerarNombreUnico(imagen.FileName);
            var uploads = Path.Combine(hostingEnviroment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, nombreUnico);

            imagen.CopyTo(new FileStream(filePath, FileMode.Create));

            prendaNueva.urlImagen = nombreUnico;

            prendaDAO.CrearPrenda(prendaNueva, db, idGuardarropa);

            TempData["SuccessMessage"] = "Prenda creada correctamente! :D ";

            return RedirectToAction("Index", "Prendas", new {idUsuario = idUsuario});
        }

        [HttpPost]
        public IActionResult EliminarPrenda(IFormCollection form){

            PrendaRepository prendaDAO = new PrendaRepository();
            DB db = new DB();

            string idPrendaString = form["idPrenda"];
            string idUsuarioString = form["idUsuario"];

            int idPrenda = Convert.ToInt32(idPrendaString);
            int idUser = Convert.ToInt32(idUsuarioString);

            if(prendaDAO.EliminarPrenda(idPrenda, idUser, db)){

                TempData["SuccessMessage"] = "Prenda eliminada! :D";

                return RedirectToAction("Index", "Prendas", new {idUsuario = idUser});

            } else {

                TempData["ErrorMessage"] = "No podes eliminar esta prenda porque no sos su dueño!";

                return RedirectToAction("Index", "Prendas", new {idUsuario = idUser});

            }

        }
        public List<Prenda> TraerPrendasDelUsuario(int idUsuario){
            
            PrendaRepository prendaDAO = new PrendaRepository();

            List<Prenda> prendasDelUsuario = prendaDAO.PrendasDelUsuario(idUsuario);

            return prendasDelUsuario;

        }
        public List<Tela> TraerTelas(){

            DB db = new DB();

            return db.telas.ToList();

        }
        public List<TipoPrenda> TraerTiposDePrenda(){
            DB db = new DB();

            return db.tipoprendas.ToList();
        }
        private string GenerarNombreUnico(string nombreDeArchivo){
            nombreDeArchivo = Path.GetFileName(nombreDeArchivo);
            return Path.GetFileNameWithoutExtension(nombreDeArchivo)+"-"+Guid.NewGuid().ToString().Substring(0,4)+Path.GetExtension(nombreDeArchivo);
        }
    }
}
