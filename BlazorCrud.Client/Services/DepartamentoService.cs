using BlazorCrud.Shared;
using System.Net.Http.Json;

namespace BlazorCrud.Client.Services
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly HttpClient _http;

        public DepartamentoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<DepartamentoDTO>> Lista()
        {
            var result = await _http.GetFromJsonAsync<ResponseAPI<List<DepartamentoDTO>>>("api/Departamento/Lista");// podemos confirmar la ruta dentro de swager ejecutando el PGRAM

            if (result!.EsCorrecto) return result.Valor!; // "!" indica que espera un resultado
            else { throw new Exception(result.Mensaje); }
        }
    }
}
