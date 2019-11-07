using System;
using QueMePongo;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
        public void EliminarPrenda(int prendaId, DB context)
        {
            Prenda g = new Prenda();
            g = context.prendas.Single(b => b.id_prenda == prendaId);
            List<guardarropaXprendaRepository> gpr = new List<guardarropaXprendaRepository>();
            gpr = context.guardarropaXprendaRepositories.Where(u => u.id_prenda == prendaId).ToList();
            foreach (guardarropaXprendaRepository a in gpr)
            {
                context.guardarropaXprendaRepositories.Remove(a);
            }
            context.prendas.Remove(g);
            context.SaveChanges();
        }

        public Prenda BuscarPrendaPorId(int idPrenda){
            DB db = new DB();

            Prenda prenda = db.prendas.FromSqlRaw($"Select * From prendas Where id_prenda = '{idPrenda}'").FirstOrDefault();

            return prenda;

        }
    }
}