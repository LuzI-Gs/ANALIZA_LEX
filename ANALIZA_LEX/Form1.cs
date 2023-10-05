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
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.LinkLabel;

namespace ANALIZA_LEX
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            unIdentificador = new Identificador();           
        }
        private void Clear()
        {
            intEstado = 0;                          //Variables
            contador = 0;
            contadorLineas = 1;
            token = "";
            sintactico = "";
            LenguajeNat = "";
            tipoDato = "";
            strValorIde = "";
            yaExiste = false;
            txtLineasLenguaje.Text = "";            //Contador de Lineas
            txtLenguaje.Text = "";                  //Ingresa lineas de codigo
            txtLineasLexico.Text = "";              //Contador de lineas
            txtTokens.Text = "";                    //Mostrador de tokens
            txtSintactico.Text = "";                //Mostrador de estados                                          
            dgvErroresSemanticos.Rows.Clear();      // Limpia todas las filas
            dgvErroresSemanticos.Columns.Clear();   // Limpia todas las columnas
            dgvIden.Rows.Clear();
            dgvIden.Columns.Clear();
            dgvErroresLexicos.Rows.Clear();
            dgvErroresLexicos.Columns.Clear();

        }
        int intEstado = 0, contador = 0, contadorLineas = 1;
        string token = "", sintactico = "", LenguajeNat = "", tipoDato = "", strValorIde = "";
        bool yaExiste = false;
        ConexionBD conexion = new ConexionBD();
        //arreglo donde se almacenan la información para la tabla de simbolos
        List<string> tablaSimbolo = new List<string>();
        List<Error> errores = new List<Error>();             
        List<Identificador> unaLista = new List<Identificador>();
        Identificador unIdentificador;
        Error unError;        
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
            for (int i = 0; i < dtgMatriz.ColumnCount; i++) //RECORRE TODAS LAS COLUMNAS DE LA TABLA 
            {
               
                if (dtgMatriz.Columns[i].HeaderText == "FDC") //SI ENCUENTRA LA COLUMNA QUE SE LLAMA TOKEN PROCEDE A HACER LAS SIG INSTRUCCIONES
                {               
                    tk=dtgMatriz.Rows[Estado].Cells[i+1].Value.ToString();     //TOMA EL VALOR DEL TOKEN DEPENDIENDO DEL ESTADO Y LOS ALMACENA EN LA VARIABLE Y LOS VA SUMANDO PARA FORMAR LA CADENA DE TOKENS 
                    //PINTA DE COLOR AZUL LA CELDA DEL TOKEN 
                    dtgMatriz.Rows[Estado].Cells[i+1].Style.BackColor = Color.Blue; 
                }
            }
            if (string.IsNullOrWhiteSpace(tk))
            {
                MessageBox.Show("No se encontró un token para el estado especificado.");
            }                    
            return tk.TrimStart();            
        }
        public void DescomponerCadenas()
        {
            try
            {
                char[] del1 = { '\n' };
                LenguajeNat = txtLenguaje.Text;
                txtTokens.Text = "";               
                string strPalabras = LenguajeNat; //TOMA LA ORACION DE CADA LINEA EN EL TEXTBOX INGRESADA
                string[] Lenguajes = strPalabras.Split(del1);//METE LAS ORACIONES EN UN ARREGLO 
                string evaluacion = "";
                int linea = 0;
                int contadoroalabras = 0;
                int palabrastotales = 0;
                int contadorletras=0;
                foreach (string palabras in Lenguajes)
                {
                    linea++;                 
                    char[] del = { ' ' };//AHORA TOMA CADA PALABRA DE CADA ORACION 
                    char chMuestra = ' ';
                    int j = 0;
                    palabrastotales = palabras.Length; //su funcion es contar cuantas letras hay en una oracion
                   // MessageBox.Show($"Cantidad de letras totales: {palabrastotales}");                    
                    string strPalabra = palabras;    //TOMA LA PALABRA DE LA ORACION  INGRESADA               
                    string[] Lenguaje = strPalabra.Split(del);     //METE LAS PALABRAS EN UN ARREGLO
                    int banderatipodato = 0;
                    //INVOCA EL METODO DE LA PILA PARA VERIFICAR QUE ESTEN CORRECTAMENTE EL BALANCE DE LOS CORCHETES Y SIGNOS DE ?
                    evaluacion = EvaluarExpresion(palabras);
                    if (evaluacion == "Expresion incorrecta fsi")
                    {
                        dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: Falta signo de inicio");
                    }
                    else
                    if (evaluacion == "Expresion incorrecta fsf")
                    {
                        dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: Falta signo de final");
                    }
                    if (evaluacion == "Expresion incorrecta fci")
                    {
                        dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: Falta corchete de inicio");
                    }
                    else
                    if (evaluacion == "Expresion incorrecta fcf")
                    {
                        dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: Falta corchete de final");
                    }
                    foreach (string palabra in Lenguaje)//RECORRE EL ARREGLO DE LAS PALABRAS 
                    {                       
                        foreach (string valor in Lenguaje)
                        {                          
                            strValorIde = valor; // toma el ultimo valor de la linea
                            if (valor == "ent" || valor == "flo" || valor == "boo" || valor == "car" || valor == "txt")
                            {
                                tipoDato = valor; // contiene el tipo de dato
                            }
                        }                  
                        string strIDE = palabra;                        
                        if (strIDE.StartsWith("_"))//sirve para dectectar los identificadores ya que inician con un _
                        {                            
                            yaExiste = false;
                            foreach (string elemento in tablaSimbolo)
                            {
                                if (strIDE == elemento)
                                {
                                    yaExiste = true;
                                    break; // Ya encontramos una coincidencia, no es necesario seguir buscando
                                }
                            }
                            if (!yaExiste)
                            {            
                                // si no existe el identificador anteriormente se agrega al objeto
                                unIdentificador.Nombre = strIDE;
                                unIdentificador.Valor = strValorIde;
                            }
                        }
                        switch (palabra)
                        {
                            case "txt":
                                int banderainicio = 0;
                                for (int i = 0; i < strValorIde.Length; i++)
                                {
                                    if (strValorIde[i] == '"')
                                    {
                                        banderainicio++;
                                    }
                                }
                                if (banderainicio != 2)
                                {
                                    dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: Faltan Comillas dobles.");
                                }
                                break;
                            case "ent":
                                if (char.IsDigit(strValorIde[0]) || strValorIde[0] == '+' || strValorIde[0] == '-')
                                {
                                    bool caracteresNumericos = true;
                                    for (int i = 0; i < strValorIde.Length; i++)
                                    {
                                        if (!char.IsDigit(strValorIde[i]))
                                        {
                                            caracteresNumericos = false;
                                            break; // Salir del bucle si se encuentra un carácter no numérico
                                        }
                                    }
                                    if (caracteresNumericos == true)
                                    {
                                        //MessageBox.Show("El formato es válido: " + strValorIde);
                                    }
                                    else
                                    {
                                        //dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error2: Se esperaba un valor entero ");
                                    }
                                }
                                else
                                {
                                    dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: Se esperaba un tipo de dato numerico entero.");
                                }
                                break;
                            case "flo":

                                if (strValorIde[0] != '+' && strValorIde[0] != '-' && !char.IsDigit(strValorIde[0]))
                                {
                                    dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: El primer carácter debe ser '+' o '-' o un numero");
                                }
                                else
                                {
                                    bool caracteresNumericos = true;
                                    for (int i = 1; i < strValorIde.Length; i++)
                                    {
                                        if (!char.IsDigit(strValorIde[i]) && strValorIde[i] != '.')
                                        {
                                            caracteresNumericos = false;
                                            break; // Salir del bucle si se encuentra un carácter no numérico
                                        }
                                    }
                                    if (caracteresNumericos == true)
                                    {
                                        //MessageBox.Show("El formato es válido: " + strValorIde);
                                    }
                                    else
                                    {
                                        dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: Se encontraron valores no numéricos");
                                    }
                                }                                                           
                                break;
                            case "boo":
                                if (strValorIde[0] == 'v' && strValorIde[1] == 'e' && strValorIde[2] == 'r' && strValorIde[3] =='d')
                                {
                                    MessageBox.Show("Es correcto");
                                }
                                else
                                {
                                    if (strValorIde[0] == 'f' && strValorIde[1] == 'a' && strValorIde[2] == 'l')
                                    {
                                        MessageBox.Show("Es correcto");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Se esperaba un fal o un verd");
                                    }
                                }
                                break;
                        }                                     
                        for (int i = 0; i < strIDE.Length; i++) //CICLO QUE EXTRAE CADA LETRA DE CADA PALABRA
                        { 
                            //Validar tipo de dato string que este completo
                            chMuestra = strIDE[i];                                                                         

                            ValidarIDE(intEstado, char.ToUpper(chMuestra));  //INVOCA UN METODO QUE TIPO BOOLEANO EL VERIFICA SI EL IDE ES ACEPTABLE
                        }   
                        
                        if (intEstado != 0) //MIENTRAS EL ESTADO SEA DIFERENTE DE 0 BUSCARA EL TOKEN AL QUE CORRESPONDE INVOCANDO UN METODO 
                        {                          
                            token = BuscarToken(intEstado);  //BUSCA EL TOKEN DE CADA PALABRA DESPUES DE VALIDAR EL IDE
                            //VA CONCATENANDO CADA TOKEN
                            //////////////////////////  es para cuando detecta que es un identificador  ////////////////////////////////////////////////////////
                            //si detecta que es un identificador le agrega un valor a un objeto de la clase Identificador
                            if (token.Trim() == "IDE"  )
                            {
                                contador++;
                                unIdentificador.Numero = contador;                                
                                unIdentificador.TipoDato = tipoDato;                              
                                
                            }                            
                            if (token.Trim() == "IDE" && !yaExiste)
                            {                                
                                unaLista.Add(unIdentificador);                               
                            }
                            if (token.Trim().StartsWith("ER"))
                            {
                                Error unError = new Error();
                                unError.NomError = token.Trim();
                                errores.Add(unError);
                            }
                       
                            txtTokens.Text = txtTokens.Text +" "+token.Trim();
                            //INICIALIZA DE NUEVO LA VARIABLE 
                            intEstado = 0;
                        }
                        else
                        {
                            //si no encuenta el caracter que se ingreso mandara un mensaje diciendo ese mensaje 
                            string strCadenaError ="CARACTER NO VALIDO";
                            MessageBox.Show($"El caracter ->  {chMuestra}  <- no es valido en este lenguaje por favor ingresa uno que lo sea ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtTokens.Text = txtTokens.Text + " " +strCadenaError;                        
                        }
                    }                
                    if (palabras != "\r\n")     //SI ENCUENTRA UN SALTO DE LINEA LO RESPETA Y LO VUELVE A COLOCAR
                    { 
                       txtTokens.Text =txtTokens.Text + "\r\n"; 
                    }
                    if (palabras != " ")
                    {
                        txtTokens.Text =txtTokens.Text +"";   
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Por favor ingresa una cadena", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //método para buscar las lineas donde surge el error
        int numeroDeLinea = -1;
        private string BuscarLineaError(string valor)
        {
            // Dividir el texto del TextBox en líneas utilizando '\n' como separador.
            string[] lineas = txtLenguaje.Text.Split('\n');
            // Buscar "ES03" en cada línea.
            for (int i = 0; i < lineas.Length; i++)
            {
                if (lineas[i].Contains(valor))
                {
                    numeroDeLinea = i + 1; // Se suma 1 para obtener el número de línea correcto.
                    break; // Detener la búsqueda una vez que se encuentre la primera ocurrencia.
                }
            }
            return numeroDeLinea.ToString();         
        }
        private void btnValidar_Click(object sender, EventArgs e)
        {
            unaLista.Clear();
            try
            {              
                DescomponerCadenas();  //INVOCA EL METODO
                LenguajeNat ="";
                txtTokens.ReadOnly= true;                
                string textoOriginal = txtTokens.Text;//metodo para quitar la sangria 
                string[] lineas = textoOriginal.Split('\n'); // Dividir el texto en líneas
                for (int i = 0; i < lineas.Length; i++)
                {
                    lineas[i] = lineas[i].TrimStart(); // Quitar espacios al inicio de cada línea
                }
                string textoModificado = string.Join("\n", lineas); // Unir las líneas modificadas
                txtTokens.Text = textoModificado;
                dgvIden.Rows.Clear();//mostrar la información en la tabla de simbolos
                foreach (Identificador miIdentificador in unaLista)
                {                   
                    dgvIden.Rows.Add(miIdentificador.Numero, "IDE" + miIdentificador.Numero, miIdentificador.Nombre, miIdentificador.TipoDato, miIdentificador.Valor);
                }
                dgvErroresLexicos.Rows.Clear();
                foreach(Error error in errores)
                {
                    dgvErroresLexicos.Rows.Add(error.NomError, error.MostrarCaracteristica());
                }
                for (int i = 1; i < contadorLineas; i++)
                {
                    txtLineasLexico.AppendText(i.ToString());
                }          
            }
            catch (Exception)
            {
                MessageBox.Show("Por favor ingresa una cadena", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public bool ValidarIDE(int Estado, char letra)
        {                 
            bool seEncontroCaracter = false;                 // Variable para indicar si se encontró el carácter        
            for (int i = 1; i < dtgMatriz.ColumnCount - 2; i++)  //RECORRE LAS COLUMAS DE LA TABLA OSEA EL ALFABETO-NUMEROS-OTROS
            {
                //BUSCA POR LAS COLUMNAS  DESDE LA POSICION 1 OSEA DESDE LA lETRA -A- EN LA COLUMNA (0) QUE SON LOS NOMBRES DE LA TABLA 
                //SI EL CARACTER  ES IGUAL A LA LETRA ENVIADA 
                if (dtgMatriz.Columns[i].HeaderText[0] == letra )
                {
                    // EN CASO DE QUE NO SEA ALGO VALIDO VERIFICA QUE SU ESTADO SEA  
                    if (dtgMatriz.Rows[Estado].Cells[i].Value.ToString() == "108")
                    {
                        this.intEstado = int.Parse(dtgMatriz.Rows[Estado].Cells[i].Value.ToString());
                    }
                    else
                    {
                        //SI NO LO ES PINTA DE AZUL LA CELDA DEL CARACTER
                        // Si encuentra el Letra, obtiene el estado correspondiente y retorna true y pinta la letra correspondiente 
                        dtgMatriz.Rows[Estado].Cells[i].Style.BackColor = Color.Blue;
                        this.intEstado = int.Parse(dtgMatriz.Rows[Estado].Cells[i].Value.ToString());
                    }
                    //DEVUELVE UN TRUE REPRESENTADO QUE SE VALIDO EL IDE 
                    seEncontroCaracter = true;
                    break;
                }
            }            
            return seEncontroCaracter;//DEVUELVE UN FALSE  REPRESENTADO QUE SE NO VALIDO EL IDE 
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            MostrarMatriz();
            txtTokens.ReadOnly= true;
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
        private void txtTokens_TextChanged(object sender, EventArgs e)
        {
            unIdentificador = new Identificador();
        }
        private void btnValidarSint_Click(object sender, EventArgs e)
        {
            try
            {
                string texto = txtTokens.Text;
                EnviarLineasHastaDelimitador(texto);              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }     
        private void picMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void EnviarLineasHastaDelimitador(string texto)
        {
            char[] del1 = { '\n' };
            string[] lineas = texto.Split(del1, StringSplitOptions.RemoveEmptyEntries);
            foreach (string linea in lineas)
            {
                bool esLineaValida = VerificarAsignacion(linea);
                string mensaje = esLineaValida ? " --> VALIDA" : " --> NO VALIDA";
                txtSintactico.AppendText(linea + mensaje + "\r\n");
            }
        }
        private bool VerificarAsignacion(string linea)
        {
            //modificaciones para corchetes opraciones
            string patronOperacion1 = @"CE10\sENTE\s(?:OPA[1-5])\sENTE\sCE11";
            bool matchOperacion1 = Regex.IsMatch(linea, patronOperacion1, RegexOptions.IgnoreCase);
            string patronOperacion2 = @"CE10\sENTE\s(?:OPR[1-5])\sCE10\sENTE\sOPA2\sENTE\sCE11\sCE11";
           // string patronOperacion2 = @"CE10\s(?:ENTE|ES02|ES03)$\s(?:OPR[1-5])\sCE10\sENTE\s(?:OPA[1-5])\sENTE\sCE11\sCE11";
            bool matchOperacion2 = Regex.IsMatch(linea, patronOperacion2, RegexOptions.IgnoreCase);
            // Expresión regular para la estructura 1
            string patronEstructura1 = @"^IDE\sPR05\sCE06\s(?:ENTE|ES02|ES03)$";
            string patronEstructura11 = @"^IDE\sPR08\sCE06\s(?:ES04|ES05|FLOT)$";
            string patronEstructura12 = @"^IDE\sPR18\sCE06\sES07$";
            string patronEstructura13 = @"^IDE\sPR18\sCE06\s(?:PR06|PR19)$";
            // Expresión regular para la estructura 2
            string patronEstructura2 = @" ^ PR14$|^CE05\sIDE\s(?:OPR[1-5])\sENTE\sCE03$|^PR11$|^PR10\sIDE$|^IDE\sCE06\sIDE\sOPA1\sENTE$|^PR07$";
            //Expresion regular para la estrucrura 3
            // string patronEstructura3 = @"^PR16$|^PR11$|^PR10\s(?:ES07|IDE)$|^PR07$|^CE05\sIDE\s(?:(?:OPA[1-5])|(?:OPR[1-5]))\sENTE\s(?:(?:OPR[1-5])|(?:OPL[1-3]))\sIDE\s(?:(?:OPA[1-5])|(?:OPR[1-5]))\sENTE\sCE03$";
            string patronEstructura3 = @"^PR16$|^PR11$|^PR10\s(?:ES07|IDE)$|^PR07$|^CE05\sIDE\s(?:(?:OPA[1-5])|(?:OPR[1-5]))\sFLOT\s(?:(?:OPR[1-5])|(?:OPL[1-3]))\sIDE\s(?:(?:OPA[1-5])|(?:OPR[1-5]))\sFLOT\sCE03$|^PR19$|^PR17$|^PR06$";
            //Expresion regular para la estrucrura 4
            string patronEstructura4 = @"^PR15$|^CE05\sIDE\sPR05\sCE06\sENTE\sCE02\sIDE\s(?:OPR[1-5])\sENTE\sCE02\sIDE\sCE06\sIDE\sOPA1\sENTE\sCE03$|^PR11$|^PR10\s(?:ES07|IDE)$|^PR07$";
            //Expresion regular para la estrucrura 5
            string patronEstructura5 = @"^PR09$|^PR11$|^IDE\sCE06\sIDE\sOPA1\sENTE$|^PR10\sES07$|^PR14$|^CE05\sIDE\s(?:OPR[1-5])\sENTE\sCE03$|^PR07$";
            // string patronEstructura4 = @"^PR15$|^CE05\sIDE\sPR05\sCE06\sENTE\sCE02\sIDE\sOPR2\sENTE\sCE02\sIDE\sCE06\sIDE\sOPA1\sENTE\sCE03$|^PR11$|^PR10\sIDE$|^PR07$";
            bool matchEstructura1 = Regex.IsMatch(linea, patronEstructura1, RegexOptions.IgnoreCase);
            bool matchEstructura11 = Regex.IsMatch(linea, patronEstructura11, RegexOptions.IgnoreCase);
            bool matchEstructura12 = Regex.IsMatch(linea, patronEstructura12, RegexOptions.IgnoreCase);
            bool matchEstructura13 = Regex.IsMatch(linea, patronEstructura13, RegexOptions.IgnoreCase);
            bool matchEstructura2 = Regex.IsMatch(linea, patronEstructura2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            bool matchEstructura3 = Regex.IsMatch(linea, patronEstructura3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            bool matchEstructura4 = Regex.IsMatch(linea, patronEstructura4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            bool matchEstructura5 = Regex.IsMatch(linea, patronEstructura5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return matchEstructura1 || matchEstructura11 || matchEstructura12 || matchEstructura13 || matchEstructura2 || matchEstructura3 || matchEstructura4 || matchEstructura5||matchOperacion1||matchOperacion2;
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Clear(); //ejecuta metodo que limpia tableros, variables globales
        }
        bool primerCambio = true;
        private void txtLenguaje_TextChanged(object sender, EventArgs e) //se agregan las lineas de codigo cada vez que se detecta el salto de linea
        {          
            string codigoEntrada = txtLenguaje.Text;  // Obtén el texto del primer TextBox
            contadorLineas = 1; // Divide el texto en líneas usando el carácter de salto de línea ('\n')
             string[] lineasCodigo = codigoEntrada.Split('\n');          
            txtLineasLenguaje.Clear();  // Borra el contenido actual del segundo TextBox                                       
            int numeroDeLineas = lineasCodigo.Length;// Agrega cada línea al segundo TextBox     
            StringBuilder codigoConNumerosDeLinea = new StringBuilder();// Actualizar el contador de líneas y mostrarlo como números de línea de código.
            if (primerCambio)
            {
                codigoConNumerosDeLinea.Append("\n");
                primerCambio = false;
            }
            for (int i = 0; i < numeroDeLineas; i++)
            {
                codigoConNumerosDeLinea.Append(contadorLineas + i + " " + "\n");
            }           
            txtLineasLenguaje.Text = codigoConNumerosDeLinea.ToString(); // Mostrar el código con números de línea en el TextBox.                    
            contadorLineas += numeroDeLineas; // Actualizar el contador de líneas para la próxima vez.
        }
        private void btnValidar_Click_1(object sender, EventArgs e)
        {
            unaLista.Clear();
            try
            {
                //INVOCA EL METODO
                DescomponerCadenas();
                LenguajeNat = "";
                txtTokens.ReadOnly = true;
                //metodo para quitar la sangria 
                string textoOriginal = txtTokens.Text;
                string[] lineas = textoOriginal.Split('\n'); // Dividir el texto en líneas
                for (int i = 0; i < lineas.Length; i++)
                {
                    lineas[i] = lineas[i].TrimStart(); // Quitar espacios al inicio de cada línea
                }
                string textoModificado = string.Join("\n", lineas); // Unir las líneas modificadas
                txtTokens.Text = textoModificado;
                //mostrar la información en la tabla de simbolos
                dgvIden.Rows.Clear();
                foreach (Identificador miIdentificador in unaLista)
                {
                    dgvIden.Rows.Add(miIdentificador.Numero, "IDE" + miIdentificador.Numero, miIdentificador.Nombre, miIdentificador.TipoDato, miIdentificador.Valor);
                }
                dgvErroresLexicos.Rows.Clear();
                foreach (Error error in errores)
                {
                    dgvErroresLexicos.Rows.Add(error.NomError, error.MostrarCaracteristica());
                }
                for (int i = 1; i < contadorLineas; i++)
                {
                    txtLineasLexico.AppendText($"\n{i.ToString()}");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Por favor ingresa una cadena", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnValidarSint_Click_1(object sender, EventArgs e)
        {
            try
            {
                string texto = txtTokens.Text;
                EnviarLineasHastaDelimitador(texto);          
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void picSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //METODO QUE COMPRUEBA LOS signos¿? y corchetes[]
        private string EvaluarExpresion(string expresion)
        {
            ClasePila pila = new ClasePila();
            string cadena = expresion;
            if (cadena.Contains("¿") || cadena.Contains("?"))
            {
                for (int i = 0; i < cadena.Length; i++)
                {
                    if (cadena.ElementAt(i) == '¿')
                    {
                        pila.Insertar(cadena.ElementAt(i));
                    }
                    else
                    {
                        if (cadena.ElementAt(i) == '?')
                        {
                            if (pila.Extraer() != '¿')
                            {
                                return "Expresion incorrecta fsi";//falta signo inicio
                            }
                        }
                    }
                }
                if (pila.Vacia())
                {
                    return "Expresion correcta";
                }
                else
                {
                    if (pila.Extraer() == '¿')
                    {
                        return "Expresion incorrecta fsf";//falta signo final
                    }
                    else
                    {
                        return "Expresion incorrecta fsi";//falta signo inicio
                    }
                }
            }
            else
            {
                for (int i = 0; i < cadena.Length; i++)
                {
                    if (cadena.ElementAt(i) == '[')
                    {
                        pila.Insertar(cadena.ElementAt(i));
                    }
                    else
                    {
                        if (cadena.ElementAt(i) == ']')
                        {
                            if (pila.Extraer() != '[')
                            {
                                return "Expresion incorrecta fci";//falta corchete inicio
                            }
                        }
                    }
                }
                if (pila.Vacia())
                {
                    return "Expresion correcta";
                }
                else
                {
                    if (pila.Extraer() == '[')
                    {
                        return "Expresion incorrecta fcf";//falta corchete final
                    }
                    else
                    {
                        return "Expresion incorrecta fci";//falta corchete inicio
                    }
                }
            }
        }
    }
}
