using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using pserv3.Windows;
using pserv3.Properties;

namespace pserv3
{
    public partial class MainForm : Form, IServiceView
    {
        public IServiceController CurrentController = null;
        public static MainForm Instance;
        private readonly IServiceController ServicesController;
        private readonly IServiceController DevicesController;
        private readonly IServiceController WindowsController;
        private readonly IServiceController UninstallerController;
        private readonly IServiceController ProcessesController;
        private readonly IServiceController ModulesController;
        private readonly IServiceController SystemEventsController;
        private readonly IServiceController SecurityEventsController;
        private readonly IServiceController ApplicationEventsController;
        private readonly ListViewColumnSorter Sorter;
        private readonly GPrint.IPrinter Printer = new GPrint.TextPrinter();
        private readonly List<IServiceColumn> VisibleColumns = new List<IServiceColumn>();
        private readonly List<IServiceColumn> HiddenColumns = new List<IServiceColumn>();
        public readonly List<IServiceObject> CurrentObjects = new List<IServiceObject>();

        /// <summary>
        /// array to cache items for the virtual list
        /// </summary>
        private ListViewItem[] DisplayObjectsCache = null;

        /// <summary>
        /// stores the index of the first item in the cache
        /// </summary>
        private int IndexOfFirstItemInCache;
        
        public MainForm()
        {
            InitializeComponent();
            listView1.MouseWheel += OnMouseWheel;
            Instance = this;

            tsmFile.Text = IDS.Menu_File;
            tsmFile_ConnectToLocalMachine.Text = IDS.Menu_File_ConnectToLocalMachine;
            tsmFile_ConnectToRemoteMachine.Text = IDS.Menu_File_ConnectToRemoteMachine;
            tsmFile_ApplyTemplate.Text = IDS.Menu_File_ApplyTemplate;
            tsmFile_SaveAsXML.Text = IDS.Menu_File_SaveAsXML;
            tsmFile_CopyToClipboard.Text = IDS.Menu_File_CopyToClipboard;
            tsmFile_Print.Text = IDS.Menu_File_Print;
            tsmFile_PrintPreview.Text = IDS.Menu_File_PrintPreview;
            tsmFile_PageSetup.Text = IDS.Menu_File_PageSetup;
            tsmFile_Exit.Text = IDS.Menu_File_Exit;

            tsmOptions.Text = IDS.Menu_Options;
            tsmOptions_ChooseColumns.Text = IDS.Menu_Options_ChooseColumns;
            tsmOptions_Font.Text = IDS.Menu_Options_Font;

            tsmView.Text = IDS.Menu_View;
            tsmView_Services.Text = IDS.Menu_View_Services;
            tsmView_Devices.Text = IDS.Menu_View_Devices;
            tsmView_Windows.Text = IDS.Menu_View_Windows;
            tsmView_Uninstaller.Text = IDS.Menu_View_Uninstaller;
            tsmView_SystemEvents.Text = IDS.Menu_View_SystemJournal;
            tsmView_SecurityEvents.Text = IDS.Menu_View_SecurityJournal;
            tsmView_ApplicationEvents.Text = IDS.Menu_View_ApplicationJournal;
            tsmView_Processes.Text = IDS.Menu_View_Processes;
            tsmView_Modules.Text = IDS.Menu_View_Modules;
            tsmView_Refresh.Text = IDS.Menu_View_Refresh;

            tsmHelp.Text = IDS.Menu_Help;
            tsmHelp_About.Text = IDS.Menu_Help_About;

            tsbServices.Text = IDS.Button_Services_Text;
            tsbServices.ToolTipText = IDS.Button_Services_ToolTip;

            tsbDevices.Text = IDS.Button_Devices_Text;
            tsbDevices.ToolTipText = IDS.Button_Devices_ToolTip;

            tsbWindows.Text = IDS.Button_Windows_Text;
            tsbWindows.ToolTipText = IDS.Button_Windows_ToolTip;

            tsbUninstaller.Text = IDS.Button_Uninstall_Text;
            tsbUninstaller.ToolTipText = IDS.Button_Uninstall_ToolTip;

            tsbProcesses.Text = IDS.Button_Processes_Text;
            tsbProcesses.ToolTipText = IDS.Button_Processes_ToolTip;

            tsbModules.Text = IDS.Button_Modules_Text;
            tsbModules.ToolTipText = IDS.Button_Modules_ToolTip;

            tsbItemStart.Text = IDS.Button_Item_Start;
            tsbItemStop.Text = IDS.Button_Item_Stop;
            tsbItemRestart.Text = IDS.Button_Item_Restart;
            tsbItemPause.Text = IDS.Button_Item_Pause;
            tsbItemContinue.Text = IDS.Button_Item_Continue;

            toolStripLabel1.Text = IDS.Button_Filter;

            tsbProperties.Text = IDS.Button_Properties_Text;
            tsbProperties.ToolTipText = IDS.Button_Properties_ToolTip;

            tsbRefresh.Text = IDS.Button_Refresh_Text;
            tsbRefresh.ToolTipText = IDS.Button_Refresh_ToolTip;

            listView1.VirtualMode = true;
            listView1.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(listView1_RetrieveVirtualItem);
            listView1.CacheVirtualItems += new CacheVirtualItemsEventHandler(listView1_CacheVirtualItems);

            Settings settings = new Settings();
            listView1.Font = new Font(settings.FontFace, settings.FontHeight);

            ServicesController = new Services.ServicesController(listView1);
            DevicesController = new Services.DevicesController(listView1);
            WindowsController = new Windows.WindowsController(listView1);
            UninstallerController = new Uninstaller.UninstallerController(listView1);
            ProcessesController = new Processes.ProcessesController(listView1);
            ModulesController = new Modules.ModulesController(listView1);
            SystemEventsController = new EventJournal.EventJournalController(listView1, "System");
            SecurityEventsController = new EventJournal.EventJournalController(listView1, "Security");
            ApplicationEventsController = new EventJournal.EventJournalController(listView1, "Application");

            Sorter = new ListViewColumnSorter(this);

            SwitchController(ServicesController);
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                float fontheight = listView1.Font.SizeInPoints;
                if (e.Delta > 0)
                {
                    fontheight += (float)1.0;
                }
                else
                {
                    if (fontheight > 4.0)
                    {
                        fontheight -= (float)1.0;
                    }
                }
                listView1.Font = new Font(listView1.Font.Name, (float)fontheight);
            }
        }

        /// <summary>
        /// The basic VirtualMode function.  Dynamically returns a ListViewItem
        /// with the required properties; in this case, the square of the index.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            //check to see if the requested item is currently in the cache
            if (DisplayObjectsCache != null && e.ItemIndex >= IndexOfFirstItemInCache && e.ItemIndex < IndexOfFirstItemInCache + DisplayObjectsCache.Length)
            {
                //A cache hit, so get the ListViewItem from the cache instead of making a new one.
                e.Item = DisplayObjectsCache[e.ItemIndex - IndexOfFirstItemInCache];
            }
            else
            {
                //A cache miss, so create a new ListViewItem and pass it back.
                e.Item = CreateItemFromIndex(e.ItemIndex);
            }
        }

        private ListViewItem CreateItemFromIndex(int itemIndex)
        {
            IServiceObject so = CurrentObjects[itemIndex];

            ListViewItem result = null;
            foreach (IServiceColumn vc in VisibleColumns)
            {
                string itemText = so.GetText(vc.GetID());
                if( result == null )
                    result = new ListViewItem(itemText);
                else
                    result.SubItems.Add(itemText);
            }
            if (result != null)
            {
                result.ToolTipText = so.GetToolTipText();
                result.ForeColor = so.GetForegroundColor();
                result.Tag = so;
            }
            return result;
        }

        public void UpdateDisplay()
        {
            DisplayObjectsCache = null;
            listView1.Refresh();
        }

        /// <summary>
        /// Manages the cache.  ListView calls this when it might need a cache refresh.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            //We've gotten a request to refresh the cache.
            //First check if it's really neccesary.
            if (DisplayObjectsCache != null && e.StartIndex >= IndexOfFirstItemInCache && e.EndIndex <= IndexOfFirstItemInCache + DisplayObjectsCache.Length)
            {
                //If the newly requested cache is a subset of the old cache, 
                //no need to rebuild everything, so do nothing.
                return;
            }

            //Now we need to rebuild the cache.
            IndexOfFirstItemInCache = e.StartIndex;
            int length = e.EndIndex - e.StartIndex + 1; //indexes are inclusive
            DisplayObjectsCache = new ListViewItem[length];

            //Fill the cache with the appropriate ListViewItems.
            int x = 0;
            for (int i = 0; i < length; i++)
            {
                x = (i + IndexOfFirstItemInCache) * (i + IndexOfFirstItemInCache);
                DisplayObjectsCache[i] = CreateItemFromIndex(i + IndexOfFirstItemInCache);
            }
        }

        private void SwitchController(IServiceController scon)
        {
            CurrentController = scon;
            CurrentController.SetView(this);
            listView1.ContextMenu = CurrentController.CreateContextMenu();
            listView1.Columns.Clear();

            // take columns, replace by list of visible columns
            VisibleColumns.Clear();
            HiddenColumns.Clear();
            RefreshDisplay();
            listView1_SelectedIndexChanged(null, null);

            bool CanConnectToRemoteMachine = CurrentController.CanConnectToRemoteMachine();
            tsmFile_ConnectToLocalMachine.Enabled = CanConnectToRemoteMachine;
            tsmFile_ConnectToRemoteMachine.Enabled = CanConnectToRemoteMachine; 
        }

        private void ApplyDisplayFilter(string Text)
        {
            CurrentObjects.Clear();

            int ActiveItems = 0;
            int DisabledItems = 0;
            int HiddenItems = 0;
            int TotalItems = 0;
            bool createColumns = (VisibleColumns.Count == 0);
            if (createColumns)
            {
                foreach (IServiceColumn sc in CurrentController.GetColumns())
                {
                    VisibleColumns.Add(sc);                    
                }
            }

            int[] maxLen = new int[VisibleColumns.Count];
            string[] maxText = new string[VisibleColumns.Count];

            for (int i = 0; i < maxLen.Length; ++i)
            {
                maxLen[i] = 0;
                maxText[i] = "";
            }

            foreach (IServiceObject so in CurrentController.GetObjects())
            {
                // get all display texts for this item
                int i = 0;
                bool visible = string.IsNullOrEmpty(Text);
                foreach (IServiceColumn vc in VisibleColumns)
                {
                    string itemText = so.GetText(vc.GetID());
                    if (!string.IsNullOrEmpty(itemText))
                    {
                        if (itemText.Length > maxLen[i])
                        {
                            maxLen[i] = itemText.Length;
                            maxText[i] = itemText;
                        }
                        if (!visible && (itemText.IndexOf(Text, 0, StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            visible = true;
                            if (!createColumns)
                                break;
                        }
                    }
                    ++i;
                }
                if (visible)
                {
                    Color c = so.GetForegroundColor();
                    if (Color.Blue == c)
                    {
                        ++ActiveItems;
                    }
                    if (Color.Gray == c)
                    {
                        ++DisabledItems;
                    }
                    CurrentObjects.Add(so);
                }
                else
                {
                    ++HiddenItems;
                }
                ++TotalItems;
            }

            if (createColumns)
            {
                int i = 0;
                using (Graphics g = listView1.CreateGraphics())
                {
                    foreach (IServiceColumn sc in CurrentController.GetColumns())
                    {
                        ColumnHeader c = listView1.Columns.Add(sc.GetName(), g.MeasureString(maxText[i], listView1.Font).ToSize().Width + 10);
                        c.TextAlign = sc.GetTextAlign();
                        if (c.TextAlign != HorizontalAlignment.Left)
                        {
                            Trace.TraceInformation("{0} is right aligned", c.Name);
                        }
                        ++i;
                    }
                }
            }
            listView1.VirtualListSize = CurrentObjects.Count;
            UpdateDisplay();

            tsslNrItems.Text = string.Format(IDS.StatusBar_NumberOfVisibleItems, CurrentObjects.Count);
            tsslRunning.Text = string.Format(IDS.StatusBar_NumberOfActiveItems, ActiveItems);
            tsslDisabled.Text = string.Format(IDS.StatusBar_NumberOfDisabledItems, DisabledItems);
            tsslHidden.Text = string.Format(IDS.StatusBar_NumberOfHiddenItems, HiddenItems);
            tsslTotal.Text = string.Format(IDS.StatusBar_NumberOfTotalItems, TotalItems);
        }

        public void RefreshDisplay()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (CurrentController.ItemDataIsCached())
                {
                    listView1.VirtualListSize = 0;
                    new LongTaskDialog(CurrentController).ShowDialog();
                }
                else
                {
                    CurrentController.Refresh();
                }
                DateTime n = DateTime.Now;
                ApplyDisplayFilter(toolStripComboBox1.Text);
                Trace.TraceInformation("Time to ApplyDisplayFilter: {0}", DateTime.Now - n);

                if (Sorter != null)
                {
                    Sorter.OnColumnClick(null, null);
                }
                Text = string.Format(IDS.MainForm_Caption, GSharpTools.AppVersion.Get(), CurrentController.GetCaption());
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void tsmiViewRefresh_Click(object sender, EventArgs e)
        {
            RefreshDisplay();
        }

        private void tsmiSwitchServices_Click(object sender, EventArgs e)
        {
            SwitchController(ServicesController);
        }

        private void tsmiSwitchDevices_Click(object sender, EventArgs e)
        {
            SwitchController(DevicesController);
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            ApplyDisplayFilter(toolStripComboBox1.Text);
        }
        
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            SwitchController(WindowsController);
        }

        private void tsbShowWindows_Click(object sender, EventArgs e)
        {
            SwitchController(WindowsController);
        }

        private void tsbServices_Click(object sender, EventArgs e)
        {
            SwitchController(ServicesController);
        }

        private void tsbDevices_Click(object sender, EventArgs e)
        {
            SwitchController(DevicesController);
        }

        private void tsbItemStart_Click(object sender, EventArgs e)
        {
            CurrentController.OnContextStart();
        }

        private void tsbItemStop_Click(object sender, EventArgs e)
        {
            CurrentController.OnContextStop();
        }

        private void tsbItemRestart_Click(object sender, EventArgs e)
        {
            CurrentController.OnContextRestart();
        }

        private void tsbPause_Click(object sender, EventArgs e)
        {
            CurrentController.OnContextPause();
        }

        private void tsbContinue_Click(object sender, EventArgs e)
        {
            CurrentController.OnContextContinue();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tsbItemStart.Enabled = CurrentController.IsContextStartEnabled();
            tsbItemStop.Enabled = CurrentController.IsContextStopEnabled();
            tsbItemPause.Enabled = CurrentController.IsContextPauseEnabled();
            tsbItemContinue.Enabled = CurrentController.IsContextContinueEnabled();
            tsbItemRestart.Enabled = CurrentController.IsContextRestartEnabled();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            CurrentController.OnShowProperties();
        }

        private void tsbItemProperties_Click(object sender, EventArgs e)
        {
            CurrentController.OnShowProperties();
        }

        private void columnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new ChooseColumns(VisibleColumns, HiddenColumns).ShowDialog() == DialogResult.OK)
            {
                listView1.Columns.Clear();

                foreach (IServiceColumn sc in VisibleColumns)
                {
                    ColumnHeader ch = listView1.Columns.Add(sc.GetName());
                    ch.TextAlign = sc.GetTextAlign();
                }

                RefreshDisplay();
                listView1_SelectedIndexChanged(null, null);
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = listView1.Font;
            if (fd.ShowDialog() != DialogResult.Cancel)
            {
                listView1.Font = fd.Font;
            }
        }

        private void tsmFile_Print_Click(object sender, EventArgs e)
        {
            Printer.SetDocument(CurrentController.CreatePrintDocument());
            Printer.PrintFile(true);
        }

        private void tsmFile_PageSetup_Click(object sender, EventArgs e)
        {
            Printer.ShowPageSettings();
        }

        private void tsmFile_PrintPreview_Click(object sender, EventArgs e)
        {
            Printer.SetDocument(CurrentController.CreatePrintDocument());
            Printer.ShowPrintPreview();
        }

        private void tsmFile_SaveAsXML_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = IDS.MainForm_TemplateFileFilter;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CurrentController.SaveAsXML(ofd.FileName);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings settings = new Settings();

            settings.FontFace = listView1.Font.Name;
            settings.FontHeight = listView1.Font.SizeInPoints;
            settings.Save();
        }

        private void tsmFile_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsmFile_ConnectToLocalMachine_Click(object sender, EventArgs e)
        {
            Services.ServicesController.MachineName = null;
            RefreshDisplay();
            listView1_SelectedIndexChanged(null, null);
        }

        private void tsmFile_ConnectToRemoteMachine_Click(object sender, EventArgs e)
        {
            BrowseComputerName box = new BrowseComputerName();
            if (box.ShowDialog() == DialogResult.OK)
            {
                RefreshDisplay();
                listView1_SelectedIndexChanged(null, null);
            }
        }

        private void tsbShowUninstaller_Click(object sender, EventArgs e)
        {
            SwitchController(UninstallerController);
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://p-nand-q.com/download/pserv_cpl.html");
            }
            catch (Exception)
            {
                
            }
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            RefreshDisplay();
        }

        private void tsbApplyTemplate_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = IDS.MainForm_TemplateFileFilter;

            global::pserv3.Properties.Settings settings = new global::pserv3.Properties.Settings();

            if (!string.IsNullOrEmpty(settings.LastUsedTemplate.Trim()))
            {
                dialog.InitialDirectory = Path.GetDirectoryName(settings.LastUsedTemplate);
                dialog.FileName = Path.GetFileName(settings.LastUsedTemplate);
            }
            dialog.Title = IDS.MainForm_SelectTemplateFile;
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                CurrentController.ApplyTemplate(dialog.FileName);
            }
        }

        private void tsbCopyToClipboard_Click(object sender, EventArgs e)
        {
            CurrentController.SaveAsXML(null);
        }

        private void tsbViewProcesses_Click(object sender, EventArgs e)
        {
            SwitchController(ProcessesController);
        }

        private void tsbModules_Click(object sender, EventArgs e)
        {
            SwitchController(ModulesController);
        }

        private void tsmSystemEvents_Click(object sender, EventArgs e)
        {
            SwitchController(SystemEventsController);
        }

        private void tsmSecurityEvents_Click(object sender, EventArgs e)
        {
            SwitchController(SecurityEventsController);
        }

        private void tsmApplicationEvents_Click(object sender, EventArgs e)
        {
            SwitchController(ApplicationEventsController);
        }
    }
}
