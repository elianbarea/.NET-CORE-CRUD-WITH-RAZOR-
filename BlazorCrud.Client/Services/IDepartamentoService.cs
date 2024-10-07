using BlazorCrud.Shared;

namespace BlazorCrud.Client.Services
{
    public interface IDepartamentoService//agregamos los metodos con los cuales vamos a trabajar
    {
        Task<List<DepartamentoDTO>> Lista();
    }
}
