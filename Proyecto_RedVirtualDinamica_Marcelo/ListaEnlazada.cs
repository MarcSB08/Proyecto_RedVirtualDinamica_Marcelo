using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_RedVirtualDinamica_Marcelo
{
    public class ListaEnlazada<T>
    {
        public NodoLista<T> Cabeza { get; private set; }
        public NodoLista<T> Cola { get; private set; }
        public int Count { get; private set; }

        public ListaEnlazada()
        {
            Cabeza = null;
            Cola = null;
            Count = 0;
        }

        public bool EstaVacia() => Cabeza == null;

        public void InsertarFinal(T datos)
        {
            NodoLista<T> nuevoNodo = new NodoLista<T>(datos);

            if (EstaVacia())
            {
                Cabeza = nuevoNodo;
                Cola = nuevoNodo;
            }
            else
            {
                nuevoNodo.Anterior = Cola;
                Cola.Siguiente = nuevoNodo;
                Cola = nuevoNodo;
            }
            Count++;
        }

        public void InsertarInicio(T datos)
        {
            NodoLista<T> nuevoNodo = new NodoLista<T>(datos);

            if (EstaVacia())
            {
                Cabeza = nuevoNodo;
                Cola = nuevoNodo;
            }
            else
            {
                nuevoNodo.Siguiente = Cabeza;
                Cabeza.Anterior = nuevoNodo;
                Cabeza = nuevoNodo;
            }
            Count++;
        }

        public T EliminarInicio()
        {
            if (EstaVacia()) return default(T);

            T datos = Cabeza.Datos;

            if (Cabeza.Siguiente != null)
            {
                Cabeza = Cabeza.Siguiente;
                Cabeza.Anterior = null;
            }
            else
            {
                Cabeza = null;
                Cola = null;
            }
            Count--;

            return datos;
        }

        public T EliminarFinal()
        {
            if (EstaVacia()) return default(T);

            T datos = Cola.Datos;

            if (Cola.Anterior != null)
            {
                Cola = Cola.Anterior;
                Cola.Siguiente = null;
            }
            else
            {
                Cabeza = null;
                Cola = null;
            }
            Count--;

            return datos;
        }

        public NodoLista<T> Buscar(T dato)
        {
            NodoLista<T> actual = Cabeza;

            while (actual != null)
            {
                if (EqualityComparer<T>.Default.Equals(actual.Datos, dato))
                    return actual;
                actual = actual.Siguiente;
            }

            return null;
        }

        public bool Eliminar(T dato)
        {
            NodoLista<T> actual = Buscar(dato);
            if (actual == null) return false;

            if (actual.Anterior != null)
                actual.Anterior.Siguiente = actual.Siguiente;
            else
                Cabeza = actual.Siguiente;

            if (actual.Siguiente != null)
                actual.Siguiente.Anterior = actual.Anterior;
            else
                Cola = actual.Anterior;

            Count--;
            return true;
        }

        public List<T> ToList()
        {
            List<T> lista = new List<T>();
            NodoLista<T> actual = Cabeza;

            while (actual != null)
            {
                lista.Add(actual.Datos);
                actual = actual.Siguiente;
            }

            return lista;
        }

        public IEnumerable<T> Recorrer()
        {
            NodoLista<T> actual = Cabeza;
            while (actual != null)
            {
                yield return actual.Datos;
                actual = actual.Siguiente;
            }
        }
    }
}
