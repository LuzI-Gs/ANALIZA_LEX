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
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.LinkLabel;
using System.Linq.Expressions;

namespace ANALIZA_LEX
{
    public partial class Form1 : Form
    {
        private Nodo raiz;
        private Arbol arbol;
        int m, mx, my;
        public Form1()
        {
            InitializeComponent();
            arbol = new Arbol();
            unIdentificador = new Identificador();
        }
        private void Clear()
        { /*Hugo Ramos - Metodo que limpia variables y dgv */
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
            txtLineasLexico.Text = "";              //Contador de lineas        
            txtSintactico.Text = "";                //Mostrador de estados                                          
            dgvErroresSemanticos.Rows.Clear();      // Limpia todas las filas
            dgvErroresSemanticos.Columns.Clear();   // Limpia todas las columnas
            dgvIden.Rows.Clear();
            dgvIden.Columns.Clear();
            dgvErroresLexicos.Rows.Clear();
            dgvErroresLexicos.Columns.Clear();
        }
        //Hugo Ramos - Variables para Triplos y asignacion de una expresion
        int indice = 1, indiceConstante = 0; //Hugo Ramos Zarate - Variables que almacenan valores int y string para el proceso de ayuda  de triplos o asignacion de variables o constantes. 
        string tipoDatoConstante = "", operacion = "", primerLinea = "", nuevaExpresion = "", igualacionValorVariable = "", operador = "", operando = "", operacionDescripcion = "", operacion1 = "", inputExpression = "", asignacionInicial = "", asignacionFinal = "", resultadoEsperado = "Dato objeto = {0}, Dato Fuente = {1}, Operador = {2}, Descripcion: {3}";
        List<string> logicLines = new List<string>(); //Hugo Ramos Zarate - variables para triplos
        //Aqui terminan variables de triplos y asignacion de una expresion
        int intEstado = 0, contador = 0, contadorLineas = 1;
        string token = "", sintactico = "", LenguajeNat = "", tipoDato = "", strValorIde = "";
        bool yaExiste = false;
        ConexionBD conexion = new ConexionBD();
        List<string> tablaSimbolo = new List<string>();  //arreglo donde se almacenan la información para la tabla de simbolos
        List<Error> errores = new List<Error>();
        List<Identificador> unaLista = new List<Identificador>();
        List<Triplo> listaTriplo = new List<Triplo>();
        Identificador unIdentificador;
        Triplo triplo;
        Error unError;
        List<TriploVerdadero> listaTriploVerdadero = new List<TriploVerdadero>();
        List<TriploFalso> listaTriploFalso = new List<TriploFalso>();
        TriploVerdadero triploVerdadero;
        TriploFalso triploFalso;
        public void MostrarMatriz()
        {
            conexion.abrir();
            SqlCommand query1 = new SqlCommand(" Select * from  Matriz", conexion.Conectarbd); //SqlCommand query1 = new SqlCommand(" Select * from MANL", conexion.Conectarbd);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = query1;
            DataTable Matriz = new DataTable();
            adaptador1.Fill(Matriz);
            dtgMatriz.DataSource = Matriz;
            conexion.cerrar();
        }
        public string BuscarToken(int Estado)
        {
            string tk = "";
            for (int i = 0; i < dtgMatriz.ColumnCount; i++)
            { /*RECORRE TODAS LAS COLUMNAS DE LA TABLA */
                if (dtgMatriz.Columns[i].HeaderText == "FDC")
                { /*SI ENCUENTRA LA COLUMNA QUE SE LLAMA TOKEN PROCEDE A HACER LAS SIG INSTRUCCIONES */
                    tk = dtgMatriz.Rows[Estado].Cells[i + 1].Value.ToString();     //TOMA EL VALOR DEL TOKEN DEPENDIENDO DEL ESTADO Y LOS ALMACENA EN LA VARIABLE Y LOS VA SUMANDO PARA FORMAR LA CADENA DE TOKENS 
                    //PINTA DE COLOR AZUL LA CELDA DEL TOKEN 
                    dtgMatriz.Rows[Estado].Cells[i + 1].Style.BackColor = Color.Blue;
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
                foreach (string palabras in Lenguajes)
                {
                    linea++;
                    char[] del = { ' ' };//AHORA TOMA CADA PALABRA DE CADA ORACION 
                    char chMuestra = ' ';
                    string strPalabra = palabras;    //TOMA LA PALABRA DE LA ORACION  INGRESADA               
                    string[] Lenguaje = strPalabra.Split(del);     //METE LAS PALABRAS EN UN ARREGLO                                           
                    evaluacion = EvaluarExpresion(palabras);//INVOCA EL METODO DE LA PILA PARA VERIFICAR QUE ESTEN CORRECTAMENTE EL BALANCE DE LOS CORCHETES Y SIGNOS DE ?
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
                    foreach (string palabra in Lenguaje)
                    {/*RECORRE EL ARREGLO DE LAS PALABRAS*/
                        foreach (string valor in Lenguaje)
                        {
                            strValorIde = valor; // toma el ultimo valor de la linea

                            if (valor == "ent" || valor == "flo" || valor == "boo" || valor == "car" || valor == "txt")
                            {
                                tipoDato = valor; // contiene el tipo de dato
                            }
                        }
                        string strIDE = palabra;

                        if (strIDE.StartsWith("_"))
                        { /* Sirve para dectectar los identificadores ya que inician con un _ */
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
                            { /* si no existe el identificador anteriormente se agrega al objeto*/
                                unIdentificador.Nombre = strIDE;
                                unIdentificador.Valor = strValorIde;
                            }
                        }

                        for (int i = 0; i < strIDE.Length; i++)
                        { /* CICLO QUE EXTRAE CADA LETRA DE CADA PALABRA */
                            chMuestra = strIDE[i];            //Validar tipo de dato string que este completo                                                              
                            ValidarIDE(intEstado, char.ToUpper(chMuestra));  //INVOCA UN METODO QUE TIPO BOOLEANO EL VERIFICA SI EL IDE ES ACEPTABLE
                        }
                        if (intEstado != 0) //MIENTRAS EL ESTADO SEA DIFERENTE DE 0 BUSCARA EL TOKEN AL QUE CORRESPONDE INVOCANDO UN METODO 
                        {
                            token = BuscarToken(intEstado);  //BUSCA EL TOKEN DE CADA PALABRA DESPUES DE VALIDAR EL IDE
                            //VA CONCATENANDO CADA TOKEN
                            //////////////////////////  es para cuando detecta que es un identificador  ////////////////////////////////////////////////////////
                            //si detecta que es un identificador le agrega un valor a un objeto de la clase Identificador
                            if (token.Trim() == "IDE")
                            {
                                //código anterior
                                /*contador++;
                                unIdentificador.Numero = contador;                                
                                unIdentificador.TipoDato = tipoDato;  */
                                foreach (var identificador in unaLista)
                                {
                                    if (identificador.Nombre == strIDE)
                                    {
                                        yaExiste = true;
                                        token = identificador.strIdentificador;
                                        identificador.Valor = strValorIde;
                                    }
                                }
                                if (!yaExiste)
                                {
                                    contador++;
                                    unIdentificador.strIdentificador = "IDE" + contador;
                                    unIdentificador.Numero = contador;
                                    unIdentificador.TipoDato = tipoDato;
                                    token = "IDE" + contador;
                                    unaLista.Add(unIdentificador);
                                }

                            }
                            /*if (token.Trim() == "IDE" && !yaExiste)
                            {                                
                                unaLista.Add(unIdentificador);                               
                            }*/
                            if (token.Trim().StartsWith("ER"))
                            {
                                Error unError = new Error();
                                unError.NomError = token.Trim();
                                errores.Add(unError);
                            }
                            txtTokens.Text = txtTokens.Text + " " + token.Trim();
                            intEstado = 0;//INICIALIZA DE NUEVO LA VARIABLE
                        }
                        else
                        { /*  si no encuenta el caracter que se ingreso mandara un mensaje diciendo ese mensaje */
                            string strCadenaError = "CARACTER NO VALIDO";
                            MessageBox.Show($"El caracter ->  {chMuestra}  <- no es valido en este lenguaje por favor ingresa uno que lo sea ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtTokens.Text = txtTokens.Text + " " + strCadenaError;
                        }
                    }
                    if (palabras != "\r\n")     //SI ENCUENTRA UN SALTO DE LINEA LO RESPETA Y LO VUELVE A COLOCAR
                    {
                        txtTokens.Text = txtTokens.Text + "\r\n";
                    }
                    if (palabras != " ")
                    {
                        txtTokens.Text = txtTokens.Text + "";
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Por favor ingresa una cadena", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
        int numeroDeLinea = -1;
        private string BuscarLineaError(string valor)
        { /* Buscar "ES03" en cada línea. */
            string[] lineas = txtLenguaje.Text.Split('\n');// Dividir el texto del TextBox en líneas utilizando '\n' como separador.
            for (int i = 0; i < lineas.Length; i++)
            { /* // Buscar "ES03" en cada línea.*/
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
                LenguajeNat = "";
                txtTokens.ReadOnly = true;
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
                foreach (Error error in errores) { dgvErroresLexicos.Rows.Add(error.NomError, error.MostrarCaracteristica()); }
                for (int i = 1; i < contadorLineas; i++) { txtLineasLexico.AppendText(i.ToString()); }
            }
            catch (Exception) { MessageBox.Show("Por favor ingresa una cadena", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
        public bool ValidarIDE(int Estado, char letra)
        {
            bool seEncontroCaracter = false; // Variable para indicar si se encontró el carácter        
            for (int i = 1; i < dtgMatriz.ColumnCount - 2; i++)
            {   /*RECORRE LAS COLUMAS DE LA TABLA OSEA EL ALFABETO-NUMEROS-OTROS*/
                //BUSCA POR LAS COLUMNAS  DESDE LA POSICION 1 OSEA DESDE LA lETRA -A- EN LA COLUMNA (0) QUE SON LOS NOMBRES DE LA TABLA 
                if (dtgMatriz.Columns[i].HeaderText[0] == letra)
                { /*    //SI EL CARACTER  ES IGUAL A LA LETRA ENVIADA */
                    if (dtgMatriz.Rows[Estado].Cells[i].Value.ToString() == "108")
                    { /*  // EN CASO DE QUE NO SEA ALGO VALIDO VERIFICA QUE SU ESTADO SEA  */
                        this.intEstado = int.Parse(dtgMatriz.Rows[Estado].Cells[i].Value.ToString());
                    }
                    else
                    { //SI NO LO ES PINTA DE AZUL LA CELDA DEL CARACTER

                        dtgMatriz.Rows[Estado].Cells[i].Style.BackColor = Color.Blue;// Si encuentra el Letra, obtiene el estado correspondiente y retorna true y pinta la letra correspondiente 
                        this.intEstado = int.Parse(dtgMatriz.Rows[Estado].Cells[i].Value.ToString());
                    }
                    seEncontroCaracter = true;//DEVUELVE UN TRUE REPRESENTADO QUE SE VALIDO EL IDE 
                    break;
                }
            }
            return seEncontroCaracter;//DEVUELVE UN FALSE  REPRESENTADO QUE SE NO VALIDO EL IDE 
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            MostrarMatriz();
            txtTokens.ReadOnly = true;
        }
        public void ChecarEspacios()
        {
            string textp = txtSintactico.Text;
            textp = textp.TrimEnd();
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
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void picMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        //METODO PARA CREAR EL ARCHIVO RECIBE COMO PARAMETRO LA RUTA Y EL NOMBRE DEL ARCHIVO
        public void crear(string nombre)
        {
            //PROPIEDAD QUE VA A ESCRIBIR LO QUEBTENEMOS EN EL TRIPLO 
            StreamWriter sw = new StreamWriter(nombre);
            //AQUI ESCRIBE LAS LINEAS POR DEAFULT .DATA Y .MODEL STACK
            // sw.WriteLine(".model small \n.stack 100h\n.data\n.code");
            //DESPUES AGREGA LO QUE UNO ESCRIBA EN EL RICHTEXBOX
            sw.WriteLine(rch.Text + "\n");
            //   sw.WriteLine(".MOV AH, 4CH\n.INT 21H");
            sw.Close();


        }

        private void btnDocumento_Click(object sender, EventArgs e)
        {
            SaveFileDialog sapF = new SaveFileDialog();
            sapF.Filter = "Assembler source|*.asm";
            if (sapF.ShowDialog() == DialogResult.OK)
            {

                //INVOCA AL MET
                crear(sapF.FileName);
            }
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

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            string textoARch = ".model small\n.stack 100h\n.data\nmsg db 10,13,\"Es mayor$\",0\nmsg1 db 10,13,\"Es menor$\",0\n.code" + Environment.NewLine;
            foreach (var triplo in listaTriplo)
            {
                switch (triplo.Operador)
                {
                    case "=":
                        if (triplo.DatoObjeto == "T1")
                        {

                            textoARch += "SUB AL,30h \r\nMOV AL," + triplo.DatoFuente + Environment.NewLine;
                        }
                        else
                        {
                            // textoARch += "MOV " + triplo.DatoObjeto + ",AL" + Environment.NewLine;

                        }
                        break;
                    case "add":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "ADD AL," + triplo.DatoFuente + Environment.NewLine;
                            textoARch += "MOV ah,09h\r\nmov dl,AL\r\nadd dl,30h\r\nmov ah,02h\r\nint 21h";
                        }
                        else
                        {
                            textoARch += " ADD " + triplo.DatoObjeto + ",AL" + Environment.NewLine;

                        }
                        break;
                    case "dec":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "SUB AL," + triplo.DatoFuente + Environment.NewLine;
                            textoARch += "MOV ah,09h\r\nmov dl,AL\r\nadd dl,30h\r\nmov ah,02h\r\nint 21h";
                        }
                        else
                        {
                            // textoARch +=  "SUB " + triplo.DatoObjeto + ", AL" + Environment.NewLine;

                        }
                        break;
                    case "div":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "DIV AL," + triplo.DatoFuente + Environment.NewLine;
                            textoARch += "MOV ah,09h\r\nmov dl,AL\r\nadd dl,30h\r\nmov ah,02h\r\nint 21h";
                        }
                        else
                        {
                            // textoARch += "DIV " + triplo.DatoObjeto + ",AL" + Environment.NewLine;

                        }
                        break;
                    case "mul":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "MUL AL," + triplo.DatoFuente + Environment.NewLine;
                            textoARch += "MOV ah,09h\r\nmov dl,AL\r\nadd dl,30h\r\nmov ah,02h\r\nint 21h";
                        }
                        else
                        {
                            // textoARch +=  "MUL " + triplo.DatoObjeto + ",AL" + Environment.NewLine;

                        }
                        break;
                    case "|y|":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "AND AX, " + triplo.DatoFuente + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "AND " + triplo.DatoObjeto + ",AX" + Environment.NewLine;
                        }
                        break;
                    case "|o|":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "OR AX, " + triplo.DatoFuente + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "OR " + triplo.DatoObjeto + ",  AX" + Environment.NewLine;
                        }
                        break;
                    case "|no|":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "NOT AX" + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "NOT " + triplo.DatoObjeto + Environment.NewLine;
                        }
                        break;
                    case "<":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "CMP AX, " + triplo.DatoFuente + Environment.NewLine + " JL <InstruccionMenor>" + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "CMP " + triplo.DatoObjeto + ", AX" + Environment.NewLine + " JL <InstruccionMenor>" + Environment.NewLine;
                        }
                        break;
                    case ">":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "CMP AX," + triplo.DatoFuente + Environment.NewLine + " JG <InstruccionMayor>" + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "CMP " + triplo.DatoObjeto + ",AX" + Environment.NewLine + " JG <InstruccionMayor>" + Environment.NewLine;
                        }
                        break;
                    case "<>": // DIFERENTE
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "CMP AX, " + triplo.DatoFuente + Environment.NewLine + "JNE <InstruccionDiferente>" + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "CMP " + triplo.DatoObjeto + ",AX" + Environment.NewLine + "JNE <InstruccionDiferente>" + Environment.NewLine;
                        }
                        break;
                    case "<=": // MENOR O IGUAL
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "CMP AX," + triplo.DatoFuente + Environment.NewLine + "JG <InstruccionMayorOIgual>" + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "CMP " + triplo.DatoObjeto + ",AX" + Environment.NewLine + "JG <InstruccionMayorOIgual>" + Environment.NewLine;
                        }
                        break;
                    case ">=": // MAYOR O IGUAL
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "CMP AX, " + triplo.DatoFuente + Environment.NewLine + "\n JLE <InstruccionMenorOIgual>" + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "CMP " + triplo.DatoObjeto + ",AX" + Environment.NewLine + "\n JLE <InstruccionMenorOIgual>" + Environment.NewLine;
                        }
                        break;
                    default:
                        break;
                }
            }
            textoARch += "\r\nMOV AH, 4CH\r\nINT 21H \r\nend";
            rch.Text = textoARch;
        }

        private void rch_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {

            if (m == 1)
            {
                this.SetDesktopLocation(MousePosition.X - mx, MousePosition.Y - my);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {

            m = 0;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            m = 1;
            mx = e.X;
            my = e.Y;
        }

        private void btnGenerar_Click_1(object sender, EventArgs e)
        {
            string textoARch = ".model small\n.stack 100h\n.data\nmsg db 10,13,\"Es mayor$\",0\nmsg1 db 10,13,\"Es menor$\",0\n.code\r\nmov ax, @data\r\nmov ds, ax" + Environment.NewLine;
            foreach (var triplo in listaTriplo)
            {
                switch (triplo.Operador)
                {
                    case "=":
                        if (triplo.DatoObjeto == "T1")
                        {

                            textoARch += "SUB AL,30h \r\nMOV AL," + triplo.DatoFuente + Environment.NewLine;
                        }
                        else
                        {
                            // textoARch += "MOV " + triplo.DatoObjeto + ",AL" + Environment.NewLine;

                        }
                        if (triplo.DatoObjeto == "T2")
                        {

                            textoARch += "SUB BL,30h \r\nMOV BL," + triplo.DatoFuente + Environment.NewLine;
                        }
                        else
                        {
                            // textoARch += "MOV " + triplo.DatoObjeto + ",AL" + Environment.NewLine;

                        }
                        break;
                  
                    case "add":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "ADD AL," + triplo.DatoFuente + Environment.NewLine;
                            textoARch += "MOV ah,09h\r\nmov dl,AL\r\nadd dl,30h\r\nmov ah,02h\r\nint 21h";
                        }
                        else
                        {
                            textoARch += " ADD " + triplo.DatoObjeto + ",AL" + Environment.NewLine;

                        }
                        break;
                    case "dec":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "SUB AL," + triplo.DatoFuente + Environment.NewLine;
                            textoARch += "MOV ah,09h\r\nmov dl,AL\r\nadd dl,30h\r\nmov ah,02h\r\nint 21h";
                        }
                        else
                        {
                            // textoARch +=  "SUB " + triplo.DatoObjeto + ", AL" + Environment.NewLine;

                        }
                        break;
                    case "div":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "DIV AL," + triplo.DatoFuente + Environment.NewLine;
                            textoARch += "MOV ah,09h\r\nmov dl,AL\r\nadd dl,30h\r\nmov ah,02h\r\nint 21h";
                        }
                        else
                        {
                            // textoARch += "DIV " + triplo.DatoObjeto + ",AL" + Environment.NewLine;

                        }
                        break;
                    case "mul":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "MUL AL," + triplo.DatoFuente + Environment.NewLine;
                            textoARch += "MOV ah,09h\r\nmov dl,AL\r\nadd dl,30h\r\nmov ah,02h\r\nint 21h";
                        }
                        else
                        {
                            // textoARch +=  "MUL " + triplo.DatoObjeto + ",AL" + Environment.NewLine;

                        }
                        break;
                    case "|y|":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "AND AX, " + triplo.DatoFuente + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "AND " + triplo.DatoObjeto + ",AX" + Environment.NewLine;
                        }
                        break;
                    case "|o|":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "OR AX, " + triplo.DatoFuente + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "OR " + triplo.DatoObjeto + ",  AX" + Environment.NewLine;
                        }
                        break;
                    case "|no|":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "NOT AX" + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "NOT " + triplo.DatoObjeto + Environment.NewLine;
                        }
                        break;
                    case "OPR3":
                        if (triplo.DatoObjeto == "T1")
                        {
                           
                            
                            textoARch += "CMP AL,BL "  + Environment.NewLine + " jg _if_true\r\nlea dx, msg1\r\n mov ah, 9 \r\nmov cx, 10\r\nint 21h\r\n jmp exit_program\r\n_if_true:\r\nlea dx, msg\r\nmov ah, 9\r\nmov cx, 10\r\nint 21h\r\nexit_program:\r\nmov ah, 4ch\r\nint 21h " + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "CMP " + triplo.DatoObjeto + ", AX" + Environment.NewLine + "  " + Environment.NewLine;
                        }
                        break;
                   
                    /* case ">":
                         if (triplo.DatoObjeto == "T1")
                         {
                             textoARch += "CMP AX," + triplo.DatoFuente + Environment.NewLine + " JG <InstruccionMayor>" + Environment.NewLine;
                         }
                         else
                         {
                             textoARch += "CMP " + triplo.DatoObjeto + ",AX" + Environment.NewLine + " JG <InstruccionMayor>" + Environment.NewLine;
                         }
                         break;
                     case "<>": // DIFERENTE
                         if (triplo.DatoObjeto == "T1")
                         {
                             textoARch += "CMP AX, " + triplo.DatoFuente + Environment.NewLine + "JNE <InstruccionDiferente>" + Environment.NewLine;
                         }
                         else
                         {
                             textoARch += "CMP " + triplo.DatoObjeto + ",AX" + Environment.NewLine + "JNE <InstruccionDiferente>" + Environment.NewLine;
                         }
                         break;
                     case "<=": // MENOR O IGUAL
                         if (triplo.DatoObjeto == "T1")
                         {
                             textoARch += "CMP AX," + triplo.DatoFuente + Environment.NewLine + "JG <InstruccionMayorOIgual>" + Environment.NewLine;
                         }
                         else
                         {
                             textoARch += "CMP " + triplo.DatoObjeto + ",AX" + Environment.NewLine + "JG <InstruccionMayorOIgual>" + Environment.NewLine;
                         }
                         break;
                     case ">=": // MAYOR O IGUAL
                         if (triplo.DatoObjeto == "T1")
                         {
                             textoARch += "CMP AX, " + triplo.DatoFuente + Environment.NewLine + "\n JLE <InstruccionMenorOIgual>" + Environment.NewLine;
                         }
                         else
                         {
                             textoARch += "CMP " + triplo.DatoObjeto + ",AX" + Environment.NewLine + "\n JLE <InstruccionMenorOIgual>" + Environment.NewLine;
                         }
                         break;*/
                    default:
                        break;
                }
            }
            textoARch += "\r\nMOV AH, 4CH\r\nINT 21H \r\nend";
            rch.Text = textoARch;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            SaveFileDialog sapF = new SaveFileDialog();
            sapF.Filter = "Assembler source|*.asm";
            if (sapF.ShowDialog() == DialogResult.OK)
            {

                //INVOCA AL MET
                crear(sapF.FileName);
            }
        }

        private bool VerificarAsignacion(string linea)
        {
            string patronOperacion1 = @"^IDE[12N]\sCE06\sENTE\s(?:OPA[1-5])\sENTE"; //ya
            bool matchOperacion1 = Regex.IsMatch(linea, patronOperacion1, RegexOptions.IgnoreCase);
            string patronOperacion2 = @"CE10\sENTE\s(?:OPR[1-5])\sCE10\sENTE\sOPA2\sENTE\sCE11\sCE11\s\[\d+\]\s\[\d+\]$";//@"CE10\sENTE\s(?:OPR[1-5])\sCE10\sENTE\sOPA2\sENTE\sCE11\sCE11\s\[\d+\]\s\[\d+\]$";//CE10\sENTE\s(?:OPR[1-5])\sCE10\sENTE\sOPA2\sENTE\sCE11\sCE11";                                                                                                             // string patronOperacion2 = @"CE10\s(?:ENTE|ES02|ES03)$\s(?:OPR[1-5])\sCE10\sENTE\s(?:OPA[1-5])\sENTE\sCE11\sCE11";
            bool matchOperacion2 = Regex.IsMatch(linea, patronOperacion2, RegexOptions.IgnoreCase);
            string patronEstructura1 = @"^IDE[12N]\sPR05\sCE06\s(?:ENTE|ES02|ES03)$";
            // Expresión regular para la estructura 1
            string patronEstructura11 = @"^IDE[12N]\sPR08\sCE06\s(?:ES04|ES05|FLOT)$";
            string patronEstructura12 = @"^IDE[12N]\sPR18\sCE06\sES07$";
            string patronEstructura13 = @"^IDE[12N]\sPR18\sCE06\s(?:PR06|PR19)$";
            // Expresión regular para la estructura 2
            // string patronEstructura2 = @"^PR14$|^CE05\sIDE[12N]\sOPR[1-5]\sENTE\sCE03$|^PR11$|^PR10\sIDE[12N]\sCE06\sIDE[12N]\sOPA[1-5]\sENTE$|^PR07$";// @"^PR14$|^CE05\sIDE[12N]\s(?:OPR[1-5])\sENTE\sCE03$|^PR11$|^PR10\sIDE[12N]\sCE06\sIDE[12N]\s(?:OPA[1-5])\sENTE$|^PR07$";
            string patronEstructura2 = @"^PR14$|^CE05\sIDE[12N]\sOPR[1-5]\sENTE\sCE03$|^PR11$|^PR10\sIDE[12N]$|^IDE[0-9]+\sCE06\sIDE[0-9]+\sOPA[1-5]\sENTE$|^PR07$";//ya
            //Expresion regular para la estrucrura 3
            // string patronEstructura3 = @"^PR16$|^PR11$|^PR10\s(?:ES07|IDE)$|^PR07$|^CE05\sIDE\s(?:(?:OPA[1-5])|(?:OPR[1-5]))\sENTE\s(?:(?:OPR[1-5])|(?:OPL[1-3]))\sIDE\s(?:(?:OPA[1-5])|(?:OPR[1-5]))\sENTE\sCE03$";
               // string patronEstructura3 = @"^PR16$|^PR11$|^PR10\s(?:ES07|IDE[12N])$|^PR07$|^CE05\sENTE\s(?:OPR[1-5])\sENTE\sCE03$|^PR19$|^PR17$|^PR06$";
                //string patronEstructura3 = @"^PR16$|^PR11$|^PR10\s(?:ES07|IDE[12N])$|^PR07$|^CE05\sENTE\s(?:OPR[1-5])\sENTE\sCE03$|^PR19$|^PR17$|^PR06$";
                string patronEstructura3 = @"^PR16\sCE05\sENTE\sOPR3\sENTE\sCE03$|^PR11$|^PR19$|^PR10\sES07$|^PR17$|^PR06$|^PR10\sES07$|^PR07$";

            //string patronEstructura3 = @"^PR16$|^PR11$|^PR10\s(?:ES07|sIDE[12N])$|^PR07$|^CE05\sIDE[12N]\s(?:(?:OPA[1-5])|(?:OPR[1-5]))\sFLOT\s(?:(?:OPR[1-5])|(?:OPL[1-3]))\sIDE[12N]\s(?:(?:OPA[1-5])|(?:OPR[1-5]))\sFLOT\sCE03$|^PR19$|^PR17$|^PR06$";
            //Expresion regular para la estrucrura 4
            // string patronEstructura4 = @"^PR15$|^CE05\sIDE[12N]\sPR05\sCE06\sENTE\sCE02\sIDE[12N]\s(?:OPR[1-5])\sENTE\sCE02\sIDE[12]\sCE06\sIDE[12N]\sOPA1\sENTE\sCE03$|^PR11$|^PR10\s(?:ES07|sIDE[12N])$|^PR07$";
            string patronEstructura4 = @"^PR15$|^CE05\sIDE1\sPR05\sCE06\sENTE\sCE02\sIDE2\sOPR2\sENTE\sCE02\sIDE3\sCE06\sIDE4\sOPA1\sENTE\sCE03$|^PR11$|^PR10\sIDE5$|^PR07$";

            //Expresion regular para la estrucrura 5
            string patronEstructura5 = @"^IDE[0-9]+\sPR05\sCE06\sENTE$|^PR09$|^PR11$|^IDE[0-9]+\sCE06\sIDE[0-9]+\sOPA1\sENTE$|^PR10\sES07$|^PR14$|^CE05\sIDE[0-9]+\sOPR3\sENTE\sCE03$|^PR07$";//ya
                                                                                                                                                                                              // string patronEstructura5 = @"^PR09$|^PR11$|^IDE[12N]\sCE06\sIDE[12N]\sOPA1\sENTE$|^PR10\sES07$|^PR14$|^CE05\sIDE[12N]\s(?:OPR[1-5])\sENTE\sCE03$|^PR07$";
                                                                                                                                                                                              // string patronEstructura4 = @"^PR15$|^CE05\sIDE\sPR05\sCE06\sENTE\sCE02\sIDE\sOPR2\sENTE\sCE02\sIDE\sCE06\sIDE\sOPA1\sENTE\sCE03$|^PR11$|^PR10\sIDE$|^PR07$";
            bool matchEstructura1 = Regex.IsMatch(linea, patronEstructura1, RegexOptions.IgnoreCase);
            bool matchEstructura11 = Regex.IsMatch(linea, patronEstructura11, RegexOptions.IgnoreCase);
            bool matchEstructura12 = Regex.IsMatch(linea, patronEstructura12, RegexOptions.IgnoreCase);
            bool matchEstructura13 = Regex.IsMatch(linea, patronEstructura13, RegexOptions.IgnoreCase);
            bool matchEstructura2 = Regex.IsMatch(linea, patronEstructura2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            bool matchEstructura3 = Regex.IsMatch(linea, patronEstructura3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            bool matchEstructura4 = Regex.IsMatch(linea, patronEstructura4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            bool matchEstructura5 = Regex.IsMatch(linea, patronEstructura5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return matchEstructura1 || matchEstructura11 || matchEstructura12 || matchEstructura13 || matchEstructura2 || matchEstructura3 || matchEstructura4 || matchEstructura5 || matchOperacion1 || matchOperacion2;
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Clear(); //ejecuta metodo que limpia tableros, variables globales
        }
        bool primerCambio = true;
        private void txtLenguaje_TextChanged(object sender, EventArgs e)
        { /*se agregan las lineas de codigo cada vez que se detecta el salto de linea */
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
            txtLineasLexico.Text = "";
            listaTriplo.Clear();
            dgvTriplos.Rows.Clear();
            listaTriplo.Clear();
            listaTriploVerdadero.Clear();
            listaTriploFalso.Clear();
            dgvTriploVerdadero.Visible = false;
            dgvTriploFalso.Visible = false;
            contador = 0;
            try
            {
                DescomponerCadenas(); //INVOCA EL METODO
                LenguajeNat = "";
                txtTokens.ReadOnly = true;
                string textoOriginal = txtTokens.Text; //metodo para quitar la sangria 
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
                    dgvIden.Rows.Add(miIdentificador.Numero, miIdentificador.strIdentificador, miIdentificador.Nombre, miIdentificador.TipoDato, miIdentificador.Valor);
                }
                dgvErroresLexicos.Rows.Clear();
                foreach (Error error in errores)
                {
                    dgvErroresLexicos.Rows.Add(error.NomError, error.MostrarCaracteristica());
                }
                for (int i = 1; i < contadorLineas; i++)
                {
                    txtLineasLexico.AppendText($"\n{i.ToString()}  ");
                }
                ErroresSemanticos();
            }
            catch (Exception) { MessageBox.Show("Por favor ingresa una cadena", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            InstruccionesSeleccion();
            CicloMientras();

        }
        private void btnValidarSint_Click_1(object sender, EventArgs e)
        {
            try
            {
                string texto = txtTokens.Text;
                EnviarLineasHastaDelimitador(texto);
                Ejecucion();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void picSalir_Click(object sender, EventArgs e) { Application.Exit(); }
        private string EvaluarExpresion(string expresion)
        { /* METODO QUE COMPRUEBA LOS signos¿? y corchetes[] */
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
        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            if (radPreorden.Checked)
            {
                arbol.InsertarEnCola(txtLenguaje.Text);
                raiz = arbol.CrearArbol();
                arbol.Limpiar();
                rtxt.Text = arbol.InsertarPre(raiz);
            }
            if (radPostorden.Checked)
            {
                arbol.InsertarEnCola(txtLenguaje.Text);
                raiz = arbol.CrearArbol();
                arbol.Limpiar();
                rtxt.Text = arbol.InsertarPost(raiz);
            }
            if (radInOrden.Checked)
            {
                arbol.InsertarEnCola(txtLenguaje.Text);
                raiz = arbol.CrearArbol();
                arbol.Limpiar();
                rtxt.Text = arbol.InsertarIn(raiz);
            }
        }
        /*---------------*/
        // realiza la busqueda de variables en la tabla para poder hacer la nueva expresion con asignacion
        public void metodoBusqueda()
        {
            int numeroDeLineas = txtLenguaje.Lines.Length;
            for (int miPosicionLinea = 0; miPosicionLinea < numeroDeLineas; miPosicionLinea++)
            {
                primerLinea = txtLenguaje.Lines.Length > 0 ? txtLenguaje.Lines[miPosicionLinea] : string.Empty;
                // Dividimos la expresión en tokens por espacios en blanco
                string[] tokens = primerLinea.Split(' ');
                //evalua la linea si es es expresion
                if (EsExpresion(primerLinea))
                {
                    int numeroDeFilas = dgvVariables.Rows.Count;
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        //Compara la posicion de la palaraba(token) donde esta la linea en que se encuentra la expresion aritmetica 
                        if (i == 0)
                        {
                            //Mensaje de pantalla que ayuda visualmente saber cual es la variable donde se guardara el valor de la expresion
                            nuevaExpresion = tokens[i];
                        }
                        if (i == 1)
                        {
                            // MessageBox.Show("Identifica si despues de la variable donde se almacenara el valor si es = "+nuevaExpresion);
                            nuevaExpresion = nuevaExpresion + " " + tokens[i];
                        }
                        // - Verifica si es un entero y si la linea contiene algun operador, logicamente indica que sea una expresion y no otra instruccion
                        if (EsEntero(tokens[i]) || tokens[i] == "add" || tokens[i] == "dec" || tokens[i] == "mul")
                        {
                            if (EsEntero(tokens[i]))
                            { //Verifica si es un entero y si si concatena el valor 
                                nuevaExpresion = nuevaExpresion + " " + tokens[i]; // -  metodo que detecta que valor tiene la variable en la expresion e imprime el valor
                            }
                            else
                            {   /*Nos ayuda a identificar el cual es el valor de la variable que se encuentra en la expresion aritmetica,
                                 * nos ayudara en un fururo para realizar las operaciones y su postfijo */
                                nuevaExpresion = nuevaExpresion + " " + tokens[i];
                            }
                        }
                        else
                        {
                            foreach (DataGridViewRow fila in dgvVariables.Rows)
                            { /*Recorre las filas y columnas de datagridview */
                                object valorCelda = fila.Cells[1].Value;
                                if (valorCelda != null)
                                { /* Como aveces los valores de las celdas son vacios - ayuda a que no tome en cuenta ese valor y solo los que si tienen */
                                    string variablesDeLaColumna = valorCelda.ToString(); //convierte el valor de la tabla en un string para poder manejarlo
                                    if (variablesDeLaColumna == tokens[i] && variablesDeLaColumna != tokens[0])
                                    {
                                        //Obtiene el valor de variable segun la fila y columna de variable
                                        igualacionValorVariable = fila.Cells[3].Value.ToString();
                                        // Nos ayuda a identificar el cual es el valor de la variable que se encuentra en la expresion aritmetica,
                                        // nos ayudara en un fururo para realizar las operaciones y su postfijo
                                        nuevaExpresion = nuevaExpresion + " " + igualacionValorVariable;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            MessageBox.Show($"Expresion con asignacion de valores: {nuevaExpresion}"); //Resultado final de la identificacion de la variables dentro de la expresion       
        }
        public void Triplos() //Proceso de triplos - Identifica Dato Objeto, Dato Fuente, Operador y una descripcion de la expresion
        {
            int numeroDeLineas = txtLenguaje.Lines.Length;
            // se genera un objeto nuevo de la clase Triplo
            for (int miPosicionLinea = 0; miPosicionLinea < numeroDeLineas; miPosicionLinea++)
            {

                primerLinea = txtLenguaje.Lines.Length > 0 ? txtLenguaje.Lines[miPosicionLinea] : string.Empty;
                string[] tokens = primerLinea.Split(' '); // Dividimos la expresión en tokens por espacios en blanco
                if (EsExpresion(primerLinea))
                {
                    int numeroDeFilas = dgvVariables.Rows.Count; //Cuenta el numero de filas para saber en que fila se ira imprimiendo los mensajes de ayuda  
                    //MessageBox.Show("Esta es la expresion aritmetica a resolver: "+inputExpression); //Imprime en pantalla la expresion aritmetica
                    inputExpression = primerLinea.ToString();
                    string[] elemento = inputExpression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);// Dividir la expresión en tokens   
                    //Asignación inicial - Asigna el valor cuando es =, por ejemplo: Asigna el valor de '{elemento[2]}' a una variable temporal (t1)
                    asignacionInicial = string.Format(resultadoEsperado, "T1", elemento[2], "=", $"Asigna el valor de '{elemento[2]}' a una variable temporal (t1)");
                    //Imprime la asignacion inicial para saber cual es la primera, se guarda para saber posteriormente si es inicial y si no lo es guardar
                    //en la variable que se asigna el resultado
                    triplo = new Triplo();
                    triplo.DatoObjeto = "T1";
                    triplo.DatoFuente = elemento[2];
                    triplo.Operador = "=";
                    listaTriplo.Add(triplo);
                    dgvTriplos.Rows.Add("T1", elemento[2], "=", $"Asigna el valor de '{elemento[2]}' a una variable temporal (t1)");
                    logicLines.Add(asignacionInicial);
                    triplo = new Triplo();
                    for (int i = 3; i < elemento.Length; i += 2) //Procesar las operaciones
                    {
                        operador = elemento[i]; //almancena temporalmente el operador 
                        operando = elemento[i + 1]; //Almacena temporalmente  el operando para usos de iteracion
                        operacionDescripcion = $"Se {ObtenerDescripcionOperacion(operador)} el valor de '{operando}' a la variable temporal (T1)";
                        //valor que se mando a LogicLines que contiene operando, operador, operacion descripcion
                        operacion1 = string.Format(resultadoEsperado, "T1", operando, operador, operacionDescripcion);
                        triplo.DatoObjeto = "T1";
                        triplo.DatoFuente = operando;
                        triplo.Operador = operador;
                        dgvTriplos.Rows.Add("T1", operando, operador, operacionDescripcion);//Agrega a la tabla la informacion recabada de t1, operador, operacion descripcion
                        logicLines.Add(operacion1);
                    }
                    listaTriplo.Add(triplo);
                    triplo = new Triplo();
                    triplo.DatoObjeto = elemento[0];
                    triplo.DatoFuente = "T1";
                    triplo.Operador = "=";
                    listaTriplo.Add(triplo);
                    //Asignación final -  variable que almacena la cadena y concatenacionn de resultadoEsperado y elemento, Se asigna la variable temporal al identificador final
                    asignacionFinal = string.Format(resultadoEsperado, elemento[0], "T1", "=", "Se asigna la variable temporal al identificador final");
                    //Mensaje de pantalla que ayuda a imprimir asignacion final                                                                                                                       
                    dgvTriplos.Rows.Add(elemento[0], "T1", "=", "Se asigna la variable temporal al identificador final");
                    logicLines.Add(asignacionFinal); //Imprimir la lógica generada de linea por linea de triplos por si se ocupa a futuro
                }
            }
        }
        public void Ejecucion()
        {
            int numeroDeLineas = txtLenguaje.Lines.Length; //Se obtiene el texto y se define el tamano 
            for (int miPosicionLinea = 0; miPosicionLinea < numeroDeLineas; miPosicionLinea++)
            {
                primerLinea = txtLenguaje.Lines.Length > 0 ? txtLenguaje.Lines[miPosicionLinea] : string.Empty; //Se fragmenta en lineas del texto completo
                string[] tokens = primerLinea.Split(' '); //Dividimos la expresión en tokens por espacios en blanco
                if (EsDeclaracionVariable(primerLinea))
                {
                    var datosVariable = ObtenerDatosDeclaracionVariable(primerLinea);
                    dgvVariables.Rows.Add(indice, datosVariable.Nombre, datosVariable.TipoDato, datosVariable.Valor);
                    indice = indice + 1;



                }
                else if (EsExpresion(primerLinea))
                {
                    string patron = @"(?<variable>\w+)\s*=\s*(?<operacion>.*)";  //Patrón para buscar la variable y la operación
                    Match match = Regex.Match(primerLinea, patron); //Buscar coincidencias en la expresión 
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        string token = tokens[i];
                        if (token.All(c => char.IsDigit(c) || c == '.')) //compara que sea digito entero o db, si es db debe aceptar .
                        {
                            if (EsEntero(token)) { tipoDatoConstante = "Entero"; }
                            else if (EsDouble(token)) { tipoDatoConstante = "Double"; }
                            else
                            {/*MessageBox.Show("El valor ingresado no es ni un entero ni un double válido."); //Detecta que no sea una expresion, ayuda a identificar 
                                   * que hacer cuando no sea y seguir su proceso por ejemplo luego puede seguir con una iteracion o etc*/
                            }
                            //Agrega a la tabla de constantes, la constante que se detecto en la expresion  
                            dgvConstantes.Rows.Add(indiceConstante = indiceConstante + 1, token, tipoDatoConstante);
                        }
                    }
                }
                else {/* Lo puse por si en un furuto se necesita identificar que no sea variable o constante */}
            }
            metodoBusqueda(); //ejecuta el metodo de busqueda de datos
            Triplos();//ejecuta el metodo de triplos
        }
        static bool EsDeclaracionVariable(string entrada)
        {
            //Utiliza una expresión regular para verificar si la entrada coincide con un patrón de declaración de variable.
           //  string patron = @"^\s*(\w+)\s+(\w+)\s*=\s*(\d+)\s*$"; //por si falla el patron de una variable xd
            string patron = @"^\s*(_?\w+)\s+(\w+)\s*=\s*(\w+)\s*$";

            return Regex.IsMatch(entrada, patron);
        }
        static (string Nombre, string TipoDato, string Valor) ObtenerDatosDeclaracionVariable(string entrada)
        {
            // Utiliza una expresión regular para extraer el nombre de la variable, el tipo de dato y el valor de una declaración de variable.
            string patron = @"^\s*(\w+)\s+(\w+)\s*=\s*(\d+)\s*$";
            Match coincidencia = Regex.Match(entrada, patron);
            return (coincidencia.Groups[1].Value, coincidencia.Groups[2].Value, coincidencia.Groups[3].Value);
        }
        static bool EsExpresion(string entrada)
        {
            string patron = @"^\w+\s*=\s*(.*)$";
            return Regex.IsMatch(entrada, patron); //Utiliza una expresión regular para verificar si la entrada es una expresión válida.
        }
        static bool EsEntero(string input)
        {
            //Metodo que ingresa una entrada y verifica si puede convertirse a int y si si, devuelve que tipo de dato es 
            int resultado;
            return int.TryParse(input, out resultado);
        }
        static bool EsDouble(string input)
        {
            //Metodo que ingresa una entrada y verifica si puede convertirse a db y si si, devuelve que tipo de dato es 
            double resultado;
            return double.TryParse(input, out resultado);
        }
        //Metodo que ingresa un operador y devuelve una cadena segun su tipo, para imprimir la descripcion de la tabla de triplos
        static string ObtenerDescripcionOperacion(string operador)
        {
            switch (operador)
            {
                case "add": return "suma"; break;
                case "dec": return "resta"; break;
                case "mul": return "multiplica"; break;
                case "div": return "divide"; break;
                case "|y|": return "operador and"; break;
                case "|o|": return "operador or"; break;
                case "|no|": return "operador not"; break;
                case ">": return "operador mayor que"; break;
                case "<": return "operador menor que"; break;
                case "<>": return "operador diferente"; break;
                case "<=": return "menor que"; break;
                case ">=": return "mayor que"; break;
                default: return "realiza una operación desconocida";
            }
        }

        //CON ESTE METODO SE VA ABRIR EL ARCHIVO QUE SE CREO 
        public void Leer(string ruta)
        {
            StreamReader leer = new StreamReader(ruta, true);
            string linea;
            try
            {
                linea = leer.ReadLine();
                while (linea != null)
                {

                    rch.AppendText(linea + "\n");
                    linea = leer.ReadLine();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("error");
            }
        }
        public void ErroresSemanticos()
        {

            int numLineasSemantico = txtLenguaje.Lines.Length; //Se obtiene el texto y se define el tamano 
            for (int miPosicionLinea = 0; miPosicionLinea < numLineasSemantico; miPosicionLinea++)
            {
                primerLinea = txtLenguaje.Lines.Length > 0 ? txtLenguaje.Lines[miPosicionLinea] : string.Empty; //Se fragmenta en lineas del texto completo
                string[] unToke = primerLinea.Split(' '); //Dividimos la expresión en tokens por espacios en blanco
                for (int i = 0; i < unToke.Length; i++)
                {
                    switch (unToke[i])
                    {
                        case "ENTE": token = "PR05"; break;
                        case "FLO": token = "PR08"; break;
                        case "TXT": token = "PR018"; break;
                        case "BOO": token = "PR02"; break;
                        case "ente": token = "PR05"; break;
                        case "flo": token = "PR08"; break;
                        case "txt": token = "PR018"; break;
                        case "boo": token = "PR02";MessageBox.Show("bandera"); break;
                    }
                }              

                string palabra =unToke[3];
                //MessageBox.Show($"{palabra}");
                char[] arregloDeCaracteres = palabra.ToCharArray();
                //Muestra cada caracter del arreglo
                //foreach (char caracter in arregloDeCaracteres)
                //{
                //    MessageBox.Show("este es: " + caracter.ToString());
                //}
                //MessageBox.Show($"Esta es la linea : {miPosicionLinea + 1}");
                switch (token)
                {
                    case "PR018":
                        int banderainicio = 0;
                        for (int i = 0; i < arregloDeCaracteres.Length; i++)
                        {
                            if (arregloDeCaracteres[i] == '"')
                            {
                                banderainicio++;
                            }
                        }
                        if (banderainicio != 2)
                        {
                            dgvErroresSemanticos.Rows.Add(miPosicionLinea+1, "Error: Cadena Invalida.");
                        }
                        break;
                    case "PR05":
                        if (char.IsDigit(arregloDeCaracteres[0]) || arregloDeCaracteres[0] == '+' || arregloDeCaracteres[0] == '-')
                        {
                            if (char.IsDigit(arregloDeCaracteres[0]))
                            {
                                for (int i = 0; i < arregloDeCaracteres.Length; i++)
                                {
                                    // MessageBox.Show($"Empieza en : {i}");  // MessageBox.Show($"{strValorIde[i]}");
                                    char miCaracter2 = arregloDeCaracteres[i];
                                    int valorAscii2 = (int)miCaracter2;
                                    if (valorAscii2 >= 48 && valorAscii2 <= 57) //valores numericos en ascii para el resto
                                    {
                                        //MessageBox.Show($"{strValorIde[i]} - si pertenece a un numero");                                                
                                    }
                                    else
                                    {
                                        //  MessageBox.Show($"{strValorIde[i]} - no pertenece a un numero");
                                        dgvErroresSemanticos.Rows.Add(miPosicionLinea+1, $"{arregloDeCaracteres[i]} - no pertenece a un numero");
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: Se esperaba un tipo de dato numerico entero.");
                        }
                        break;
                    case "PR08":
                        if (arregloDeCaracteres[0] != '+' && arregloDeCaracteres[0] != '-' && !char.IsDigit(arregloDeCaracteres[0]))
                        {
                            dgvErroresSemanticos.Rows.Add(miPosicionLinea+1, "Error: El primer carácter debe ser '+' o '-' o un numero");
                        }
                        else
                        {
                            bool caracteresNumericos = true;
                            for (int i = 1; i < arregloDeCaracteres.Length; i++)
                            {
                                if (!char.IsDigit(arregloDeCaracteres[i]) && arregloDeCaracteres[i] != '.')
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
                                dgvErroresSemanticos.Rows.Add(miPosicionLinea + 1, "Error: Se encontraron valores no numéricos");
                            }
                        }
                        break;
                    case "PR02":
                        MessageBox.Show("otra bandera");
                        if (arregloDeCaracteres.Contains('v') && arregloDeCaracteres.Contains('e') && arregloDeCaracteres.Contains('r') && arregloDeCaracteres.Contains('d'))
                        {
                           // MessageBox.Show("Es correcto");
                        }
                        else if (arregloDeCaracteres.Contains('f') && arregloDeCaracteres.Contains('a') && arregloDeCaracteres.Contains('l'))
                        {
                           // MessageBox.Show("Es correcto");
                        }
                        else
                        {
                            dgvErroresSemanticos.Rows.Add(miPosicionLinea + 1, "Se esperaba un fal o un verd");
                             
                        }


                        break;
                }
            }
        }
        //MÉTODO PARA CUANDO ES UNA INSTRUCCION DE SELECCION 
        public void InstruccionesSeleccion()
        {
            bool inicioCondicion = false;
            bool finCondicion = false;
            int contadorTemporales = 1;
            string operador = "";
            bool secuenciaVerdadera = false;
            bool secuenciaFalsa = false;
            bool esSeleccion = false;
            bool cadena = false;
            bool comparacion = false;

            int numeroDeLineas = txtTokens.Lines.Length - 1; //Se obtiene el texto y se define el tamano 
            int numeroDeLineasLenguaje = txtLenguaje.Lines.Length;
            int indexToken = 0;
            for (int miPosicionLinea = 0; miPosicionLinea < numeroDeLineas; miPosicionLinea++)
            {
                primerLinea = txtTokens.Lines.Length > 0 ? txtTokens.Lines[miPosicionLinea] : string.Empty; //Se fragmenta en lineas del texto completo
                string[] tokens = primerLinea.Split(' '); //Dividimos la expresión en tokens por espacios en blanco
                string lineasLenguaje = txtLenguaje.Lines.Length > 0 ? txtLenguaje.Lines[miPosicionLinea] : string.Empty;
                string[] lenguaje = lineasLenguaje.Split(' ');

                if (tokens[0] == "PR16")
                {
                    esSeleccion = true;
                }



                if (esSeleccion == true)
                {
                    foreach (var token in tokens)
                    {
                        if (token == "CE05")
                        {
                            inicioCondicion = true;
                        }
                        if (inicioCondicion == true)
                        {
                            if (token.StartsWith("ENTE") || token.StartsWith("IDE"))
                            {
                                Triplo miObjetoTriplo = new Triplo();
                                miObjetoTriplo.DatoObjeto = "T" + contadorTemporales;
                                miObjetoTriplo.DatoFuente = lenguaje[indexToken];
                                miObjetoTriplo.Operador = "=";
                                listaTriplo.Add(miObjetoTriplo);
                                contadorTemporales++;
                            }
                        }
                        if (token == "CE03")
                        {
                            inicioCondicion = false;
                        }

                        if (token.StartsWith("OP"))
                        {
                            operador = token;
                        }
                        if (comparacion == true)
                        {
                            Triplo miObjetoTriplo = new Triplo();
                            miObjetoTriplo.DatoObjeto = "T1";
                            miObjetoTriplo.DatoFuente = "T2";
                            miObjetoTriplo.Operador = operador;
                            listaTriplo.Add(miObjetoTriplo);
                            comparacion = false;
                        }
                        if (inicioCondicion == false && listaTriplo.Count == 2)
                        {

                            comparacion = true;


                        }


                        if (token == "PR11")
                        {
                            secuenciaVerdadera = true;

                        }
                        if (secuenciaVerdadera == true)
                        {
                            if (secuenciaVerdadera == true && token != "PR19")
                            {
                                if (token.StartsWith("IDE"))
                                {
                                    TriplosVerdaderos();
                                }
                                else if (cadena == true)
                                {
                                    TriploVerdadero miObjetoTriplo = new TriploVerdadero();
                                    miObjetoTriplo.DatoObjeto = lenguaje[1];
                                    miObjetoTriplo.Operador = "PR10";
                                    listaTriploVerdadero.Add(miObjetoTriplo);
                                    cadena = false;
                                }
                                else if (token == "PR10")
                                {
                                    cadena = true;
                                }


                            }
                        }

                        if (token == "PR17")
                        {
                            secuenciaVerdadera = false;
                            secuenciaFalsa = true;

                        }
                        if (secuenciaFalsa)
                        {
                            if (secuenciaFalsa == true && token != "PR06")
                            {
                                if (token.StartsWith("IDE"))
                                {
                                    TriplosFalso();
                                }
                                else if (cadena == true)
                                {
                                    TriploFalso miObjetoTriplo = new TriploFalso();
                                    miObjetoTriplo.DatoObjeto = lenguaje[1];
                                    miObjetoTriplo.Operador = "PR10";
                                    listaTriploFalso.Add(miObjetoTriplo);
                                    cadena = false;
                                }
                                else if (token == "PR10")
                                {
                                    cadena = true;
                                }

                            }
                        }

                        if (token == "PR07")
                        {
                            Triplo miObjetoTriplo = new Triplo();
                            miObjetoTriplo.DatoObjeto = "ET";
                            miObjetoTriplo.DatoFuente = "TRTRUE";

                            listaTriplo.Add(miObjetoTriplo);

                            if (secuenciaFalsa == true)
                            {
                                Triplo tr = new Triplo();
                                tr.DatoObjeto = "ET";
                                tr.DatoFuente = "TRFALSE";

                                listaTriplo.Add(tr);
                            }
                        }

                        indexToken++;
                    }



                }
            }

            foreach (var triplo in listaTriplo)
            {

                dgvTriplos.Rows.Add(triplo.DatoObjeto, triplo.DatoFuente, triplo.Operador, "des");
                if (triplo.DatoFuente == "TRTRUE")
                {
                    foreach (var triploVerdadero in listaTriploVerdadero)
                    {
                        dgvTriploVerdadero.Visible = true;
                        dgvTriploVerdadero.Rows.Add(triploVerdadero.DatoObjeto, triploVerdadero.DatoFuente, triploVerdadero.Operador);
                    }
                }
                if (triplo.DatoFuente == "TRFALSE")
                {
                    foreach (var triploFalso in listaTriploFalso)
                    {
                        dgvTriploFalso.Visible = true;
                        dgvTriploFalso.Rows.Add(triploFalso.DatoObjeto, triploFalso.DatoFuente, triploFalso.Operador);
                    }
                }
            }
        }

        public void CicloMientras()
        {
            bool palabraReservada = false;
            bool inicioCondicion = false;
            bool finCondicion = false;
            int contadorTemporales = 1;
            string operador = "";
            bool secuenciaVerdadera = false;
            bool secuenciaFalsa = false;
            bool esSeleccion = false;
            bool cadena = false;
            bool comparacion = false;

            int numeroDeLineas = txtTokens.Lines.Length - 1; //Se obtiene el texto y se define el tamano 
            int numeroDeLineasLenguaje = txtLenguaje.Lines.Length;
            int indexToken = 0;
            for (int miPosicionLinea = 0; miPosicionLinea < numeroDeLineas; miPosicionLinea++)
            {
                primerLinea = txtTokens.Lines.Length > 0 ? txtTokens.Lines[miPosicionLinea] : string.Empty; //Se fragmenta en lineas del texto completo
                string[] tokens = primerLinea.Split(' '); //Dividimos la expresión en tokens por espacios en blanco
                string lineasLenguaje = txtLenguaje.Lines.Length > 0 ? txtLenguaje.Lines[miPosicionLinea] : string.Empty;
                string[] lenguaje = lineasLenguaje.Split(' ');





                foreach (var token in tokens)
                {
                    if (token == "PR14")
                    {
                        esSeleccion = true;
                    }
                    if (esSeleccion == true)
                    {
                        if (token == "CE05")
                        {
                            inicioCondicion = true;
                        }
                        if (inicioCondicion == true)
                        {
                            if (token.StartsWith("ENTE") || token.StartsWith("IDE"))
                            {
                                Triplo miObjetoTriplo = new Triplo();
                                miObjetoTriplo.DatoObjeto = "T" + contadorTemporales;
                                miObjetoTriplo.DatoFuente = lenguaje[indexToken];
                                miObjetoTriplo.Operador = "=";
                                listaTriplo.Add(miObjetoTriplo);
                                contadorTemporales++;
                            }
                        }
                        if (token == "CE03")
                        {
                            inicioCondicion = false;
                        }

                        if (token.StartsWith("OP"))
                        {
                            operador = token;
                        }
                        if (comparacion == true)
                        {
                            Triplo miObjetoTriplo = new Triplo();
                            miObjetoTriplo.DatoObjeto = "T1";
                            miObjetoTriplo.DatoFuente = "T2";
                            miObjetoTriplo.Operador = operador;
                            listaTriplo.Add(miObjetoTriplo);
                            comparacion = false;
                        }
                        if (inicioCondicion == false && listaTriplo.Count == 2)
                        {

                            comparacion = true;


                        }


                        if (token == "PR11")
                        {
                            secuenciaVerdadera = true;

                        }
                        if (secuenciaVerdadera == true)
                        {
                            if (secuenciaVerdadera == true && token != "PR19")
                            {
                                if (token.StartsWith("IDE"))
                                {
                                    TriplosVerdaderos();
                                }
                                else if (cadena == true)
                                {
                                    TriploVerdadero miObjetoTriplo = new TriploVerdadero();
                                    miObjetoTriplo.DatoObjeto = lenguaje[1];
                                    miObjetoTriplo.Operador = "PR10";
                                    listaTriploVerdadero.Add(miObjetoTriplo);
                                    cadena = false;
                                }
                                else if (token == "PR10")
                                {
                                    cadena = true;
                                }


                            }
                        }

                        //if (token == "PR17")
                        //{
                        //    secuenciaVerdadera = false;
                        //    secuenciaFalsa = true;

                        //}
                        //if (secuenciaFalsa)
                        //{
                        //    if (secuenciaFalsa == true && token != "PR06")
                        //    {
                        //        if (token.StartsWith("IDE"))
                        //        {
                        //            Ejecucion();
                        //        }
                        //        else if (cadena == true)
                        //        {
                        //            TriploFalso miObjetoTriplo = new TriploFalso();
                        //            miObjetoTriplo.DatoObjeto = lenguaje[1];
                        //            miObjetoTriplo.Operador = "PR10";
                        //            listaTriploFalso.Add(miObjetoTriplo);
                        //            cadena = false;
                        //        }
                        //        else if (token == "PR10")
                        //        {
                        //            cadena = true;
                        //        }

                        //    }
                        //}

                        if (token == "PR07")
                        {
                            Triplo miObjetoTriplo = new Triplo();
                            miObjetoTriplo.DatoObjeto = "ET";
                            miObjetoTriplo.DatoFuente = "TRTRUE";

                            listaTriplo.Add(miObjetoTriplo);

                            if (secuenciaFalsa == true)
                            {
                                Triplo tr = new Triplo();
                                tr.DatoObjeto = "ET";
                                tr.DatoFuente = "TRFALSE";

                                listaTriplo.Add(tr);
                            }
                        }

                        indexToken++;
                    }

                }




            }

            foreach (var triplo in listaTriplo)
            {

                dgvTriplos.Rows.Add(triplo.DatoObjeto, triplo.DatoFuente, triplo.Operador, "des");
                if (triplo.DatoFuente == "TRTRUE")
                {
                    foreach (var triploVerdadero in listaTriploVerdadero)
                    {
                        dgvTriploVerdadero.Visible = true;
                        dgvTriploVerdadero.Rows.Add(triploVerdadero.DatoObjeto, triploVerdadero.DatoFuente, triploVerdadero.Operador);
                    }
                }
                if (triplo.DatoFuente == "TRFALSE")
                {
                    foreach (var triploFalso in listaTriploFalso)
                    {
                        dgvTriploFalso.Visible = true;
                        dgvTriploFalso.Rows.Add(triploFalso.DatoObjeto, triploFalso.DatoFuente, triploFalso.Operador);
                    }
                }
            }

        }

        public void TriplosVerdaderos() //Proceso de triplos - Identifica Dato Objeto, Dato Fuente, Operador y una descripcion de la expresion
        {
            int numeroDeLineas = txtLenguaje.Lines.Length;
            // se genera un objeto nuevo de la clase Triplo
            for (int miPosicionLinea = 0; miPosicionLinea < numeroDeLineas; miPosicionLinea++)
            {

                primerLinea = txtLenguaje.Lines.Length > 0 ? txtLenguaje.Lines[miPosicionLinea] : string.Empty;
                string[] tokens = primerLinea.Split(' '); // Dividimos la expresión en tokens por espacios en blanco
                if (EsExpresion(primerLinea))
                {
                    int numeroDeFilas = dgvVariables.Rows.Count; //Cuenta el numero de filas para saber en que fila se ira imprimiendo los mensajes de ayuda  
                    //MessageBox.Show("Esta es la expresion aritmetica a resolver: "+inputExpression); //Imprime en pantalla la expresion aritmetica
                    inputExpression = primerLinea.ToString();
                    string[] elemento = inputExpression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);// Dividir la expresión en tokens   
                    //Asignación inicial - Asigna el valor cuando es =, por ejemplo: Asigna el valor de '{elemento[2]}' a una variable temporal (t1)
                    asignacionInicial = string.Format(resultadoEsperado, "T1", elemento[2], "=", $"Asigna el valor de '{elemento[2]}' a una variable temporal (t1)");
                    //Imprime la asignacion inicial para saber cual es la primera, se guarda para saber posteriormente si es inicial y si no lo es guardar
                    //en la variable que se asigna el resultado
                    triploVerdadero = new TriploVerdadero();
                    triploVerdadero.DatoObjeto = "T1";
                    triploVerdadero.DatoFuente = elemento[2];
                    triploVerdadero.Operador = "=";
                    listaTriploVerdadero.Add(triploVerdadero);
                    //dgvTriplos.Rows.Add("T1", elemento[2], "=", $"Asigna el valor de '{elemento[2]}' a una variable temporal (t1)");
                    logicLines.Add(asignacionInicial);
                    triploVerdadero = new TriploVerdadero();
                    for (int i = 3; i < elemento.Length; i += 2) //Procesar las operaciones
                    {
                        operador = elemento[i]; //almancena temporalmente el operador 
                        operando = elemento[i + 1]; //Almacena temporalmente  el operando para usos de iteracion
                        operacionDescripcion = $"Se {ObtenerDescripcionOperacion(operador)} el valor de '{operando}' a la variable temporal (T1)";
                        //valor que se mando a LogicLines que contiene operando, operador, operacion descripcion
                        operacion1 = string.Format(resultadoEsperado, "T1", operando, operador, operacionDescripcion);
                        triploVerdadero.DatoObjeto = "T1";
                        triploVerdadero.DatoFuente = operando;
                        triploVerdadero.Operador = operador;
                        //dgvTriplos.Rows.Add("T1", operando, operador, operacionDescripcion);//Agrega a la tabla la informacion recabada de t1, operador, operacion descripcion
                        logicLines.Add(operacion1);
                    }
                    listaTriploVerdadero.Add(triploVerdadero);
                    triploVerdadero = new TriploVerdadero();
                    triploVerdadero.DatoObjeto = elemento[0];
                    triploVerdadero.DatoFuente = "T1";
                    triploVerdadero.Operador = "=";
                    listaTriploVerdadero.Add(triploVerdadero);
                    //Asignación final -  variable que almacena la cadena y concatenacionn de resultadoEsperado y elemento, Se asigna la variable temporal al identificador final
                    asignacionFinal = string.Format(resultadoEsperado, elemento[0], "T1", "=", "Se asigna la variable temporal al identificador final");
                    //Mensaje de pantalla que ayuda a imprimir asignacion final                                                                                                                       
                    //dgvTriplos.Rows.Add(elemento[0], "T1", "=", "Se asigna la variable temporal al identificador final");
                    logicLines.Add(asignacionFinal); //Imprimir la lógica generada de linea por linea de triplos por si se ocupa a futuro
                }
            }
        }

        public void TriplosFalso() //Proceso de triplos - Identifica Dato Objeto, Dato Fuente, Operador y una descripcion de la expresion
        {
            int numeroDeLineas = txtLenguaje.Lines.Length;
            // se genera un objeto nuevo de la clase Triplo
            for (int miPosicionLinea = 0; miPosicionLinea < numeroDeLineas; miPosicionLinea++)
            {

                primerLinea = txtLenguaje.Lines.Length > 0 ? txtLenguaje.Lines[miPosicionLinea] : string.Empty;
                string[] tokens = primerLinea.Split(' '); // Dividimos la expresión en tokens por espacios en blanco
                if (EsExpresion(primerLinea))
                {
                    int numeroDeFilas = dgvVariables.Rows.Count; //Cuenta el numero de filas para saber en que fila se ira imprimiendo los mensajes de ayuda  
                    //MessageBox.Show("Esta es la expresion aritmetica a resolver: "+inputExpression); //Imprime en pantalla la expresion aritmetica
                    inputExpression = primerLinea.ToString();
                    string[] elemento = inputExpression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);// Dividir la expresión en tokens   
                    //Asignación inicial - Asigna el valor cuando es =, por ejemplo: Asigna el valor de '{elemento[2]}' a una variable temporal (t1)
                    asignacionInicial = string.Format(resultadoEsperado, "T1", elemento[2], "=", $"Asigna el valor de '{elemento[2]}' a una variable temporal (t1)");
                    //Imprime la asignacion inicial para saber cual es la primera, se guarda para saber posteriormente si es inicial y si no lo es guardar
                    //en la variable que se asigna el resultado
                    triploFalso = new TriploFalso();
                    triploFalso.DatoObjeto = "T1";
                    triploFalso.DatoFuente = elemento[2];
                    triploFalso.Operador = "=";
                    listaTriploFalso.Add(triploFalso);
                    //dgvTriplos.Rows.Add("T1", elemento[2], "=", $"Asigna el valor de '{elemento[2]}' a una variable temporal (t1)");
                    logicLines.Add(asignacionInicial);
                    triploFalso = new TriploFalso();
                    for (int i = 3; i < elemento.Length; i += 2) //Procesar las operaciones
                    {
                        operador = elemento[i]; //almancena temporalmente el operador 
                        operando = elemento[i + 1]; //Almacena temporalmente  el operando para usos de iteracion
                        operacionDescripcion = $"Se {ObtenerDescripcionOperacion(operador)} el valor de '{operando}' a la variable temporal (T1)";
                        //valor que se mando a LogicLines que contiene operando, operador, operacion descripcion
                        operacion1 = string.Format(resultadoEsperado, "T1", operando, operador, operacionDescripcion);
                        triploFalso.DatoObjeto = "T1";
                        triploFalso.DatoFuente = operando;
                        triploFalso.Operador = operador;
                        //dgvTriplos.Rows.Add("T1", operando, operador, operacionDescripcion);//Agrega a la tabla la informacion recabada de t1, operador, operacion descripcion
                        logicLines.Add(operacion1);
                    }
                    listaTriploFalso.Add(triploFalso);
                    triploFalso = new TriploFalso();
                    triploFalso.DatoObjeto = elemento[0];
                    triploFalso.DatoFuente = "T1";
                    triploFalso.Operador = "=";
                    listaTriploFalso.Add(triploFalso);
                    //Asignación final -  variable que almacena la cadena y concatenacionn de resultadoEsperado y elemento, Se asigna la variable temporal al identificador final
                    asignacionFinal = string.Format(resultadoEsperado, elemento[0], "T1", "=", "Se asigna la variable temporal al identificador final");
                    //Mensaje de pantalla que ayuda a imprimir asignacion final                                                                                                                       
                    //dgvTriplos.Rows.Add(elemento[0], "T1", "=", "Se asigna la variable temporal al identificador final");
                    logicLines.Add(asignacionFinal); //Imprimir la lógica generada de linea por linea de triplos por si se ocupa a futuro
                }
            }
        }
    }
}
