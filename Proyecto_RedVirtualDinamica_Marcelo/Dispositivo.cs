using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public abstract class Dispositivo
    {
        #region Atributos
        public string IP { get; set; }
        public string Nombre { get; set; }
        public ListaEnlazada ColaPaquetes { get; set; }
        #endregion

        #region Metodos
        protected Dispositivo(string ip, string nombre)
        {
            IP = ip;
            Nombre = nombre;
            ColaPaquetes = new ListaEnlazada();
        }

        public abstract bool RecibirPaquete(Paquete paquete);
        public abstract Paquete EnviarPaquete();
        #endregion
    }
}
