namespace CourseWorkForm
{
    partial class StartWork
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
            this.components = new System.ComponentModel.Container();
            this.logBot = new System.Windows.Forms.RichTextBox();
            this.labelStopwatch = new System.Windows.Forms.Label();
            this.stopwatch = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // logBot
            // 
            this.logBot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logBot.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.logBot.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.logBot.Location = new System.Drawing.Point(11, 31);
            this.logBot.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.logBot.Name = "logBot";
            this.logBot.ReadOnly = true;
            this.logBot.Size = new System.Drawing.Size(765, 447);
            this.logBot.TabIndex = 0;
            this.logBot.Text = "";
            // 
            // labelStopwatch
            // 
            this.labelStopwatch.AutoSize = true;
            this.labelStopwatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelStopwatch.Location = new System.Drawing.Point(9, 4);
            this.labelStopwatch.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStopwatch.Name = "labelStopwatch";
            this.labelStopwatch.Size = new System.Drawing.Size(265, 25);
            this.labelStopwatch.TabIndex = 1;
            this.labelStopwatch.Text = "Время работы: {15:05:32}";
            // 
            // stopwatch
            // 
            this.stopwatch.Interval = 1;
            // 
            // StartWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 492);
            this.Controls.Add(this.labelStopwatch);
            this.Controls.Add(this.logBot);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "StartWork";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox logBot;
        private System.Windows.Forms.Label labelStopwatch;
        private System.Windows.Forms.Timer stopwatch;
    }
}

