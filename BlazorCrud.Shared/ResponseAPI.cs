using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCrud.Shared
{
    public class ResponseAPI<T> //respuestas del lado del servidor hacia el cliente.//Es generico "T" por que puede devolver distintos DATOS
    {
        public bool EsCorrecto { get; set; }
        public T? Valor { get; set; }//devuelve ya sea la lista de EMPLEADOS,DEPARTAMENTES o ID empleado generado etc.
        public string? Mensaje { get; set; }
    }
}
