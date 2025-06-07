using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class Subred
    {
        #region Atributos
        public string ID { get; set; }
        public PC Computadora { get; set; }
        public Router Enrutador { get; set; }
        #endregion

        #region Metodos
        public Subred(string id, string ip_base)
        {
            ID = id;
            Enrutador = new Router($"{ip_base}.0", $"R{id}");
            Computadora = new PC($"{ip_base}.01", $"PC{id}");
        }

        public bool EnviarPaquete(Paquete paquete)
        {
            if (Computadora.IP == paquete.IPOrigen)
            {
                return Enrutador.RecibirPaquete(paquete);
            }
            return false;
        }

        public bool RecibirPaquete(Paquete paquete)
        {
            if (Computadora.IP == paquete.IPDestino)
            {
                return Computadora.RecibirPaquete(paquete);
            }
            return false;
        }
        #endregion
    }
}
