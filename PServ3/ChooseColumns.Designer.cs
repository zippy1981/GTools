namespace pserv3
{
    partial class ChooseColumns
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseColumns));
            this.lbAvailableColumns = new System.Windows.Forms.ListBox();
            this.lbVisibleColumns = new System.Windows.Forms.ListBox();
            this.btShow = new System.Windows.Forms.Button();
            this.btHide = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btTop = new System.Windows.Forms.Button();
            this.btUp = new System.Windows.Forms.Button();
            this.btDown = new System.Windows.Forms.Button();
            this.btBottom = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbAvailableColumns
            // 
            this.lbAvailableColumns.FormattingEnabled = true;
            this.lbAvailableColumns.Location = new System.Drawing.Point(10, 38);
            this.lbAvailableColumns.Name = "lbAvailableColumns";
            this.lbAvailableColumns.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAvailableColumns.Size = new System.Drawing.Size(179, 199);
            this.lbAvailableColumns.TabIndex = 1;
            this.lbAvailableColumns.DoubleClick += new System.EventHandler(this.btShow_Click);
            // 
            // lbVisibleColumns
            // 
            this.lbVisibleColumns.FormattingEnabled = true;
            this.lbVisibleColumns.Location = new System.Drawing.Point(281, 38);
            this.lbVisibleColumns.Name = "lbVisibleColumns";
            this.lbVisibleColumns.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbVisibleColumns.Size = new System.Drawing.Size(179, 199);
            this.lbVisibleColumns.TabIndex = 5;
            this.lbVisibleColumns.DoubleClick += new System.EventHandler(this.btHide_Click);
            // 
            // btShow
            // 
            this.btShow.Image = ((System.Drawing.Image)(resources.GetObject("btShow.Image")));
            this.btShow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btShow.Location = new System.Drawing.Point(194, 99);
            this.btShow.Name = "btShow";
            this.btShow.Size = new System.Drawing.Size(81, 27);
            this.btShow.TabIndex = 2;
            this.btShow.Text = "&Show";
            this.btShow.UseVisualStyleBackColor = true;
            this.btShow.Click += new System.EventHandler(this.btShow_Click);
            // 
            // btHide
            // 
            this.btHide.Image = ((System.Drawing.Image)(resources.GetObject("btHide.Image")));
            this.btHide.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btHide.Location = new System.Drawing.Point(194, 132);
            this.btHide.Name = "btHide";
            this.btHide.Size = new System.Drawing.Size(81, 27);
            this.btHide.TabIndex = 3;
            this.btHide.Text = "&Hide ";
            this.btHide.UseVisualStyleBackColor = true;
            this.btHide.Click += new System.EventHandler(this.btHide_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Available Columns";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(278, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "&Visible Columns";
            // 
            // btTop
            // 
            this.btTop.Image = ((System.Drawing.Image)(resources.GetObject("btTop.Image")));
            this.btTop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btTop.Location = new System.Drawing.Point(465, 69);
            this.btTop.Name = "btTop";
            this.btTop.Size = new System.Drawing.Size(89, 27);
            this.btTop.TabIndex = 6;
            this.btTop.Text = "&Top";
            this.btTop.UseVisualStyleBackColor = true;
            this.btTop.Click += new System.EventHandler(this.btTop_Click);
            // 
            // btUp
            // 
            this.btUp.Image = ((System.Drawing.Image)(resources.GetObject("btUp.Image")));
            this.btUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btUp.Location = new System.Drawing.Point(465, 101);
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(89, 27);
            this.btUp.TabIndex = 7;
            this.btUp.Text = "&Up";
            this.btUp.UseVisualStyleBackColor = true;
            this.btUp.Click += new System.EventHandler(this.btUp_Click);
            // 
            // btDown
            // 
            this.btDown.Image = ((System.Drawing.Image)(resources.GetObject("btDown.Image")));
            this.btDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btDown.Location = new System.Drawing.Point(465, 134);
            this.btDown.Name = "btDown";
            this.btDown.Size = new System.Drawing.Size(89, 27);
            this.btDown.TabIndex = 8;
            this.btDown.Text = "&Down";
            this.btDown.UseVisualStyleBackColor = true;
            this.btDown.Click += new System.EventHandler(this.btDown_Click);
            // 
            // btBottom
            // 
            this.btBottom.Image = ((System.Drawing.Image)(resources.GetObject("btBottom.Image")));
            this.btBottom.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btBottom.Location = new System.Drawing.Point(465, 166);
            this.btBottom.Name = "btBottom";
            this.btBottom.Size = new System.Drawing.Size(89, 27);
            this.btBottom.TabIndex = 9;
            this.btBottom.Text = "&Bottom";
            this.btBottom.UseVisualStyleBackColor = true;
            this.btBottom.Click += new System.EventHandler(this.btBottom_Click);
            // 
            // btOK
            // 
            this.btOK.Image = ((System.Drawing.Image)(resources.GetObject("btOK.Image")));
            this.btOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOK.Location = new System.Drawing.Point(371, 242);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(88, 26);
            this.btOK.TabIndex = 10;
            this.btOK.Text = "&OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Image = ((System.Drawing.Image)(resources.GetObject("btCancel.Image")));
            this.btCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btCancel.Location = new System.Drawing.Point(465, 242);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(89, 26);
            this.btCancel.TabIndex = 11;
            this.btCancel.Text = "&Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // ChooseColumns
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(564, 277);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btBottom);
            this.Controls.Add(this.btDown);
            this.Controls.Add(this.btUp);
            this.Controls.Add(this.btTop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btHide);
            this.Controls.Add(this.btShow);
            this.Controls.Add(this.lbVisibleColumns);
            this.Controls.Add(this.lbAvailableColumns);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChooseColumns";
            this.Text = "Choose Columns";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbAvailableColumns;
        private System.Windows.Forms.ListBox lbVisibleColumns;
        private System.Windows.Forms.Button btShow;
        private System.Windows.Forms.Button btHide;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btTop;
        private System.Windows.Forms.Button btUp;
        private System.Windows.Forms.Button btDown;
        private System.Windows.Forms.Button btBottom;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
    }
}