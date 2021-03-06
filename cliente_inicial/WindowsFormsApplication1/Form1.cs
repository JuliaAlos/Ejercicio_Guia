﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Socket server;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

           
        }

        private void Conectar_Click(object sender, EventArgs e)
        {
             //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9040);


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
            }
            catch (SocketException)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            } 

        }
   
        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
               
                if (Longitud.Checked)
                {
                    // Quiere saber la longitud
                    string mensaje = "1/" + nombre.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                    MessageBox.Show("La longitud de tu nombre es: " + mensaje);
                }
                else if (Bonito.Checked)
                {
                    // Quiere saber si el nombre es bonito
                    string mensaje = "2/" + nombre.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];


                    if (mensaje == "SI")
                        MessageBox.Show("Tu nombre ES bonito.");
                    else if (mensaje == "NO")
                        MessageBox.Show("Tu nombre NO bonito. Lo siento.");
                    else
                        MessageBox.Show("Júlia tu eres mi emperatriz");
                }
                else
                {
                    // Quiere saber si soy alto
                    string mensaje = "3/" + nombre.Text + "/" + Altura.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                    MessageBox.Show(mensaje);
                }
            }
            catch (Exception)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("Error con la peticion");
                return;
            } 
          

        }

        private void Desconectar_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] msg = System.Text.Encoding.ASCII.GetBytes("0/");
                server.Send(msg);
                // Se terminó el servicio. 
                // Nos desconectamos
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();



            }
            catch (Exception )
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("Error al cerrar conexion");
                return;
            } 
        }

        private void Servicios_Click(object sender, EventArgs e)
        {
            //Pedir numeros de servicios  realizados
            byte[] msg = System.Text.Encoding.ASCII.GetBytes("4/");
            server.Send(msg);
           
            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            string mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            label3.Text =mensaje;
        }
    }
}
