﻿namespace ANALIZA_LEX
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picMinimizar = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.picSalir = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dtgMatriz = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvErroresLexicos = new System.Windows.Forms.DataGridView();
            this.Error = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Caracteristica = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvIden = new System.Windows.Forms.DataGridView();
            this.intNumero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtIdentificador = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.strNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.strTipoDato = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.strValor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgvErroresSemanticos = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.rtxt = new System.Windows.Forms.RichTextBox();
            this.txtLineasLexico = new System.Windows.Forms.TextBox();
            this.txtLineasLenguaje = new System.Windows.Forms.TextBox();
            this.txtTokens = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSintactico = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLenguaje = new System.Windows.Forms.TextBox();
            this.btnValidarSint = new System.Windows.Forms.Button();
            this.btnValidar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.radInOrden = new System.Windows.Forms.RadioButton();
            this.radPreorden = new System.Windows.Forms.RadioButton();
            this.radPostorden = new System.Windows.Forms.RadioButton();
            this.btnOrdenar = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.dgvAsignacionConstantes = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAsignaVariable = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMinimizar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSalir)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgMatriz)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErroresLexicos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIden)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErroresSemanticos)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAsignacionConstantes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAsignaVariable)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(1346, 784);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 32);
            this.button1.TabIndex = 24;
            this.button1.Tag = " ";
            this.button1.Text = "Volver a validar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(200)))));
            this.panel1.Controls.Add(this.picMinimizar);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.picSalir);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1036, 44);
            this.panel1.TabIndex = 26;
            // 
            // picMinimizar
            // 
            this.picMinimizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMinimizar.Image = global::ANALIZA_LEX.Properties.Resources.minimazar;
            this.picMinimizar.Location = new System.Drawing.Point(973, 10);
            this.picMinimizar.Name = "picMinimizar";
            this.picMinimizar.Size = new System.Drawing.Size(19, 27);
            this.picMinimizar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picMinimizar.TabIndex = 40;
            this.picMinimizar.TabStop = false;
            this.picMinimizar.Click += new System.EventHandler(this.picMinimizar_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(33, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(328, 37);
            this.label4.TabIndex = 0;
            this.label4.Text = "Lenguajes Automatas";
            // 
            // picSalir
            // 
            this.picSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSalir.Image = global::ANALIZA_LEX.Properties.Resources.cerrar;
            this.picSalir.Location = new System.Drawing.Point(1003, 10);
            this.picSalir.Name = "picSalir";
            this.picSalir.Size = new System.Drawing.Size(19, 27);
            this.picSalir.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSalir.TabIndex = 39;
            this.picSalir.TabStop = false;
            this.picSalir.Click += new System.EventHandler(this.picSalir_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 440);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1026, 265);
            this.tabControl1.TabIndex = 35;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dtgMatriz);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(1018, 234);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Recorrido de la matriz";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dtgMatriz
            // 
            this.dtgMatriz.AllowUserToAddRows = false;
            this.dtgMatriz.AllowUserToDeleteRows = false;
            this.dtgMatriz.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dtgMatriz.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dtgMatriz.BackgroundColor = System.Drawing.Color.White;
            this.dtgMatriz.ColumnHeadersHeight = 29;
            this.dtgMatriz.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtgMatriz.Location = new System.Drawing.Point(5, 20);
            this.dtgMatriz.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtgMatriz.Name = "dtgMatriz";
            this.dtgMatriz.ReadOnly = true;
            this.dtgMatriz.RowHeadersWidth = 30;
            this.dtgMatriz.RowTemplate.Height = 24;
            this.dtgMatriz.Size = new System.Drawing.Size(978, 207);
            this.dtgMatriz.TabIndex = 17;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvErroresLexicos);
            this.tabPage2.Controls.Add(this.dgvIden);
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage2.Size = new System.Drawing.Size(1018, 234);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabla de simbolos";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvErroresLexicos
            // 
            this.dgvErroresLexicos.AllowUserToAddRows = false;
            this.dgvErroresLexicos.AllowUserToDeleteRows = false;
            this.dgvErroresLexicos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvErroresLexicos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvErroresLexicos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Error,
            this.Caracteristica});
            this.dgvErroresLexicos.Location = new System.Drawing.Point(609, 25);
            this.dgvErroresLexicos.Name = "dgvErroresLexicos";
            this.dgvErroresLexicos.ReadOnly = true;
            this.dgvErroresLexicos.RowHeadersWidth = 51;
            this.dgvErroresLexicos.Size = new System.Drawing.Size(325, 201);
            this.dgvErroresLexicos.TabIndex = 1;
            // 
            // Error
            // 
            this.Error.HeaderText = "Error";
            this.Error.MinimumWidth = 6;
            this.Error.Name = "Error";
            this.Error.ReadOnly = true;
            // 
            // Caracteristica
            // 
            this.Caracteristica.HeaderText = "Caracteristica";
            this.Caracteristica.MinimumWidth = 6;
            this.Caracteristica.Name = "Caracteristica";
            this.Caracteristica.ReadOnly = true;
            // 
            // dgvIden
            // 
            this.dgvIden.AllowUserToAddRows = false;
            this.dgvIden.AllowUserToDeleteRows = false;
            this.dgvIden.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIden.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIden.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.intNumero,
            this.txtIdentificador,
            this.strNombre,
            this.strTipoDato,
            this.strValor});
            this.dgvIden.Location = new System.Drawing.Point(31, 25);
            this.dgvIden.Name = "dgvIden";
            this.dgvIden.ReadOnly = true;
            this.dgvIden.RowHeadersWidth = 51;
            this.dgvIden.Size = new System.Drawing.Size(516, 201);
            this.dgvIden.TabIndex = 0;
            // 
            // intNumero
            // 
            this.intNumero.HeaderText = "Num";
            this.intNumero.MinimumWidth = 6;
            this.intNumero.Name = "intNumero";
            this.intNumero.ReadOnly = true;
            // 
            // txtIdentificador
            // 
            this.txtIdentificador.HeaderText = "Identificador";
            this.txtIdentificador.MinimumWidth = 6;
            this.txtIdentificador.Name = "txtIdentificador";
            this.txtIdentificador.ReadOnly = true;
            // 
            // strNombre
            // 
            this.strNombre.HeaderText = "Nombre";
            this.strNombre.MinimumWidth = 6;
            this.strNombre.Name = "strNombre";
            this.strNombre.ReadOnly = true;
            // 
            // strTipoDato
            // 
            this.strTipoDato.HeaderText = "TipoDato";
            this.strTipoDato.MinimumWidth = 6;
            this.strTipoDato.Name = "strTipoDato";
            this.strTipoDato.ReadOnly = true;
            // 
            // strValor
            // 
            this.strValor.HeaderText = "Valor";
            this.strValor.MinimumWidth = 6;
            this.strValor.Name = "strValor";
            this.strValor.ReadOnly = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgvErroresSemanticos);
            this.tabPage3.Location = new System.Drawing.Point(4, 27);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1018, 234);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Errores Semanticos";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgvErroresSemanticos
            // 
            this.dgvErroresSemanticos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvErroresSemanticos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvErroresSemanticos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dgvErroresSemanticos.Location = new System.Drawing.Point(36, 11);
            this.dgvErroresSemanticos.Name = "dgvErroresSemanticos";
            this.dgvErroresSemanticos.RowHeadersWidth = 51;
            this.dgvErroresSemanticos.Size = new System.Drawing.Size(923, 150);
            this.dgvErroresSemanticos.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Linea";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Error";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.rtxt);
            this.tabPage4.Location = new System.Drawing.Point(4, 27);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage4.Size = new System.Drawing.Size(1018, 234);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Ordenar";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // rtxt
            // 
            this.rtxt.Location = new System.Drawing.Point(29, 15);
            this.rtxt.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rtxt.Name = "rtxt";
            this.rtxt.Size = new System.Drawing.Size(314, 121);
            this.rtxt.TabIndex = 0;
            this.rtxt.Text = "";
            // 
            // txtLineasLexico
            // 
            this.txtLineasLexico.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLineasLexico.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLineasLexico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtLineasLexico.Location = new System.Drawing.Point(351, 95);
            this.txtLineasLexico.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtLineasLexico.Multiline = true;
            this.txtLineasLexico.Name = "txtLineasLexico";
            this.txtLineasLexico.Size = new System.Drawing.Size(24, 259);
            this.txtLineasLexico.TabIndex = 37;
            this.txtLineasLexico.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtLineasLenguaje
            // 
            this.txtLineasLenguaje.BackColor = System.Drawing.Color.White;
            this.txtLineasLenguaje.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLineasLenguaje.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLineasLenguaje.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtLineasLenguaje.Location = new System.Drawing.Point(11, 94);
            this.txtLineasLenguaje.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtLineasLenguaje.Multiline = true;
            this.txtLineasLenguaje.Name = "txtLineasLenguaje";
            this.txtLineasLenguaje.Size = new System.Drawing.Size(31, 259);
            this.txtLineasLenguaje.TabIndex = 36;
            // 
            // txtTokens
            // 
            this.txtTokens.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTokens.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTokens.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtTokens.Location = new System.Drawing.Point(393, 96);
            this.txtTokens.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTokens.Name = "txtTokens";
            this.txtTokens.Size = new System.Drawing.Size(247, 258);
            this.txtTokens.TabIndex = 34;
            this.txtTokens.Text = "";
            this.txtTokens.TextChanged += new System.EventHandler(this.txtTokens_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.label3.Location = new System.Drawing.Point(720, 55);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(217, 25);
            this.label3.TabIndex = 32;
            this.label3.Text = "Analizador sintactico ";
            // 
            // txtSintactico
            // 
            this.txtSintactico.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSintactico.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSintactico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtSintactico.Location = new System.Drawing.Point(725, 97);
            this.txtSintactico.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSintactico.Multiline = true;
            this.txtSintactico.Name = "txtSintactico";
            this.txtSintactico.Size = new System.Drawing.Size(263, 258);
            this.txtSintactico.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.label2.Location = new System.Drawing.Point(389, 55);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 26);
            this.label2.TabIndex = 30;
            this.label2.Text = "Analizador lexico ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.label1.Location = new System.Drawing.Point(54, 55);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 25);
            this.label1.TabIndex = 29;
            this.label1.Text = "Lenguaje";
            // 
            // txtLenguaje
            // 
            this.txtLenguaje.BackColor = System.Drawing.Color.White;
            this.txtLenguaje.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLenguaje.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLenguaje.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtLenguaje.Location = new System.Drawing.Point(58, 95);
            this.txtLenguaje.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtLenguaje.Multiline = true;
            this.txtLenguaje.Name = "txtLenguaje";
            this.txtLenguaje.Size = new System.Drawing.Size(212, 258);
            this.txtLenguaje.TabIndex = 28;
            this.txtLenguaje.TextChanged += new System.EventHandler(this.txtLenguaje_TextChanged);
            // 
            // btnValidarSint
            // 
            this.btnValidarSint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(200)))));
            this.btnValidarSint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidarSint.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidarSint.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnValidarSint.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnValidarSint.Location = new System.Drawing.Point(724, 378);
            this.btnValidarSint.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnValidarSint.Name = "btnValidarSint";
            this.btnValidarSint.Size = new System.Drawing.Size(263, 44);
            this.btnValidarSint.TabIndex = 38;
            this.btnValidarSint.Text = "Validar";
            this.btnValidarSint.UseVisualStyleBackColor = false;
            this.btnValidarSint.Click += new System.EventHandler(this.btnValidarSint_Click_1);
            // 
            // btnValidar
            // 
            this.btnValidar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(200)))));
            this.btnValidar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidar.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidar.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnValidar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnValidar.Location = new System.Drawing.Point(58, 378);
            this.btnValidar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnValidar.Name = "btnValidar";
            this.btnValidar.Size = new System.Drawing.Size(212, 39);
            this.btnValidar.TabIndex = 33;
            this.btnValidar.Text = "Validar";
            this.btnValidar.UseVisualStyleBackColor = false;
            this.btnValidar.Click += new System.EventHandler(this.btnValidar_Click_1);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(195, 59);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(75, 23);
            this.btnLimpiar.TabIndex = 39;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // radInOrden
            // 
            this.radInOrden.AutoSize = true;
            this.radInOrden.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radInOrden.Location = new System.Drawing.Point(530, 425);
            this.radInOrden.Name = "radInOrden";
            this.radInOrden.Size = new System.Drawing.Size(85, 24);
            this.radInOrden.TabIndex = 46;
            this.radInOrden.Text = "InOrden";
            this.radInOrden.UseVisualStyleBackColor = true;
            // 
            // radPreorden
            // 
            this.radPreorden.AutoSize = true;
            this.radPreorden.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radPreorden.Location = new System.Drawing.Point(530, 402);
            this.radPreorden.Name = "radPreorden";
            this.radPreorden.Size = new System.Drawing.Size(95, 24);
            this.radPreorden.TabIndex = 45;
            this.radPreorden.Text = "PreOrden";
            this.radPreorden.UseVisualStyleBackColor = true;
            // 
            // radPostorden
            // 
            this.radPostorden.AutoSize = true;
            this.radPostorden.Checked = true;
            this.radPostorden.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radPostorden.Location = new System.Drawing.Point(530, 375);
            this.radPostorden.Name = "radPostorden";
            this.radPostorden.Size = new System.Drawing.Size(103, 24);
            this.radPostorden.TabIndex = 44;
            this.radPostorden.TabStop = true;
            this.radPostorden.Text = "PostOrden";
            this.radPostorden.UseVisualStyleBackColor = true;
            this.radPostorden.CheckedChanged += new System.EventHandler(this.radPostorden_CheckedChanged);
            // 
            // btnOrdenar
            // 
            this.btnOrdenar.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOrdenar.Location = new System.Drawing.Point(370, 378);
            this.btnOrdenar.Name = "btnOrdenar";
            this.btnOrdenar.Size = new System.Drawing.Size(119, 39);
            this.btnOrdenar.TabIndex = 43;
            this.btnOrdenar.Text = "ordenar";
            this.btnOrdenar.UseVisualStyleBackColor = true;
            this.btnOrdenar.Click += new System.EventHandler(this.btnOrdenar_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.dgvAsignacionConstantes);
            this.tabPage5.Controls.Add(this.dgvAsignaVariable);
            this.tabPage5.Location = new System.Drawing.Point(4, 27);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1018, 234);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Expresion_Asignacion";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // dgvAsignacionConstantes
            // 
            this.dgvAsignacionConstantes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAsignacionConstantes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.Column5,
            this.Column6});
            this.dgvAsignacionConstantes.Location = new System.Drawing.Point(426, 15);
            this.dgvAsignacionConstantes.Name = "dgvAsignacionConstantes";
            this.dgvAsignacionConstantes.Size = new System.Drawing.Size(339, 150);
            this.dgvAsignacionConstantes.TabIndex = 4;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Indice";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Nombre";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Tipo Dato";
            this.Column6.Name = "Column6";
            // 
            // dgvAsignaVariable
            // 
            this.dgvAsignaVariable.AllowUserToAddRows = false;
            this.dgvAsignaVariable.AllowUserToDeleteRows = false;
            this.dgvAsignaVariable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAsignaVariable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAsignaVariable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7});
            this.dgvAsignaVariable.Location = new System.Drawing.Point(8, 15);
            this.dgvAsignaVariable.Name = "dgvAsignaVariable";
            this.dgvAsignaVariable.ReadOnly = true;
            this.dgvAsignaVariable.RowHeadersWidth = 51;
            this.dgvAsignaVariable.Size = new System.Drawing.Size(386, 201);
            this.dgvAsignaVariable.TabIndex = 3;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Indice";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Nombre";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "TipoDato";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Contenido";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1036, 640);
            this.Controls.Add(this.radInOrden);
            this.Controls.Add(this.radPreorden);
            this.Controls.Add(this.radPostorden);
            this.Controls.Add(this.btnOrdenar);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnValidarSint);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtLineasLexico);
            this.Controls.Add(this.txtLineasLenguaje);
            this.Controls.Add(this.txtTokens);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSintactico);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLenguaje);
            this.Controls.Add(this.btnValidar);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMinimizar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSalir)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgMatriz)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvErroresLexicos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIden)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvErroresSemanticos)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAsignacionConstantes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAsignaVariable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnValidarSint;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dtgMatriz;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvErroresLexicos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Error;
        private System.Windows.Forms.DataGridViewTextBoxColumn Caracteristica;
        private System.Windows.Forms.DataGridView dgvIden;
        private System.Windows.Forms.DataGridViewTextBoxColumn intNumero;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtIdentificador;
        private System.Windows.Forms.DataGridViewTextBoxColumn strNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn strTipoDato;
        private System.Windows.Forms.DataGridViewTextBoxColumn strValor;
        private System.Windows.Forms.TextBox txtLineasLexico;
        private System.Windows.Forms.TextBox txtLineasLenguaje;
        private System.Windows.Forms.RichTextBox txtTokens;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSintactico;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLenguaje;
        private System.Windows.Forms.Button btnValidar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox picMinimizar;
        private System.Windows.Forms.PictureBox picSalir;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgvErroresSemanticos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.RadioButton radInOrden;
        private System.Windows.Forms.RadioButton radPreorden;
        private System.Windows.Forms.RadioButton radPostorden;
        private System.Windows.Forms.Button btnOrdenar;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.RichTextBox rtxt;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.DataGridView dgvAsignacionConstantes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridView dgvAsignaVariable;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    }
}

