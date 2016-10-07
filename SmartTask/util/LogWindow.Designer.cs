namespace SmartTask
{
    partial class LogWindow
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
            this.txtLogOut = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtLogOut
            // 
            this.txtLogOut.BackColor = System.Drawing.Color.White;
            this.txtLogOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogOut.Location = new System.Drawing.Point(0, 0);
            this.txtLogOut.Multiline = true;
            this.txtLogOut.Name = "txtLogOut";
            this.txtLogOut.ReadOnly = true;
            this.txtLogOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLogOut.Size = new System.Drawing.Size(629, 493);
            this.txtLogOut.TabIndex = 0;
            // 
            // LogWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 493);
            this.Controls.Add(this.txtLogOut);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LogWindow";
            this.Text = "Smart Task Log@Peter.Peng";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LogWindow_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLogOut;

    }
}