using BlazorCrud.Shared;
using System.Net.Http.Json;

namespace BlazorCrud.Client.Services
{
    public class EmpleadoService : IEmpleadoService
    {

        private readonly HttpClient _http;

        public EmpleadoService(HttpClient http)
        {
            _http = http;
        }
        public async Task<List<EmpleadoDTO>> Lista()
        {
            var result = await _http.GetFromJsonAsync<ResponseAPI<List<EmpleadoDTO>>>("api/Empleado/Lista");// podemos confirmar la ruta dentro de swager ejecutando el PGRAM

            if (result!.EsCorrecto) return result.Valor!; // "!" indica que espera un resultado
            else { throw new Exception(result.Mensaje); }
        }
        public async Task<EmpleadoDTO> Buscar(int id)
        {
            var result = await _http.GetFromJsonAsync<ResponseAPI<EmpleadoDTO>>($"api/Empleado/Buscar/{id}");// $ sirve para que reconosca lo que esta entre llaves

            if (result!.EsCorrecto) return result.Valor!; // "!" indica que espera un resultado
            else { throw new Exception(result.Mensaje); }
        }
        public async Task<int> Guardar(EmpleadoDTO empleado)
        {
            var result = await _http.PostAsJsonAsync("api/Empleado/Guardar", empleado);// PostAsJsonAsync convierte el objeto "empleado" en un json
            var response = await result.Content.ReadFromJsonAsync<ResponseAPI<int>>();

            if (response!.EsCorrecto) return response.Valor!; // "!" indica que espera un resultado
            else { throw new Exception(response.Mensaje); }
        }
        public async Task<int> Editar(EmpleadoDTO empleado)
        {
            var result = await _http.PutAsJsonAsync($"api/Empleado/Editar/{empleado.IdEmpleado}", empleado);// PostAsJsonAsync convierte el objeto "empleado" en un json
            var response = await result.Content.ReadFromJsonAsync<ResponseAPI<int>>();

            if (response!.EsCorrecto) return response.Valor!; // "!" indica que espera un resultado
            else { throw new Exception(response.Mensaje); }
        }
        public async Task<bool> Eliminar(int id)
        {
            var result = await _http.DeleteAsync($"api/Empleado/Eliminar/{id}");// PostAsJsonAsync convierte el objeto "empleado" en un json
            var response = await result.Content.ReadFromJsonAsync<ResponseAPI<int>>();

            if (response!.EsCorrecto) return response.EsCorrecto!; // "!" indica que espera un resultado
            else { throw new Exception(response.Mensaje); }
        }



    }
}
