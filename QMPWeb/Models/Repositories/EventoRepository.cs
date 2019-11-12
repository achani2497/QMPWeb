using System;
using System.Collections.Generic;
using QueMePongo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

public class EventoRepository
    {
        public void Insert(Evento evento, DB context)
        {
            context.eventos.Add(evento);
            context.SaveChanges();
        }

    public void Update(Evento evento, DB context)
        {

        }

    public void Delete(int eventoId)
        {
            DB db = new DB();
            db.Database.ExecuteSqlRaw($"delete from eventos Where id_evento = '{eventoId}'");
        }
    }

