using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANALIZA_LEX
{
    public class Nodo
    {
        #region CAMPOS DE CLASE
        private Object datos;
        private Nodo nodoIzquiedo;
        private Nodo nodoDerecho;
        #endregion

        #region CONSTRUCTORES
        public Nodo()
        {
            nodoDerecho = nodoIzquiedo = null;
        }

        public Nodo(Object datos)
        {
            this.datos = datos;
            nodoDerecho = nodoIzquiedo = null;
        }

        public Nodo(Nodo derecho, Nodo izquierdo, Object valor)
        {
            this.nodoDerecho = derecho;
            this.nodoIzquiedo = izquierdo;
            this.datos = valor;
        }
        #endregion

        #region PROPIEDADES CLASE NODO
        //nodoIzquierdo
        public Nodo NodoIzquierdo { get => nodoIzquiedo; set => nodoIzquiedo = value; }

        //nodoDerecho
        public Nodo NodoDerecho { get => nodoDerecho; set => nodoDerecho = value; }

        //datos
        public Object Datos { get => datos; set => datos = value; }
        #endregion 
    }
}
