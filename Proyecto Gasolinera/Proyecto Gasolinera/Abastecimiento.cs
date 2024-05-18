using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Gasolinera
{
    internal class Abastecimiento
    {
        private Cliente cliente;
        private DateTime fecha;
        private string fecha2;
        private string tipo;
        private double cantidad;
        private double precio;

        public Abastecimiento(Cliente cliente, DateTime fecha, string tipo, double cantidad, double precio)
        {
            this.Cliente = cliente;
            this.fecha = fecha;
            this.tipo = tipo;
            this.cantidad = cantidad;
            this.precio = precio;
        }

        internal Cliente Cliente { get => cliente; set => cliente = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        
        public string Tipo { get => tipo; set => tipo = value; }
        public double Cantidad { get => cantidad; set => cantidad = value; }
        public double Precio { get => precio; set => precio = value; }

    }
}
