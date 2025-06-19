namespace client
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
            this.label2 = new System.Windows.Forms.Label();
            this.connect_button = new System.Windows.Forms.Button();
            this.ip_textBox = new System.Windows.Forms.TextBox();
            this.port_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.name_textBox = new System.Windows.Forms.TextBox();
            this.disconnect_button = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.rock_button = new System.Windows.Forms.Button();
            this.paper_button = new System.Windows.Forms.Button();
            this.scissors_button = new System.Windows.Forms.Button();
            this.leave_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(82, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(76, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            // 
            // connect_button
            // 
            this.connect_button.Location = new System.Drawing.Point(131, 204);
            this.connect_button.Name = "connect_button";
            this.connect_button.Size = new System.Drawing.Size(75, 23);
            this.connect_button.TabIndex = 2;
            this.connect_button.Text = "Connect";
            this.connect_button.UseVisualStyleBackColor = true;
            this.connect_button.Click += new System.EventHandler(this.connect_button_Click);
            // 
            // ip_textBox
            // 
            this.ip_textBox.Location = new System.Drawing.Point(118, 61);
            this.ip_textBox.Name = "ip_textBox";
            this.ip_textBox.Size = new System.Drawing.Size(100, 20);
            this.ip_textBox.TabIndex = 3;
            // 
            // port_textBox
            // 
            this.port_textBox.Location = new System.Drawing.Point(118, 106);
            this.port_textBox.Name = "port_textBox";
            this.port_textBox.Size = new System.Drawing.Size(100, 20);
            this.port_textBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Name: ";
            // 
            // name_textBox
            // 
            this.name_textBox.Location = new System.Drawing.Point(118, 157);
            this.name_textBox.Name = "name_textBox";
            this.name_textBox.Size = new System.Drawing.Size(100, 20);
            this.name_textBox.TabIndex = 6;
            // 
            // disconnect_button
            // 
            this.disconnect_button.Location = new System.Drawing.Point(131, 248);
            this.disconnect_button.Name = "disconnect_button";
            this.disconnect_button.Size = new System.Drawing.Size(75, 23);
            this.disconnect_button.TabIndex = 8;
            this.disconnect_button.Text = "Disconnect";
            this.disconnect_button.UseVisualStyleBackColor = true;
            this.disconnect_button.Click += new System.EventHandler(this.disconnect_button_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(278, 46);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(355, 271);
            this.logs.TabIndex = 9;
            this.logs.Text = "";
            // 
            // rock_button
            // 
            this.rock_button.Location = new System.Drawing.Point(304, 337);
            this.rock_button.Name = "rock_button";
            this.rock_button.Size = new System.Drawing.Size(75, 23);
            this.rock_button.TabIndex = 10;
            this.rock_button.Text = "Rock";
            this.rock_button.UseVisualStyleBackColor = true;
            this.rock_button.Click += new System.EventHandler(this.rock_button_Click);
            // 
            // paper_button
            // 
            this.paper_button.Location = new System.Drawing.Point(418, 337);
            this.paper_button.Name = "paper_button";
            this.paper_button.Size = new System.Drawing.Size(75, 23);
            this.paper_button.TabIndex = 11;
            this.paper_button.Text = "Paper";
            this.paper_button.UseVisualStyleBackColor = true;
            this.paper_button.Click += new System.EventHandler(this.paper_button_Click);
            // 
            // scissors_button
            // 
            this.scissors_button.Location = new System.Drawing.Point(532, 337);
            this.scissors_button.Name = "scissors_button";
            this.scissors_button.Size = new System.Drawing.Size(75, 23);
            this.scissors_button.TabIndex = 12;
            this.scissors_button.Text = "Scissors";
            this.scissors_button.UseVisualStyleBackColor = true;
            this.scissors_button.Click += new System.EventHandler(this.scissors_button_Click);
            // 
            // leave_button
            // 
            this.leave_button.Location = new System.Drawing.Point(131, 293);
            this.leave_button.Name = "leave_button";
            this.leave_button.Size = new System.Drawing.Size(75, 23);
            this.leave_button.TabIndex = 13;
            this.leave_button.Text = "Leave";
            this.leave_button.UseVisualStyleBackColor = true;
            this.leave_button.Click += new System.EventHandler(this.leave_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 415);
            this.Controls.Add(this.leave_button);
            this.Controls.Add(this.scissors_button);
            this.Controls.Add(this.paper_button);
            this.Controls.Add(this.rock_button);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.disconnect_button);
            this.Controls.Add(this.name_textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.port_textBox);
            this.Controls.Add(this.ip_textBox);
            this.Controls.Add(this.connect_button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button connect_button;
        private System.Windows.Forms.TextBox ip_textBox;
        private System.Windows.Forms.TextBox port_textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox name_textBox;
        private System.Windows.Forms.Button disconnect_button;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Button rock_button;
        private System.Windows.Forms.Button paper_button;
        private System.Windows.Forms.Button scissors_button;
        private System.Windows.Forms.Button leave_button;
    }
}

