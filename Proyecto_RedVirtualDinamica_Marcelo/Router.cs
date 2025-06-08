using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class Router : Dispositivo
    {
        private const int CapacidadMaxima = 4;
        public Red Red { get; set; }

        public Router(string ip, string nombre, Red red) : base(ip, nombre)
        {
            Red = red;
        }

        public override bool RecibirPaquete(Paquete paquete)
        {
            if (ColaEnvio.Count >= CapacidadMaxima)
                return false;

            ColaEnvio.InsertarFinal(paquete);
            paquete.Estado = EstadoPaquete.EnTransito;
            paquete.AgregarTraza("Router", IP);
            return true;
        }

        public override Paquete EnviarPaquete()
        {
            var paquete = ColaEnvio.EliminarInicio();
            if (paquete != null)
            {
                paquete.Estado = EstadoPaquete.Enviado;
                paquete.AgregarTraza("Router", IP);
            }
            return paquete;
        }

        public bool ReenviarPaquete(Paquete paquete)
        {
            // Lógica para reenviar a otro router si este está lleno
            foreach (var router in Red.ObtenerRouters())
            {
                if (router.IP != IP && router.RecibirPaquete(paquete))
                {
                    return true;
                }
            }
            return false;
        }

        public override string ObtenerEstado()
        {
            return $"Router {Nombre} ({IP})\n" +
                   $" - Paquetes en cola: {ColaEnvio.Count}/{CapacidadMaxima}\n" +
                   $" - Estado: {(ColaEnvio.Count >= CapacidadMaxima ? "Lleno" : "Disponible")}";
        }
    }
}
