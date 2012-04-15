namespace DLLUsage
{
    partial class DLLUsageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DLLUsageForm));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btListByDLL = new System.Windows.Forms.ToolStripButton();
            this.btListByDLLPath = new System.Windows.Forms.ToolStripButton();
            this.btListByProcessName = new System.Windows.Forms.ToolStripButton();
            this.btListByProcessPath = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btRefresh = new System.Windows.Forms.ToolStripButton();
            this.btExpandAll = new System.Windows.Forms.ToolStripButton();
            this.btCollapseAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDependencies = new System.Windows.Forms.ToolStripButton();
            this.tsbConsole = new System.Windows.Forms.ToolStripButton();
            this.tsbExplorer = new System.Windows.Forms.ToolStripButton();
            this.tsbCopyToClipboard = new System.Windows.Forms.ToolStripButton();
            this.tsbCopyToClipboardInQuotationmarks = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmFile_SaveAsXML = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmFile_Print = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmFile_PrintPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmFile_PageSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmFile_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmActions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDependencies = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmConsole = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToClipboardInquotationmarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmView_ExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmView_CollapseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiViewRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutPserv3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Location = new System.Drawing.Point(-1, 52);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(1009, 677);
            this.treeView1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btListByDLL,
            this.btListByDLLPath,
            this.btListByProcessName,
            this.btListByProcessPath,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.toolStripComboBox1,
            this.toolStripSeparator1,
            this.btRefresh,
            this.btExpandAll,
            this.btCollapseAll,
            this.toolStripSeparator7,
            this.tsbDependencies,
            this.tsbConsole,
            this.tsbExplorer,
            this.tsbCopyToClipboard,
            this.tsbCopyToClipboardInQuotationmarks});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1008, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btListByDLL
            // 
            this.btListByDLL.Image = ((System.Drawing.Image)(resources.GetObject("btListByDLL.Image")));
            this.btListByDLL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btListByDLL.Name = "btListByDLL";
            this.btListByDLL.Size = new System.Drawing.Size(82, 22);
            this.btListByDLL.Text = "DLL Name";
            this.btListByDLL.ToolTipText = "List by DLL name";
            this.btListByDLL.Click += new System.EventHandler(this.btListByDLL_Click);
            // 
            // btListByDLLPath
            // 
            this.btListByDLLPath.Image = ((System.Drawing.Image)(resources.GetObject("btListByDLLPath.Image")));
            this.btListByDLLPath.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btListByDLLPath.Name = "btListByDLLPath";
            this.btListByDLLPath.Size = new System.Drawing.Size(74, 22);
            this.btListByDLLPath.Text = "DLL Path";
            this.btListByDLLPath.Click += new System.EventHandler(this.btListByDLLPath_Click);
            // 
            // btListByProcessName
            // 
            this.btListByProcessName.Image = ((System.Drawing.Image)(resources.GetObject("btListByProcessName.Image")));
            this.btListByProcessName.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btListByProcessName.Name = "btListByProcessName";
            this.btListByProcessName.Size = new System.Drawing.Size(81, 22);
            this.btListByProcessName.Text = "EXE Name";
            this.btListByProcessName.Click += new System.EventHandler(this.btListByProcessName_Click);
            // 
            // btListByProcessPath
            // 
            this.btListByProcessPath.Image = ((System.Drawing.Image)(resources.GetObject("btListByProcessPath.Image")));
            this.btListByProcessPath.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btListByProcessPath.Name = "btListByProcessPath";
            this.btListByProcessPath.Size = new System.Drawing.Size(73, 22);
            this.btListByProcessPath.Text = "EXE Path";
            this.btListByProcessPath.Click += new System.EventHandler(this.btListByProcessPath_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(36, 22);
            this.toolStripLabel1.Text = "Filter:";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(200, 25);
            this.toolStripComboBox1.TextChanged += new System.EventHandler(this.toolStripComboBox1_TextChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btRefresh
            // 
            this.btRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btRefresh.Image")));
            this.btRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(23, 22);
            this.btRefresh.Text = "Refresh";
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // btExpandAll
            // 
            this.btExpandAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btExpandAll.Image = ((System.Drawing.Image)(resources.GetObject("btExpandAll.Image")));
            this.btExpandAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btExpandAll.Name = "btExpandAll";
            this.btExpandAll.Size = new System.Drawing.Size(23, 22);
            this.btExpandAll.Text = "Expand all";
            this.btExpandAll.Click += new System.EventHandler(this.btExpandAll_Click);
            // 
            // btCollapseAll
            // 
            this.btCollapseAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btCollapseAll.Image = ((System.Drawing.Image)(resources.GetObject("btCollapseAll.Image")));
            this.btCollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btCollapseAll.Name = "btCollapseAll";
            this.btCollapseAll.Size = new System.Drawing.Size(23, 22);
            this.btCollapseAll.Text = "Collapse all";
            this.btCollapseAll.Click += new System.EventHandler(this.btCollapseAll_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbDependencies
            // 
            this.tsbDependencies.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDependencies.Image = ((System.Drawing.Image)(resources.GetObject("tsbDependencies.Image")));
            this.tsbDependencies.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDependencies.Name = "tsbDependencies";
            this.tsbDependencies.Size = new System.Drawing.Size(23, 22);
            this.tsbDependencies.Text = "Dependencies";
            this.tsbDependencies.Click += new System.EventHandler(this.tsmDependencies_Click);
            // 
            // tsbConsole
            // 
            this.tsbConsole.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbConsole.Image = ((System.Drawing.Image)(resources.GetObject("tsbConsole.Image")));
            this.tsbConsole.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbConsole.Name = "tsbConsole";
            this.tsbConsole.Size = new System.Drawing.Size(23, 22);
            this.tsbConsole.Text = "Console";
            this.tsbConsole.Click += new System.EventHandler(this.tsmConsole_Click);
            // 
            // tsbExplorer
            // 
            this.tsbExplorer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExplorer.Image = ((System.Drawing.Image)(resources.GetObject("tsbExplorer.Image")));
            this.tsbExplorer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExplorer.Name = "tsbExplorer";
            this.tsbExplorer.Size = new System.Drawing.Size(23, 22);
            this.tsbExplorer.Text = "Explorer";
            this.tsbExplorer.Click += new System.EventHandler(this.tsmExplorer_Click);
            // 
            // tsbCopyToClipboard
            // 
            this.tsbCopyToClipboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCopyToClipboard.Image = ((System.Drawing.Image)(resources.GetObject("tsbCopyToClipboard.Image")));
            this.tsbCopyToClipboard.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopyToClipboard.Name = "tsbCopyToClipboard";
            this.tsbCopyToClipboard.Size = new System.Drawing.Size(23, 22);
            this.tsbCopyToClipboard.Text = "Copy to clipboard";
            this.tsbCopyToClipboard.Click += new System.EventHandler(this.copyToClipboardToolStripMenuItem_Click);
            // 
            // tsbCopyToClipboardInQuotationmarks
            // 
            this.tsbCopyToClipboardInQuotationmarks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCopyToClipboardInQuotationmarks.Image = ((System.Drawing.Image)(resources.GetObject("tsbCopyToClipboardInQuotationmarks.Image")));
            this.tsbCopyToClipboardInQuotationmarks.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopyToClipboardInQuotationmarks.Name = "tsbCopyToClipboardInQuotationmarks";
            this.tsbCopyToClipboardInQuotationmarks.Size = new System.Drawing.Size(23, 22);
            this.tsbCopyToClipboardInQuotationmarks.Text = "Copy to clipboard in quotation marks";
            this.tsbCopyToClipboardInQuotationmarks.Click += new System.EventHandler(this.copyToClipboardInquotationmarksToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.tsmActions,
            this.toolStripMenuItem2,
            this.viewToolStripMenuItem,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmFile_SaveAsXML,
            this.toolStripSeparator8,
            this.tsmFile_Print,
            this.tsmFile_PrintPreview,
            this.tsmFile_PageSetup,
            this.toolStripSeparator6,
            this.tsmFile_Exit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // tsmFile_SaveAsXML
            // 
            this.tsmFile_SaveAsXML.Image = ((System.Drawing.Image)(resources.GetObject("tsmFile_SaveAsXML.Image")));
            this.tsmFile_SaveAsXML.Name = "tsmFile_SaveAsXML";
            this.tsmFile_SaveAsXML.Size = new System.Drawing.Size(152, 22);
            this.tsmFile_SaveAsXML.Text = "&Save as XML";
            this.tsmFile_SaveAsXML.Click += new System.EventHandler(this.tsmFile_SaveAsXML_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(149, 6);
            // 
            // tsmFile_Print
            // 
            this.tsmFile_Print.Image = ((System.Drawing.Image)(resources.GetObject("tsmFile_Print.Image")));
            this.tsmFile_Print.Name = "tsmFile_Print";
            this.tsmFile_Print.Size = new System.Drawing.Size(152, 22);
            this.tsmFile_Print.Text = "&Print...";
            this.tsmFile_Print.Click += new System.EventHandler(this.tsmFile_Print_Click);
            // 
            // tsmFile_PrintPreview
            // 
            this.tsmFile_PrintPreview.Image = ((System.Drawing.Image)(resources.GetObject("tsmFile_PrintPreview.Image")));
            this.tsmFile_PrintPreview.Name = "tsmFile_PrintPreview";
            this.tsmFile_PrintPreview.Size = new System.Drawing.Size(152, 22);
            this.tsmFile_PrintPreview.Text = "Print Pre&view...";
            this.tsmFile_PrintPreview.Click += new System.EventHandler(this.tsmFile_PrintPreview_Click);
            // 
            // tsmFile_PageSetup
            // 
            this.tsmFile_PageSetup.Image = ((System.Drawing.Image)(resources.GetObject("tsmFile_PageSetup.Image")));
            this.tsmFile_PageSetup.Name = "tsmFile_PageSetup";
            this.tsmFile_PageSetup.Size = new System.Drawing.Size(152, 22);
            this.tsmFile_PageSetup.Text = "P&age Setup...";
            this.tsmFile_PageSetup.Click += new System.EventHandler(this.tsmFile_PageSetup_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(149, 6);
            // 
            // tsmFile_Exit
            // 
            this.tsmFile_Exit.Image = ((System.Drawing.Image)(resources.GetObject("tsmFile_Exit.Image")));
            this.tsmFile_Exit.Name = "tsmFile_Exit";
            this.tsmFile_Exit.Size = new System.Drawing.Size(152, 22);
            this.tsmFile_Exit.Text = "E&xit";
            this.tsmFile_Exit.Click += new System.EventHandler(this.tsmFile_Exit_Click);
            // 
            // tsmActions
            // 
            this.tsmActions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmDependencies,
            this.tsmConsole,
            this.tsmExplorer,
            this.toolStripSeparator3,
            this.copyToClipboardToolStripMenuItem,
            this.copyToClipboardInquotationmarksToolStripMenuItem});
            this.tsmActions.Name = "tsmActions";
            this.tsmActions.Size = new System.Drawing.Size(59, 20);
            this.tsmActions.Text = "&Actions";
            // 
            // tsmDependencies
            // 
            this.tsmDependencies.Image = ((System.Drawing.Image)(resources.GetObject("tsmDependencies.Image")));
            this.tsmDependencies.Name = "tsmDependencies";
            this.tsmDependencies.Size = new System.Drawing.Size(269, 22);
            this.tsmDependencies.Text = "&Show dependencies...";
            this.tsmDependencies.Click += new System.EventHandler(this.tsmDependencies_Click);
            // 
            // tsmConsole
            // 
            this.tsmConsole.Image = ((System.Drawing.Image)(resources.GetObject("tsmConsole.Image")));
            this.tsmConsole.Name = "tsmConsole";
            this.tsmConsole.Size = new System.Drawing.Size(269, 22);
            this.tsmConsole.Text = "&Console...";
            this.tsmConsole.Click += new System.EventHandler(this.tsmConsole_Click);
            // 
            // tsmExplorer
            // 
            this.tsmExplorer.Image = ((System.Drawing.Image)(resources.GetObject("tsmExplorer.Image")));
            this.tsmExplorer.Name = "tsmExplorer";
            this.tsmExplorer.Size = new System.Drawing.Size(269, 22);
            this.tsmExplorer.Text = "&Explorer...";
            this.tsmExplorer.Click += new System.EventHandler(this.tsmExplorer_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(266, 6);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToClipboardToolStripMenuItem.Image")));
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.copyToClipboardToolStripMenuItem.Text = "C&opy to clipboard";
            this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyToClipboardToolStripMenuItem_Click);
            // 
            // copyToClipboardInquotationmarksToolStripMenuItem
            // 
            this.copyToClipboardInquotationmarksToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToClipboardInquotationmarksToolStripMenuItem.Image")));
            this.copyToClipboardInquotationmarksToolStripMenuItem.Name = "copyToClipboardInquotationmarksToolStripMenuItem";
            this.copyToClipboardInquotationmarksToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.copyToClipboardInquotationmarksToolStripMenuItem.Text = "Copy to clipboard in &quotationmarks";
            this.copyToClipboardInquotationmarksToolStripMenuItem.Click += new System.EventHandler(this.copyToClipboardInquotationmarksToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fontToolStripMenuItem});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(61, 20);
            this.toolStripMenuItem2.Text = "&Options";
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fontToolStripMenuItem.Image")));
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fontToolStripMenuItem.Text = "&Font...";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.fontToolStripButton_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmView_ExpandAll,
            this.tsmView_CollapseAll,
            this.toolStripSeparator5,
            this.tsmiViewRefresh});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // tsmView_ExpandAll
            // 
            this.tsmView_ExpandAll.Image = ((System.Drawing.Image)(resources.GetObject("tsmView_ExpandAll.Image")));
            this.tsmView_ExpandAll.Name = "tsmView_ExpandAll";
            this.tsmView_ExpandAll.Size = new System.Drawing.Size(152, 22);
            this.tsmView_ExpandAll.Text = "&Expand all";
            this.tsmView_ExpandAll.Click += new System.EventHandler(this.btExpandAll_Click);
            // 
            // tsmView_CollapseAll
            // 
            this.tsmView_CollapseAll.Image = ((System.Drawing.Image)(resources.GetObject("tsmView_CollapseAll.Image")));
            this.tsmView_CollapseAll.Name = "tsmView_CollapseAll";
            this.tsmView_CollapseAll.Size = new System.Drawing.Size(152, 22);
            this.tsmView_CollapseAll.Text = "&Collapse all";
            this.tsmView_CollapseAll.Click += new System.EventHandler(this.btCollapseAll_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(149, 6);
            // 
            // tsmiViewRefresh
            // 
            this.tsmiViewRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tsmiViewRefresh.Image")));
            this.tsmiViewRefresh.Name = "tsmiViewRefresh";
            this.tsmiViewRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.tsmiViewRefresh.Size = new System.Drawing.Size(152, 22);
            this.tsmiViewRefresh.Text = "&Refresh";
            this.tsmiViewRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutPserv3ToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(24, 20);
            this.toolStripMenuItem1.Text = "&?";
            // 
            // aboutPserv3ToolStripMenuItem
            // 
            this.aboutPserv3ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutPserv3ToolStripMenuItem.Image")));
            this.aboutPserv3ToolStripMenuItem.Name = "aboutPserv3ToolStripMenuItem";
            this.aboutPserv3ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.aboutPserv3ToolStripMenuItem.Text = "&About DLLUSAGE";
            this.aboutPserv3ToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripButton_Click);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // DLLUsageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.treeView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DLLUsageForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DLLUsageForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripButton btRefresh;
        private System.Windows.Forms.ToolStripButton btListByDLL;
        private System.Windows.Forms.ToolStripButton btListByProcessName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btExpandAll;
        private System.Windows.Forms.ToolStripButton btCollapseAll;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmFile_SaveAsXML;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem tsmFile_Print;
        private System.Windows.Forms.ToolStripMenuItem tsmFile_PrintPreview;
        private System.Windows.Forms.ToolStripMenuItem tsmFile_PageSetup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem tsmFile_Exit;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmView_ExpandAll;
        private System.Windows.Forms.ToolStripMenuItem tsmView_CollapseAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tsmiViewRefresh;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutPserv3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btListByDLLPath;
        private System.Windows.Forms.ToolStripButton btListByProcessPath;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.ToolStripMenuItem tsmActions;
        private System.Windows.Forms.ToolStripMenuItem tsmDependencies;
        private System.Windows.Forms.ToolStripMenuItem tsmConsole;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardInquotationmarksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmExplorer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton tsbDependencies;
        private System.Windows.Forms.ToolStripButton tsbConsole;
        private System.Windows.Forms.ToolStripButton tsbExplorer;
        private System.Windows.Forms.ToolStripButton tsbCopyToClipboard;
        private System.Windows.Forms.ToolStripButton tsbCopyToClipboardInQuotationmarks;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}

