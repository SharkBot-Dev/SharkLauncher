namespace SharkLauncher
{
    partial class inviteSharkBot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(inviteSharkBot));
            selectGuilds = new ListBox();
            actions = new Panel();
            cancel = new Button();
            invite = new Button();
            actions.SuspendLayout();
            SuspendLayout();
            // 
            // selectGuilds
            // 
            selectGuilds.BackColor = SystemColors.ActiveCaption;
            selectGuilds.Dock = DockStyle.Fill;
            selectGuilds.FormattingEnabled = true;
            selectGuilds.Location = new Point(0, 0);
            selectGuilds.Name = "selectGuilds";
            selectGuilds.Size = new Size(339, 450);
            selectGuilds.TabIndex = 0;
            // 
            // actions
            // 
            actions.Controls.Add(cancel);
            actions.Controls.Add(invite);
            actions.Dock = DockStyle.Bottom;
            actions.Location = new Point(0, 390);
            actions.Name = "actions";
            actions.Size = new Size(339, 60);
            actions.TabIndex = 1;
            // 
            // cancel
            // 
            cancel.Dock = DockStyle.Right;
            cancel.Location = new Point(169, 0);
            cancel.Name = "cancel";
            cancel.Size = new Size(170, 60);
            cancel.TabIndex = 1;
            cancel.Text = "キャンセル";
            cancel.UseVisualStyleBackColor = true;
            cancel.Click += cancel_Click;
            // 
            // invite
            // 
            invite.Dock = DockStyle.Left;
            invite.Location = new Point(0, 0);
            invite.Name = "invite";
            invite.Size = new Size(163, 60);
            invite.TabIndex = 0;
            invite.Text = "招待する";
            invite.UseVisualStyleBackColor = true;
            invite.Click += invite_Click;
            // 
            // inviteSharkBot
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(339, 450);
            Controls.Add(actions);
            Controls.Add(selectGuilds);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "inviteSharkBot";
            Text = "SharkBotを招待する";
            Load += inviteSharkBot_Load;
            actions.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ListBox selectGuilds;
        private Panel actions;
        private Button cancel;
        private Button invite;
    }
}