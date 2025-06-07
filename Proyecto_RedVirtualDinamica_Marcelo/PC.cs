using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class PC : Dispositivo
    {
        #region Atributos
        public ListaEnlazada ColaRecibidos { get; set; }
        public ListaEnlazada MensajesRecibidos { get; set; }
        #endregion

        #region Metodos
        public PC(string ip, string nombre) : base(ip, nombre)
        {
            ColaRecibidos = new ListaEnlazada();
            MensajesRecibidos = new ListaEnlazada();
        }

        public override bool RecibirPaquete(Paquete paquete)
        {
            if (paquete.IPDestino != IP) return false;

            ColaRecibidos.InsertarFinal(paquete);
            paquete.Estado = "Recibido";
            paquete.AgregarTraza("PC", IP);
            return true;
        }

        public override Paquete EnviarPaquete()
        {
            return ColaPaquetes.EliminarInicio();
        }

        public void ProcesarMensajesRecibidos()
        {
            NodoLista actual = ColaRecibidos.Cabeza;
            while (actual != null)
            {
                MensajesRecibidos.InsertarFinal(actual.Datos);
                actual = actual.Siguiente;
            }
            ColaRecibidos = new ListaEnlazada(); // Limpiar cola
        }
        #endregion
    }
}
