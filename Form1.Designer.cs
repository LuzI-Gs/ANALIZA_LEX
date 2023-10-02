namespace ANALIZA_LEX
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
            this.btnValidar = new System.Windows.Forms.Button();
            this.dtgMatriz = new System.Windows.Forms.DataGridView();
            this.grpTexto = new System.Windows.Forms.GroupBox();
            this.btnValidarS = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSintactico = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTokens = new System.Windows.Forms.TextBox();
            this.txtLenguaje = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtgMatriz)).BeginInit();
            this.grpTexto.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnValidar
            // 
            this.btnValidar.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidar.Location = new System.Drawing.Point(488, 355);
            this.btnValidar.Name = "btnValidar";
            this.btnValidar.Size = new System.Drawing.Size(113, 38);
            this.btnValidar.TabIndex = 18;
            this.btnValidar.Text = "Validar";
            this.btnValidar.UseVisualStyleBackColor = true;
            this.btnValidar.Click += new System.EventHandler(this.btnValidar_Click);
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
            this.dtgMatriz.Location = new System.Drawing.Point(12, 464);
            this.dtgMatriz.Name = "dtgMatriz";
            this.dtgMatriz.ReadOnly = true;
            this.dtgMatriz.RowHeadersWidth = 30;
            this.dtgMatriz.RowTemplate.Height = 24;
            this.dtgMatriz.Size = new System.Drawing.Size(1900, 495);
            this.dtgMatriz.TabIndex = 17;
            // 
            // grpTexto
            // 
            this.grpTexto.BackColor = System.Drawing.Color.Wheat;
            this.grpTexto.Controls.Add(this.btnValidarS);
            this.grpTexto.Controls.Add(this.label3);
            this.grpTexto.Controls.Add(this.txtSintactico);
            this.grpTexto.Controls.Add(this.label2);
            this.grpTexto.Controls.Add(this.label1);
            this.grpTexto.Controls.Add(this.txtTokens);
            this.grpTexto.Controls.Add(this.txtLenguaje);
            this.grpTexto.Controls.Add(this.btnValidar);
            this.grpTexto.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpTexto.Location = new System.Drawing.Point(12, 12);
            this.grpTexto.Name = "grpTexto";
            this.grpTexto.Size = new System.Drawing.Size(1934, 399);
            this.grpTexto.TabIndex = 16;
            this.grpTexto.TabStop = false;
            this.grpTexto.Text = "Texto";
            // 
            // btnValidarS
            // 
            this.btnValidarS.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidarS.Location = new System.Drawing.Point(1360, 259);
            this.btnValidarS.Name = "btnValidarS";
            this.btnValidarS.Size = new System.Drawing.Size(129, 38);
            this.btnValidarS.TabIndex = 23;
            this.btnValidarS.Text = "Validar";
            this.btnValidarS.UseVisualStyleBackColor = true;
            this.btnValidarS.Click += new System.EventHandler(this.btnValidarS_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1329, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(259, 27);
            this.label3.TabIndex = 5;
            this.label3.Text = "Analizador sintactico ";
            // 
            // txtSintactico
            // 
            this.txtSintactico.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSintactico.Location = new System.Drawing.Point(1003, 48);
            this.txtSintactico.Multiline = true;
            this.txtSintactico.Name = "txtSintactico";
            this.txtSintactico.Size = new System.Drawing.Size(897, 184);
            this.txtSintactico.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(191, 188);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(215, 27);
            this.label2.TabIndex = 3;
            this.label2.Text = "Analizador lexico ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(184, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 27);
            this.label1.TabIndex = 2;
            this.label1.Text = "Lenguaje";
            // 
            // txtTokens
            // 
            this.txtTokens.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTokens.Location = new System.Drawing.Point(22, 218);
            this.txtTokens.Multiline = true;
            this.txtTokens.Name = "txtTokens";
            this.txtTokens.Size = new System.Drawing.Size(975, 131);
            this.txtTokens.TabIndex = 1;
            // 
            // txtLenguaje
            // 
            this.txtLenguaje.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLenguaje.Location = new System.Drawing.Point(22, 48);
            this.txtLenguaje.Multiline = true;
            this.txtLenguaje.Name = "txtLenguaje";
            this.txtLenguaje.Size = new System.Drawing.Size(975, 131);
            this.txtLenguaje.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(1795, 965);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 40);
            this.button1.TabIndex = 24;
            this.button1.Tag = " ";
            this.button1.Text = "Volver a validar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1924, 1017);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dtgMatriz);
            this.Controls.Add(this.grpTexto);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgMatriz)).EndInit();
            this.grpTexto.ResumeLayout(false);
            this.grpTexto.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnValidar;
        private System.Windows.Forms.DataGridView dtgMatriz;
        private System.Windows.Forms.GroupBox grpTexto;
        private System.Windows.Forms.TextBox txtTokens;
        private System.Windows.Forms.TextBox txtLenguaje;
        private System.Windows.Forms.Button btnValidarS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSintactico;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}

