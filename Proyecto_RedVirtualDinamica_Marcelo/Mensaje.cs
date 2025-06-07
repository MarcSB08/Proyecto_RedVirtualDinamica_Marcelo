using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    internal class Mensaje
    {
        #region Atributos
        public string IDMensaje { get; set; }   // Ej: "M01"
        public string IPOrigen { get; set; }    // Ej: "190.01"
        public string IPDestino { get; set; }  // Ej: "180.01"
        public string Contenido { get; set; }   // Cadena de texto
        public string Estado { get; set; }      // "Nuevo", "Enviado", "Recibido", "Dañado"
        public ListaEnlazada Paquetes { get; set; }
        #endregion

        #region Metodos
        public Mensaje(string id, string ip_origen, string ip_destino, string contenido)
        {
            IDMensaje = id;
            IPOrigen = ip_origen;
            IPDestino = ip_destino;
            Contenido = contenido;
            Estado = "Nuevo";
            Paquetes = new ListaEnlazada();

            // Crear paquetes para cada carácter del mensaje
            for (int i = 0; i < contenido.Length; i++)
            {
                Paquete paquete = new Paquete
                {
                    IDPaquete = $"{IDMensaje}P{(i + 1).ToString("00")}",
                    IPOrigen = IPOrigen,
                    IPDestino = IPDestino,
                    NumeroSecuencia = i + 1,
                    Dato = Contenido[i],
                    Estado = "Nuevo"
                };

                Paquetes.InsertarFinal(paquete);
            }
        }

        public bool VerificarIntegridad()
        {
            if (Estado == "Dañado") return false;

            NodoLista actual = Paquetes.Cabeza;
            string mensaje_reconstruido = "";

            while (actual != null)
            {
                if (actual.Datos.Estado != "Recibido")
                    return false;

                mensaje_reconstruido += actual.Datos.Dato;
                actual = actual.Siguiente;
            }

            return mensaje_reconstruido == Contenido;
        }
        #endregion
    }
}
