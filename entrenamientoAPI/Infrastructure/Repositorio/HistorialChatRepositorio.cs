using entrenamientoAPI.Infrastructure.Abstracta;
using entrenamientoAPI.Infrastructure.Interfaz;

namespace entrenamientoAPI.Infrastructure.Repositorio
{
    public class HistorialChatRepositorio<Parametre, Resul> : BaseRepository<Parametre, Resul>, IHistorialChatRepositorio<Parametre, Resul>
        where Parametre : class
        where Resul : class
    {
        public Parametre Parameters { get; set; }

        public async Task<Resul> GuardarDatos()
        {
            return await this.ExecuteStoredProcedureAsync("PA_Company_Insert");
        }
    }
}
