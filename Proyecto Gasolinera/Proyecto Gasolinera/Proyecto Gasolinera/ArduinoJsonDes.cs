using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Gasolinera
{
    internal class ArduinoJsonDes
    {
        private int cantidadRestante;
        private int bomba;

        public ArduinoJsonDes()
        {
            this.cantidadRestante = 0;
            this.bomba = 0;
        }

        public int CantidadRestante { get => cantidadRestante; set => cantidadRestante = value; }
        public int Bomba { get => bomba; set => bomba = value; }
    }
}
