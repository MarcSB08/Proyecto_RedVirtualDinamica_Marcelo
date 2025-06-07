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
                nuevo_nodo.Anterior = Cola;
                Cola.Siguiente = nuevo_nodo;
                Cola = nuevo_nodo;
            }
        }

        public void InsertarInicio(Paquete datos)
        {
            NodoLista nuevo_nodo = new NodoLista(datos);

            if (EstaVacia())
            {
                Cabeza = nuevo_nodo;
                Cola = nuevo_nodo;
            }
            else
            {
                nuevo_nodo.Siguiente = Cabeza;
                Cabeza.Anterior = nuevo_nodo;
                Cabeza = nuevo_nodo;
            }
        }

        public Paquete EliminarInicio()
        {
            if (EstaVacia()) return null;

            Paquete datos = Cabeza.Datos;

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

            return datos;
        }

        public Paquete EliminarFinal()
        {
            if (EstaVacia()) return null;

            Paquete datos = Cola.Datos;

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
            NodoLista actual = Buscar(id_paquete);
            if (actual == null) return false;

            if (actual.Anterior != null)
                actual.Anterior.Siguiente = actual.Siguiente;
            else
                Cabeza = actual.Siguiente;

            if (actual.Siguiente != null)
                actual.Siguiente.Anterior = actual.Anterior;
            else
                Cola = actual.Anterior;

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

        public void InsertarDespues(NodoLista nodo_anterior, Paquete datos)
        {
            if (nodo_anterior == null) return;

            NodoLista nuevo_nodo = new NodoLista(datos);
            nuevo_nodo.Siguiente = nodo_anterior.Siguiente;
            nuevo_nodo.Anterior = nodo_anterior;

            if (nodo_anterior.Siguiente != null)
                nodo_anterior.Siguiente.Anterior = nuevo_nodo;
            else
                Cola = nuevo_nodo;

            nodo_anterior.Siguiente = nuevo_nodo;
        }
        #endregion
    }
}
