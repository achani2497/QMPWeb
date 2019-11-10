using System;
using QueMePongo;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace queMePongo.Repositories
{
    public class PrendaRepository
    {
        public void CrearPrenda(Prenda prenda, DB context, int idGuardarropa)
        {
            context.prendas.Add(prenda);
            context.SaveChanges();
            guardarropaXprendaRepository gpr = new guardarropaXprendaRepository();
            gpr.id_guardarropa = idGuardarropa;
            gpr.id_prenda = prenda.id_prenda;
            context.guardarropaXprendaRepositories.Add(gpr);
            context.SaveChanges();
        }

        public void EditarPrenda(Prenda prenda, DB context)
        {
            var s = context.prendas.Single(b => b.id_prenda == prenda.id_prenda);
            s.calificacion = prenda.calificacion;
            s.cantCalif = prenda.cantCalif;
            context.SaveChanges();
        }
        public bool EliminarPrenda(int prendaId, int idUsuario, DB context)
        {
            Prenda prenda = new Prenda();
            prenda = context.prendas.FromSqlRaw($"Select * from prendas where id_prenda='{prendaId}' and id_duenio='{idUsuario}'").FirstOrDefault();

            if(prenda != null){

                string pathDeImagenADeletear = "wwwroot/uploads/"+prenda.urlImagen;

                List<guardarropaXprendaRepository> gpr = new List<guardarropaXprendaRepository>();
                gpr = context.guardarropaXprendaRepositories.Where(u => u.id_prenda == prendaId).ToList();
                foreach (guardarropaXprendaRepository a in gpr)
                {
                    context.guardarropaXprendaRepositories.Remove(a);
                }

                File.Delete(pathDeImagenADeletear);

                context.prendas.Remove(prenda);
                context.SaveChanges();

                return true;
            } else {
                return false;
            }

        }

        public Prenda BuscarPrendaPorId(int idPrenda){
            DB db = new DB();

            Prenda prenda = db.prendas.FromSqlRaw($"Select * From prendas Where id_prenda = '{idPrenda}'").FirstOrDefault();

            return prenda;

        }

        public List<Prenda> PrendasDelUsuario(int idUsuario){

            DB db = new DB();

            return db.prendas.Where(prenda => prenda.id_duenio == idUsuario).ToList();

        }
    }
}