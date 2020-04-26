namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.Pixels = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.radioClusters = new System.Windows.Forms.RadioButton();
            this.numClusters = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Pixels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numClusters)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(16, 15);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(363, 191);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            this.richTextBox1.Visible = false;
            // 
            // Pixels
            // 
            this.Pixels.BackColor = System.Drawing.Color.Yellow;
            this.Pixels.Location = new System.Drawing.Point(13, 214);
            this.Pixels.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Pixels.Name = "Pixels";
            this.Pixels.Size = new System.Drawing.Size(351, 646);
            this.Pixels.TabIndex = 7;
            this.Pixels.TabStop = false;
            this.Pixels.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(279, -1);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 449);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // radioClusters
            // 
            this.radioClusters.AutoSize = true;
            this.radioClusters.Location = new System.Drawing.Point(102, 23);
            this.radioClusters.Name = "radioClusters";
            this.radioClusters.Size = new System.Drawing.Size(74, 20);
            this.radioClusters.TabIndex = 9;
            this.radioClusters.TabStop = true;
            this.radioClusters.Text = "Clusters";
            this.radioClusters.UseVisualStyleBackColor = true;
            // 
            // numClusters
            // 
            this.numClusters.Location = new System.Drawing.Point(84, 49);
            this.numClusters.Name = "numClusters";
            this.numClusters.Size = new System.Drawing.Size(120, 22);
            this.numClusters.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(146, 87);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Reset";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(53, 87);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "Run";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1794, 778);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.numClusters);
            this.Controls.Add(this.radioClusters);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Pixels);
            this.Controls.Add(this.richTextBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Pixels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numClusters)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.PictureBox Pixels;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton radioClusters;
        private System.Windows.Forms.NumericUpDown numClusters;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

