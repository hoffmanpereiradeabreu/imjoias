﻿namespace Apresentação.Pessoa.Horário
{
    partial class ControleHorário
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
            this.SuspendLayout();
            // 
            // ControleHorário
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "ControleHorário";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ControleHorário_MouseClick);
            this.Resize += new System.EventHandler(this.ControleHorário_Resize);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ControleHorário_Paint);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
