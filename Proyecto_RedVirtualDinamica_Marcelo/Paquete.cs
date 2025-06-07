using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class Paquete
    {
        public string IDPaquete { get; set; }       // Ej: "M01P01"
        public string IPOrigen { get; set; }       // Ej: "190.01"
        public string IPDestino { get; set; }      // Ej: "180.01"
        public int NumeroSecuencia { get; set; }   // Número de paquete
        public char Dato { get; set; }             // Carácter del mensaje
        public string Estado { get; set; }         // "Nuevo", "Enviado", "Recibido", "Dañado"
        public ListaEnlazada Traza { get; set; }  // Pila para registrar la traza

        public Paquete()
        {
            Traza = new ListaEnlazada();
        }

        public void AgregarTraza(string equipo, string ip)
        {
            Traza.InsertarFinal(new Paquete
            {
                IDPaquete = $"{equipo}-{ip}",
                Estado = "Traza"
            });
        }
    }
}
