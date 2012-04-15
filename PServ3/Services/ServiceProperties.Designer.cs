namespace pserv3.Services
{
    partial class ServiceProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceProperties));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.cbStartupTypes = new System.Windows.Forms.ComboBox();
            this.btChooseImagePath = new System.Windows.Forms.Button();
            this.tbImagePath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbPassword2 = new System.Windows.Forms.TextBox();
            this.tbPassword1 = new System.Windows.Forms.TextBox();
            this.tbAccountName = new System.Windows.Forms.TextBox();
            this.btThisAccount = new System.Windows.Forms.RadioButton();
            this.btInteractWithDesktop = new System.Windows.Forms.CheckBox();
            this.btSystemAccount = new System.Windows.Forms.RadioButton();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDisplayName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbServiceName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cbLoadOrderGroup = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btAccept = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(2, 11);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(473, 406);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.cbStartupTypes);
            this.tabPage1.Controls.Add(this.btChooseImagePath);
            this.tabPage1.Controls.Add(this.tbImagePath);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.tbDescription);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.tbDisplayName);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.tbServiceName);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(465, 380);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 203);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Startup Type:";
            // 
            // cbStartupTypes
            // 
            this.cbStartupTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbStartupTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStartupTypes.FormattingEnabled = true;
            this.cbStartupTypes.Location = new System.Drawing.Point(126, 201);
            this.cbStartupTypes.Name = "cbStartupTypes";
            this.cbStartupTypes.Size = new System.Drawing.Size(315, 21);
            this.cbStartupTypes.TabIndex = 10;
            // 
            // btChooseImagePath
            // 
            this.btChooseImagePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btChooseImagePath.Image = ((System.Drawing.Image)(resources.GetObject("btChooseImagePath.Image")));
            this.btChooseImagePath.Location = new System.Drawing.Point(420, 174);
            this.btChooseImagePath.Name = "btChooseImagePath";
            this.btChooseImagePath.Size = new System.Drawing.Size(25, 23);
            this.btChooseImagePath.TabIndex = 8;
            this.btChooseImagePath.UseVisualStyleBackColor = true;
            this.btChooseImagePath.Click += new System.EventHandler(this.btChooseImagePath_Click);
            // 
            // tbImagePath
            // 
            this.tbImagePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbImagePath.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.tbImagePath.Location = new System.Drawing.Point(126, 174);
            this.tbImagePath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbImagePath.Name = "tbImagePath";
            this.tbImagePath.Size = new System.Drawing.Size(293, 21);
            this.tbImagePath.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 177);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Image Path:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbPassword2);
            this.groupBox1.Controls.Add(this.tbPassword1);
            this.groupBox1.Controls.Add(this.tbAccountName);
            this.groupBox1.Controls.Add(this.btThisAccount);
            this.groupBox1.Controls.Add(this.btInteractWithDesktop);
            this.groupBox1.Controls.Add(this.btSystemAccount);
            this.groupBox1.Location = new System.Drawing.Point(5, 227);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 143);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log on as";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(49, 110);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Confirm:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 83);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Password:";
            // 
            // tbPassword2
            // 
            this.tbPassword2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassword2.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.tbPassword2.Location = new System.Drawing.Point(117, 106);
            this.tbPassword2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbPassword2.Name = "tbPassword2";
            this.tbPassword2.Size = new System.Drawing.Size(315, 21);
            this.tbPassword2.TabIndex = 7;
            this.tbPassword2.UseSystemPasswordChar = true;
            // 
            // tbPassword1
            // 
            this.tbPassword1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassword1.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.tbPassword1.Location = new System.Drawing.Point(117, 79);
            this.tbPassword1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbPassword1.Name = "tbPassword1";
            this.tbPassword1.Size = new System.Drawing.Size(315, 21);
            this.tbPassword1.TabIndex = 5;
            this.tbPassword1.UseSystemPasswordChar = true;
            // 
            // tbAccountName
            // 
            this.tbAccountName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAccountName.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.tbAccountName.Location = new System.Drawing.Point(117, 52);
            this.tbAccountName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbAccountName.Name = "tbAccountName";
            this.tbAccountName.Size = new System.Drawing.Size(315, 21);
            this.tbAccountName.TabIndex = 3;
            // 
            // btThisAccount
            // 
            this.btThisAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btThisAccount.AutoSize = true;
            this.btThisAccount.Location = new System.Drawing.Point(11, 54);
            this.btThisAccount.Name = "btThisAccount";
            this.btThisAccount.Size = new System.Drawing.Size(97, 17);
            this.btThisAccount.TabIndex = 2;
            this.btThisAccount.TabStop = true;
            this.btThisAccount.Text = "This Account";
            this.btThisAccount.UseVisualStyleBackColor = true;
            this.btThisAccount.Click += new System.EventHandler(this.btThisAccount_Click);
            // 
            // btInteractWithDesktop
            // 
            this.btInteractWithDesktop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btInteractWithDesktop.AutoSize = true;
            this.btInteractWithDesktop.Location = new System.Drawing.Point(138, 21);
            this.btInteractWithDesktop.Name = "btInteractWithDesktop";
            this.btInteractWithDesktop.Size = new System.Drawing.Size(147, 17);
            this.btInteractWithDesktop.TabIndex = 1;
            this.btInteractWithDesktop.Text = "Interact with desktop";
            this.btInteractWithDesktop.UseVisualStyleBackColor = true;
            // 
            // btSystemAccount
            // 
            this.btSystemAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btSystemAccount.AutoSize = true;
            this.btSystemAccount.Location = new System.Drawing.Point(11, 21);
            this.btSystemAccount.Name = "btSystemAccount";
            this.btSystemAccount.Size = new System.Drawing.Size(117, 17);
            this.btSystemAccount.TabIndex = 0;
            this.btSystemAccount.TabStop = true;
            this.btSystemAccount.Text = "System Account";
            this.btSystemAccount.UseVisualStyleBackColor = true;
            this.btSystemAccount.Click += new System.EventHandler(this.btSystemAccount_Click);
            // 
            // tbDescription
            // 
            this.tbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDescription.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.tbDescription.Location = new System.Drawing.Point(126, 59);
            this.tbDescription.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(315, 109);
            this.tbDescription.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 63);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Description:";
            // 
            // tbDisplayName
            // 
            this.tbDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDisplayName.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.tbDisplayName.Location = new System.Drawing.Point(126, 32);
            this.tbDisplayName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbDisplayName.Name = "tbDisplayName";
            this.tbDisplayName.Size = new System.Drawing.Size(315, 21);
            this.tbDisplayName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 36);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Display Name:";
            // 
            // tbServiceName
            // 
            this.tbServiceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServiceName.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.tbServiceName.Location = new System.Drawing.Point(126, 6);
            this.tbServiceName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbServiceName.Name = "tbServiceName";
            this.tbServiceName.ReadOnly = true;
            this.tbServiceName.Size = new System.Drawing.Size(315, 21);
            this.tbServiceName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Service Name:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cbLoadOrderGroup);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(465, 380);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Details";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cbLoadOrderGroup
            // 
            this.cbLoadOrderGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLoadOrderGroup.FormattingEnabled = true;
            this.cbLoadOrderGroup.Location = new System.Drawing.Point(150, 7);
            this.cbLoadOrderGroup.Name = "cbLoadOrderGroup";
            this.cbLoadOrderGroup.Size = new System.Drawing.Size(309, 21);
            this.cbLoadOrderGroup.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(29, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(115, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Load Order Group:";
            // 
            // btAccept
            // 
            this.btAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAccept.Image = ((System.Drawing.Image)(resources.GetObject("btAccept.Image")));
            this.btAccept.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btAccept.Location = new System.Drawing.Point(249, 422);
            this.btAccept.Name = "btAccept";
            this.btAccept.Size = new System.Drawing.Size(108, 30);
            this.btAccept.TabIndex = 0;
            this.btAccept.Text = "&OK";
            this.btAccept.UseVisualStyleBackColor = true;
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Image = ((System.Drawing.Image)(resources.GetObject("btCancel.Image")));
            this.btCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btCancel.Location = new System.Drawing.Point(363, 422);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(110, 30);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "&Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // ServiceProperties
            // 
            this.AcceptButton = this.btAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(480, 460);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btAccept);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ServiceProperties";
            this.Text = "Properties";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPassword2;
        private System.Windows.Forms.TextBox tbPassword1;
        private System.Windows.Forms.TextBox tbAccountName;
        private System.Windows.Forms.RadioButton btThisAccount;
        private System.Windows.Forms.CheckBox btInteractWithDesktop;
        private System.Windows.Forms.RadioButton btSystemAccount;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDisplayName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbServiceName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btAccept;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbStartupTypes;
        private System.Windows.Forms.Button btChooseImagePath;
        private System.Windows.Forms.TextBox tbImagePath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbLoadOrderGroup;
        private System.Windows.Forms.Label label8;

    }
}