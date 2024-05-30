using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Gasolinera
{
    internal class ListadoAbastecimientos
    {

        private Abastecimiento primero;
        private Abastecimiento ultimo;

        public ListadoAbastecimientos()
        {
            this.primero = null;
            this.ultimo = null;
        }   

        public Abastecimiento Primero { get => primero; set => primero = value; }
        public Abastecimiento Ultimo { get => ultimo; set => ultimo = value; }

        public void agregarAbastecimientoInicio(Abastecimiento abastecimiento)
        {
            Abastecimiento nuevo = new Abastecimiento();
            nuevo.Bomba = abastecimiento.Bomba;
            nuevo.Cantidad = abastecimiento.Cantidad;
            nuevo.Cliente = abastecimiento.Cliente;
            nuevo.Fecha = abastecimiento.Fecha;
            nuevo.Fecha2 = abastecimiento.Fecha2;
            nuevo.Precio = abastecimiento.Precio;
            nuevo.Tipo = abastecimiento.Tipo;

            primero = nuevo;
            ultimo = nuevo;
            ultimo.Siguiente = null;
            
        }

        public void agregarAbastecimiento(Abastecimiento abastecimiento)
        {
            if(primero == null)
            {
                agregarAbastecimientoInicio(abastecimiento);
            }
            else
            {
                Abastecimiento nuevo = new Abastecimiento();
                nuevo.Bomba = abastecimiento.Bomba;
                nuevo.Cantidad = abastecimiento.Cantidad;
                nuevo.Cliente = abastecimiento.Cliente;
                nuevo.Fecha = abastecimiento.Fecha;
                nuevo.Fecha2 = abastecimiento.Fecha2;
                nuevo.Precio = abastecimiento.Precio;
                nuevo.Tipo = abastecimiento.Tipo;

                ultimo.Siguiente = nuevo;
                ultimo = nuevo;
                ultimo.Siguiente = null;
            }
        }

        public void limpiarLista()
        {
         while (primero != null)
            {
                Abastecimiento aux = primero;
                primero = primero.Siguiente;
                aux = null;
            }
        }

        //Filtrar por cuantos se hicieron por Prepago limitado
        
    }
}
