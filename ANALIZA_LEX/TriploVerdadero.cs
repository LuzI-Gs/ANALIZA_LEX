using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANALIZA_LEX
{
    class TriploVerdadero
    {
        private string _strDatoObjeto;

        public string DatoObjeto
        {
            get { return _strDatoObjeto; }
            set { _strDatoObjeto = value; }
        }

        private string _strDatoFuente;
        public string DatoFuente
        {
            get { return _strDatoFuente; }
            set { _strDatoFuente = value; }
        }

        private string _strOperador;

        public string Operador
        {
            get { return _strOperador; }
            set { _strOperador = value; }
        }
    }
}
