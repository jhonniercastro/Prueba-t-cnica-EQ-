using System;
using System.Collections.Generic;
using System.Text;

namespace PruebaTecnicaEQ.Models
{
    public class MListaTareas
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public bool estado { get; set; } = false;

        public int categoriaid { get; set; }
        public string descripcionCategoria { get; set; }
    }
}
