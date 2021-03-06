﻿namespace Apresentação.Atendimento.Atendente
{
    partial class BaseListaAtendimentos
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bgRecuperarVisitantes = new System.ComponentModel.BackgroundWorker();
            this.quadroAtendimento = new Apresentação.Formulários.Quadro();
            this.opçãoHistóricoAtendimentos = new Apresentação.Formulários.Opção();
            this.esquerda.SuspendLayout();
            this.quadroAtendimento.SuspendLayout();
            this.SuspendLayout();
            // 
            // esquerda
            // 
            this.esquerda.Controls.Add(this.quadroAtendimento);
            this.esquerda.Controls.SetChildIndex(this.quadroAtendimento, 0);
            // 
            // bgRecuperarVisitantes
            // 
            this.bgRecuperarVisitantes.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgRecuperarVisitantes_DoWork);
            // 
            // quadroAtendimento
            // 
            this.quadroAtendimento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.quadroAtendimento.bInfDirArredondada = true;
            this.quadroAtendimento.bInfEsqArredondada = true;
            this.quadroAtendimento.bSupDirArredondada = true;
            this.quadroAtendimento.bSupEsqArredondada = true;
            this.quadroAtendimento.Controls.Add(this.opçãoHistóricoAtendimentos);
            this.quadroAtendimento.Cor = System.Drawing.Color.Black;
            this.quadroAtendimento.FundoTítulo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(165)))), ((int)(((byte)(159)))), ((int)(((byte)(97)))));
            this.quadroAtendimento.LetraTítulo = System.Drawing.Color.White;
            this.quadroAtendimento.Location = new System.Drawing.Point(7, 116);
            this.quadroAtendimento.MostrarBotãoMinMax = false;
            this.quadroAtendimento.Name = "quadroAtendimento";
            this.quadroAtendimento.Size = new System.Drawing.Size(160, 67);
            this.quadroAtendimento.TabIndex = 3;
            this.quadroAtendimento.Tamanho = 30;
            this.quadroAtendimento.Título = "Atendimento";
            // 
            // opçãoHistóricoAtendimentos
            // 
            this.opçãoHistóricoAtendimentos.BackColor = System.Drawing.Color.Transparent;
            this.opçãoHistóricoAtendimentos.Descrição = "Visualizar histórico de atendimentos";
            this.opçãoHistóricoAtendimentos.Imagem = global::Apresentação.Resource.document1;
            this.opçãoHistóricoAtendimentos.Location = new System.Drawing.Point(7, 30);
            this.opçãoHistóricoAtendimentos.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.opçãoHistóricoAtendimentos.MaximumSize = new System.Drawing.Size(150, 100);
            this.opçãoHistóricoAtendimentos.MinimumSize = new System.Drawing.Size(150, 16);
            this.opçãoHistóricoAtendimentos.Name = "opçãoHistóricoAtendimentos";
            this.opçãoHistóricoAtendimentos.Size = new System.Drawing.Size(150, 33);
            this.opçãoHistóricoAtendimentos.TabIndex = 2;
            this.opçãoHistóricoAtendimentos.Click += new System.EventHandler(this.opçãoHistóricoAtendimentos_Click);
            // 
            // BaseListaAtendimentos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "BaseListaAtendimentos";
            this.esquerda.ResumeLayout(false);
            this.quadroAtendimento.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bgRecuperarVisitantes;
        private Apresentação.Formulários.Quadro quadroAtendimento;
        private Apresentação.Formulários.Opção opçãoHistóricoAtendimentos;
    }
}
