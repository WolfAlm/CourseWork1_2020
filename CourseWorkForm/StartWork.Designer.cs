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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartWork));
            this.logBot = new System.Windows.Forms.RichTextBox();
            this.labelStopwatch = new System.Windows.Forms.Label();
            this.stopwatch = new System.Windows.Forms.Timer(this.components);
            this.startedBot = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // logBot
            // 
            this.logBot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logBot.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.logBot.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.logBot.Location = new System.Drawing.Point(15, 38);
            this.logBot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.logBot.Name = "logBot";
            this.logBot.ReadOnly = true;
            this.logBot.Size = new System.Drawing.Size(1019, 549);
            this.logBot.TabIndex = 0;
            this.logBot.Text = "";
            // 
            // labelStopwatch
            // 
            this.labelStopwatch.AutoSize = true;
            this.labelStopwatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelStopwatch.Location = new System.Drawing.Point(12, 5);
            this.labelStopwatch.Name = "labelStopwatch";
            this.labelStopwatch.Size = new System.Drawing.Size(333, 31);
            this.labelStopwatch.TabIndex = 1;
            this.labelStopwatch.Text = "Время работы: {15:05:32}";
            // 
            // stopwatch
            // 
            this.stopwatch.Interval = 1;
            // 
            // startedBot
            // 
            this.startedBot.AutoSize = true;
            this.startedBot.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.startedBot.Location = new System.Drawing.Point(394, 5);
            this.startedBot.Name = "startedBot";
            this.startedBot.Size = new System.Drawing.Size(365, 31);
            this.startedBot.TabIndex = 2;
            this.startedBot.Text = "БОТ УСПЕШНО ЗАПУЩЕН!";
            // 
            // StartWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1049, 606);
            this.Controls.Add(this.startedBot);
            this.Controls.Add(this.labelStopwatch);
            this.Controls.Add(this.logBot);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "StartWork";
            this.Text = "OftenColorBot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BotForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox logBot;
        private System.Windows.Forms.Label labelStopwatch;
        private System.Windows.Forms.Timer stopwatch;
        private System.Windows.Forms.Label startedBot;
    }
}

