using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatDam2
{
    class Usuario
    {
        String nombre;
        String clave;
        byte[] imagen;

        public string Nombre
        {
            get
            {return nombre;}
            set
            {nombre = value;}
        }

        public string Clave
        {
            get
            {return clave;}
            set
            {clave = value;}
        }

        public byte[] Imagen
        {
            get
            {return imagen;}
            set
            {imagen = value;}
        }

        public Usuario(string nombre, string clave, byte[] imagen)
        {
            this.Nombre = nombre;
            this.Clave = clave;
            this.Imagen = imagen;
        }

        public Usuario()
        {
        }
    }
}
