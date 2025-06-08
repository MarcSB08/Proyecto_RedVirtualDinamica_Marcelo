using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class Paquete
    {
        public string IDPaquete { get; set; }
        public string IPOrigen { get; set; }
        public string IPDestino { get; set; }
        public int NumeroSecuencia { get; set; }
        public char Dato { get; set; }
        public EstadoPaquete Estado { get; set; }
        public Stack<string> Traza { get; set; }

        public Paquete()
        {
            Traza = new Stack<string>();
        }

        public void AgregarTraza(string dispositivo, string ip)
        {
            Traza.Push($"{dispositivo} ({ip})");
        }

        public string ObtenerTraza()
        {
            return string.Join(" → ", Traza);
        }
    }
}
