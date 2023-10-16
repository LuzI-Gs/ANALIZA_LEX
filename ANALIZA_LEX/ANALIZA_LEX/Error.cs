using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANALIZA_LEX
{
    class Error
    {
        private string _strNomError;

        public string NomError
        {
            get { return _strNomError; }
            set { _strNomError = value; }
        }

        private string _strCaracteristica;

        public string Caracteristica
        {
            get { return _strCaracteristica; }
            set { _strCaracteristica = value; }
        }

        public string MostrarCaracteristica()
        {
            switch (NomError)
            {
                case "ERR1":
                    return "Error de cadena";
                case "ERR2":
                    return "Error comentario";
                case "ERR3":
                    return "Error de ingreso arreglo";
                case "ERR4":
                    return "Error general";
                case "ERR5":
                    return "Error de caracter";
                default:
                    return "Error";
            }
        }
    }
}
