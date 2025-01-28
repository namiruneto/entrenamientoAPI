namespace entrenamientoAPI.Infrastructure.Interfaz
{
    public interface IHistorialChatRepositorio<Parametre, Resul>
    {
        Parametre Parameters { get; set; }
        Task<Resul> GuardarDatos();
    }
}

