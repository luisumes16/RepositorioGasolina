using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Gasolinera
{
    internal class Cliente
    {
        private string nombre;
        private string apellido;
        private int nit;
        private string direccion;

        public Cliente(string nombre, string apellido, int nit, string direccion)
        {
            this.Nombre = nombre;
            this.apellido = apellido;
            this.nit = nit;
            this.direccion = direccion;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public int Nit { get => nit; set => nit = value; }
        public string Direccion { get => direccion; set => direccion = value; }

        public string getNombre()
        {
            return "El nombre es " + nombre;
        }
        public void setNombre(string nombre)
        {
            this.nombre = nombre;
        }
    }
}
