using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ChatDam2
{
    public partial class CHAT : Form
    {
        MySqlConnection conexion = new MySqlConnection();
        List<Usuario> listaUsuarios = new List<Usuario>();
        List<Usuario> listaConectados = new List<Usuario>();


        public String usuario;
        int cont=0;
        public int contadorSesion = 500;

        public CHAT()
        {
            InitializeComponent();
        }

        private void Form2Chat_Load(object sender, EventArgs e)
        {
            lblWarning.Visible = false;
            lblUser.Text = usuario;
            conexion.ConnectionString = "server=remotemysql.com;Database=Pr1mdxAdrh;Uid=Pr1mdxAdrh;Pwd=fNBUrxid1O";
            usuarioConectado(true);
            cargarChat();
            cargarImagenes(flp1);
            cargarImagenesConectados(flp2);
            timer1.Enabled=true;
            
        }

        private void btsalir_Click(object sender, EventArgs e)
        {
            DialogResult op = MessageBox.Show("¿Estas seguro?", "Chat Dam2", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (op == DialogResult.Yes)
            {
                Close();
                LOGIN f1 = new LOGIN();
                f1.Show();
                usuarioConectado(false);
                timer1.Enabled = false;

            }
        }

        public void cargarChat()
        {
            lblUser.Text = usuario;
            conexion.Open();
            String cadenaSql = "select * from chat order by 3 desc";
            MySqlCommand comando = new MySqlCommand(cadenaSql, conexion);
            MySqlDataReader datos = comando.ExecuteReader();

            while (datos.Read())
            {
                lbChat.Items.Add(datos["usuario"] + " :\t" + datos["mensaje"] + "\t " + datos["fecha"]);
            }
            lbChat.Show();
            conexion.Close();
            txtMsg.Focus();

        }

        public void cargarImagenes(FlowLayoutPanel flp)
        {
            cargarListaUsuario();
            //el tamaño de la lista de Usuarios
            //determinará el número de botones
            //que se van a contruir
            flp.Controls.Clear();
            for (int i = 0; i < listaUsuarios.Count; i++)
            {
                //construir el objeto tipo Button
                Button botonX = new Button();
                //propiedad para dimensionar en alto
                botonX.Height = 50;
                //propiedad ancho
                botonX.Width = 40;
                //dotar al botón de capacidad de ejecutar
                //eventos de un click
                botonX.Click += new EventHandler(mostrarInformacion);
                //  botonX.MouseHover += new EventHandler(mostrarInformacion);
                //fichero binario  para convertir
                //el array byte[] en un objeto Image
                MemoryStream ms = new MemoryStream(listaUsuarios[i].Imagen);
                botonX.BackgroundImage = Image.FromStream(ms);
                //ajustar el tamaño de la imagen al botón
                botonX.BackgroundImageLayout = ImageLayout.Stretch;
                //asignar posición de la lista a posición
                //del botón dentro del contenedor
                //  botonX.Tag = i;
                botonX.Tag = listaUsuarios[i].Nombre;
                //añadir el nuevo botón al contenedor

                flp.Controls.Add(botonX);
            }
        }

        public void cargarImagenesConectados(FlowLayoutPanel flp)
        {
            cargarListaConectados();
            //el tamaño de la lista de Usuarios
            //determinará el número de botones
            //que se van a contruir
            flp.Controls.Clear();
            for (int i = 0; i < listaConectados.Count; i++)
            {
                //construir el objeto tipo Button
                Button botonX = new Button();
                //propiedad para dimensionar en alto
                botonX.Height = 50;
                //propiedad ancho
                botonX.Width = 40;
                //dotar al botón de capacidad de ejecutar
                //eventos de un click
                botonX.Click += new EventHandler(mostrarInformacion);
                //  botonX.MouseHover += new EventHandler(mostrarInformacion);
                //fichero binario  para convertir
                //el array byte[] en un objeto Image
                MemoryStream ms = new MemoryStream(listaConectados[i].Imagen);
                botonX.BackgroundImage = Image.FromStream(ms);
                //ajustar el tamaño de la imagen al botón
                botonX.BackgroundImageLayout = ImageLayout.Stretch;
                //asignar posición de la lista a posición
                //del botón dentro del contenedor
                //  botonX.Tag = i;
                botonX.Tag = listaConectados[i].Nombre;
                //añadir el nuevo botón al contenedor

                flp.Controls.Add(botonX);
            }
        }

        public void cargarListaUsuario()
        {
            listaUsuarios.Clear();
            conexion.Open();
            String cadenaSql = "select * from usuarios";
            MySqlCommand comando = new MySqlCommand(cadenaSql, conexion);
            MySqlDataReader datos = comando.ExecuteReader();
            while (datos.Read())
            {
                Usuario us = new Usuario();
                us.Nombre =Convert.ToString(datos["nombre"]);
                us.Clave = Convert.ToString(datos["nombre"]);
                us.Imagen = (byte[])datos["imagen"];

                listaUsuarios.Add(us);

            }
            conexion.Close();
        }
        public void cargarListaConectados()
        {
            listaConectados.Clear();
            conexion.Open();
            String cadenaSql = "select * from usuarios where activo=1";
            MySqlCommand comando = new MySqlCommand(cadenaSql, conexion);
            MySqlDataReader datos = comando.ExecuteReader();
            while (datos.Read())
            {
                Usuario us = new Usuario();
                us.Nombre = Convert.ToString(datos["nombre"]);
                us.Clave = Convert.ToString(datos["nombre"]);
                us.Imagen = (byte[])datos["imagen"];

                listaConectados.Add(us);
            }
            conexion.Close();

        }

        public void mostrarInformacion(object sender,EventArgs e)
        {
            Button botonX = (Button)sender;
            MessageBox.Show("Usuario: "+Convert.ToString(botonX.Tag));
        }

        private void txtMsg_KeyUp(object sender, KeyEventArgs e)
        {
            contadorSesion = 500;
            lblWarning.Visible = false;

            if (e.KeyCode == Keys.Enter)
            {
                conexion.Open();
                String cadenaSql = "insert into chat values(id,?us,?fecha,?texto)";
                MySqlCommand comando =new MySqlCommand(cadenaSql,conexion);
                comando.Parameters.Add("?texto", MySqlDbType.String).Value = txtMsg.Text;
                comando.Parameters.Add("?us", MySqlDbType.String).Value = usuario;
                comando.Parameters.Add("?fecha", MySqlDbType.DateTime).Value = DateTime.Now;
                comando.ExecuteNonQuery();
                conexion.Close();
                txtMsg.Text = "";
                lbChat.Items.Clear();
                recarga(); 
            }

        }

        public void recarga()
        {
            cargarImagenes(flp1);
            cargarImagenesConectados(flp2);

            conexion.Open();
            String cadenaSql = "select * from chat order by 3 desc";
            MySqlCommand comando = new MySqlCommand(cadenaSql, conexion);
            MySqlDataReader datos = comando.ExecuteReader();

            while (datos.Read())
            {
                lbChat.Items.Add(datos["usuario"] + " :\t" + datos["mensaje"] + " :\t" + datos["fecha"]);
            }
            conexion.Close();
            lbChat.Show();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cont++;
            contadorSesion--;
            if (cont == 5)
            {
                recarga();
                cont = 0;
            }
            if(contadorSesion<=10 && contadorSesion > 0)
            {
                lblWarning.Text = ""+contadorSesion;
                lblWarning.Visible = true;
            }
            else if (contadorSesion <= 0)
            {
                timer1.Enabled = false;
                usuarioConectado(false);
                LOGIN log1 = new LOGIN(); log1.Show();
                this.Close();
                
            }
        }

        private void lblUser_Click(object sender, EventArgs e)
        {
            PERFIL f3 = new PERFIL();
            f3.Show();
            f3.usuarioPerfil=usuario;
            f3.contadorSesion = contadorSesion;
            this.Hide();
            timer1.Enabled = false;
        }

        public void usuarioConectado(Boolean flag)
        {
            conexion.Open();
            String cadenaSql = flag ? "update usuarios set activo=1 where nombre=?nom" : "update usuarios set activo=0 where nombre=?nom";
            MySqlCommand comando = new MySqlCommand(cadenaSql, conexion);
            comando.Parameters.Add("?nom", MySqlDbType.String).Value = usuario;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
    }
}
