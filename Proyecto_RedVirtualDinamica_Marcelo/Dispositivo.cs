using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public abstract class Dispositivo
    {
        public string IP { get; set; }
        public string Nombre { get; set; }
        public ListaEnlazada<Paquete> ColaEnvio { get; set; }

        protected Dispositivo(string ip, string nombre)
        {
            IP = ip;
            Nombre = nombre;
            ColaEnvio = new ListaEnlazada<Paquete>();
        }

        public abstract bool RecibirPaquete(Paquete paquete);
        public abstract Paquete EnviarPaquete();
        public abstract string ObtenerEstado();
    }
}
