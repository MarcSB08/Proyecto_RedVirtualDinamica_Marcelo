using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class Mensaje
    {
        public string IDMensaje { get; set; }
        public string IPOrigen { get; set; }
        public string IPDestino { get; set; }
        public string Contenido { get; set; }
        public EstadoMensaje Estado { get; set; }
        public ListaEnlazada<Paquete> Paquetes { get; set; }

        public Mensaje(string id, string ipOrigen, string ipDestino, string contenido)
        {
            IDMensaje = id;
            IPOrigen = ipOrigen;
            IPDestino = ipDestino;
            Contenido = contenido;
            Estado = EstadoMensaje.Nuevo;
            Paquetes = new ListaEnlazada<Paquete>();

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
                    Estado = EstadoPaquete.Nuevo
                };
                paquete.AgregarTraza("PC", IPOrigen);

                Paquetes.InsertarFinal(paquete);
            }
        }
    }
}
