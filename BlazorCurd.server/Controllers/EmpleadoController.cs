using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazorCurd.server.Models;
using BlazorCrud.Shared;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Runtime.InteropServices.JavaScript;
using System;
using Microsoft.IdentityModel.Tokens;

namespace BlazorCurd.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {

        private readonly DbcrudBlazorContext _dbContext;

        public EmpleadoController(DbcrudBlazorContext dbContext)// injectado el servicio de la DB dentro de nuestro servicio de controller.
        {
            _dbContext = dbContext;
        }

        public class ApiClass// cre la clase para probar consumir apis desde URLS
        {
            public int id { get; set; }
            public string title { get; set; }
            public string body { get; set; }
            public int userId { get; set; }
        }

        [HttpGet]//me trae info de una api
        [Route("NEWAPI/")]
        public async Task<IActionResult> ApiNueva()
        {
            var responseApi = new ResponseAPI<ApiClass>();
                       
            try
            {                            
                 
                    using (var client = new HttpClient())
                    {

                        string URL = "https://jsonplaceholder.typicode.com/posts/12";
                        client.DefaultRequestHeaders.Clear();
                        var response =  client.GetAsync(URL).Result; 
                        if(response.IsSuccessStatusCode) { 
                        var valor =  response.Content.ReadAsStringAsync().Result;
                        responseApi.Valor = JsonSerializer.Deserialize<ApiClass>(valor);
                        responseApi.EsCorrecto = true;
                        }
                        else {
                        responseApi.EsCorrecto = false;
                        responseApi.Mensaje = "No encontrado.";
                        }


                    }
                                


            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
                throw;
            }

            return Ok(responseApi);

        }


        [HttpGet]//Lista que devuelve la lista de empleados
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var responseApi = new ResponseAPI<List<EmpleadoDTO>>(); //Devolucion de la respuesta List de empleadosDTO
            var listaEmpleadoDTO = new List<EmpleadoDTO>();

            try
            {
                foreach (var item in await _dbContext.Empleados.Include(d => d.IdDepartamentoNavigation).ToListAsync()) //Listamos de manera asincrona, mientras recorre los datos puede continuar...
                {
                    listaEmpleadoDTO.Add(new EmpleadoDTO // rellenamos la lista dentro de estos items en un nueva clase empleados.
                    {
                        IdEmpleado = item.IdEmpleado,
                        NombreCompleto= item.NombreCompleto,
                        IdDepartamento= item.IdDepartamento,
                        Sueldo= item.Sueldo,
                        FechaContrato = item.FechaContrato,
                        Departamento = new DepartamentoDTO 
                        {
                            IdDepartamento = item.IdDepartamentoNavigation.IdDepartamento,
                            Nombre = item.IdDepartamentoNavigation.Nombre,
                        }
                    });
                }

                responseApi.EsCorrecto = true;
                responseApi.Valor = listaEmpleadoDTO;

            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
                throw;
            }

            return Ok(responseApi);

        }

        [HttpGet]//devuelve un empleado
        [Route("Buscar/{id}")]
        public async Task<IActionResult> Buscar(int id)
        {
            var responseApi = new ResponseAPI<EmpleadoDTO>(); //Devolucion de la respuesta List de empleadosDTO
            var EmpleadoDTO = new EmpleadoDTO();

            try
            {
                var dbempleado = await _dbContext.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == id);

                if (dbempleado != null)
                {
                    EmpleadoDTO.IdEmpleado = dbempleado.IdEmpleado;
                    EmpleadoDTO.NombreCompleto = dbempleado.NombreCompleto;
                    EmpleadoDTO.IdDepartamento = dbempleado.IdDepartamento;
                    EmpleadoDTO.Sueldo = dbempleado.Sueldo;
                    EmpleadoDTO.FechaContrato = dbempleado.FechaContrato;


                    responseApi.EsCorrecto = true;
                    responseApi.Valor = EmpleadoDTO;

                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "No encontrado.";
                }


            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
                throw;
            }

            return Ok(responseApi);

        }

        [HttpPost]//Guarda empleado
        [Route("Guardar")]
        public async Task<IActionResult> Guardar(EmpleadoDTO empleado)
        {
            var responseApi = new ResponseAPI<int>(); //Devolucion de guardado

            try
            {
                var dbEmpleado = new Empleado 
                {
                    NombreCompleto = empleado.NombreCompleto,
                    IdDepartamento = empleado.IdDepartamento,
                    Sueldo = empleado.Sueldo,
                    FechaContrato=empleado.FechaContrato,
                };

                _dbContext.Empleados.Add(dbEmpleado);
                await _dbContext.SaveChangesAsync();

                if(dbEmpleado.IdEmpleado != 0) 
                {
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = dbEmpleado.IdEmpleado;

                }
                else 
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "No guardado";
                }

            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
                throw;
            }

            return Ok(responseApi);

        }

        [HttpPut]//Guarda empleado
        [Route("Editar/{id}")]
        public async Task<IActionResult> Editar(EmpleadoDTO empleado, int id)
        {
            var responseApi = new ResponseAPI<int>(); //Devolucion de editado

            try
            {
                var dbEmpleado = await _dbContext.Empleados.FirstOrDefaultAsync(e => e.IdEmpleado == id);


                if (dbEmpleado != null)
                {

                    dbEmpleado.NombreCompleto = empleado.NombreCompleto;
                    dbEmpleado.IdDepartamento = empleado.IdDepartamento;
                    dbEmpleado.Sueldo= empleado.Sueldo;
                    dbEmpleado.FechaContrato = empleado.FechaContrato;

                    _dbContext.Empleados.Update(dbEmpleado);
                    await _dbContext.SaveChangesAsync();

                    responseApi.EsCorrecto = true;
                    responseApi.Valor = dbEmpleado.IdEmpleado;

                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Empleado no encontrado";
                }

            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
                throw;
            }

            return Ok(responseApi);

        }

        [HttpDelete]//Elimina empleado
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var responseApi = new ResponseAPI<int>(); //Devolucion de editado

            try
            {
                var dbEmpleado = await _dbContext.Empleados.FirstOrDefaultAsync(e => e.IdEmpleado == id);


                if (dbEmpleado != null)
                {

                    _dbContext.Empleados.Remove(dbEmpleado);
                    await _dbContext.SaveChangesAsync();

                    responseApi.EsCorrecto = true;

                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Empleado no encontrado";
                }

            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
                throw;
            }

            return Ok(responseApi);

        }

    }
}
