using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class Subred
    {
        public string ID { get; set; }
        public PC Computadora { get; set; }
        public Router Enrutador { get; set; }

        public Subred(string id, string ipBase, Red red)
        {
            ID = id;
            Enrutador = new Router($"{ipBase}.0", $"R{id}", red);
            Computadora = new PC($"{ipBase}.01", $"PC{id}");
        }

        public bool EnviarPaqueteDesdePC(Paquete paquete)
        {
            if (Computadora.IP == paquete.IPOrigen)
            {
                var paqueteEnviado = Computadora.EnviarPaquete();
                if (paqueteEnviado != null)
                {
                    return Enrutador.RecibirPaquete(paqueteEnviado);
                }
            }
            return false;
        }

        public bool RecibirPaqueteEnPC(Paquete paquete)
        {
            if (Computadora.IP == paquete.IPDestino)
            {
                return Computadora.RecibirPaquete(paquete);
            }
            return false;
        }

        public string ObtenerEstado()
        {
            return $"Subred {ID}\n" +
                   $" - {Computadora.ObtenerEstado()}\n" +
                   $" - {Enrutador.ObtenerEstado()}";
        }
    }
}
