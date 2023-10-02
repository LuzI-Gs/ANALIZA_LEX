using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANALIZA_LEX
{
    internal class ClasePila
    {
        /*cada nodo representa un elemento
en la pila y contiene un caracter(char) llamado simbolo
y una referencia al siguiente nodo (siguiente)*/
        class Nodo
        {
            public char simbolo;
            public Nodo siguiente;
        }
        private Nodo top;//nodo en la parte superior de la pila
        public ClasePila()
        {
            top = null; //se establece en null porque la pila esta vacia cuando se crea una instancia de ña cñase Pila
        }
        /*agrega un caracter a la pila, crea un nuevo nodo con el caracter
        x y lo coloca en la parte superior de la pila*/
        public void Insertar(char x)
        {
            Nodo nuevo;
            nuevo = new Nodo();
            nuevo.simbolo = x;
            if (top == null)
            {
                nuevo.siguiente = null;
                top = nuevo;
            }
            else
            {
                nuevo.siguiente = top;
                top = nuevo;
            }
        }
        /*extrae el carácter en la parte superior
        * de la pila y devolverlo. Si la pila no
        * está vacía, se extrae el carácter y se
        * actualiza la referencia de top(nodo superior de la pila).
        * Si la pila está vacía, se devuelve char.MaxValue
        * para indicar que la pila está vacía.*/
        public char Extraer()
        {
            if (top != null)
            {
                char informacion = top.simbolo;
                top = top.siguiente;
                return informacion;
            }
            else
            {
                return char.MaxValue;
            }
        }
        public bool Vacia()
        {
            if (top == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
