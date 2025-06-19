namespace server
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.port_textBox = new System.Windows.Forms.TextBox();
            this.listen_button = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.message_textBox = new System.Windows.Forms.TextBox();
            this.send_button = new System.Windows.Forms.Button();
            this.leaderBoard_logs = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port: ";
            // 
            // port_textBox
            // 
            this.port_textBox.Location = new System.Drawing.Point(110, 50);
            this.port_textBox.Name = "port_textBox";
            this.port_textBox.Size = new System.Drawing.Size(141, 20);
            this.port_textBox.TabIndex = 1;
            // 
            // listen_button
            // 
            this.listen_button.Location = new System.Drawing.Point(276, 47);
            this.listen_button.Name = "listen_button";
            this.listen_button.Size = new System.Drawing.Size(75, 23);
            this.listen_button.TabIndex = 2;
            this.listen_button.Text = "Listen";
            this.listen_button.UseVisualStyleBackColor = true;
            this.listen_button.Click += new System.EventHandler(this.listen_button_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(65, 85);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(297, 293);
            this.logs.TabIndex = 3;
            this.logs.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 397);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Message:";
            // 
            // message_textBox
            // 
            this.message_textBox.Location = new System.Drawing.Point(121, 394);
            this.message_textBox.Name = "message_textBox";
            this.message_textBox.Size = new System.Drawing.Size(158, 20);
            this.message_textBox.TabIndex = 5;
            // 
            // send_button
            // 
            this.send_button.Location = new System.Drawing.Point(287, 394);
            this.send_button.Name = "send_button";
            this.send_button.Size = new System.Drawing.Size(75, 23);
            this.send_button.TabIndex = 6;
            this.send_button.Text = "send";
            this.send_button.UseVisualStyleBackColor = true;
            this.send_button.Click += new System.EventHandler(this.send_button_Click);
            // 
            // leaderBoard_logs
            // 
            this.leaderBoard_logs.Location = new System.Drawing.Point(399, 85);
            this.leaderBoard_logs.Name = "leaderBoard_logs";
            this.leaderBoard_logs.Size = new System.Drawing.Size(210, 293);
            this.leaderBoard_logs.TabIndex = 7;
            this.leaderBoard_logs.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(396, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Leaderboard:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.leaderBoard_logs);
            this.Controls.Add(this.send_button);
            this.Controls.Add(this.message_textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.listen_button);
            this.Controls.Add(this.port_textBox);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox port_textBox;
        private System.Windows.Forms.Button listen_button;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox message_textBox;
        private System.Windows.Forms.Button send_button;
        private System.Windows.Forms.RichTextBox leaderBoard_logs;
        private System.Windows.Forms.Label label3;
    }
}

