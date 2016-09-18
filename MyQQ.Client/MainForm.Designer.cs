namespace MyQQ
{
    partial class MainForm
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
            this.grpFriends = new System.Windows.Forms.GroupBox();
            this.lstMyFriends = new System.Windows.Forms.ListView();
            this.grpQQGroups = new System.Windows.Forms.GroupBox();
            this.lstMyQQGroups = new System.Windows.Forms.ListView();
            this.grpDiscussionGroups = new System.Windows.Forms.GroupBox();
            this.lstMyDiscussionGroups = new System.Windows.Forms.ListView();
            this.grpShowMessageWindow = new System.Windows.Forms.GroupBox();
            this.txtShowMessageWindow = new System.Windows.Forms.TextBox();
            this.grpInputMessageBox = new System.Windows.Forms.GroupBox();
            this.btnSendMessae = new System.Windows.Forms.Button();
            this.txtInputMessageBox = new System.Windows.Forms.TextBox();
            this.grpFriends.SuspendLayout();
            this.grpQQGroups.SuspendLayout();
            this.grpDiscussionGroups.SuspendLayout();
            this.grpShowMessageWindow.SuspendLayout();
            this.grpInputMessageBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpFriends
            // 
            this.grpFriends.Controls.Add(this.lstMyFriends);
            this.grpFriends.Location = new System.Drawing.Point(12, 12);
            this.grpFriends.Name = "grpFriends";
            this.grpFriends.Size = new System.Drawing.Size(166, 524);
            this.grpFriends.TabIndex = 0;
            this.grpFriends.TabStop = false;
            this.grpFriends.Text = "Friends";
            // 
            // lstMyFriends
            // 
            this.lstMyFriends.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMyFriends.Location = new System.Drawing.Point(3, 16);
            this.lstMyFriends.Name = "lstMyFriends";
            this.lstMyFriends.Size = new System.Drawing.Size(160, 505);
            this.lstMyFriends.TabIndex = 0;
            this.lstMyFriends.UseCompatibleStateImageBehavior = false;
            // 
            // grpQQGroups
            // 
            this.grpQQGroups.Controls.Add(this.lstMyQQGroups);
            this.grpQQGroups.Location = new System.Drawing.Point(199, 12);
            this.grpQQGroups.Name = "grpQQGroups";
            this.grpQQGroups.Size = new System.Drawing.Size(155, 308);
            this.grpQQGroups.TabIndex = 1;
            this.grpQQGroups.TabStop = false;
            this.grpQQGroups.Text = "QQ Groups";
            // 
            // lstMyQQGroups
            // 
            this.lstMyQQGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMyQQGroups.Location = new System.Drawing.Point(3, 16);
            this.lstMyQQGroups.Name = "lstMyQQGroups";
            this.lstMyQQGroups.Size = new System.Drawing.Size(149, 289);
            this.lstMyQQGroups.TabIndex = 0;
            this.lstMyQQGroups.UseCompatibleStateImageBehavior = false;
            // 
            // grpDiscussionGroups
            // 
            this.grpDiscussionGroups.Controls.Add(this.lstMyDiscussionGroups);
            this.grpDiscussionGroups.Location = new System.Drawing.Point(199, 343);
            this.grpDiscussionGroups.Name = "grpDiscussionGroups";
            this.grpDiscussionGroups.Size = new System.Drawing.Size(149, 193);
            this.grpDiscussionGroups.TabIndex = 2;
            this.grpDiscussionGroups.TabStop = false;
            this.grpDiscussionGroups.Text = "Discussion Group";
            // 
            // lstMyDiscussionGroups
            // 
            this.lstMyDiscussionGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMyDiscussionGroups.Location = new System.Drawing.Point(3, 16);
            this.lstMyDiscussionGroups.Name = "lstMyDiscussionGroups";
            this.lstMyDiscussionGroups.Size = new System.Drawing.Size(143, 174);
            this.lstMyDiscussionGroups.TabIndex = 0;
            this.lstMyDiscussionGroups.UseCompatibleStateImageBehavior = false;
            // 
            // grpShowMessageWindow
            // 
            this.grpShowMessageWindow.Controls.Add(this.txtShowMessageWindow);
            this.grpShowMessageWindow.Location = new System.Drawing.Point(361, 12);
            this.grpShowMessageWindow.Name = "grpShowMessageWindow";
            this.grpShowMessageWindow.Size = new System.Drawing.Size(404, 308);
            this.grpShowMessageWindow.TabIndex = 3;
            this.grpShowMessageWindow.TabStop = false;
            this.grpShowMessageWindow.Text = "Message Window";
            // 
            // txtShowMessageWindow
            // 
            this.txtShowMessageWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtShowMessageWindow.Location = new System.Drawing.Point(3, 16);
            this.txtShowMessageWindow.Multiline = true;
            this.txtShowMessageWindow.Name = "txtShowMessageWindow";
            this.txtShowMessageWindow.Size = new System.Drawing.Size(398, 289);
            this.txtShowMessageWindow.TabIndex = 0;
            // 
            // grpInputMessageBox
            // 
            this.grpInputMessageBox.Controls.Add(this.btnSendMessae);
            this.grpInputMessageBox.Controls.Add(this.txtInputMessageBox);
            this.grpInputMessageBox.Location = new System.Drawing.Point(361, 343);
            this.grpInputMessageBox.Name = "grpInputMessageBox";
            this.grpInputMessageBox.Size = new System.Drawing.Size(401, 101);
            this.grpInputMessageBox.TabIndex = 4;
            this.grpInputMessageBox.TabStop = false;
            this.grpInputMessageBox.Text = "Message Window";
            // 
            // btnSendMessae
            // 
            this.btnSendMessae.Location = new System.Drawing.Point(319, 19);
            this.btnSendMessae.Name = "btnSendMessae";
            this.btnSendMessae.Size = new System.Drawing.Size(75, 62);
            this.btnSendMessae.TabIndex = 1;
            this.btnSendMessae.Text = "Send";
            this.btnSendMessae.UseVisualStyleBackColor = true;
            // 
            // txtInputMessageBox
            // 
            this.txtInputMessageBox.Location = new System.Drawing.Point(6, 19);
            this.txtInputMessageBox.Multiline = true;
            this.txtInputMessageBox.Name = "txtInputMessageBox";
            this.txtInputMessageBox.Size = new System.Drawing.Size(307, 63);
            this.txtInputMessageBox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 548);
            this.Controls.Add(this.grpInputMessageBox);
            this.Controls.Add(this.grpShowMessageWindow);
            this.Controls.Add(this.grpDiscussionGroups);
            this.Controls.Add(this.grpQQGroups);
            this.Controls.Add(this.grpFriends);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Smart QQ";
            this.grpFriends.ResumeLayout(false);
            this.grpQQGroups.ResumeLayout(false);
            this.grpDiscussionGroups.ResumeLayout(false);
            this.grpShowMessageWindow.ResumeLayout(false);
            this.grpShowMessageWindow.PerformLayout();
            this.grpInputMessageBox.ResumeLayout(false);
            this.grpInputMessageBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFriends;
        private System.Windows.Forms.GroupBox grpQQGroups;
        private System.Windows.Forms.GroupBox grpDiscussionGroups;
        private System.Windows.Forms.GroupBox grpShowMessageWindow;
        private System.Windows.Forms.TextBox txtShowMessageWindow;
        private System.Windows.Forms.GroupBox grpInputMessageBox;
        private System.Windows.Forms.Button btnSendMessae;
        private System.Windows.Forms.TextBox txtInputMessageBox;
        private System.Windows.Forms.ListView lstMyFriends;
        private System.Windows.Forms.ListView lstMyQQGroups;
        private System.Windows.Forms.ListView lstMyDiscussionGroups;
    }
}