using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class PC : Dispositivo
    {
        public ListaEnlazada<Paquete> ColaRecibidos { get; set; }
        public ListaEnlazada<Paquete> MensajesRecibidos { get; set; }

        public PC(string ip, string nombre) : base(ip, nombre)
        {
            ColaRecibidos = new ListaEnlazada<Paquete>();
            MensajesRecibidos = new ListaEnlazada<Paquete>();
        }

        public override bool RecibirPaquete(Paquete paquete)
        {
            if (paquete.IPDestino != IP) return false;

            ColaRecibidos.InsertarFinal(paquete);
            paquete.Estado = EstadoPaquete.Recibido;
            paquete.AgregarTraza("PC", IP);
            return true;
        }

        public override Paquete EnviarPaquete()
        {
            var paquete = ColaEnvio.EliminarInicio();
            if (paquete != null)
            {
                paquete.Estado = EstadoPaquete.Enviado;
                paquete.AgregarTraza("PC", IP);
            }
            return paquete;
        }

        public void ProcesarMensajesRecibidos()
        {
            // Implementación para procesar mensajes completos
            // Esta lógica debería verificar mensajes completos y moverlos a MensajesRecibidos
        }

        public override string ObtenerEstado()
        {
            return $"PC {Nombre} ({IP})\n" +
                   $" - Paquetes en cola de envío: {ColaEnvio.Count}\n" +
                   $" - Paquetes recibidos: {ColaRecibidos.Count}\n" +
                   $" - Mensajes completos recibidos: {MensajesRecibidos.Count}";
        }
    }
}
