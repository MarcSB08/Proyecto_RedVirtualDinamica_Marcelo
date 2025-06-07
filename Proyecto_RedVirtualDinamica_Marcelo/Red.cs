using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class Red
    {
        #region Atributos
        public ListaEnlazada Subredes { get; set; }
        public ListaEnlazada Mensajes { get; set; }
        #endregion

        #region Metodos
        public Red()
        {
            Subredes = new ListaEnlazada();
            Mensajes = new ListaEnlazada();
        }

        public void ConfigurarRed(int cantidad_subredes)
        {
            for (int i = 1; i <= cantidad_subredes; i++)
            {
                string id_subred = i.ToString("00");
                string ip_base = (180 + i).ToString();
                Subred subred = new Subred(id_subred, ip_base);

                Subredes.InsertarFinal(new Paquete
                {
                    IDPaquete = $"SUB{id_subred}",
                    IPOrigen = subred.Enrutador.IP,
                    IPDestino = subred.Computadora.IP,
                    Estado = "Activo"
                });
            }
        }

        public void CrearMensaje(string id_mensaje, string ip_origen, string ip_destino, string contenido)
        {
            Mensaje mensaje = new Mensaje(id_mensaje, ip_origen, ip_destino, contenido);

            Mensajes.InsertarFinal(new Paquete
            {
                IDPaquete = id_mensaje,
                IPOrigen = ip_origen,
                IPDestino = ip_destino,
                Estado = "Nuevo"
            });

            // Agregar paquetes a la cola de envío del PC origen
            NodoLista nodo_subred = BuscarSubredPorIP(ip_origen);
            if (nodo_subred != null)
            {
                NodoLista actual = mensaje.Paquetes.Cabeza;
                while (actual != null)
                {
                    // En implementación real, agregar a la cola del PC
                    actual = actual.Siguiente;
                }
            }
        }

        private NodoLista BuscarSubredPorIP(string ip)
        {
            NodoLista actual = Subredes.Cabeza;

            while (actual != null)
            {
                if (actual.Datos.IPOrigen == ip || actual.Datos.IPDestino == ip)
                    return actual;
                actual = actual.Siguiente;
            }

            return null;
        }
        #endregion
    }
}
