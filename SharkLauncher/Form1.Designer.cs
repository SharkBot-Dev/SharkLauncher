namespace SharkLauncher
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            sidebar = new Panel();
            inviteSharkBot = new Button();
            userId = new Label();
            userName = new Label();
            avatarImage = new PictureBox();
            loginButton = new Button();
            main = new Panel();
            sidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)avatarImage).BeginInit();
            SuspendLayout();
            // 
            // sidebar
            // 
            sidebar.BackColor = SystemColors.ActiveCaption;
            sidebar.Controls.Add(inviteSharkBot);
            sidebar.Controls.Add(userId);
            sidebar.Controls.Add(userName);
            sidebar.Controls.Add(avatarImage);
            sidebar.Controls.Add(loginButton);
            sidebar.Dock = DockStyle.Left;
            sidebar.Location = new Point(0, 0);
            sidebar.Name = "sidebar";
            sidebar.Size = new Size(200, 450);
            sidebar.TabIndex = 0;
            // 
            // inviteSharkBot
            // 
            inviteSharkBot.Dock = DockStyle.Top;
            inviteSharkBot.Location = new Point(0, 46);
            inviteSharkBot.Name = "inviteSharkBot";
            inviteSharkBot.Size = new Size(200, 46);
            inviteSharkBot.TabIndex = 4;
            inviteSharkBot.Text = "SharkBotを招待する";
            inviteSharkBot.UseVisualStyleBackColor = true;
            inviteSharkBot.Click += inviteSharkBot_Click;
            // 
            // userId
            // 
            userId.AutoSize = true;
            userId.Location = new Point(56, 426);
            userId.Name = "userId";
            userId.Size = new Size(22, 15);
            userId.TabIndex = 3;
            userId.Text = "???";
            // 
            // userName
            // 
            userName.AutoSize = true;
            userName.Location = new Point(56, 409);
            userName.Name = "userName";
            userName.Size = new Size(54, 15);
            userName.TabIndex = 2;
            userName.Text = "未ログイン";
            // 
            // avatarImage
            // 
            avatarImage.Location = new Point(0, 400);
            avatarImage.Name = "avatarImage";
            avatarImage.Size = new Size(50, 50);
            avatarImage.TabIndex = 1;
            avatarImage.TabStop = false;
            // 
            // loginButton
            // 
            loginButton.Dock = DockStyle.Top;
            loginButton.Location = new Point(0, 0);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(200, 46);
            loginButton.TabIndex = 0;
            loginButton.Text = "🔑ログインする";
            loginButton.UseVisualStyleBackColor = true;
            loginButton.Click += loginButton_Click;
            // 
            // main
            // 
            main.BackColor = SystemColors.GradientActiveCaption;
            main.Dock = DockStyle.Fill;
            main.Location = new Point(200, 0);
            main.Name = "main";
            main.Size = new Size(600, 450);
            main.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(main);
            Controls.Add(sidebar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "SharkLauncher";
            Load += Form1_Load;
            sidebar.ResumeLayout(false);
            sidebar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)avatarImage).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel sidebar;
        private Panel main;
        private Button loginButton;
        private Label userName;
        private PictureBox avatarImage;
        private Label userId;
        private Button inviteSharkBot;
    }
}
