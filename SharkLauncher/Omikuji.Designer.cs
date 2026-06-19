namespace SharkLauncher
{
    partial class Omikuji
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Omikuji));
            omikujiResult = new PictureBox();
            reOpenButton = new Button();
            ((System.ComponentModel.ISupportInitialize)omikujiResult).BeginInit();
            SuspendLayout();
            // 
            // omikujiResult
            // 
            omikujiResult.Location = new Point(12, 12);
            omikujiResult.Name = "omikujiResult";
            omikujiResult.Size = new Size(400, 200);
            omikujiResult.TabIndex = 0;
            omikujiResult.TabStop = false;
            // 
            // reOpenButton
            // 
            reOpenButton.Location = new Point(12, 218);
            reOpenButton.Name = "reOpenButton";
            reOpenButton.Size = new Size(75, 23);
            reOpenButton.TabIndex = 1;
            reOpenButton.Text = "再度引く";
            reOpenButton.UseVisualStyleBackColor = true;
            reOpenButton.Click += reOpenButton_Click;
            // 
            // Omikuji
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(427, 250);
            Controls.Add(reOpenButton);
            Controls.Add(omikujiResult);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Omikuji";
            Text = "おみくじを引く";
            Load += Omikuji_Load;
            ((System.ComponentModel.ISupportInitialize)omikujiResult).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox omikujiResult;
        private Button reOpenButton;
    }
}