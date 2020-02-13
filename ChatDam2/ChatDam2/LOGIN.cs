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
    public partial class LOGIN : Form
    {
        String imagen;
        MySqlConnection conexion = new MySqlConnection();
        public LOGIN()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conexion.ConnectionString = "server=remotemysql.com;Database=Pr1mdxAdrh;Uid=Pr1mdxAdrh;Pwd=fNBUrxid1O";
            txtUser.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (comprobarUsuario())
            {
                CHAT fchat = new CHAT();
                fchat.usuario = txtUser.Text;
                limpiar();
                fchat.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario NO registrado o clave incorrecta");
                limpiar();
            }
        }

        private void limpiar()
        {
            txtUser.Text = "";
            txtPwd.Text = "";
            txtUser.Focus();
        }

        public Boolean comprobarUsuario()
        {
            conexion.Open();
            String cadenaSql = "select * from usuarios where nombre=?us and clave=?pw";
            MySqlCommand comando = new MySqlCommand(cadenaSql, conexion);
            comando.Parameters.Add("?us", MySqlDbType.String).Value = txtUser.Text;
            comando.Parameters.Add("?pw", MySqlDbType.String).Value = txtPwd.Text;
            MySqlDataReader datos = comando.ExecuteReader();
            int c = 0;
            
            while (datos.Read())
            {
                c++;
            }
            conexion.Close();
            return (c == 1 ? true : false);
            
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult op = MessageBox.Show("¿Estas seguro?", "Chat Dam2", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (op == DialogResult.Yes)
            {
                //     this.Close();
                Application.Exit();
            }
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    imagen = openFileDialog1.FileName;
                }
                //leer y escribir datos de un fichero
                FileStream fs = new FileStream(imagen, FileMode.Open, FileAccess.Read);
                long tamanio = fs.Length;
                //métodos que simplifican la lectura de los tipos de datos primitivos de una secuencia. 
                BinaryReader br = new BinaryReader(fs);
                byte[] bloque = br.ReadBytes((int)fs.Length);
                fs.Read(bloque, 0, Convert.ToInt32(tamanio));

                conexion.Open();
                String cadenaSql = "insert into usuarios values(?nom,?cl,?img,0)";
                MySqlCommand comando = new MySqlCommand(cadenaSql, conexion);
                comando.Parameters.Add("?nom", MySqlDbType.String).Value = txtUser.Text;
                comando.Parameters.Add("?cl", MySqlDbType.String).Value = txtPwd.Text;
                comando.Parameters.Add("?img", MySqlDbType.Blob).Value = bloque;
                comando.ExecuteNonQuery();
                conexion.Close();
                limpiar();

                MessageBox.Show("Usuario Registrado");

            }catch(MySqlException ex)
            {
                MessageBox.Show("Error clave duplicada: " + ex.Message);
            }
            
        }   
    }
}
