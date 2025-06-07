using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class NodoLista
    {
        #region Atributos
        public Paquete Datos { get; set; }
        public NodoLista Siguiente { get; set; }
        public NodoLista Anterior { get; set; }
        #endregion

        #region Metodos
        public NodoLista(Paquete datos)
        {
            Datos = datos;
            Siguiente = null;
            Anterior = null;
        }
        #endregion
    }
}
