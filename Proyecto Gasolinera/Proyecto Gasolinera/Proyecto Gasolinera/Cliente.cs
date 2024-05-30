using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Gasolinera
{
    internal class Cliente
    {
        private string nombre;//atributos
        private string apellido;//
        private int nit;
        private string direccion;

        public Cliente()
        {
            this.nombre = "";
            this.apellido = "";
            this.nit = 0;
            this.direccion = "";
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public int Nit { get => nit; set => nit = value; }
        public string Direccion { get => direccion; set => direccion = value; }

    }
}
