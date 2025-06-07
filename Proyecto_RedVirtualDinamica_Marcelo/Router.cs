using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class Router : Dispositivo
    {
        #region Atributos
        private const int CapacidadMaxima = 4;
        #endregion

        #region Metodos
        public Router(string ip, string nombre) : base(ip, nombre) { }

        public override bool RecibirPaquete(Paquete paquete)
        {
            if (ColaPaquetes.Contar() >= CapacidadMaxima)
                return false;

            ColaPaquetes.InsertarFinal(paquete);
            paquete.Estado = "Enviado";
            paquete.AgregarTraza("Router", IP);
            return true;
        }

        public override Paquete EnviarPaquete()
        {
            Paquete paquete = ColaPaquetes.EliminarInicio();
            if (paquete != null)
            {
                paquete.Estado = "Enviado";
            }
            return paquete;
        }
        #endregion
    }
}
