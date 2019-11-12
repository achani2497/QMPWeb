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

