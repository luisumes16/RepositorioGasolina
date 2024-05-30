using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
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

        int Preciogasolina = 40;//precio de la gasolina

        ListadoAbastecimientos listado = new ListadoAbastecimientos();//crea un objeto de la clase ListadoAbastecimientos
        public Form1()
        {
            InitializeComponent();//inicializa los componentes
            Arduino.PortName = "COM52";//selecciona el puerto
            Arduino.BaudRate = 9600;//selecciona la velocidad
            Arduino.Open();//abre el puerto
            Arduino.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataReceived);//evento de recepcion de datos
        }
        void DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string dato = Arduino.ReadLine();//lee el dato

            string json = "";
            this.Invoke(new MethodInvoker(delegate//invoca el metodo
            {
                label27.Text = dato;//muestra el dato en el label

                json += dato;
                if(json.Contains("}"))
                {
                    DesenrializarJSON(json);
                    json = "";
                }
                
                //SenrializarJson();
            }));
            //Deserializar el JSON {"sensor":"gps","time":1351824120,"data":{"latitude":48.75608,"longitude":2.302038}}

            //string json = '@'+ dato;
            //JsonAccount account = JsonConvert.DeserializeObject<JsonAccount>(json);
            //textBox1.Text = json;

            //Invalid character after parsing property name. Expected ':' but got: d. Path '', line 1, position 15.'


        }
        private void DesenrializarJSON(string json)
        {
            ArduinoJsonDes account = JsonConvert.DeserializeObject<ArduinoJsonDes>(json);
            int cantidad = account.CantidadRestante;
            int bomba = account.Bomba;
            string tipo = "";
            if (cantidad == 0)
            {
                MessageBox.Show("Se ha llenado el tanque");

            }
            else
            {
                MessageBox.Show("Quedan " + cantidad + " galones");
            }



            Abastecimiento abastecimiento = new Abastecimiento();
            abastecimiento.Bomba = bomba;
            abastecimiento.Cantidad = cantidad;

            abastecimiento.Cliente = new Cliente();
            abastecimiento.Cliente.Nombre = txtNombre.Text;
            abastecimiento.Cliente.Apellido = txtApellido.Text;
            abastecimiento.Cliente.Nit = Convert.ToInt32(txtNit.Text);
            abastecimiento.Cliente.Direccion = txtDireccion.Text;

            abastecimiento.Fecha = DateTime.Now;
            if (txtPrepago.Text == " ") { tipo = "Tanque lleno"; }
            else { tipo = "Prepago limitado"; }
            abastecimiento.Tipo = tipo;
            
            abastecimiento.Precio = cantidad * Preciogasolina;
            listado.agregarAbastecimiento(abastecimiento);
            MostrarEnDataView();
            EscribirenText();

        }



        private void SenrializarJson(double cantidad, int bomba)
        {
            JsonAccount account = new JsonAccount();//crea un objeto de la clase JsonAccount

            MessageBox.Show("BOMBA" + bomba);
            account.preciodia = Preciogasolina;//asigna el precio del dia
            account.bomba = bomba;//asigna la bomba
            account.cantidad = cantidad;//asigna la cantidad de gasolina
            string json = JsonConvert.SerializeObject(account);//serializa el objeto
            //label27.Text = json;
            Arduino.WriteLine(json);//envia el json al arduino
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarArchivo();

        }

        private void txtLlenar_Click(object sender, EventArgs e)
        {
            double cantidad = 0;//inicializa la cantidad
            double precioSolicitado = 0;//  inicializa el precio solicitado
            String tipoSeleccionado = "";//inicializa el tipo seleccionado
            //Indicar que combobox se ha seleccionado
            int tanqueSeleccionado = 0;//inicializa el tanque seleccionado
            if(radioButton1.Checked  == true)//si el radiobutton 1 esta seleccionado
                tanqueSeleccionado = 1;//asigna el tanque 1
            else if(radioButton2.Checked == true)//si el radiobutton 2 esta seleccionado
                tanqueSeleccionado = 2;//asigna el tanque 2
            else if(radioButton3.Checked == true)//si el radiobutton 3 esta seleccionado
                tanqueSeleccionado = 3;//asigna el tanque 3
            else if(radioButton4.Checked == true)//si el radiobutton 4 esta seleccionado
                tanqueSeleccionado = 4;//asigna el tanque 4
            if(tanqueSeleccionado == 0)//si no se ha seleccionado ningun tanque
            {
                //No se ha seleccionado ningun tanque, messagebox. Error
                MessageBox.Show("No se ha seleccionado ningun tanque", "Tanque",MessageBoxButtons.OK, MessageBoxIcon.Error);

           
                
            }else
            {

                if (txtPrepago.Text == "")//si no se ha ingresado ningun monto
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
                        
                        cantidad = 0;//calcula la cantidad de gasolina
                        label5.Text = Preciogasolina.ToString();//muestra el precio de la gasolina
                        label1.Text = 00.ToString();//muestra la cantidad de gasolina
                        MessageBox.Show("Cantidad de gasolina: " + cantidad);//muestra la cantidad de gasolina
                        tipoSeleccionado = "Prepago limitado";//asigna el tipo seleccionado
                        SenrializarJson(cantidad, tanqueSeleccionado);//serializa el json
                    }

                }else
                {
                    //Significa que se ha seleccionado la opcion de prepago limitado
                    precioSolicitado = Convert.ToDouble(txtPrepago.Text);
                    cantidad = precioSolicitado / Preciogasolina;//calcula la cantidad de gasolina
                    label5.Text = Preciogasolina.ToString();//muestra el precio de la gasolina
                    label1.Text = cantidad.ToString();//muestra la cantidad de gasolina
                    MessageBox.Show("Cantidad de gasolina: " + cantidad);//muestra la cantidad de gasolina
                    tipoSeleccionado = "Prepago limitado";//asigna el tipo seleccionado
                    SenrializarJson(cantidad, tanqueSeleccionado);//serializa el json
                    //luego de senrializar el json, debe de esperar a que el arduino le envie la cantidad restante de gasolina
                    //para poder continuar con el proceso, por lo que se debe de esperar a que el arduino le envie la cantidad restante. Lo leera y lo mandara a deserializar y retornara la cantidad restante. 
                    //luego de obtener la cantidad restante, se procedera a crear el objeto cliente y el objeto abastecimiento y se guardara en el archivo de texto
                    //luego se mostrara en el datagridview
                    //que se debe de hacer para esperar?
                    //se debe de crear un evento que se ejecute cuando el arduino le envie la cantidad restante
                    //crealo

                    //Crea una promesa que se ejecute cuando el arduino le envie la cantidad restante
                    



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

        
        private void CargarArchivo()
        {
            string nombrearchivo = "Abastecimientos.txt";
            FileStream stream = new FileStream(nombrearchivo, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader LeerArchivo = new StreamReader(stream);
            string linea = LeerArchivo.ReadLine();
            while(linea != null)
            {
                string[] datos = linea.Split(';');
                //dataGridView1.Rows.Add(datos);
                linea = LeerArchivo.ReadLine();

                //utiliza el metodo trim para eliminar los espacios en blanco
                //MessageBox.

                
                
                Abastecimiento abastecimiento = new Abastecimiento();

                Cliente cliente = new Cliente();
                //abastecimiento.Cliente.Nombre = datos[0].ToString();
                //abastecimiento.Cliente.Apellido = datos[0].ToString();
                //Luis;Colop;1921029;Ciudad;2;40;1;29/05/2024;
                cliente.Nombre = datos[0];
                cliente.Apellido = datos[1];
                cliente.Nit = Convert.ToInt32(datos[2]);
                cliente.Direccion = datos[3];
                abastecimiento.Cliente = cliente;

                abastecimiento.Cantidad = Convert.ToDouble(datos[4]);
                abastecimiento.Precio = Convert.ToDouble(datos[5]);
                abastecimiento.Bomba = Convert.ToInt32(datos[6]);
                abastecimiento.Fecha2 = datos[7];
                abastecimiento.Tipo = datos[8];
                
                
                
                listado.agregarAbastecimiento(abastecimiento);

            }
            LeerArchivo.Close();
            stream.Close();
            MostrarEnDataView();
        }

        private void MostrarEnDataView()
        {
            dataGridView1.Rows.Clear();
            Abastecimiento actual = listado.Primero;
            while (actual != null)
            {
                dataGridView1.Rows.Add(actual.Fecha, actual.Cantidad, actual.Tipo, actual.Bomba);
                actual = actual.Siguiente;
            }
        }

        private void EscribirenText()
        {
            string archivo = "Abastecimientos.txt";
            FileStream stream = new FileStream(archivo, FileMode.Create, FileAccess.Write);
            StreamWriter escribir = new StreamWriter(stream);
            Abastecimiento actual = listado.Primero;
            while (actual != null)
            {
                escribir.WriteLine(actual.Cliente.Nombre + ";" + actual.Cliente.Apellido + ";" + actual.Cliente.Nit + ";" + actual.Cliente.Direccion + ";" + actual.Cantidad + ";" + actual.Precio + ";" + actual.Bomba + ";" + actual.Fecha2 + ";" + actual.Tipo);
                actual = actual.Siguiente;
            }
            escribir.Close();
            stream.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();
            Abastecimiento actual = listado.Primero;
            while (actual != null)
            {
                if(actual.Tipo == "Prepago limitado")
                {
                    dataGridView1.Rows.Add(actual.Fecha, actual.Cantidad, actual.Tipo, actual.Bomba);
                }
                actual = actual.Siguiente;
            }
        }
        //evaluar que bomba es la mas utilizada
       
        

        private void button3_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            int bomba1 = 0;
            int bomba2 = 0;
            int bomba3 = 0;
            int bomba4 = 0;
            Abastecimiento actual = listado.Primero;
            while (actual != null)
            {
                if (actual.Bomba == 1)
                {
                    bomba1++;
                }
                else if (actual.Bomba == 2)
                {
                    bomba2++;
                }
                else if (actual.Bomba == 3)
                {
                    bomba3++;
                }
                else if (actual.Bomba == 4)
                {
                    bomba4++;
                }
                actual = actual.Siguiente;
            }
            if (bomba1 > bomba2 && bomba1 > bomba3 && bomba1 > bomba4)
            {
                mensaje = "La bomba mas utilizada es la bomba 1";
            }
            else if (bomba2 > bomba1 && bomba2 > bomba3 && bomba2 > bomba4)
            {
                mensaje = "La bomba mas utilizada es la bomba 2";
            }
            else if (bomba3 > bomba1 && bomba3 > bomba2 && bomba3 > bomba4)
            {
                mensaje = "La bomba mas utilizada es la bomba 3";
            }
            else if (bomba4 > bomba1 && bomba4 > bomba2 && bomba4 > bomba3)
            {
                mensaje = "La bomba mas utilizada es la bomba 4";
            }

            label18.Text = mensaje;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
