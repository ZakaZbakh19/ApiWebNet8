namespace GestionApi.Service.Interfaces
{
    public interface IBaseService
    {
        T GetEntity<T, TBody>(TBody? body) where T : class;
    }
}
