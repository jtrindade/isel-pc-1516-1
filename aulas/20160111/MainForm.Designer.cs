namespace GUI
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
            this.txtResult = new System.Windows.Forms.TextBox();
            this.prgFeedback = new System.Windows.Forms.ProgressBar();
            this.butDoWrong = new System.Windows.Forms.Button();
            this.butDoOldStyle = new System.Windows.Forms.Button();
            this.butJustDoIt = new System.Windows.Forms.Button();
            this.butDoItIllegal = new System.Windows.Forms.Button();
            this.butDoItRaw = new System.Windows.Forms.Button();
            this.bgwWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(13, 12);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(409, 20);
            this.txtResult.TabIndex = 0;
            // 
            // prgFeedback
            // 
            this.prgFeedback.Location = new System.Drawing.Point(13, 39);
            this.prgFeedback.Name = "prgFeedback";
            this.prgFeedback.Size = new System.Drawing.Size(409, 23);
            this.prgFeedback.TabIndex = 1;
            // 
            // butDoWrong
            // 
            this.butDoWrong.Location = new System.Drawing.Point(13, 69);
            this.butDoWrong.Name = "butDoWrong";
            this.butDoWrong.Size = new System.Drawing.Size(75, 23);
            this.butDoWrong.TabIndex = 2;
            this.butDoWrong.Text = "Do it wrong!";
            this.butDoWrong.UseVisualStyleBackColor = true;
            this.butDoWrong.Click += new System.EventHandler(this.butDoWrong_Click);
            // 
            // butDoOldStyle
            // 
            this.butDoOldStyle.Location = new System.Drawing.Point(256, 70);
            this.butDoOldStyle.Name = "butDoOldStyle";
            this.butDoOldStyle.Size = new System.Drawing.Size(85, 23);
            this.butDoOldStyle.TabIndex = 3;
            this.butDoOldStyle.Text = "Do it old style!";
            this.butDoOldStyle.UseVisualStyleBackColor = true;
            this.butDoOldStyle.Click += new System.EventHandler(this.butDoOldStyle_Click);
            // 
            // butJustDoIt
            // 
            this.butJustDoIt.Location = new System.Drawing.Point(347, 70);
            this.butJustDoIt.Name = "butJustDoIt";
            this.butJustDoIt.Size = new System.Drawing.Size(75, 23);
            this.butJustDoIt.TabIndex = 4;
            this.butJustDoIt.Text = "Do it now!";
            this.butJustDoIt.UseVisualStyleBackColor = true;
            this.butJustDoIt.Click += new System.EventHandler(this.butDoItNow_Click);
            // 
            // butDoItIllegal
            // 
            this.butDoItIllegal.Location = new System.Drawing.Point(94, 70);
            this.butDoItIllegal.Name = "butDoItIllegal";
            this.butDoItIllegal.Size = new System.Drawing.Size(75, 23);
            this.butDoItIllegal.TabIndex = 5;
            this.butDoItIllegal.Text = "Do it illegal!";
            this.butDoItIllegal.UseVisualStyleBackColor = true;
            this.butDoItIllegal.Click += new System.EventHandler(this.butDoItIllegal_Click);
            // 
            // butDoItRaw
            // 
            this.butDoItRaw.Location = new System.Drawing.Point(175, 70);
            this.butDoItRaw.Name = "butDoItRaw";
            this.butDoItRaw.Size = new System.Drawing.Size(75, 23);
            this.butDoItRaw.TabIndex = 6;
            this.butDoItRaw.Text = "Do it raw!";
            this.butDoItRaw.UseVisualStyleBackColor = true;
            this.butDoItRaw.Click += new System.EventHandler(this.butDoItRaw_Click);
            // 
            // bgwWorker
            // 
            this.bgwWorker.WorkerReportsProgress = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 105);
            this.Controls.Add(this.butDoItRaw);
            this.Controls.Add(this.butDoItIllegal);
            this.Controls.Add(this.butJustDoIt);
            this.Controls.Add(this.butDoOldStyle);
            this.Controls.Add(this.butDoWrong);
            this.Controls.Add(this.prgFeedback);
            this.Controls.Add(this.txtResult);
            this.Name = "MainForm";
            this.Text = "GUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.ProgressBar prgFeedback;
        private System.Windows.Forms.Button butDoWrong;
        private System.Windows.Forms.Button butDoOldStyle;
        private System.Windows.Forms.Button butJustDoIt;
        private System.Windows.Forms.Button butDoItIllegal;
        private System.Windows.Forms.Button butDoItRaw;
        private System.ComponentModel.BackgroundWorker bgwWorker;
    }
}

