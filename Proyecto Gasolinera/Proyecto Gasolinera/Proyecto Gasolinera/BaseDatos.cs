using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Gasolinera
{
    internal class BaseDatos
    {
        private List <Abastecimiento> abastecimiento = new List<Abastecimiento>();

        public string ruta;


        public BaseDatos(string ruta)
        {
           if(!File.Exists(ruta))
            {
                File.Create(ruta).Close();
            }
            this.ruta = ruta;
        }

        public void guardarAbastecimiento()
        {
            string json= JsonConvert.SerializeObject(abastecimiento);
            File.WriteAllText(ruta, json);
        }
      
    }
}
