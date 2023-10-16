using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;//agregue esto

namespace ANALIZA_LEX
{
    public class Arbol
    {
        #region CAMPOS DE CLASE
        //INSERCION EN UNA COLA
        private string precedencia = "+-*/^";
        private string[] delimitadores = { "+", "-", "*", "/", "^" };
        private string[] operandosArray;
        private string[] operandoresArray;
        private Queue colaExpresion;

        //CREACION DEL ARBOL
        private string token;
        private string operadorTemp;
        private int i = 0;
        private Stack pilaOperadores;
        private Stack pilaOperandos;
        private Stack pilaDot;
        private Nodo raiz = null;
        public Nodo nodoDot { get; set; }

        //PROPIEDADES PARA RECORRIDOS
        public string cadenaPreorden { get; set; }
        public string cadenaInorden { get; set; }
        public string cadenaPostorden { get; set; }

        #endregion

        #region CONSTRUCTORES
        public Arbol()
        {
            pilaOperadores = new Stack();
            pilaOperandos = new Stack();
            pilaDot = new Stack();
            colaExpresion = new Queue();
        }
        #endregion

        #region INSERCION EN LA COLA
        public void InsertarEnCola(string expresion)
        {
            operandosArray = expresion.Split(delimitadores, StringSplitOptions.RemoveEmptyEntries);
            operandoresArray = expresion.Split(operandosArray, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; colaExpresion.Count < operandosArray.Length + (operandoresArray.Length - 1); i++)
            {
                colaExpresion.Enqueue(operandosArray[i]);
                colaExpresion.Enqueue(operandoresArray[i]);

            }

            colaExpresion.Enqueue(operandosArray[operandosArray.Length - 1]);
        }
        #endregion

        #region ARBOL
        public Nodo CrearArbol()
        {
            while (colaExpresion.Count != 0)
            {
                token = (string)colaExpresion.Dequeue();
                if (precedencia.IndexOf(token) < 0)
                {
                    pilaOperandos.Push(new Nodo(token));
                    pilaDot.Push(new Nodo($"nodo{++i}[label=\"{token}\"]"));

                }
                else
                {
                    if (pilaOperadores.Count != 0)
                    {
                        operadorTemp = (string)pilaOperadores.Peek();
                        while (pilaOperadores.Count != 0 &&
                            precedencia.IndexOf(operadorTemp) >= precedencia.IndexOf(token))
                        {
                            GuardaSubArbol();
                            if (pilaOperadores.Count != 0)
                            {
                                operadorTemp = (string)pilaOperadores.Peek();
                            }
                        }
                    }
                    pilaOperadores.Push(token);
                }
            }

            raiz = (Nodo)pilaOperandos.Peek();
            nodoDot = (Nodo)pilaDot.Peek();
            while (pilaOperadores.Count != 0)
            {
                GuardaSubArbol();
                raiz = (Nodo)pilaOperandos.Peek();
                nodoDot = (Nodo)pilaDot.Peek();

            }
            return raiz;
        }

        private void GuardaSubArbol()
        {
            Nodo derecho = (Nodo)pilaOperandos.Pop();
            Nodo izquierdo = (Nodo)pilaOperandos.Pop();
            pilaOperandos.Push(new Nodo(derecho, izquierdo, pilaOperadores.Peek()));

            Nodo derechoG = (Nodo)pilaDot.Pop();
            Nodo izquierdoG = (Nodo)pilaDot.Pop();
            pilaDot.Push(new Nodo(derechoG, izquierdoG, $"nodo{++i}[label=\"{pilaOperadores.Pop()}\"]"));


        }
        #endregion

        #region RECORRIDOS

        //preorden
        public string InsertarPre(Nodo tree)
        {
            if (tree != null)
            {
                cadenaPreorden += tree.Datos + " ";
                InsertarPre(tree.NodoIzquierdo);
                InsertarPre(tree.NodoDerecho);
            }
            return cadenaPreorden;
        }


        //inorden
        public string InsertarIn(Nodo tree)
        {
            if (tree != null)
            {

                InsertarIn(tree.NodoIzquierdo);
                cadenaInorden += tree.Datos + " ";
                InsertarIn(tree.NodoDerecho);
            }
            return cadenaInorden;
        }

        //postorden
        public string InsertarPost(Nodo tree)
        {
            if (tree != null)
            {

                InsertarPost(tree.NodoIzquierdo);
                InsertarPost(tree.NodoDerecho);
                cadenaPostorden += tree.Datos + " ";
            }
            return cadenaPostorden;
        }

        #endregion


        public void Limpiar()
        {
            cadenaPreorden = "";
            cadenaInorden = "";
            cadenaPostorden = "";
        }
    }
}
