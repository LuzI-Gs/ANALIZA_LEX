using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ANALIZA_LEX
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int intEstado = 0;
        string token="";
        string sintactico ="";
        string LenguajeNat = "";
        ConexionBD conexion= new ConexionBD();
        public void MostrarMatriz()
        {
            conexion.abrir();

            SqlCommand query1 = new SqlCommand(" Select * from  Matriz", conexion.Conectarbd);
            //SqlCommand query1 = new SqlCommand(" Select * from MANL", conexion.Conectarbd);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = query1;
            DataTable Matriz = new DataTable();
            adaptador1.Fill(Matriz);
            dtgMatriz.DataSource = Matriz;
            conexion.cerrar();
        }
        public string BuscarToken(int Estado)
        {
            string tk="";
            //RECORRE TODAS LAS COLUMNAS DE LA TABLA 
            for (int i = 0; i < dtgMatriz.ColumnCount; i++)
            {
                //SI ENCUENTRA LA COLUMNA QUE SE LLAMA TOKEN PROCEDE A HACER LAS SIG INSTRUCCIONES
                if (dtgMatriz.Columns[i].HeaderText == "FDC")
                {
                    //TOMA EL VALOR DEL TOKEN DEPENDIENDO DEL ESTADO Y LOS ALMACENA EN LA VARIABLE Y LOS VA SUMANDO PARA FORMAR LA CADENA DE TOKENS 
                    tk=dtgMatriz.Rows[Estado].Cells[i+1].Value.ToString();
                    //PINTA DE COLOR AZUL LA CELDA DEL TOKEN 
                    dtgMatriz.Rows[Estado].Cells[i+1].Style.BackColor = Color.Blue;
                    //SE ESCRIBE EN EL TEXTBOX LA CADENA DE TOKENS 
                   // txtTokens.Text = token;
                }

            }
            return tk;
        }
        public void DescomponerCadenas()
        {
            try
            {
                char[] del1 = { '\n' };
                LenguajeNat = txtLenguaje.Text;
                txtTokens.Text = "";
                //TOMA LA LINEA INGRESADA
                string strPalabras = LenguajeNat;
                //METE LAS LINEAS EN UN ARREGLO 
                string[] Lenguajes = strPalabras.Split(del1);
                foreach (string palabras in Lenguajes)
                {
                    //listBox3.Items.Add(palabras);
                    char[] del = { ' ' };
                    char chMuestra = ' ';
                    int j = 0;
                    //TOMA LA ORACION  DE LA LINEA INGRESADA
                    string strPalabra = palabras;
                    //METE LAS ORACIONES DE CADA LINEA  EN UN ARREGLO 
                    string[] Lenguaje = strPalabra.Split(del);
                    //RECORRE EL ARREGLO DE LAS PALABRAS 
                    foreach (string palabra in Lenguaje)
                    {
//                        listBox1.Items.Add(palabra);

                        //VARIABLE PARA INGRESAR EL IDE
                        string strIDE = palabra;
                        //CICLO QUE EXTRAE CADA LETRA
                        for (int i = 0; i < strIDE.Length; i++)
                        {
                            chMuestra = strIDE[i];
                            //AGREGA CADA Letra AL LISTBOX SOLO ES DE PRUEBA NOMAS PARA QUE SE VEA EL FUNCIONAMIENTO
                          //  listBox2.Items.Add(chMuestra);
                            ValidarIDE(intEstado, char.ToUpper(chMuestra));
                        }
                       
                        if (intEstado != 0)
                        {
                            //BUSCA EL TOKEN DE CADA PALABRA DESPUES DE VALIDAR EL IDE
                            
                            token =BuscarToken(intEstado);
                            txtTokens.Text =txtTokens.Text +" "+token.Trim();
                            intEstado = 0;

                        }
                    }
                    if (palabras != "\r\n")
                    {
                        txtTokens.Text = txtTokens.Text + "\r\n";
                    }
                    if (palabras != " ")
                    {
                        txtTokens.Text = txtTokens.Text +"";
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Por favor ingresa una cadena", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnValidar_Click(object sender, EventArgs e)
        {
            try
            {
                //INVOCA EL METODO
                DescomponerCadenas();
                LenguajeNat = "";
                txtTokens.ReadOnly= true;
            }
            catch (Exception)
            {
                MessageBox.Show("Por favor ingresa una cadena", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public bool ValidarIDE(int Estado, char letra)
        {

            //RECORRE LAS COLUMAS DE LA TABLA OSEA EL ALFABETO-NUMEROS-OTROS
            for (int i = 1; i < dtgMatriz.ColumnCount - 2; i++)
            {
                //BUSCA POR LAS COLUMNAS  DESDE LA POSICION 1 OSEA DESDE LA lETRA -A- EN LA COLUMNA (0) QUE SON LOS NOMBRES DE LA TABLA 
                //SI EL CARACTER  ES IGUAL A LA LETRA ENVIADA 
                if (dtgMatriz.Columns[i].HeaderText[0] == letra)
                {
                    // EN CASO DE QUE SEA UN CARACTER NO VALIDO  VERIFICA QUE SU ESTADO SEA 113 
                    if (dtgMatriz.Rows[Estado].Cells[i].Value.ToString() == "108")
                    {
                        //LO COLOREA DE ROJO ESE CARACTER
                        dtgMatriz.Rows[Estado].Cells[i].Style.BackColor = Color.Red;
                        //COMO ES UN ESTADO DE ERROR MANDA A TODOS AL ESTADO 113 Y LOS PINTA DE ROJO POR EJEMPLO SI INGRESA 1HOLA LOS PINTARA DE ROJO

                        this.intEstado = int.Parse(dtgMatriz.Rows[Estado].Cells[i].Value.ToString());
                    }
                    else
                    {
                        //SI NO LO ES PINTA DE AZUL LA CELDA DEL CARACTER
                        // Si encuentra el Letra, obtiene el estado correspondiente y retorna true y pinta la letra correspondiente 
                        dtgMatriz.Rows[Estado].Cells[i ].Style.BackColor = Color.Blue;
                        this.intEstado = int.Parse(dtgMatriz.Rows[Estado].Cells[i].Value.ToString());
                    }
                    return true;
                }

            }

            return false;



        }


        private void Form1_Load(object sender, EventArgs e)
        {
            MostrarMatriz();
            txtTokens.ReadOnly= true;
       

        }

        private void dtgV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnValidarS_Click(object sender, EventArgs e)
        {
            //-------LISTOS-------//

            /* string textoCompleto = txtTokens.Text.Trim();
             string[] palabras = textoCompleto.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

             if (palabras.Length > 0)
             {
                 string primeraPalabra = palabras[0];

                 if (primeraPalabra == " PR16")
                 {
                     ESTRUCTURADECISION();
                 }
                 else if (primeraPalabra == "IDENTV01")
                 {
                     if (palabras.Length > 2)
                     {
                         string segundaPalabra = palabras[2];
                         bool coincideOPA = Regex.IsMatch(segundaPalabra, ".*OPA.*");
                         bool coincideOPR = Regex.IsMatch(segundaPalabra, ".*OPR.*");
                         bool coincideOPL = Regex.IsMatch(segundaPalabra, ".*OPL.*");
                         bool coincideASIG = Regex.IsMatch(segundaPalabra, ".*PR.*");

                         if (coincideOPA)
                         {
                             ESTRUCTURAOPA();
                         }
                         else if (coincideOPR)
                         {
                             ESTRUCTURAOPR();
                         }
                         else if (coincideOPL)
                         {
                             ESTRUCTURAOPL();
                         }
                         else if (coincideASIG)
                         {
                             ESTRUCTURAIVARIABALE();
                         }
                         else
                         {
                             MessageBox.Show("Segunda palabra inválida para IDENTV01.");
                         }
                     }
                     else
                     {
                         MessageBox.Show("Falta la segunda palabra después de IDENTV01.");
                     }
                 }
                 else if (primeraPalabra == "PR15")
                 {
                     ESTRUCTURAPARA();
                 }
                 else if (primeraPalabra == "PR14")
                 {
                     ESTRUCTURAMIENTRAS();
                 }
                 else if (primeraPalabra == "PR09")
                 {
                     ESTRUCTURAHAZ();
                 }
                 else if (primeraPalabra == "PR10")
                 {
                     ESTRUCTURAIMP();
                 }
                 else
                 {
                     MessageBox.Show("Primera palabra inválida.");
                 }
             }
             else
             {
                 MessageBox.Show("El texto está vacío.");
             }*/

            //  ESTRUCTURADECISION();
            // ESTRUCTURAHAZ();
            // ESTRUCTURAMIENTRAS();
           
            string textoCompleto = txtTokens.Text.Trim(); // Obtener el contenido del TextBox y eliminar espacios en blanco al principio y al final
            string[] lineas = textoCompleto.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

               // ESTRUCTURAIVARIABALE(textoCompleto);
              /* if (lineas[0].Contains("IDENTV01 .*OPA.* ") )
                {
                    ESTRUCTURAOPA(lineas[0]);
                }
                else if (lineas[1].Contains("IDENTV01") && lineas[1].Contains(".*OPR.*"))
                {
                    ESTRUCTURAOPR(lineas[1]);
                }
                else if (lineas[1].Contains("IDENTV01") && lineas[1].Contains(".*OPL.*"))
                {
                    ESTRUCTURAOPL(lineas[1]);
                }else if (lineas[1].Contains("IDENVT01"))
                {
                    ESTRUCTURAIVARIABALE(lineas[1]);
                }
                else if (lineas[1].Contains("PR11"))
                {
                         ESTRUCTURAIMP(lineas[1]);
                }*/
                    
                if (lineas[0].Contains("IDENVT01") && lineas[1].Contains("IDENVT01"))
                {
                    ESTRUCTURAIVARIABALE(lineas[0]);
                    ESTRUCTURAIVARIABALE(lineas[1]);
                }
                else if (lineas[0].Contains("IDENVT01"))
                {
                    ESTRUCTURAIVARIABALE(lineas[0]);

                }
                //POR SI EMPIEZA PRIMERO CON EL SI --
                else if (lineas[0].Contains("PR16"))
                {
                    ESTRUCTURADECISION(lineas[0], lineas[1], lineas[2], lineas[3], lineas[4]);

                }
               
                //POR SI SOLO QUIERE SER EL MIENTRAS 
                else if (lineas[1].Contains("PR14"))
                {
                    ESTRUCTURAMIENTRAS(lineas[0], lineas[1], lineas[2], lineas[3], lineas[4], lineas[5], lineas[6]);
                }
                //POR SI DECLARA UNA VARIABLE ANTES DEL MIENTRAS---
                else if (lineas[2].Contains("PR14"))
                {
                    ESTRUCTURAMIENTRAS(lineas[1], lineas[2], lineas[3], lineas[4], lineas[5], lineas[6], lineas[7]);
                }
                //POR SI SOLO QUIERE VER EL HAZ
                else if (lineas[1].Contains("PR09"))
                {
                    ESTRUCTURAHAZ(lineas[0], lineas[1], lineas[2], lineas[3], lineas[4], lineas[5], lineas[6], lineas[7]);
                }
                //POR SI DECLARA UNA VARIABLE ANTES DEL HAZ
                else if (lineas[2].Contains("PR09"))
                {
                    ESTRUCTURAHAZ(lineas[1], lineas[2], lineas[3], lineas[4], lineas[5], lineas[6], lineas[7], lineas[8]);
                }
                //POR SI SOLO QUIERE VER EL PARA
                else if (lineas[0].Contains("PR15"))
                {
                    ESTRUCTURAPARA(lineas[0], lineas[1], lineas[2], lineas[3], lineas[4]);
                }
                //POR SI DECLARA UNA VARIABLE ANTES DEL PARA 
                else if (lineas[1].Contains("PR15"))
                {
                    ESTRUCTURAPARA(lineas[1], lineas[2], lineas[3], lineas[4], lineas[5]);
                }
            




        }



        public void ValidarLinea()
        {
            try
            {
                string[] tokens =new string[90];
                char[] del1 = { '\n' };
                //TOMA LA LINEA INGRESADA
                string pp = txtTokens.Text;
        
                pp=  pp.Trim().TrimEnd();
                string strPalabras = pp;

                //METE LAS LINEAS EN UN ARREGLO 
                string[] Lenguajes = strPalabras.Split(del1);
                int i = 0;
                foreach (string palabras in Lenguajes)
                {
                 //  listBox4.Items.Add(palabras);

                    ValidarTokens(palabras);
                    ChecarEspacios();
                   
                }
              
            }
            catch (Exception)
            {
                MessageBox.Show("Por favor ingresa una cadena", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void ValidarTokens(string palabras)
        {
            
            char[] del = { ' ' };
            char chMuestra = ' ';
            int j = 0;
          
            palabras = palabras.TrimEnd();
            //TOMA LA ORACION  DE LA LINEA INGRESADA
            string strPalabra =palabras;
            //METE LAS ORACIONES DE CADA LINEA  EN UN ARREGLO 
            string[] Lenguaje = strPalabra.Split();
            //RECORRE EL ARREGLO DE LAS PALABRAS 
            for (int i=0;i<Lenguaje.Length;i++) 
            {
                //listBox5.Items.Add(Lenguaje[i]);
                switch (Lenguaje[i])
                {
                       
                    case "PR16":   Decision(strPalabra); 
                        break;
                    case "PR10":
                        imprime(strPalabra);
                        break;
                    case "IDENTV01":
                        if(Lenguaje[i+1] == "PR05" || Lenguaje[i + 1] == "PR18" || Lenguaje[i + 1] == "PR08" )
                        {
                            TiposDatos(strPalabra);

                        }
                        if(Lenguaje[i + 1] == "OPA01" | Lenguaje[i + 1] == "OPA02" | Lenguaje[i + 1] == "OPA03" | Lenguaje[i + 1] == "OPA04"| Lenguaje[i + 1] == "OPA05")
                        {
                            OPA(strPalabra);
                        }
                        
                        if (Lenguaje[i + 1] == "OPR01" | Lenguaje[i + 1] == "OPR02" | Lenguaje[i + 1] == "OPR03" | Lenguaje[i + 1] == "OPR04" | Lenguaje[i + 1] == "OPR05")
                        {
                            OPR(strPalabra);
                        }
                        
                        if (Lenguaje[i + 1] == "OPL01" | Lenguaje[i + 1] == "OPL02" | Lenguaje[i + 1] == "OPL03" )
                        {
                            OPL(strPalabra);
                        }
                        if (Lenguaje[i + 1] == "IDENTV01")
                        {
                         Errores(strPalabra);   
                        }
                        break;


                }
              
            }



        }
        public void imprime(string LineaCodigo)
        {

            //listBox5.Items.Clear();
            char[] del = { ' ' };
            char chMuestra = ' ';
            int j = 0;


            //TOMA LA ORACION  DE LA LINEA INGRESADA
            string strPalabra = LineaCodigo;
            //METE LAS ORACIONES DE CADA LINEA  EN UN ARREGLO 
            string[] Lenguaje = strPalabra.Split();
            //RECORRE EL ARREGLO DE LAS PALABRAS 
            foreach (string palabra in Lenguaje)
            {
               // listBox5.Items.Add(palabra);
                switch (palabra)
                {
                    case "PR10":
                        sintactico = "PR10 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    case "ES07":
                        sintactico = "ES07 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    case "IDENTV01":
                        sintactico = "ARG06 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                }
               
                if (palabra != "  ")
                {
                    txtSintactico.Text = txtSintactico.Text + "";
                }
            }
        }
        public void OPA(string LineaCodigo)
        {
            //listBox5.Items.Clear();
            string p = txtSintactico.Text;
            char[] del = { ' ' };
            
            //TOMA LA ORACION  DE LA LINEA INGRESADA
            string strPalabra = LineaCodigo;

            //METE LAS ORACIONES DE CADA LINEA  EN UN ARREGLO 
            string[] Lenguaje = strPalabra.Split();
       
            //RECORRE EL ARREGLO DE LAS PALABRAS 
            foreach (string palabra in Lenguaje)
            {
                //listBox5.Items.Add(palabra);

                switch (palabra)
                {
                    
                    case "IDENTV01":
                        sintactico = "ARG01 (Valido)";

                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    
                    case "ENTE":
                        sintactico = "ARG01 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;

                    case "OPA01":
                        sintactico = "OAR (Valido)";

                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    
                    case "OPA02":
                        sintactico = "OAR (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    
                    case "OPA03":
                        sintactico = "OAR (Valido)";

                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    
                    case "OPA04":
                        sintactico = "OAR (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                   
                    case "OPA05":
                        sintactico = "OAR (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                 
                }
                if (palabra != "\r\n")
                {
                    txtSintactico.Text = txtSintactico.Text + "\r\n";
                }
                if (palabra != " ")
                {
                    txtSintactico.Text = txtSintactico.Text + "";
                }
            }
           
        }
        public void OPL(string LineaCodigo)
        {
            //listBox5.Items.Clear();
            string p = txtSintactico.Text;
            char[] del = { ' ' };
           
            string strPalabra = LineaCodigo;
            //CAMBIA LAS PALABRAS
            //METE LAS ORACIONES DE CADA LINEA  EN UN ARREGLO 
            string[] Lenguaje = strPalabra.Split();
            //RECORRE EL ARREGLO DE LAS PALABRAS 
            foreach (string palabra in Lenguaje)
            {
             //   listBox5.Items.Add(palabra);

                switch (palabra)
                {

                    case "IDENTV01":
                        sintactico = "ARG01 (Valido) " ;

                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;

                    case "ENTE":
                        sintactico = "ARG01 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;

                    case "OPL01":
                        sintactico = "OLG  (Valido)";

                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;

                    case "OPL02":
                        sintactico = "OLG (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;

                    case "OPL03":
                        sintactico = "OLG (Valido)";

                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;

                  
                }
                if (palabra != "\r\n")
                {
                    txtSintactico.Text = txtSintactico.Text + "\r\n";
                }
                if (palabra != " ")
                {
                    txtSintactico.Text = txtSintactico.Text + "";
                }
            } 

        }
        public void OPR(string LineaCodigo)
        {
            //listBox5.Items.Clear();
            string p = txtSintactico.Text;
            char[] del = { ' ' };
            char chMuestra = ' ';
            int j = 0;
    
            //TOMA LA ORACION  DE LA LINEA INGRESADA
            string strPalabra =LineaCodigo;
            //CAMBIA LAS PALABRAS
            //METE LAS ORACIONES DE CADA LINEA  EN UN ARREGLO 
            string[] Lenguaje = strPalabra.Split();
            //RECORRE EL ARREGLO DE LAS PALABRAS 
            foreach (string palabra in Lenguaje)
            {
                //listBox5.Items.Add(palabra);

                switch (palabra)
                {

                    case "IDENTV01":
                        sintactico = "ARG01 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;

                    case "ENTE":
                        sintactico = "ARG01 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;

                    case "OPR01":
                        sintactico = "ORL (Valido)";

                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;

                    case "OPR02":
                        sintactico = $"ORL (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;

                    case "OPR03":
                        sintactico = "ORL  (Valido)";

                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;

                    case "OPR04":
                        sintactico = "ORL (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;

                    case "OPR05":
                        sintactico = "ORL (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;

                }
              
                if (palabra != " ")
                {
                    txtSintactico.Text = txtSintactico.Text + "";
                }
            }

        }
        public void TiposDatos(string LineaCodigo)
        {

            //listBox5.Items.Clear();

                char[] del = { ' ' };
  
            //TOMA LA ORACION  DE LA LINEA INGRESADA
            string pp =LineaCodigo;
                // pp = pp.Trim();
                //TOMA LA ORACION  DE LA LINEA INGRESADA
             string strPalabra = pp;
          
            //METE LAS ORACIONES DE CADA LINEA  EN UN ARREGLO 
            string[] Lenguaje = strPalabra.Split();
            //RECORRE EL ARREGLO DE LAS PALABRAS 
            foreach (string palabra in Lenguaje)
            {
                //listBox5.Items.Add(palabra);

                switch (palabra)
                {
                   
                    //BOO
                    case "PR02":
                        sintactico = "PR02 (Valido)";

                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //CAR
                    case "PR03":
                        sintactico = "PR03 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //ENT
                    case "PR05":
                        sintactico = "PR05 (Valido)";

                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //FLO
                    case "PR08":
                        sintactico = "PR08 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //TXT
                    case "PR18":
                        sintactico = "PR18 (Valido)";

                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //=
                    case "CE06":
                        sintactico = "CE06 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //NUM ENTERO
                    case "ENTE":
                        sintactico = "ARG03 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //NUM POSITIVO
                    case "ES02":
                        sintactico = "ARG04 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //NUM NEGATIVO
                    case "ES03":
                        sintactico = "ARG04 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //NUM DECIMAL POSI
                    case "ES04":
                        sintactico = "ARG04 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //NUM DECIMAL NEG
                    case "ES05":
                        sintactico = "ARG04  (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //FLOT
                    case "FLOT":
                        sintactico = "ARG04 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //IDENT
                    case "IDENTV01":
                        sintactico = "IDENT (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                   
                }
                if (palabra != "\r\n")
                {
                    txtSintactico.Text = txtSintactico.Text + "\r\n";
                }
                if (palabra != " ")
                {
                    txtSintactico.Text = txtSintactico.Text + "";
                }

            }
           

        }
        public void Errores(string LineaCodigo)
        {
            //listBox5.Items.Clear();

            char[] del = { ' ' };
           
            //TOMA LA ORACION  DE LA LINEA INGRESADA
            string pp = LineaCodigo;
            // pp = pp.Trim();
            //TOMA LA ORACION  DE LA LINEA INGRESADA
            string strPalabra = pp;

            //METE LAS ORACIONES DE CADA LINEA  EN UN ARREGLO 
            string[] Lenguaje = strPalabra.Split();
            //RECORRE EL ARREGLO DE LAS PALABRAS 
            for(int i =0; i<Lenguaje.Length;i++)
            {
                if (Lenguaje[i] == "IDENTV01")
                {
                    sintactico = "IDENT (Valido)";
                    txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                }
                if (i + 1 < Lenguaje.Length && Lenguaje[i + 1] == "IDENTV01")
                {
                    sintactico = "ERROR (No Valido)";
                    txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                }
               if (Lenguaje[i] != "\r\n")
                {
                    txtSintactico.Text = txtSintactico.Text + "\r\n";
                }
                if (Lenguaje[i] != " ")
                {
                    txtSintactico.Text = txtSintactico.Text + "";
                }
                break;
            }
            
        }
        public void Decision(string Lineas)
        {
           

            //listBox5.Items.Clear();

            char[] del = { ' ' };
            char chMuestra = ' ';
            int j = 0;
          
            string pp = Lineas;
          
            //TOMA LA ORACION  DE LA LINEA INGRESADA
            string strPalabra = Lineas;
          
            //METE LAS ORACIONES DE CADA LINEA  EN UN ARREGLO 
            string[] Lenguaje = strPalabra.Split();
            //RECORRE EL ARREGLO DE LAS PALABRAS 
            foreach (string palabra in Lenguaje)
            {
                //listBox5.Items.Add(palabra);

                switch (palabra)
                {

                    //BOO
                    case "PR16":
                        sintactico = "PR16 (Valido)";

                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //CAR
                    case "CE05":
                        sintactico = "CE05 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //ENT
                    case "IDENTV01":
                        sintactico = "IDENT (Valido)";

                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //FLO
                    case "ENT ":
                        sintactico = "ARG01 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //TXT
                    case "CEO3":
                        sintactico = "CE03 (Valido)";

                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //=
                    case "PR11":
                        sintactico = "PR11 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    //NUM ENTERO
                    case "PR07":
                        sintactico = "PR07 (Valido)";
                        txtSintactico.Text = txtSintactico.Text + " " + sintactico;
                        break;
                    

                }
                if (palabra != "\r\n")
                {
                    txtSintactico.Text = txtSintactico.Text + "\r\n";
                }
                if (palabra != " ")
                {
                    txtSintactico.Text = txtSintactico.Text + "";
                }

            }

        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        //IF BUENO LISTO 
        public void ESTRUCTURADECISION(string lineas1,string lineas2,string lineas3,string lineas4,string lineas5)
        {
            bool Valida=true;

            lineas1 = lineas1.TrimStart(' ');
            lineas2 = lineas2.Trim();
            lineas3 = lineas3.Trim();
            lineas4 = lineas4.Trim();
            lineas5 = lineas5.Trim();
            if (!Regex.IsMatch(lineas1, "^PR16$"))
            {
                txtSintactico.Text = lineas1[0] + "\r\n";
                MessageBox.Show("Línea 1: no cumple con la estructura esperada (PR16).");
                Valida = false;
            }
            else if (!Regex.IsMatch(lineas2, @"^ (CE05 IDENTV\d+ (OPR01|OPR02|OPR03|OPR04) IDENTV\d+ CE03)|(CE05 IDENTV\d+ (OPR01|OPR02|OPR03|OPR04) (IDENTV01|ENTE) (OPL01|OPL02|OPL03) IDENTV\d+ (OPR01|OPR02|OPR03|OPR04) (IDENTV\d+|ENTE) CE03)|(CE05 IDENTV01+ (OPA01|OPA02|OPA03|OPA04|OPA05) (IDENTV\d+|ENTE) (OPR01|OPR02|OPR03|OPR04|OPR05) IDENTV\d+ (OPA01|OPA02|OPA03|OPA04|OPA05) (IDENTV\d+|ENTE) CE03)$"))
            {
                
                txtSintactico.Text = lineas2+ "\r\n" ;
                MessageBox.Show("Línea 2: no cumple con la estructura esperada.");
                Valida = false;
            }

            else if (!Regex.IsMatch(lineas3, "^PR11$"))
            {
                txtSintactico.Text = lineas3 + "\r\n";
                MessageBox.Show("Línea 3: no cumple con la estructura esperada (PR11).");
                Valida = false;
            }
            else if (!Regex.IsMatch(lineas4, "^PR10 (ES07|IDENTV01)$"))
            {
                txtSintactico.Text = lineas4 + "\r\n";
                MessageBox.Show("Línea 4: no cumple con la estructura esperada (PR10 ES07).");
                Valida = false;
            }
            else if (!Regex.IsMatch(lineas5, "^PR07$"))
            {
                txtSintactico.Text = lineas5 + "\r\n";
                MessageBox.Show("Línea 5: no cumple con la estructura esperada (PR07).");
                Valida = false;
            }
           if (Valida)
            {
                txtSintactico.Text = txtTokens.Text +"\n"+ "(ESTRUCTURA  DE DECISION VALIDA)";
                MessageBox.Show("Estructura de deicision valida");
            }
            else
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA  DE DECISION NO VALIDA)";
                MessageBox.Show("Estructura de deicision no valida");
            } 
            
        }
        //IF BUENO LISTO 
        public void ESTRUCTURADECISIONSINO(string lineas1, string lineas2, string lineas3, string lineas4, string lineas5, string lineas6, string lineas7, string lineas8, string lineas9)
        {
            bool Valida = true;

            lineas1 = lineas1.TrimStart(' ');
            lineas2 = lineas2.Trim();
            lineas3 = lineas3.Trim();
            lineas4 = lineas4.Trim();
            lineas5 = lineas5.Trim();
            lineas6 = lineas6.Trim();
            lineas7 = lineas7.Trim();
            lineas8 = lineas8.Trim();
            lineas9 = lineas9.Trim();
           
            if (!Regex.IsMatch(lineas1, "^PR16$"))
            {
                txtSintactico.Text = lineas1 + "\r\n";
                MessageBox.Show("Línea 1: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(lineas2, @"^ (CE05 IDENTV\d+ (OPR01|OPR02|OPR03|OPR04) IDENTV\d+ CE03)|(CE05 IDENTV\d+ (OPR01|OPR02|OPR03|OPR04) (IDENTV01|ENTE) (OPL01|OPL02|OPL03) IDENTV\d+ (OPR01|OPR02|OPR03|OPR04) (IDENTV\d+|ENTE) CE03)|(CE05 IDENTV01+ (OPA01|OPA02|OPA03|OPA04|OPA05) (IDENTV\d+|ENTE) (OPR01|OPR02|OPR03|OPR04|OPR05) IDENTV\d+ (OPA01|OPA02|OPA03|OPA04|OPA05) (IDENTV\d+|ENTE) CE03)$"))
            {

                txtSintactico.Text = lineas2 + "\r\n";
                MessageBox.Show("Línea 2: no cumple con la estructura esperada.");
                Valida = false;
            }

            else if (!Regex.IsMatch(lineas3, "^PR11$"))
            {
                txtSintactico.Text = lineas3 + "\r\n";
                MessageBox.Show("Línea 3: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(lineas4, "^PR19$"))
            {
                txtSintactico.Text = lineas4 + "\r\n";
                MessageBox.Show("Línea 4: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(lineas5, "^PR10 (ES07|IDENTV01)$"))
            {
                txtSintactico.Text = lineas5 + "\r\n";
                MessageBox.Show("Línea 5: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(lineas6, "^PR17$"))
            {
                txtSintactico.Text = lineas6 + "\r\n";
                MessageBox.Show("Línea 5: no cumple con la estructura esperada (PR07).");
                Valida = false;
            }
            else if (!Regex.IsMatch(lineas7, "^PR06$"))
            {
                txtSintactico.Text = lineas7 + "\r\n";
                MessageBox.Show("Línea 5: no cumple con la estructura esperada (PR07).");
                Valida = false;
            }
            else if (!Regex.IsMatch(lineas8, "^PR10 (ES07|IDENTV01)$"))
            {
                txtSintactico.Text = lineas8 + "\r\n";
                MessageBox.Show("Línea 5: no cumple con la estructura esperada (PR07).");
                Valida = false;
            }
            else if (!Regex.IsMatch(lineas9, "^PR07$"))
            {
                txtSintactico.Text = lineas9 + "\r\n";
                MessageBox.Show("Línea 5: no cumple con la estructura esperada (PR07).");
                Valida = false;
            }
            if (Valida)
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA  DE DECISION VALIDA)";
                MessageBox.Show("Estructura de deicision valida");
            }
            else
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA  DE DECISION NO VALIDA)";
                MessageBox.Show("Estructura de deicision no valida");
            }

        }
        //CICLO PARA --> FOR  LISTO
        //CICLO PARA --> FOR  LISTO
        public void ESTRUCTURAPARA(string linea1,string linea2,string linea3,string linea4,string linea5)
        {
           
            bool Valida = true;

           linea1=linea1.TrimStart(' ');
           linea2 = linea2.Trim();
           linea3 = linea3.Trim();
           linea4 = linea4.Trim();
           linea5 = linea5.Trim();
           
            if (!Regex.IsMatch(linea1, "^PR15$"))
            {
                txtSintactico.Text = linea1 + "\r\n";
                MessageBox.Show("Línea 1: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(linea2, "^CE05 IDENTV01 (PR05|PR08) CE06 (ENTE|FLOAT|ES02|ES03|ES04|ES05) CE02 IDENTV01 (OPR01|OPR02|OPR03|OPR04|OPR05|OPL01|OPL02|OPL03|OPA01|OPA02|OPA03|OPA04|OPA05) (ENTE|FLOAT|ES02|ES03|ES04|ES05) CE02 IDENTV01 CE06 IDENTV01 (OPA01|OPA02|OPA03|OPA05) ENTE CE03$"))

            {
                txtSintactico.Text = linea2 + "\r\n";
                MessageBox.Show("Línea 2: no cumple con la estructura esperada .");
                Valida = false;
            }

            else if (!Regex.IsMatch(linea3, "^PR11$"))
            {
                txtSintactico.Text = linea3 + "\r\n";
                MessageBox.Show("Línea 3: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(linea4, "^PR10 (ES07|IDENTV01)$"))
            {
                txtSintactico.Text = linea4 + "\r\n";
                MessageBox.Show("Línea 4: no cumple con la estructura esperada (PR10 IDENTV01).");
                Valida = false;
            }
            else if (!Regex.IsMatch(linea5, "^PR07$"))
            {
                txtSintactico.Text = linea5 + "\r\n";
                MessageBox.Show("Línea 5: no cumple con la estructura esperada (PR07).");
                Valida = false;
            }
            if (Valida)
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA  DE PARA VALIDA)";
            }
            else
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA  DE PARA NO VALIDA)";
            }

        }

        //CICLOS MIE --> WHILE LISTO
        public void ESTRUCTURAMIENTRAS(string linea1,string linea2,string linea3,string linea4,string linea5,string linea6, string linea7)
        {
            linea1 = linea1.TrimStart(' ');
            linea2 = linea2.Trim();
            linea3 = linea3.Trim();
            linea4 = linea4.Trim();
            linea5 = linea5.Trim();
            linea6 = linea6.Trim();
            linea7 = linea7.Trim();
            bool Valida = true;

            /*if (lineas.Length != 6)
            {
                MessageBox.Show("El texto debe tener exactamente 6 líneas.");
                return;
            }*/

            if (!Regex.IsMatch(linea1, "^IDENTV01 (PR05|PR08) CE06 (ENTE|FLOAT|ES02|ES03|ES04|ES05)$"))
            {
                txtSintactico.Text = linea1 + "\r\n";
                MessageBox.Show("Línea 1: no cumple con la estructura esperada.");
               
                Valida = false;
            }
            else if (!Regex.IsMatch(linea2, "^PR14$"))

            {
                txtSintactico.Text = linea2 + "\r\n";
                MessageBox.Show("Línea 2: no cumple con la estructura esperada .");
                Valida = false;
            }

            else if (!Regex.IsMatch(linea3, "^CE05 IDENTV01 (OPR01|OPR02|OPR03|OPR04|OPR05|OPL01|OPL02|OPL03|OPA01|OPA02|OPA03|OPA04|OPA05) (ENTE|FLOAT|ES02|ES03|ES04) CE03$"))
            {
                txtSintactico.Text = linea3 + "\r\n";
                MessageBox.Show("Línea 3: no cumple con la estructura esperada .");
                Valida = false;
            }
            else if (!Regex.IsMatch(linea4, "^PR11$"))
            {
                txtSintactico.Text = linea4 + "\r\n";
                MessageBox.Show("Línea 4: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(linea5, "^PR10 (ES07|IDENTV01)$"))
            {
                txtSintactico.Text = linea5 + "\r\n";
                MessageBox.Show("Línea 5: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(linea6, @"^IDENTV01 CE06 IDENTV01 (OPA01|OPA02|OPA03|OPA04|OPA05) ENTE$"))
            {
                txtSintactico.Text = linea6 + "\r\n";
                MessageBox.Show("Línea 6: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(linea7, "^PR07$"))
            {
                txtSintactico.Text = linea7 + "\r\n";
                MessageBox.Show("Línea 6: no cumple con la estructura esperada.");
                Valida = false;
            }
            if (Valida)
            {
                txtSintactico.Text = txtTokens.Text + " " + "(ESTRUCTURA  DE MIE VALIDA)";
                MessageBox.Show("Estructura de mientras correcta");
            }
            else
            {
                txtSintactico.Text = txtTokens.Text + " " + "(ESTRUCTURA  DE MIE NO VALIDA)";
                MessageBox.Show("Estructura de mientras incorrecta");
            }

        }

        //CICLOS HAZ --> DO WHILE 
        public void ESTRUCTURAHAZ(string linea1, string linea2, string linea3, string linea4, string linea5, string linea6, string linea7, string linea8)
        {
            linea1 = linea1.Trim();
            linea2 = linea2.Trim();
            linea3 = linea3.Trim();
            linea4 = linea4.Trim();
            linea5 = linea5.Trim();
            linea6 = linea6.Trim();
            linea7 = linea7.Trim();
            linea8 = linea8.Trim();

            bool Valida = true;

            if (!Regex.IsMatch(linea1, @"^IDENTV01 (PR05|PR08) CE06 (ENTE|FLOAT|ES02|ES03|ES04|ES05)$"))
            {
                txtSintactico.Text = linea1 + "\r\n";
                MessageBox.Show("Línea 1: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(linea2, @"^PR09$"))
            {
                txtSintactico.Text = linea2 + "\r\n";
                MessageBox.Show("Línea 2: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(linea3, @"^PR11$"))
            {
                txtSintactico.Text = linea3 + "\r\n";
                MessageBox.Show("Línea 3: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(linea4, @"^IDENTV01 CE06 IDENTV01 (OPA01|OPA02|OPA03|OPA04|OPA05) ENTE$"))
            {
                txtSintactico.Text = linea4 + "\r\n";
                MessageBox.Show("Línea 4: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(linea5, @"^PR10 (ES07|IDENTV01)$"))
            {
                txtSintactico.Text = linea5 + "\r\n";
                MessageBox.Show("Línea 5: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(linea6, @"^PR14$"))
            {
                txtSintactico.Text = linea6 + "\r\n";
                MessageBox.Show("Línea 6: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(linea7, @"^CE05 IDENTV01 (OPR01|OPR02|OPR03|OPR04|OPR05) (ENTE|FLOAT|ES02|ES03|ES04) CE03$"))
            {
                txtSintactico.Text = linea7 + "\r\n";
                MessageBox.Show("Línea 7: no cumple con la estructura esperada.");
                Valida = false;
            }
            else if (!Regex.IsMatch(linea8, @"^PR07"))
            {
                txtSintactico.Text = linea8 + "\r\n";
                MessageBox.Show("Línea 8: no cumple con la estructura esperada.");
                Valida = false; 


            }
            if (Valida)
            {
                txtSintactico.Text = txtTokens.Text + " " + "(ESTRUCTURA  DE HAZMIE VALIDA)";
                MessageBox.Show("Estructura de haz correcta");
            }
            else
            {
                txtSintactico.Text = txtTokens.Text + " " + "(ESTRUCTURA  DE HAZMIE NO VALIDA)";
                MessageBox.Show("Estructura de haz incorrecta");
            }

        }

        //OPERACION ARITMETICA
        public void ESTRUCTURAOPA(string linea)
        {
           
            bool Valida = true;
           /* if (lineas.Length != 1)
            {
                MessageBox.Show("El texto debe tener exactamente 1 líneas.");
                return;
            }*/


            if (!Regex.IsMatch(linea, "^IDENTV01 (OPA01|OPA02|OPA03|OPA04|OPA05) IDENTV01$"))
            {
                txtSintactico.Text = linea + "\r\n";
                MessageBox.Show("Línea 1: no cumple con la estructura esperada.");
                Valida = false;
            }
            if (Valida)
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA DE OPA VALIDA)";
            }
            else
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA DE OPA NO VALIDA)";
            }
        }
        //OPERACION LOGICA
        public void ESTRUCTURAOPL(string linea)
        {
           
            bool Valida = true;
           /* if (lineas.Length != 1)
            {
                MessageBox.Show("El texto debe tener exactamente 1 líneas.");
                return;
            }
           */

            if (!Regex.IsMatch(linea, "^IDENTV01 (OPL01|OPL02|OPL03) IDENTV01$"))
            {
                txtSintactico.Text = linea + "\r\n";
                MessageBox.Show("Línea 1: no cumple con la estructura esperada.");
                Valida = false;
            }
            if (Valida)
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA DE OPL VALIDA)";
            }
            else
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA DE OPL NO VALIDA)";
            }
        }
        //OPERACION RELACIONAL
        public void ESTRUCTURAOPR(string linea)
        {
            
            bool Valida = true;
          /*  if (lineas.Length != 1)
            {
                MessageBox.Show("El texto debe tener exactamente 1 líneas.");
                return;
            }*/


            if (!Regex.IsMatch(linea, "^IDENTV01 (OPR01|OPR02|OPR03|OPR04|OP05) IDENTV01$"))
            {
                txtSintactico.Text = linea + "\r\n";
                MessageBox.Show("Línea 1: no cumple con la estructura esperada.");
                Valida = false;
            }
            if (Valida)
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA DE OPR VALIDA)";
            }
            else
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA DE OPR NO VALIDA)";
            }
        }
        //PALABRA IMP --> IMPRIMIR 
        public void ESTRUCTURAIMP(string liena)
        {
            string textoCompleto = txtTokens.Text.Trim(); // Obtener el contenido del TextBox y eliminar espacios en blanco al principio y al final
            string[] lineas = textoCompleto.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            bool Valida = true;
           


            if (!Regex.IsMatch(lineas[0], "^PR10 (ES07|IDENTV01)$"))
            {
                txtSintactico.Text = lineas[0] + "\r\n";
                MessageBox.Show("Línea 1: no cumple con la estructura esperada.");
                Valida = false;
            }
            if (Valida)
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA DE IMPRIMIR VALIDA)";
            }
            else
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA DE IMPIRMIR NO VALIDA)";
            }
        }

        //PALABRA IMP --> IMPRIMIR 
        public void ESTRUCTURAIVARIABALE(string linea)
        {
           
            bool Valida = true;
           
            if (!Regex.IsMatch(linea, "^(IDENTV01 PR05 CE06 (ENTE|ES02|ES03)|IDENTV01 (PR03|PR18) CE06 ES07|IDENTV01 PR08 CE06 (FLOT|ES04|ESO5))$"))
            {
                txtSintactico.Text = linea + "\r\n";
                MessageBox.Show("Línea 1: no cumple con la estructura esperada.");
                Valida = false;
                sintactico = "Asigancion no valida";
            }
            if (Valida)
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA ASIGNACION VALIDA)";
                MessageBox.Show("Asignacion de variable correcta");
            }
            else
            {
                txtSintactico.Text = txtTokens.Text + "\n" + "(ESTRUCTURA ASIGNACION NO VALIDA)";
                MessageBox.Show("Asignacion de variable incorrecta");
            }
        }


        public void ChecarEspacios()
        {
            string textp=txtSintactico.Text;
            textp= textp.TrimEnd(); 

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
