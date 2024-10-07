using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using BlazorCurd.server.Models;
using BlazorCrud.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorCurd.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {

        private readonly DbcrudBlazorContext _dbContext;

        public DepartamentoController(DbcrudBlazorContext dbContext)// injectado el servicio de la DB dentro de nuestro servicio de controller.
        {
            _dbContext = dbContext;
        }

        [HttpGet]//Lista que devuelve la lista de departamentos
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var responseApi = new ResponseAPI<List<DepartamentoDTO>>(); //Devolucion de la respuesta List de DepartamentoDTO
            var listaDepartamentoDTO = new List<DepartamentoDTO>();

            try
            {
                foreach (var item in await _dbContext.Departamentos.ToListAsync()) //Listamos de manera asincrona, mientras recorre los datos puede continuar...
                {
                    listaDepartamentoDTO.Add(new DepartamentoDTO // rellenamos la lista dentro de estos items en un nueva clase Departamento.
                    {
                        IdDepartamento = item.IdDepartamento,
                        Nombre = item.Nombre,
                    });
                }

                responseApi.EsCorrecto = true;
                responseApi.Valor= listaDepartamentoDTO;

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
