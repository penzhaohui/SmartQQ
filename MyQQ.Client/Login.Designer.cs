namespace MyQQ.Client
{
    partial class Login
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
            this.picLoginQRCode = new System.Windows.Forms.PictureBox();
            this.lblLoginInstruction = new System.Windows.Forms.Label();
            this.btnGenerateQRCode = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picLoginQRCode)).BeginInit();
            this.SuspendLayout();
            // 
            // picLoginQRCode
            // 
            this.picLoginQRCode.Location = new System.Drawing.Point(59, 43);
            this.picLoginQRCode.Name = "picLoginQRCode";
            this.picLoginQRCode.Size = new System.Drawing.Size(184, 190);
            this.picLoginQRCode.TabIndex = 0;
            this.picLoginQRCode.TabStop = false;
            // 
            // lblLoginInstruction
            // 
            this.lblLoginInstruction.AutoSize = true;
            this.lblLoginInstruction.Location = new System.Drawing.Point(32, 18);
            this.lblLoginInstruction.Name = "lblLoginInstruction";
            this.lblLoginInstruction.Size = new System.Drawing.Size(224, 13);
            this.lblLoginInstruction.TabIndex = 1;
            this.lblLoginInstruction.Text = "Please login your QQ by scaning the QR code";
            // 
            // btnGenerateQRCode
            // 
            this.btnGenerateQRCode.Location = new System.Drawing.Point(93, 239);
            this.btnGenerateQRCode.Name = "btnGenerateQRCode";
            this.btnGenerateQRCode.Size = new System.Drawing.Size(75, 23);
            this.btnGenerateQRCode.TabIndex = 2;
            this.btnGenerateQRCode.Text = "Generate QR Code";
            this.btnGenerateQRCode.UseVisualStyleBackColor = true;
            this.btnGenerateQRCode.Click += new System.EventHandler(this.btnGenerateQRCode_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 274);
            this.Controls.Add(this.btnGenerateQRCode);
            this.Controls.Add(this.lblLoginInstruction);
            this.Controls.Add(this.picLoginQRCode);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.Text = "Smart QQ";
            this.Click += new System.EventHandler(this.Login_Click);
            ((System.ComponentModel.ISupportInitialize)(this.picLoginQRCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLoginQRCode;
        private System.Windows.Forms.Label lblLoginInstruction;
        private System.Windows.Forms.Button btnGenerateQRCode;
    }
}

