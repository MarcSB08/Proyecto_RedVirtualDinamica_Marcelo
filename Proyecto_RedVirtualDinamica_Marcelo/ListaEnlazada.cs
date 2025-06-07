using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class ListaEnlazada
    {
        #region Atributos
        public NodoLista Cabeza;
        public NodoLista Cola;
        #endregion

        #region Metodos
        public ListaEnlazada()
        {
            Cabeza = null;
            Cola = null;
        }

        public bool EstaVacia() => Cabeza == null;

        public void InsertarFinal(Paquete datos)
        {
            NodoLista nuevo_nodo = new NodoLista(datos);

            if (EstaVacia())
            {
                Cabeza = nuevo_nodo;
                Cola = nuevo_nodo;
            }
            else
            {
                Cola.Siguiente = nuevo_nodo;
                Cola = nuevo_nodo;
            }
        }

        public Paquete EliminarInicio()
        {
            if (EstaVacia()) return null;

            Paquete datos = Cabeza.Datos;
            Cabeza = Cabeza.Siguiente;

            if (Cabeza == null) Cola = null;

            return datos;
        }

        public NodoLista Buscar(string id_paquete)
        {
            NodoLista actual = Cabeza;

            while (actual != null)
            {
                if (actual.Datos.IDPaquete == id_paquete)
                    return actual;
                actual = actual.Siguiente;
            }

            return null;
        }

        public bool Eliminar(string id_paquete)
        {
            if (EstaVacia()) return false;

            NodoLista actual = Cabeza;
            NodoLista anterior = null;

            while (actual != null && actual.Datos.IDPaquete != id_paquete)
            {
                anterior = actual;
                actual = actual.Siguiente;
            }

            if (actual == null) return false;

            if (anterior == null)
            {
                Cabeza = Cabeza.Siguiente;
                if (Cabeza == null) Cola = null;
            }
            else
            {
                anterior.Siguiente = actual.Siguiente;
                if (actual.Siguiente == null) Cola = anterior;
            }

            return true;
        }

        public int Contar()
        {
            int count = 0;
            NodoLista actual = Cabeza;

            while (actual != null)
            {
                count++;
                actual = actual.Siguiente;
            }

            return count;
        }
        #endregion
    }
}
