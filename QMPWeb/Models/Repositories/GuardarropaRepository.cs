using System;
using QueMePongo;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace queMePongo.Repositories
{
    public class GuardarropaRepository
    {
        public void Create(Guardarropa guardarropa, DB context, int idUsuario)
        {
            context.guardarropas.Add(guardarropa);
            context.SaveChanges();
            guardarropaXusuarioRepository gur = new guardarropaXusuarioRepository();
            gur.id_guardarropa = guardarropa.id_guardarropa;
            gur.id_usuario = idUsuario;
            gur.nombreGuardarropa = guardarropa.nombreGuardarropas;
            context.guardarropaXusuarioRepositories.Add(gur);
            context.SaveChanges();
        }

        public bool TryUpdate(int idGuardarropa, int idUsuario, String nuevoNombreGuardarropa)
        {

            DB db = new DB();

            Guardarropa guardarropaParaActualizar = db.guardarropas.FromSqlRaw($"Select * From guardarropas Where id_guardarropa = '{idGuardarropa}'").AsNoTracking().FirstOrDefault(); 
            guardarropaXusuarioRepository gxuParaActualizar = db.guardarropaXusuarioRepositories.FromSqlRaw($"Select * From guardarropaxusuario Where id_guardarropa = '{idGuardarropa}'").AsNoTracking().FirstOrDefault();

            if(guardarropaParaActualizar.id_duenio == idUsuario){

                guardarropaXusuarioRepository gxuUpdateado = new guardarropaXusuarioRepository();
                    gxuUpdateado.guardarropaXusuario_id = gxuParaActualizar.guardarropaXusuario_id;
                    gxuUpdateado.id_guardarropa = gxuParaActualizar.id_guardarropa;
                    gxuUpdateado.id_usuario = gxuParaActualizar.id_usuario;
                    gxuUpdateado.nombreGuardarropa = nuevoNombreGuardarropa;

                Guardarropa guardarropaUpdateado = new Guardarropa();
                    guardarropaUpdateado.id_duenio = guardarropaParaActualizar.id_duenio;
                    guardarropaUpdateado.id_guardarropa = guardarropaParaActualizar.id_guardarropa ;
                    guardarropaUpdateado.nombreGuardarropas = gxuUpdateado.nombreGuardarropa;

                db.guardarropas.Update(guardarropaUpdateado);

                db.Database.ExecuteSqlRaw($"update guardarropaxusuario set nombreguardarropa = '{nuevoNombreGuardarropa}' Where id_guardarropa = '{idGuardarropa}'");

                db.SaveChanges();

                return true;

            } else {

                return false;

            }
        }

        public String Delete(int idGuardarropa, int idUsuario)
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

            return "Guardarropa "+ guardarropa.nombreGuardarropas +" eliminado!";
        }

        public Guardarropa buscarGuardarropaPorIdYPorDuenio(int idGuardarropa, int idUsuarioDuenio){
            DB db = new DB();
            return db.guardarropas.FromSqlRaw($"Select * From guardarropas Where id_guardarropa = '{idGuardarropa}' and id_duenio = '{idUsuarioDuenio}'").FirstOrDefault();
        }

    }
}