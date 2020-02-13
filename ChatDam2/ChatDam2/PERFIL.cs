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
    public partial class PERFIL : Form
    {
        public String usuarioPerfil;
        String imagen;
        String clave;
        MySqlConnection conexion = new MySqlConnection();
        public int contadorSesion;

        public PERFIL()
        {
            InitializeComponent();
        }

        private void Form3Perfil_Load(object sender, EventArgs e)
        {
            conexion.ConnectionString = "server=remotemysql.com;Database=Pr1mdxAdrh;Uid=Pr1mdxAdrh;Pwd=fNBUrxid1O";
            timer1.Enabled = true;
            lblWarning2.Visible = false;
        }

        private void salirPerfil_Click(object sender, EventArgs e)
        {
            CHAT f2 = new CHAT();
            f2.usuario = usuarioPerfil;
            f2.contadorSesion = contadorSesion;
            f2.Show();
            this.Close();
            timer1.Enabled = false;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            clave = txtClave.Text;
            if (clave.Equals(txtClave2.Text))
            {
                DialogResult op = MessageBox.Show("¿Cambiar Contraseña?", usuarioPerfil, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (op == DialogResult.Yes)
                {
                    conexion.Open();
                    String modSql = "update usuarios set clave=?cl where nombre=?nom";
                    MySqlCommand comando = new MySqlCommand(modSql, conexion);
                    comando.Parameters.Add("?nom", MySqlDbType.String).Value = usuarioPerfil;
                    comando.Parameters.Add("?cl", MySqlDbType.String).Value = clave;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    MessageBox.Show("Contraseña Cambiada Correctamente");
                    txtClave.Text = "";
                    txtClave2.Text = "";
                }
                else
                {
                    MessageBox.Show("Cambio de contraseña cancelado");
                    txtClave.Text = "";
                    txtClave2.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Las contraseñas no coinciden");
                txtClave.Text = "";
                txtClave2.Text = "";
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    imagen = openFileDialog1.FileName;
                    pictureBox1.Image = Image.FromFile(imagen);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("El archivo seleccionado no de tipo imagen");
            }
        }

        private void btnImagen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imagen = openFileDialog1.FileName;
            }
            FileStream fs = new FileStream(imagen, FileMode.Open, FileAccess.Read);
            long tamanio = fs.Length;

            BinaryReader br = new BinaryReader(fs);
            byte[] bloque = br.ReadBytes((int)fs.Length);
            fs.Read(bloque, 0, Convert.ToInt32(tamanio));

            conexion.Open();
            String cadenaSql = "update usuarios set imagen=?img where nombre=?nom";
            MySqlCommand comando = new MySqlCommand(cadenaSql, conexion);
            comando.Parameters.Add("?nom", MySqlDbType.String).Value = usuarioPerfil;
            comando.Parameters.Add("?img", MySqlDbType.Blob).Value = bloque;
            comando.ExecuteNonQuery();
            conexion.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            conexion.Open();
            String cadenaSql = "delete from chat where usuario=?us";
            MySqlCommand comando = new MySqlCommand(cadenaSql, conexion);
            comando.Parameters.Add("?us", MySqlDbType.String).Value = usuarioPerfil;
            comando.ExecuteNonQuery();
            conexion.Close();
            MessageBox.Show("Tus mensajes se han borrado de la BD");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            conexion.Open();
            String cadenaSql;
            MySqlCommand comando;

            DialogResult dr = MessageBox.Show("Tu cuenta será\n\tELIMINADA", usuarioPerfil, MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                DialogResult dr2 = MessageBox.Show("Antes de continuar\n¿Quieres borrar tu historial de mensajes?", usuarioPerfil, MessageBoxButtons.YesNo);
                if (dr2 == DialogResult.Yes) {
                    //eliminando mensajes
                    cadenaSql = "delete from chat where usuario=?us";
                    comando = new MySqlCommand(cadenaSql, conexion);
                    comando.Parameters.Add("?us", MySqlDbType.String).Value = usuarioPerfil;
                    comando.ExecuteNonQuery();
                    MessageBox.Show("Tus mensajes se han borrado de la BD");

                    //eliminando cuenta
                    cadenaSql = "delete from usuarios where nombre=?nom";
                    comando = new MySqlCommand(cadenaSql, conexion);
                    comando.Parameters.Add("?nom", MySqlDbType.String).Value = usuarioPerfil;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    MessageBox.Show("Cuenta Eliminada");
                    LOGIN f1 = new LOGIN();
                    f1.Show();
                    this.Close();
                }else
                {
                    cadenaSql = "delete from usuarios where nombre=?nom";
                    comando = new MySqlCommand(cadenaSql, conexion);
                    comando.Parameters.Add("?nom", MySqlDbType.String).Value = usuarioPerfil;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    MessageBox.Show("Cuenta Eliminada");
                    LOGIN f1 = new LOGIN();
                    f1.Show();
                    this.Close();
                }
                

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            contadorSesion--;
            if(contadorSesion<=10 && contadorSesion > 0)
            {
                lblWarning2.Visible = true;
                lblWarning2.Text = ""+contadorSesion;
            }
            else if(contadorSesion == 0)
            {
                CHAT c = new CHAT();
                c.Show();
                c.contadorSesion = contadorSesion;
                c.usuario = usuarioPerfil;
                this.Close();
                timer1.Enabled = false;
            }
        }
    }
}
