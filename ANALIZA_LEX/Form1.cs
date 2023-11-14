﻿using System;
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
namespace ANALIZA_LEX
{
    public partial class Form1 : Form
    {
        private Nodo raiz;
        private Arbol arbol;
        public Form1()
        {
            InitializeComponent();
            arbol = new Arbol();
            unIdentificador = new Identificador();           
        }
        private void Clear(){ /*Hugo Ramos - Metodo que limpia variables y dgv */
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
            string tk="";           
            for (int i = 0; i < dtgMatriz.ColumnCount; i++) { /*RECORRE TODAS LAS COLUMNAS DE LA TABLA */                          
                if (dtgMatriz.Columns[i].HeaderText == "FDC") { /*SI ENCUENTRA LA COLUMNA QUE SE LLAMA TOKEN PROCEDE A HACER LAS SIG INSTRUCCIONES */                  
                    tk=dtgMatriz.Rows[Estado].Cells[i+1].Value.ToString();     //TOMA EL VALOR DEL TOKEN DEPENDIENDO DEL ESTADO Y LOS ALMACENA EN LA VARIABLE Y LOS VA SUMANDO PARA FORMAR LA CADENA DE TOKENS 
                    //PINTA DE COLOR AZUL LA CELDA DEL TOKEN 
                    dtgMatriz.Rows[Estado].Cells[i+1].Style.BackColor = Color.Blue; 
                }
            }
            if (string.IsNullOrWhiteSpace(tk)) {
                MessageBox.Show("No se encontró un token para el estado especificado.");
            }                    
            return tk.TrimStart();            
        }
        public void DescomponerCadenas()
        {
            try{               
                char[] del1 = { '\n' };
                LenguajeNat = txtLenguaje.Text;
                txtTokens.Text = "";               
                string strPalabras = LenguajeNat; //TOMA LA ORACION DE CADA LINEA EN EL TEXTBOX INGRESADA
                string[] Lenguajes = strPalabras.Split(del1);//METE LAS ORACIONES EN UN ARREGLO 
                string evaluacion = "";
                int linea = 0;              
                foreach (string palabras in Lenguajes) {
                    linea++;                  
                    char[] del = { ' ' };//AHORA TOMA CADA PALABRA DE CADA ORACION 
                    char chMuestra = ' ';                 
                    string strPalabra = palabras;    //TOMA LA PALABRA DE LA ORACION  INGRESADA               
                    string[] Lenguaje = strPalabra.Split(del);     //METE LAS PALABRAS EN UN ARREGLO                                           
                    evaluacion = EvaluarExpresion(palabras);//INVOCA EL METODO DE LA PILA PARA VERIFICAR QUE ESTEN CORRECTAMENTE EL BALANCE DE LOS CORCHETES Y SIGNOS DE ?
                    if (evaluacion == "Expresion incorrecta fsi") {
                        dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: Falta signo de inicio");
                    }
                    else
                    if (evaluacion == "Expresion incorrecta fsf") {
                        dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: Falta signo de final");
                    }
                    if (evaluacion == "Expresion incorrecta fci") {
                        dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: Falta corchete de inicio");
                    }
                    else 
                    if (evaluacion == "Expresion incorrecta fcf")
                    {
                        dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: Falta corchete de final");
                    }
                    foreach (string palabra in Lenguaje) {/*RECORRE EL ARREGLO DE LAS PALABRAS*/                                            
                        foreach (string valor in Lenguaje) {                          
                            strValorIde = valor; // toma el ultimo valor de la linea
                            if (valor == "ent" || valor == "flo" || valor == "boo" || valor == "car" || valor == "txt")
                            {
                                tipoDato = valor; // contiene el tipo de dato
                            }
                        }
                        string strIDE = palabra;
                       
                        if (strIDE.StartsWith("_")) { /* Sirve para dectectar los identificadores ya que inician con un _ */                             
                            yaExiste = false;
                            foreach (string elemento in tablaSimbolo){
                                if (strIDE == elemento){ yaExiste = true;                                    
                                    break; // Ya encontramos una coincidencia, no es necesario seguir buscando
                                }
                            }
                            if (!yaExiste){ /* si no existe el identificador anteriormente se agrega al objeto*/
                                unIdentificador.Nombre = strIDE;
                                unIdentificador.Valor = strValorIde;
                            }
                        }
                        switch (palabra)
                        {
                            case "ent": token = "PR05"; break;
                            case "flo": token = "PR08"; break;
                            case "txt": token = "PR018"; break;
                            case "boo": token = "PR02"; break;
                        }
                        switch (token) {
                            case "PR018":
                                int banderainicio = 0;
                                for (int i = 0; i < strValorIde.Length; i++)
                                {
                                    if (strValorIde[i] == '"') {
                                        banderainicio++;
                                    }
                                }
                                if (banderainicio != 2) {
                                    dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: Cadena Invalida.");
                                }
                                break;
                            case "PR05":
                                if (char.IsDigit(strValorIde[0]) || strValorIde[0] == '+' || strValorIde[0] == '-'){
                                    if (char.IsDigit(strValorIde[0])) {
                                        for (int i = 0; i < strValorIde.Length; i++) {
                                            // MessageBox.Show($"Empieza en : {i}");  // MessageBox.Show($"{strValorIde[i]}");
                                            char miCaracter2 = strValorIde[i];
                                            int valorAscii2 = (int)miCaracter2;
                                            if (valorAscii2 >= 48 && valorAscii2 <= 57) //valores numericos en ascii para el resto
                                            {
                                                //MessageBox.Show($"{strValorIde[i]} - si pertenece a un numero");                                                
                                            }
                                            else
                                            {
                                                //  MessageBox.Show($"{strValorIde[i]} - no pertenece a un numero");
                                                dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), $"{strValorIde[i]} - no pertenece a un numero");
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
                                if (strValorIde[0] != '+' && strValorIde[0] != '-' && !char.IsDigit(strValorIde[0])) {
                                    dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: El primer carácter debe ser '+' o '-' o un numero");
                                }
                                else {
                                    bool caracteresNumericos = true;
                                    for (int i = 1; i < strValorIde.Length; i++) {
                                        if (!char.IsDigit(strValorIde[i]) && strValorIde[i] != '.')                                        {
                                            caracteresNumericos = false;
                                            break; // Salir del bucle si se encuentra un carácter no numérico
                                        }
                                    }
                                    if (caracteresNumericos == true) {
                                        //MessageBox.Show("El formato es válido: " + strValorIde);
                                    }
                                    else {
                                        dgvErroresSemanticos.Rows.Add(BuscarLineaError(strValorIde), "Error: Se encontraron valores no numéricos");
                                    }
                                }
                                break;
                            case "PR02":
                                if (strValorIde[0] == 'v' && strValorIde[1] == 'e' && strValorIde[2] == 'r' && strValorIde[3] == 'd') {
                                    MessageBox.Show("Es correcto");
                                }
                                else {
                                    if (strValorIde[0] == 'f' && strValorIde[1] == 'a' && strValorIde[2] == 'l') {
                                        MessageBox.Show("Es correcto");
                                    }else {
                                        MessageBox.Show("Se esperaba un fal o un verd");
                                    }
                                }
                                break;
                        }
                        for (int i = 0; i < strIDE.Length; i++) { /* CICLO QUE EXTRAE CADA LETRA DE CADA PALABRA */                          
                            chMuestra = strIDE[i];            //Validar tipo de dato string que este completo                                                              
                            ValidarIDE(intEstado, char.ToUpper(chMuestra));  //INVOCA UN METODO QUE TIPO BOOLEANO EL VERIFICA SI EL IDE ES ACEPTABLE
                        }                           
                        if (intEstado != 0) //MIENTRAS EL ESTADO SEA DIFERENTE DE 0 BUSCARA EL TOKEN AL QUE CORRESPONDE INVOCANDO UN METODO 
                        {                          
                            token = BuscarToken(intEstado);  //BUSCA EL TOKEN DE CADA PALABRA DESPUES DE VALIDAR EL IDE
                            //VA CONCATENANDO CADA TOKEN
                            //////////////////////////  es para cuando detecta que es un identificador  ////////////////////////////////////////////////////////
                            //si detecta que es un identificador le agrega un valor a un objeto de la clase Identificador
                            if (token.Trim() == "IDE"  ){
                                //código anterior
                                /*contador++;
                                unIdentificador.Numero = contador;                                
                                unIdentificador.TipoDato = tipoDato;  */
                                foreach (var identificador in unaLista) {
                                    if (identificador.Nombre == strIDE) {
                                        yaExiste = true;
                                        token = identificador.strIdentificador;
                                        identificador.Valor = strValorIde;
                                    }
                                }
                                if (!yaExiste) {
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
                            txtTokens.Text = txtTokens.Text +" "+token.Trim();                             
                            intEstado = 0;//INICIALIZA DE NUEVO LA VARIABLE
                        }
                        else { /*  si no encuenta el caracter que se ingreso mandara un mensaje diciendo ese mensaje */                           
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
            } catch (Exception){MessageBox.Show("Por favor ingresa una cadena", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);}
        }      
        int numeroDeLinea = -1;
        private string BuscarLineaError(string valor) { /* Buscar "ES03" en cada línea. */
            string[] lineas = txtLenguaje.Text.Split('\n');// Dividir el texto del TextBox en líneas utilizando '\n' como separador.
            for (int i = 0; i < lineas.Length; i++) { /* // Buscar "ES03" en cada línea.*/
                if (lineas[i].Contains(valor)) {
                    numeroDeLinea = i + 1; // Se suma 1 para obtener el número de línea correcto.
                    break; // Detener la búsqueda una vez que se encuentre la primera ocurrencia.
                }
            }
            return numeroDeLinea.ToString();         
        }
        private void btnValidar_Click(object sender, EventArgs e)
        {
            unaLista.Clear();
            try {              
                DescomponerCadenas();  //INVOCA EL METODO
                LenguajeNat ="";
                txtTokens.ReadOnly= true;                
                string textoOriginal = txtTokens.Text;//metodo para quitar la sangria 
                string[] lineas = textoOriginal.Split('\n'); // Dividir el texto en líneas
                for (int i = 0; i < lineas.Length; i++)                {
                    lineas[i] = lineas[i].TrimStart(); // Quitar espacios al inicio de cada línea
                }
                string textoModificado = string.Join("\n", lineas); // Unir las líneas modificadas
                txtTokens.Text = textoModificado;
                dgvIden.Rows.Clear();//mostrar la información en la tabla de simbolos
                foreach (Identificador miIdentificador in unaLista)                {                   
                    dgvIden.Rows.Add(miIdentificador.Numero, "IDE" + miIdentificador.Numero, miIdentificador.Nombre, miIdentificador.TipoDato, miIdentificador.Valor);
                }
                dgvErroresLexicos.Rows.Clear();
                foreach(Error error in errores){ dgvErroresLexicos.Rows.Add(error.NomError, error.MostrarCaracteristica()); }
                for (int i = 1; i < contadorLineas; i++) { txtLineasLexico.AppendText(i.ToString()); }          
            }catch (Exception) { MessageBox.Show("Por favor ingresa una cadena", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
        public bool ValidarIDE(int Estado, char letra) {                 
            bool seEncontroCaracter = false; // Variable para indicar si se encontró el carácter        
            for (int i = 1; i < dtgMatriz.ColumnCount - 2; i++) {   /*RECORRE LAS COLUMAS DE LA TABLA OSEA EL ALFABETO-NUMEROS-OTROS*/ 
                //BUSCA POR LAS COLUMNAS  DESDE LA POSICION 1 OSEA DESDE LA lETRA -A- EN LA COLUMNA (0) QUE SON LOS NOMBRES DE LA TABLA 
                if (dtgMatriz.Columns[i].HeaderText[0] == letra ) { /*    //SI EL CARACTER  ES IGUAL A LA LETRA ENVIADA */                
                    if (dtgMatriz.Rows[Estado].Cells[i].Value.ToString() == "108")
                    { /*  // EN CASO DE QUE NO SEA ALGO VALIDO VERIFICA QUE SU ESTADO SEA  */
                        this.intEstado = int.Parse(dtgMatriz.Rows[Estado].Cells[i].Value.ToString());
                    }else  { //SI NO LO ES PINTA DE AZUL LA CELDA DEL CARACTER

                        dtgMatriz.Rows[Estado].Cells[i].Style.BackColor = Color.Blue;// Si encuentra el Letra, obtiene el estado correspondiente y retorna true y pinta la letra correspondiente 
                        this.intEstado = int.Parse(dtgMatriz.Rows[Estado].Cells[i].Value.ToString());
                    }                    
                    seEncontroCaracter = true;//DEVUELVE UN TRUE REPRESENTADO QUE SE VALIDO EL IDE 
                    break;
                }
            }  return seEncontroCaracter;//DEVUELVE UN FALSE  REPRESENTADO QUE SE NO VALIDO EL IDE 
        }
        private void Form1_Load(object sender, EventArgs e) {
            MostrarMatriz();
            txtTokens.ReadOnly= true;
        }
        public void ChecarEspacios() {
            string textp=txtSintactico.Text;
            textp= textp.TrimEnd(); 
        }
        private void button1_Click(object sender, EventArgs e) {
            Application.Restart();
        }  
        private void txtTokens_TextChanged(object sender, EventArgs e) {
            unIdentificador = new Identificador();
        }
        private void btnValidarSint_Click(object sender, EventArgs e) {
            try  {
                string texto = txtTokens.Text;
                EnviarLineasHastaDelimitador(texto);              
            } catch (Exception ex) {MessageBox.Show(ex.Message); }
        }     
        private void picMinimizar_Click(object sender, EventArgs e) {
            this.WindowState = FormWindowState.Minimized;
        }
        //METODO PARA CREAR EL ARCHIVO RECIBE COMO PARAMETRO LA RUTA Y EL NOMBRE DEL ARCHIVO
        public void crear(string nombre)
        {
            //PROPIEDAD QUE VA A ESCRIBIR LO QUEBTENEMOS EN EL TRIPLO 
            StreamWriter sw = new StreamWriter(nombre);
            //AQUI ESCRIBE LAS LINEAS POR DEAFULT .DATA Y .MODEL STACK
            sw.WriteLine(".data \n.model stack");
            //DESPUES AGREGA LO QUE UNO ESCRIBA EN EL RICHTEXBOX
            sw.WriteLine("\n" + rch.Text + "\n");
            sw.WriteLine(".MOV AH, 4CH\n.INT 21H");
            sw.Close();


        }
        
        private void btnDocumento_Click(object sender, EventArgs e)
        {
            SaveFileDialog sapF = new SaveFileDialog();
            sapF.Filter = "Text Documen|*.txt";
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
            foreach (string linea in lineas) {
                bool esLineaValida = VerificarAsignacion(linea);
                string mensaje = esLineaValida ? " --> VALIDA" : " --> NO VALIDA";
                txtSintactico.AppendText(linea + mensaje + "\r\n");
            }
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
           string textoARch = ".data \n.model stack" + Environment.NewLine;
            foreach (var triplo in listaTriplo)
            {
                switch (triplo.Operador)
                {
                    case "=":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += " MOV AX," + triplo.DatoFuente + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += " MOV " + triplo.DatoObjeto + ",AX" + Environment.NewLine;
                        }
                        break;
                    case "add" :
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += " ADD AX," + triplo.DatoFuente + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += " ADD " + triplo.DatoObjeto + ",AX" + Environment.NewLine;
                        }
                        break;
                    case "dec":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "SUB AX," + triplo.DatoFuente + Environment.NewLine;
                        }
                        else
                        {
                            textoARch +=  "SUB " + triplo.DatoObjeto + ", AX" + Environment.NewLine;
                        }
                        break;
                    case "div":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "DIV AX," + triplo.DatoFuente + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "DIV " + triplo.DatoObjeto + ",AX" + Environment.NewLine;
                        }
                        break;
                    case "mul":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "MUL AX," + triplo.DatoFuente + Environment.NewLine;
                        }
                        else
                        {
                            textoARch +=  "MUL " + triplo.DatoObjeto + ",AX" + Environment.NewLine;
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
                            textoARch +=  "CMP " + triplo.DatoObjeto + ", AX" + Environment.NewLine  + " JL <InstruccionMenor>" + Environment.NewLine;
                        }
                        break;
                    case ">":
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "CMP AX," + triplo.DatoFuente + Environment.NewLine + " JG <InstruccionMayor>" + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "CMP " + triplo.DatoObjeto + ",AX" + Environment.NewLine  + " JG <InstruccionMayor>" + Environment.NewLine;
                        }
                        break;
                    case "<>": // DIFERENTE
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "CMP AX, " + triplo.DatoFuente + Environment.NewLine  + "JNE <InstruccionDiferente>" + Environment.NewLine;
                        }
                        else
                        {
                            textoARch += "CMP " + triplo.DatoObjeto + ",AX" + Environment.NewLine + "JNE <InstruccionDiferente>" + Environment.NewLine;
                        }
                        break;
                    case "<=": // MENOR O IGUAL
                        if (triplo.DatoObjeto == "T1")
                        {
                            textoARch += "CMP AX," + triplo.DatoFuente + Environment.NewLine  + "JG <InstruccionMayorOIgual>" + Environment.NewLine;
                        }
                        else
                        {
                            textoARch +="CMP " + triplo.DatoObjeto + ",AX" + Environment.NewLine  + "JG <InstruccionMayorOIgual>" + Environment.NewLine;
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
            textoARch += ".MOV AH, 4CH\n.INT 21H";
            rch.Text = textoARch;
        }

        private void rch_TextChanged(object sender, EventArgs e)
        {

        }

        private bool VerificarAsignacion(string linea) {           
            string patronOperacion1 = @"CE10\sENTE\s(?:OPA[1-5])\sENTE\sCE11"; //modificaciones para corchetes opraciones
            bool matchOperacion1 = Regex.IsMatch(linea, patronOperacion1, RegexOptions.IgnoreCase);
            string patronOperacion2 = @"CE10\sENTE\s(?:OPR[1-5])\sCE10\sENTE\sOPA2\sENTE\sCE11\sCE11\s\[\d+\]\s\[\d+\]$";//@"CE10\sENTE\s(?:OPR[1-5])\sCE10\sENTE\sOPA2\sENTE\sCE11\sCE11\s\[\d+\]\s\[\d+\]$";//CE10\sENTE\s(?:OPR[1-5])\sCE10\sENTE\sOPA2\sENTE\sCE11\sCE11";                                                                                                             // string patronOperacion2 = @"CE10\s(?:ENTE|ES02|ES03)$\s(?:OPR[1-5])\sCE10\sENTE\s(?:OPA[1-5])\sENTE\sCE11\sCE11";
            bool matchOperacion2 = Regex.IsMatch(linea, patronOperacion2, RegexOptions.IgnoreCase);
            string patronEstructura1 = @"^IDE[12N]\sPR05\sCE06\s(?:ENTE|ES02|ES03)$"; 
            // Expresión regular para la estructura 1
            string patronEstructura11 = @"^IDE[12N]\sPR08\sCE06\s(?:ES04|ES05|FLOT)$";
            string patronEstructura12 = @"^IDE[12N]\sPR18\sCE06\sES07$";
            string patronEstructura13 = @"^IDE[12N]\sPR18\sCE06\s(?:PR06|PR19)$";
            // Expresión regular para la estructura 2
            string patronEstructura2 = @"^PR14$|^CE05\sIDE[12N]\sOPR[1-5]\sENTE\sCE03$|^PR11$|^PR10\sIDE[12N]\sCE06\sIDE[12N]\sOPA[1-5]\sENTE$|^PR07$";// @"^PR14$|^CE05\sIDE[12N]\s(?:OPR[1-5])\sENTE\sCE03$|^PR11$|^PR10\sIDE[12N]\sCE06\sIDE[12N]\s(?:OPA[1-5])\sENTE$|^PR07$";
            //Expresion regular para la estrucrura 3
            // string patronEstructura3 = @"^PR16$|^PR11$|^PR10\s(?:ES07|IDE)$|^PR07$|^CE05\sIDE\s(?:(?:OPA[1-5])|(?:OPR[1-5]))\sENTE\s(?:(?:OPR[1-5])|(?:OPL[1-3]))\sIDE\s(?:(?:OPA[1-5])|(?:OPR[1-5]))\sENTE\sCE03$";
            string patronEstructura3 = @"^PR16$|^PR11$|^PR10\s(?:ES07|sIDE[12N])$|^PR07$|^CE05\sIDE[12N]\s(?:(?:OPA[1-5])|(?:OPR[1-5]))\sFLOT\s(?:(?:OPR[1-5])|(?:OPL[1-3]))\sIDE[12N]\s(?:(?:OPA[1-5])|(?:OPR[1-5]))\sFLOT\sCE03$|^PR19$|^PR17$|^PR06$";
            //Expresion regular para la estrucrura 4
            string patronEstructura4 = @"^PR15$|^CE05\sIDE[12N]\sPR05\sCE06\sENTE\sCE02\sIDE[12N]\s(?:OPR[1-5])\sENTE\sCE02\sIDE[12]\sCE06\sIDE[12N]\sOPA1\sENTE\sCE03$|^PR11$|^PR10\s(?:ES07|sIDE[12N])$|^PR07$";
            //Expresion regular para la estrucrura 5
            string patronEstructura5 = @"^PR09$|^PR11$|^IDE[12N]\sCE06\sIDE[12N]\sOPA1\sENTE$|^PR10\sES07$|^PR14$|^CE05\sIDE[12N]\s(?:OPR[1-5])\sENTE\sCE03$|^PR07$";
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
        private void btnLimpiar_Click(object sender, EventArgs e)        {
            Clear(); //ejecuta metodo que limpia tableros, variables globales
        }
        bool primerCambio = true;      
        private void txtLenguaje_TextChanged(object sender, EventArgs e) { /*se agregan las lineas de codigo cada vez que se detecta el salto de linea */
            string codigoEntrada = txtLenguaje.Text;  // Obtén el texto del primer TextBox
            contadorLineas = 1; // Divide el texto en líneas usando el carácter de salto de línea ('\n')
            string[] lineasCodigo = codigoEntrada.Split('\n');          
            txtLineasLenguaje.Clear();  // Borra el contenido actual del segundo TextBox                                       
            int numeroDeLineas = lineasCodigo.Length;// Agrega cada línea al segundo TextBox     
            StringBuilder codigoConNumerosDeLinea = new StringBuilder();// Actualizar el contador de líneas y mostrarlo como números de línea de código.
            if (primerCambio) {
                codigoConNumerosDeLinea.Append("\n");
                primerCambio = false;
            }
            for (int i = 0; i < numeroDeLineas; i++) {
                codigoConNumerosDeLinea.Append(contadorLineas + i + " " + "\n");
            }           
            txtLineasLenguaje.Text = codigoConNumerosDeLinea.ToString(); // Mostrar el código con números de línea en el TextBox.                    
            contadorLineas += numeroDeLineas; // Actualizar el contador de líneas para la próxima vez.
        }
        private void btnValidar_Click_1(object sender, EventArgs e) {           
            unaLista.Clear();
            txtLineasLexico.Text = "";
            listaTriplo.Clear();
            dgvTriplos.Rows.Clear();
            contador = 0;
            try {               
                DescomponerCadenas(); //INVOCA EL METODO
                LenguajeNat = "";
                txtTokens.ReadOnly = true;               
                string textoOriginal = txtTokens.Text; //metodo para quitar la sangria 
                string[] lineas = textoOriginal.Split('\n'); // Dividir el texto en líneas
                for (int i = 0; i < lineas.Length; i++) {
                    lineas[i] = lineas[i].TrimStart(); // Quitar espacios al inicio de cada línea
                }
                string textoModificado = string.Join("\n", lineas); // Unir las líneas modificadas
                txtTokens.Text = textoModificado;                
                dgvIden.Rows.Clear();//mostrar la información en la tabla de simbolos
                foreach (Identificador miIdentificador in unaLista) {
                    dgvIden.Rows.Add(miIdentificador.Numero, miIdentificador.strIdentificador, miIdentificador.Nombre, miIdentificador.TipoDato, miIdentificador.Valor);
                }
                dgvErroresLexicos.Rows.Clear();
                foreach (Error error in errores) {
                    dgvErroresLexicos.Rows.Add(error.NomError, error.MostrarCaracteristica());
                }
                for (int i = 1; i < contadorLineas; i++) {
                    txtLineasLexico.AppendText($"\n{i.ToString()}  ");
                }                
            }
            catch (Exception) { MessageBox.Show("Por favor ingresa una cadena", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            Ejecucion();
        }
        private void btnValidarSint_Click_1(object sender, EventArgs e) {
            try{
                string texto = txtTokens.Text;
                EnviarLineasHastaDelimitador(texto);          
            }catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void picSalir_Click(object sender, EventArgs e) { Application.Exit(); }
        private string EvaluarExpresion(string expresion) { /* METODO QUE COMPRUEBA LOS signos¿? y corchetes[] */
            ClasePila pila = new ClasePila();
            string cadena = expresion;
            if (cadena.Contains("¿") || cadena.Contains("?")) {
                for (int i = 0; i < cadena.Length; i++) {
                    if (cadena.ElementAt(i) == '¿') {
                        pila.Insertar(cadena.ElementAt(i));
                    }else{
                        if (cadena.ElementAt(i) == '?') {
                            if (pila.Extraer() != '¿') {
                                return "Expresion incorrecta fsi";//falta signo inicio
                            }
                        }
                    }
                }
                if (pila.Vacia()) {
                    return "Expresion correcta";
                } else {
                    if (pila.Extraer() == '¿') {
                        return "Expresion incorrecta fsf";//falta signo final
                    } else {
                        return "Expresion incorrecta fsi";//falta signo inicio
                    }
                }
            }
            else {
                for (int i = 0; i < cadena.Length; i++) {
                    if (cadena.ElementAt(i) == '[') {
                        pila.Insertar(cadena.ElementAt(i));
                    }else{
                        if (cadena.ElementAt(i) == ']') {
                            if (pila.Extraer() != '[') {
                                return "Expresion incorrecta fci";//falta corchete inicio
                            }
                        }
                    }
                }
                if (pila.Vacia()) {
                    return "Expresion correcta";
                }else {
                    if (pila.Extraer() == '[') {
                        return "Expresion incorrecta fcf";//falta corchete final
                    } else {
                        return "Expresion incorrecta fci";//falta corchete inicio
                    }
                }
            }
        } 
        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            if (radPreorden.Checked) {
                arbol.InsertarEnCola(txtLenguaje.Text);
                raiz = arbol.CrearArbol();
                arbol.Limpiar();
                rtxt.Text =arbol.InsertarPre(raiz);            
            }
            if (radPostorden.Checked) {
                arbol.InsertarEnCola(txtLenguaje.Text);
                raiz = arbol.CrearArbol();
                arbol.Limpiar();
                rtxt.Text = arbol.InsertarPost(raiz);               
            }
            if (radInOrden.Checked) {
                arbol.InsertarEnCola(txtLenguaje.Text);
                raiz = arbol.CrearArbol();
                arbol.Limpiar();
                rtxt.Text = arbol.InsertarIn(raiz);      
            }
        }
        /*---------------*/
        public void metodoBusqueda() //Hugo Ramos - realiza la busqueda de variables en la tabla para poder hacer la nueva expresion con asignacion
        {
            int numeroDeLineas = txtLenguaje.Lines.Length;
            for (int miPosicionLinea = 0; miPosicionLinea < numeroDeLineas; miPosicionLinea++)
            {
                primerLinea = txtLenguaje.Lines.Length > 0 ? txtLenguaje.Lines[miPosicionLinea] : string.Empty;
                string[] tokens = primerLinea.Split(' '); // Dividimos la expresión en tokens por espacios en blanco
                if (EsExpresion(primerLinea)) //Hugo Ramos evalua la linea si es es expresion 
                {
                    int numeroDeFilas = dgvVariables.Rows.Count;
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        if (i == 0)//Compara la posicion de la palaraba(token) donde esta la linea en que se encuentra la expresion aritmetica 
                        {
                            nuevaExpresion = tokens[i];  // MessageBox.Show("Variable en donde se guardara el valor: "+nuevaExpresion); //Mensaje de pantalla que ayuda visualmente saber cual es la variable donde se guardara el valor de la expresion
                        }
                        if (i == 1)
                        {
                            nuevaExpresion = nuevaExpresion + " " + tokens[i];// MessageBox.Show("Identifica si despues de la variable donde se almacenara el valor si es = "+nuevaExpresion);
                        }
                        if (EsEntero(tokens[i]) || tokens[i] == "+" || tokens[i] == "-" || tokens[i] == "*") //Hugo Ramos - Verifica si es un entero y si la linea contiene algun operador, logicamente indica que sea una expresion y no otra instruccion
                        {
                            if (EsEntero(tokens[i]))
                            { //Hugo Ramos - Verifica si es un entero y si si concatena el valor 
                                nuevaExpresion = nuevaExpresion + " " + tokens[i]; //MessageBox.Show(nuevaExpresion); //Hugo Ramos - Hice este metodo que detecta que valor tiene la variable en la expresion e imprime el valor
                            }
                            else { nuevaExpresion = nuevaExpresion + " " + tokens[i];  /*MessageBox.Show(nuevaExpresion); Hugo Ramos - Nos ayuda a identificar el cual es el valor de la variable que se encuentra en la expresion aritmetica, nos ayudara en un fururo para realizar las operaciones y su postfijo */}
                        }
                        else
                        {
                            foreach (DataGridViewRow fila in dgvVariables.Rows)
                            { /*Hugo Ramos - Recorre las filas y columnas de datagridview */
                                object valorCelda = fila.Cells[1].Value;
                                if (valorCelda != null)
                                { /* Hugo Ramos - Como aveces los valores de las celdas son vacios - ayuda a que no tome en cuenta ese valor y solo los que si tienen */
                                    string variablesDeLaColumna = valorCelda.ToString(); //convierte el valor de la tabla en un string para poder manejarlo
                                    if (variablesDeLaColumna == tokens[i] && variablesDeLaColumna != tokens[0])
                                    {
                                        igualacionValorVariable = fila.Cells[3].Value.ToString(); //Hugo Ramos - Obtiene el valor de variable segun la fila y columna de variable
                                        nuevaExpresion = nuevaExpresion + " " + igualacionValorVariable;//MessageBox.Show(nuevaExpresion);//Hugo Ramos - Nos ayuda a identificar el cual es el valor de la variable que se encuentra en la expresion aritmetica, nos ayudara en un fururo para realizar las operaciones y su postfijo
                                    }
                                }
                            }

                        }
                    }
                }
            }
            MessageBox.Show($"Expresion con asignacion de valores: {nuevaExpresion}"); //Resultado final de la identificacion de la variables dentro de la expresion       
        }
        public void Triplos() //Hugo Ramos - Proceso de triplos - Identifica Dato Objeto, Dato Fuente, Operador y una descripcion de la expresion
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
                    inputExpression = primerLinea.ToString();  //MessageBox.Show("Esta es la expresion aritmetica a resolver: "+inputExpression); //Imprime en pantalla la expresion aritmetica
                    string[] elemento = inputExpression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);// Dividir la expresión en tokens   
                    asignacionInicial = string.Format(resultadoEsperado, "T1", elemento[2], "=", $"Asigna el valor de '{elemento[2]}' a una variable temporal (t1)");  //Hugo Ramos - Asignación inicial - Asigna el valor cuando es =, por ejemplo: Asigna el valor de '{elemento[2]}' a una variable temporal (t1)
                    //MessageBox.Show("asignacion inicial: " + asignacionInicial); Imprime la asignacion inicial para saber cual es la primera, se guarda para saber posteriormente si es inicial y si no lo es guardar en la variable que se asigna el resultado
                    triplo = new Triplo();
                    triplo.DatoObjeto = "T1";
                    triplo.DatoFuente = elemento[2];
                    triplo.Operador = "=";
                    listaTriplo.Add(triplo);
                    dgvTriplos.Rows.Add("T1", elemento[2], "=", $"Asigna el valor de '{elemento[2]}' a una variable temporal (t1)");
                    logicLines.Add(asignacionInicial);
                    triplo = new Triplo();
                    for (int i = 3; i < elemento.Length; i += 2) //Hugo Ramos - Procesar las operaciones
                    {
                        operador = elemento[i]; //Hugo Ramos -  almancena temporalmente el operador 
                        operando = elemento[i + 1]; //Hugo Ramos -  Almacena temporalmente  el operando para usos de iteracion
                        operacionDescripcion = $"Se {ObtenerDescripcionOperacion(operador)} el valor de '{operando}' a la variable temporal (T1)";
                        operacion1 = string.Format(resultadoEsperado, "T1", operando, operador, operacionDescripcion); //Hugo Ramos - valor que se mando a LogicLines que contiene operando, operador, operacion descripcion
                        triplo.DatoObjeto = "T1";
                        triplo.DatoFuente = operando;
                        triplo.Operador = operador;
                        dgvTriplos.Rows.Add("T1", operando, operador, operacionDescripcion);//Hugo Ramos -  Agrega ala tabla la informacion recabada de t1, operador, operacion descripcion
                        logicLines.Add(operacion1);
                    }
                    listaTriplo.Add(triplo);
                    triplo = new Triplo();
                    triplo.DatoObjeto = elemento[0];
                    triplo.DatoFuente = "T1";
                    triplo.Operador = "=";
                    listaTriplo.Add(triplo);
                    asignacionFinal = string.Format(resultadoEsperado, elemento[0], "T1", "=", "Se asigna la variable temporal al identificador final");// Hugo Ramos - Asignación final -  variable que almacena la cadena y concatenacionn de resultadoEsperado y elemento, Se asigna la variable temporal al identificador final
                    //MessageBox.Show("asignacion final: "+asignacionFinal);  //Hugo Ramos -  Mensaje de pantalla que ayuda a imprimir asignacion final                                                                                                                       
                    dgvTriplos.Rows.Add(elemento[0], "T1", "=", "Se asigna la variable temporal al identificador final");
                    logicLines.Add(asignacionFinal); //Hugo Ramos - Imprimir la lógica generada de linea por linea de triplos por si se ocupa a futuro
                    //foreach (var line in logicLines){MessageBox.Show(line);}
                }
            }
        }
        public void Ejecucion()
        {
            int numeroDeLineas = txtLenguaje.Lines.Length; //Hugo Ramos Zarate - Se obtiene el texto y se define el tamano 
            for (int miPosicionLinea = 0; miPosicionLinea < numeroDeLineas; miPosicionLinea++)
            {
                primerLinea = txtLenguaje.Lines.Length > 0 ? txtLenguaje.Lines[miPosicionLinea] : string.Empty; //Hugo Ramos - Se fragmenta en lineas del texto completo
                string[] tokens = primerLinea.Split(' '); //Hugo Ramos Zarate - Dividimos la expresión en tokens por espacios en blanco
                if (EsDeclaracionVariable(primerLinea))
                {
                    var datosVariable = ObtenerDatosDeclaracionVariable(primerLinea);
                    dgvVariables.Rows.Add(indice, datosVariable.Nombre, datosVariable.TipoDato, datosVariable.Valor);
                    indice = indice + 1;
                }
                else if (EsExpresion(primerLinea))
                {
                    string patron = @"(?<variable>\w+)\s*=\s*(?<operacion>.*)";  //Hugo Ramos - Patrón para buscar la variable y la operación
                    Match match = Regex.Match(primerLinea, patron); //Hugo Ramos - Buscar coincidencias en la expresión 
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        string token = tokens[i];
                        if (token.All(c => char.IsDigit(c) || c == '.')) //Hugo Ramos -compara que sea digito entero o db, si es db debe aceptar .
                        {
                            if (EsEntero(token)) { tipoDatoConstante = "Entero"; }
                            else if (EsDouble(token)) { tipoDatoConstante = "Double"; }
                            else {/*MessageBox.Show("El valor ingresado no es ni un entero ni un double válido."); //Hugo Ramos - Detecta que no sea una expresion, ayuda a identificar que hacer cuando no sea y seguir su proceso por ejemplo luego puede seguir con una iteracion o etc*/}
                            dgvConstantes.Rows.Add(indiceConstante = indiceConstante + 1, token, tipoDatoConstante);//Hugo Ramos - Agrega a la tabla de constantes, la constante que se detecto en la expresion                                               
                        }
                    }
                }
                else {/*MessageBox.Show("No es una declaracion de variable ni una expresion valida"); //Hugo Ramos Zarate - Lo puse por si en un furuto se necesita identificar que no sea variable o constante */ }
            }
            metodoBusqueda(); //Hugo Ramos - ejecuta el metodo de busqueda de datos
            Triplos();//Hugo Ramos - ejecuta el metodo de triplos
        }
        static bool EsDeclaracionVariable(string entrada)
        {
            string patron = @"^\s*(\w+)\s+(\w+)\s*=\s*(\d+)\s*$";
            return Regex.IsMatch(entrada, patron);//Hugo Ramos - Utiliza una expresión regular para verificar si la entrada coincide con un patrón de declaración de variable.
        }
        static (string Nombre, string TipoDato, string Valor) ObtenerDatosDeclaracionVariable(string entrada)
        {
            string patron = @"^\s*(\w+)\s+(\w+)\s*=\s*(\d+)\s*$";
            Match coincidencia = Regex.Match(entrada, patron);// Utiliza una expresión regular para extraer el nombre de la variable, el tipo de dato y el valor de una declaración de variable.
            return (coincidencia.Groups[1].Value, coincidencia.Groups[2].Value, coincidencia.Groups[3].Value);
        }
        static bool EsExpresion(string entrada)
        {
            string patron = @"^\w+\s*=\s*(.*)$";
            return Regex.IsMatch(entrada, patron); //Hugo Ramos -  Utiliza una expresión regular para verificar si la entrada es una expresión válida.
        }
        static bool EsEntero(string input)
        {
            int resultado;
            return int.TryParse(input, out resultado);//Hugo Ramos - Metodo que ingresa una entrada y verifica si puede convertirse a int y si si, devuelve que tipo de dato es  
        }
        static bool EsDouble(string input)
        {
            double resultado;
            return double.TryParse(input, out resultado);//Hugo Ramos - Metodo que ingresa una entrada y verifica si puede convertirse a db y si si, devuelve que tipo de dato es 
        }
        static string ObtenerDescripcionOperacion(string operador) //Hugo Ramos Zarate - Metodo que ingresa un operador y devuelve una cadena segun su tipo, para imprimir la descripcion de la tabla de triplos
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
                case ">":return "operador mayor que"; break;
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
    }
}
