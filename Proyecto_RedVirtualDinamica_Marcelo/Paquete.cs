using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class Paquete
    {
        #region Atributos
        public string IDPaquete { get; set; }
        public string IPOrigen { get; set; }
        public string IPDestino { get; set; }
        public int NumeroSecuencia { get; set; }
        public char Dato { get; set; }
        public EstadoPaquete Estado { get; set; }
        public ListaEnlazada<string> Traza { get; set; }
        #endregion

        #region Metodos
        public Paquete()
        {
            Traza = new ListaEnlazada<string>();
            Traza.ConvertirAPila();
        }

        public void AgregarTraza(string dispositivo, string ip)
        {
            Traza.Push($"{dispositivo}|{ip}");
        }

        public IEnumerable<string> ObtenerTraza()
        {
            ListaEnlazada<string> copia = new ListaEnlazada<string>();
            NodoLista<string> actual = Traza.Cabeza;
            while (actual != null)
            {
                copia.InsertarFinal(actual.Datos);
                actual = actual.Siguiente;
            }
            copia.ConvertirAPila();

            while (!copia.EstaVacia())
            {
                yield return copia.Pop();
            }
        }
        #endregion
    }
}
