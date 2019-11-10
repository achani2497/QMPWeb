using System;
using QueMePongo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

    public class EventoRepository
    {
        public void Insert(Evento evento, DB context)
        {
            context.eventos.Add(evento);
            context.SaveChanges();
            Console.WriteLine($"\nEvento {evento.id_evento} - {evento.descripcion} creado!");
        }

        public void Update(Evento evento, DB context)
        {

        }

        public void Delete(int eventoId)
        {
            DB db = new DB();
            db.Database.ExecuteSqlRaw($"delete from evento Where id_evento = '{eventoId}'");
        }
    }

