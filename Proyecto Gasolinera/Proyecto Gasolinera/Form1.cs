using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Proyecto_Gasolinera
{
    public partial class Form1 : Form
    {
        System.IO.Ports.SerialPort Arduino = new System.IO.Ports.SerialPort();

        int Preciogasolina = 20;
        public Form1()
        {
            InitializeComponent();
            Arduino.PortName = "COM47";
            Arduino.BaudRate = 9600;
            Arduino.Open();
            Arduino.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataReceived);

        }
        void DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string dato = Arduino.ReadLine();
            this.Invoke(new MethodInvoker(delegate
            {
                label27.Text = dato;
                
                //SenrializarJson();
            }));
            //Deserializar el JSON {"sensor":"gps","time":1351824120,"data":{"latitude":48.75608,"longitude":2.302038}}

            //string json = '@'+ dato;
            //JsonAccount account = JsonConvert.DeserializeObject<JsonAccount>(json);
            //textBox1.Text = json;

            //Invalid character after parsing property name. Expected ':' but got: d. Path '', line 1, position 15.'


        }

        private void SenrializarJson(double cantidad)
        {
            JsonAccount account = new JsonAccount();
            
            
            account.preciodia = Preciogasolina;
            account.bomba = 1;
            account.cantidad = cantidad;
            string json = JsonConvert.SerializeObject(account);
            //label27.Text = json;
            Arduino.WriteLine(json);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txtLlenar_Click(object sender, EventArgs e)
        {
            double cantidad = 0;
            double precioSolicitado = 0;
            String tipoSeleccionado = "";
            //Indicar que combobox se ha seleccionado
            int tanqueSeleccionado = 0;
            if(radioButton1.Checked  == true)
                tanqueSeleccionado = 1;
            else if(radioButton2.Checked == true)
                tanqueSeleccionado = 2;
            else if(radioButton3.Checked == true)
                tanqueSeleccionado = 3;
            else if(radioButton4.Checked == true)
                tanqueSeleccionado = 4;
            if(tanqueSeleccionado == 0)
            {
                //No se ha seleccionado ningun tanque, messagebox. Error
                MessageBox.Show("No se ha seleccionado ningun tanque", "Tanque",MessageBoxButtons.OK, MessageBoxIcon.Error);

           
                
            }else
            {

                if (txtPrepago.Text == "")
                {
                    //Significa que se ha seleccionado la opcion de prepago ilimitado
                    var result = MessageBox.Show("No se ha ingresado ningun monto, se procedera a llenar ilimitado", "Abastecimiento", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if(result == DialogResult.Cancel)
                    {
                        //Se ha cancelado la operacion
                        return;
                    }
                    else
                    {
                        //Se ha aceptado la operacion
                        MessageBox.Show("Se ha llenado el tanque " + tanqueSeleccionado + " de forma ilimitada");
                        tipoSeleccionado = "Ilimitado";
                    }

                }else
                {
                    //Significa que se ha seleccionado la opcion de prepago limitado
                    precioSolicitado = Convert.ToDouble(txtPrepago.Text);
                    cantidad = precioSolicitado / Preciogasolina;
                    label5.Text = Preciogasolina.ToString();
                    label1.Text = cantidad.ToString();
                    MessageBox.Show("Cantidad de gasolina: " + cantidad);
                    tipoSeleccionado = "Prepago limitado";
                    SenrializarJson(cantidad);

                }
                /*string nombre = txtNombre.Text;
                string apellido = txtApellido.Text;
                int nit = Convert.ToInt32(txtNit.Text);
                string direccion = txtDireccion.Text;
                Cliente cliente = new Cliente(nombre, apellido, nit, direccion);
                DateTime fecha = DateTime.Now;
                Abastecimiento abastecimiento = new Abastecimiento(cliente, fecha, tipoSeleccionado, cantidad, precioSolicitado);
                */
            }
        }
    }
}
