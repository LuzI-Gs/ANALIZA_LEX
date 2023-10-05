using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANALIZA_LEX
{
    class Identificador
    {
        private int _intNumero;

        public int Numero
        {
            get { return _intNumero; }
            set { _intNumero = value; }
        }

        private string _strIdentificador;
        public string strIdentificador
        {
            get { return _strIdentificador; }
            set { _strIdentificador = value; }
        }

        private string _strNombre;

        public string Nombre
        {
            get { return _strNombre; }
            set { _strNombre = value; }
        }

        private string _strTipoDato;

        public string TipoDato
        {
            get { return _strTipoDato; }
            set { _strTipoDato = value; }
        }

        private string _strValor;

        public string Valor
        {
            get { return _strValor; }
            set { _strValor = value; }
        }
        public override string ToString()
        {
            return this.Nombre;
        }
    }
}
