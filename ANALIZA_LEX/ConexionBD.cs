using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ANALIZA_LEX
{
    internal class ConexionBD
    {
        //Cadena de Conexion

        //gema
       // string conexion = ("server = GEMA\\SQLEXPRESS01  ;database = MatrizAnalizadorLexicoLARS; integrated security = true");

        //luz
        string conexion = ("server = PCGSIL  ;database = MatrizAnalizadorLexicoLARS; integrated security = true");

        /*en server le van a poner el nombre de su servidor en SqlServer, pero primero deben crear una base de datos que se llame 
         MATRIZ_TRANSICION (CREATE DATABASE MATRIZ_TRANSICION;) ya que se haya creado, borran esa linea donde se crea la BD,
            y pegan el script de sql tal como esta*/
        //RECUERDEN COLOCAR SU PROPIO SERVER DE SU COMPU NO LO OLVIDEN NIÑOS
        public SqlConnection Conectarbd = new SqlConnection();

        //Constructor
        public ConexionBD()
        {
            Conectarbd.ConnectionString = conexion;
        }

        //Metodo para abrir la conexion
        public void abrir()

        {
            try
            {
                Conectarbd.Open();
                //MessageBox.Show("Si se abrió la conexión");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error al abrir BD " + ex.Message);
            }
        }

        //Metodo para cerrar la conexion
        public void cerrar()
        {
            Conectarbd.Close();
        }

    }
}
