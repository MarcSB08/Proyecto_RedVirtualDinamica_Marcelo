using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class NodoLista<T>
    {
        public T Datos { get; set; }
        public NodoLista<T> Siguiente { get; set; }
        public NodoLista<T> Anterior { get; set; }

        public NodoLista(T datos)
        {
            Datos = datos;
            Siguiente = null;
            Anterior = null;
        }
    }
}
