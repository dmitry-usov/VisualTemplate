using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace VisualTemplate
{
    public partial class MainForm : Form
        
    {
        private string oldFileName;
        public TempTabPage curTempTabPage;

        public enum PropShowMode
        {
            showProperties,
            showChildSignals
        }

        public PropShowMode curPropShowMode;

        public MainForm()
        {
            InitializeComponent();
            splitContainer1.Panel1Collapsed = true;
            if (Program.fileToOpen != null)
            {
                openToolStripButton_Click(null, null);
            }
            toolStripComboBox1.SelectedIndex = 0;
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            curTempTabPage = tabControl2.SelectedIndex<0?null:Program.TemplatesPages[tabControl2.SelectedIndex];
        }


        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            closeToolStripButton.Enabled = true;
            curTempTabPage = Program.CreateNewTemplate();
            curTempTabPage.Id = tabControl2.TabPages.Count;

            //tabControl2.TabPages.Add(curTempTabPage.TabPage);
            
            Program.addToTree(curTempTabPage.Template, curTempTabPage.TreeView, curTempTabPage.Id);
            if (Program.MasterFile is null) Program.newMaster(treeMasterView);

            setVisualToPage();

            //curTempTabPage.TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            //curTempTabPage.dgSettings.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellEndEdit);
            //curTempTabPage.dgProps.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridProps_CellEndEdit);
            //curTempTabPage.dgSettings.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgSettings_CellClick);
            //curTempTabPage.dgProps.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
            //curTempTabPage.dgProps.LocationChanged += new System.EventHandler(this.LastColumnComboSelectionChanged);
            //curTempTabPage.dgSettings.Columns.Add("Param", "Имя");
            //curTempTabPage.dgSettings.Columns.Add("Value", "Значение");
            //curTempTabPage.dgSettings.Columns.Add("", "");
            //curTempTabPage.dgSettings.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            //curTempTabPage.dgSettings.Columns[0].Width = 70;
            //curTempTabPage.dgSettings.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            //curTempTabPage.dgSettings.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            //curTempTabPage.dgSettings.Columns[2].Width = 50;
            //tabControl2.SelectedIndex = curTempTabPage.Id;
            //toolStripStatusLabel1.Text = Program.TemplatesPages.Count.ToString();
            //curTempTabPage.TreeView.ImageList = imageListForTree;


            curTempTabPage.Changed = true;

        }

        

        private void closeToolStripButton_Click(object sender, EventArgs e)
        {

                DialogResult res = MessageBox.Show(this, "Сохранить перед закрытием?", "Сохранить?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (res)
                {
                    case DialogResult.Yes:
                        saveToolStripButton_Click(null,null);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            

            int removeIndex = tabControl2.SelectedIndex;
            if (removeIndex < 0) return;
            TempTabPage curTemp = Program.TemplatesPages[removeIndex];
            tabControl2.TabPages.RemoveAt(removeIndex);
            curTemp = null;
            Program.TemplatesPages.Remove(removeIndex);
            int k = 0;
            List<TempTabPage> Elements = new List<TempTabPage>();
            Elements.AddRange(Program.TemplatesPages.Values);
            Program.TemplatesPages.Clear();
            foreach (TempTabPage tmp in Elements)
            {
                Program.TemplatesPages.Add(k, tmp);
                tmp.Id = k++;
               // Program.TemplatesPages.Keys[k] = k;
            }

            if(Program.TemplatesPages.Values.Count == 0)
            {
                closeToolStripButton.Enabled = false;
                tabControl2.TabPages.Clear();
                toolStrip2.Enabled = false;
                toolStrip3.Enabled = false;
            }
            toolStripStatusLabel1.Text = Program.TemplatesPages.Count.ToString();
            tabControl2.SelectedIndex = removeIndex>1?removeIndex -1:0;
        }

        private void cycleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addCycle(curTempTabPage);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Program.addElemnt(curTempTabPage);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void signalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addElemnt(curTempTabPage, "8");
            curTempTabPage.TreeView.SelectedNode.Expand();
        }


        public void openToolStripButton_Click(object sender, EventArgs e )
        {
            openFileDialog1.Filter = "jvt files (*.jvt)|*.jvt|master file (*.xmlvt)|*.xmlvt|All files (*.*)|*.*";
            DialogResult dgr;
            if (Program.fileToOpen == null)
            {
                dgr = openFileDialog1.ShowDialog();
                if (dgr == DialogResult.Cancel)
                {
                    return;
                }
            }
            else
            {
                openFileDialog1.FileName = Program.fileToOpen;
            }

            string[] q = openFileDialog1.SafeFileName.Split('.');
            closeToolStripButton.Enabled = true;
            if (q[q.Length-1] == "jvt")
            {

                curTempTabPage = Program.CreateNewTemplate();
                curTempTabPage.Id = tabControl2.TabPages.Count;

                Program.loadTemplate(openFileDialog1, curTempTabPage);
                setVisualToPage();

                curTempTabPage.TabPage.Text = curTempTabPage.Template.Name;
            }
            else if (q[q.Length - 1] == "xmlvt")
            {
               // XmlSerializer formatter = new XmlSerializer(typeof(Master));
                using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.OpenOrCreate))
                {
                    //Program.MasterFile = (Master)formatter.Deserialize(fs);
                }

                Program.MasterFile.curPath = openFileDialog1.FileName.Replace(openFileDialog1.SafeFileName,"");
                splitContainer1.Panel1Collapsed = false;
                Program.MasterFile.Files.Clear();
                Program.MasterFile.tempTabPagesDic.Clear();
                DirectoryInfo di = new DirectoryInfo(openFileDialog1.FileName.Replace(openFileDialog1.SafeFileName, ""));
                foreach(FileInfo fi in di.GetFiles("*.jvt"))
                {
                    Program.MasterFile.Files.Add(fi.Name);
                }
                
                foreach (string s in Program.MasterFile.Files)
                {
                    curTempTabPage = Program.CreateNewTemplate();
                    curTempTabPage.Id = tabControl2.TabPages.Count;
                    if( Program.loadTemplate(Program.MasterFile.curPath + s, curTempTabPage) == true)
                    Program.MasterFile.tempTabPagesDic.Add(curTempTabPage.Id, curTempTabPage);
                    setVisualToPage();
                    curTempTabPage.TabPage.Text = curTempTabPage.Template.Name;
                }

                Program.openMaster(treeMasterView);
                treeMasterView.Nodes[0].Text = openFileDialog1.SafeFileName;
            }
            Program.fileToOpen = null;
        }

        private void setVisualToPage()
        {
            tabControl2.TabPages.Add(curTempTabPage.TabPage);
            curTempTabPage.dgSettings.Columns.Add("Param", "Имя");
            curTempTabPage.dgSettings.Columns.Add("Value", "Значение");
            curTempTabPage.dgSettings.Columns.Add("", "");
            curTempTabPage.dgSettings.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            curTempTabPage.dgSettings.Columns[0].Width = 70;
            curTempTabPage.dgSettings.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            curTempTabPage.dgSettings.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            curTempTabPage.dgSettings.Columns[2].Width = 50;


            curTempTabPage.TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            curTempTabPage.dgSettings.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellEndEdit);
            curTempTabPage.dgSettings.CellBeginEdit += new DataGridViewCellCancelEventHandler(dgSettings_CellBeginEdit);
            curTempTabPage.dgProps.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            curTempTabPage.dgProps.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridProps_CellEndEdit);
            curTempTabPage.dgSettings.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgSettings_CellClick);
            curTempTabPage.dgProps.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgProps_CellClick);
            curTempTabPage.dgProps.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
            curTempTabPage.dgProps.LocationChanged += new System.EventHandler(this.LastColumnComboSelectionChanged);
            curTempTabPage.dgVariants.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellDoubleClick);
            curTempTabPage.dgProps.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgProps_CellDoubleClick);
            curTempTabPage.TreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);

            curTempTabPage.TreeView.ContextMenuStrip = trvContextMenu;

            curTempTabPage.TreeView.ImageList = imageListForTree;
            tabControl2.SelectedIndex = curTempTabPage.Id;
            toolStripStatusLabel1.Text = Program.TemplatesPages.Count.ToString();
            curTempTabPage.dgVariants.Columns.Add("Name", "Name");
            curTempTabPage.dgVariants.Columns.Add("Value", "Value");


        }

  
        private void dgProps_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var currentcell = curTempTabPage.dgProps.CurrentCellAddress;


            if (curTempTabPage.TreeView.SelectedNode.Name == "0")
            {

            }
            else
            {
                Element el = Program.getElementById(curTempTabPage.TreeView.SelectedNode.Name, curTempTabPage.Id);
                if (el is null) return;
                switch (el.GetType().ToString())
                {
                    case "VisualTemplate.Signal":
                        Signal s = el as Signal;
                        Property p = s.Properties[currentcell.Y];
                        PropEdit pe = new PropEdit(p);
                        //dataGridProps_CellEndEdit(curTempTabPage.dgProps,null);
                        if (pe.ShowDialog(this) == DialogResult.OK);
                            Program.getProperties(s, curTempTabPage.dgProps, curTempTabPage.TreeView.SelectedNode);
                        break;
                    case "VisualTemplate.Cycle":

                        break;
                }
            }

            string vNs = "$" + curTempTabPage.dgProps.Rows[currentcell.Y].Cells[0].Value.ToString() + "$";

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Настраиваем кнопки

            cycleToolStripMenuItem.Enabled = false;
            folderToolStripMenuItem.Enabled = false;
            signalToolStripMenuItem.Enabled = false;
            deleteToolStripButton2.Enabled = true;
            удалитьToolStripMenuItem.Enabled = true;
            dublToolStripButton3.Enabled = false;

            //******************************************

            TempTabPage curTmp;
            if (Program.TemplatesPages.ContainsKey(tabControl2.SelectedIndex))
                curTmp = Program.TemplatesPages[tabControl2.SelectedIndex];
            else
                return;
            TreeNode TrNdSel = curTmp.TreeView.SelectedNode;
            if(TrNdSel is null)
            {
                return;
            }
            TrNdSel.SelectedImageIndex = TrNdSel.ImageIndex;
            object obj = Program.getObjectFromTree(TrNdSel.Name, tabControl2.SelectedIndex);
            if (obj.GetType().ToString() == "VisualTemplate.Cycle")
            {
                folderToolStripMenuItem.Enabled = true;
                signalToolStripMenuItem.Enabled = true;
                cycleToolStripMenuItem.Enabled = true;
                dublToolStripButton3.Enabled = true;
                curTmp.dgProps.Columns.Clear();
                curTmp.dgProps.Columns.Add("Name", "Name");
                curTmp.dgProps.Columns.Add("Value", "Value");
                curTmp.dgProps.Columns.Add("Step", "Step");
                curTmp.dgProps.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                curTmp.dgProps.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                curTmp.dgProps.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                Cycle c = obj as Cycle;
                Program.getSettings(c, curTmp);
                Program.getVariants(c, curTmp.dgProps);

                curTempTabPage.dgVariants.Rows.Clear();

                if (c.Variatns.Count > 0)
                {
                    toolStripButton1.Enabled = true;
                    toolStripButton2.Enabled = true;
                }
                else
                {
                    toolStripButton1.Enabled = false;
                    toolStripButton2.Enabled = false;
                }
            }
            if (obj.GetType().ToString() == "VisualTemplate.Signal")
            {
                cycleToolStripMenuItem.Enabled = true;
                folderToolStripMenuItem.Enabled = true;
                signalToolStripMenuItem.Enabled = true;
                dublToolStripButton3.Enabled = true;


                if (curPropShowMode == PropShowMode.showProperties)
                {
                     getSignalProperties(obj);
                }
                else if (curPropShowMode == PropShowMode.showChildSignals)
                {
                    getSignalChildElements(obj);
                }

            }
            if (obj.GetType().ToString() == "VisualTemplate.Template")
            {
                //Настраиваем кнопки
                cycleToolStripMenuItem.Enabled = true;
                deleteToolStripButton2.Enabled = false;

                toolStripButton1.Enabled = false;
                toolStripButton2.Enabled = false;
                toolStripButton3.Enabled = false;


                удалитьToolStripMenuItem.Enabled = false;

                //****************************


                Template t = obj as Template;
                curTmp.dgProps.Columns.Clear();
                curTmp.dgProps.Columns.Add("Name", "Name");
                curTmp.dgProps.Columns.Add("Path", "Path");
                curTmp.dgProps.Columns.Add("Separator", "Separator");
                curTmp.dgProps.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                curTmp.dgProps.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                curTmp.dgProps.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
               
                DataGridViewComboBoxColumn EncType = new DataGridViewComboBoxColumn();
                
                EncType.Items.Add("ASCII");
                EncType.Items.Add("Default");
                EncType.Items.Add("UTF8");
                EncType.HeaderText = "Кодировка";
                curTmp.dgProps.Columns.Add(EncType);
                curTmp.dgProps.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

                curTmp.dgProps.Columns.Add(new DataGridViewButtonColumn() { Text = "Edit", UseColumnTextForButtonValue = true });
                curTmp.dgProps.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;

                curTempTabPage.dgVariants.Rows.Clear();

                if (t.CsvVars.Count > 0)
                {
                    toolStripButton1.Enabled = true;
                    toolStripButton2.Enabled = true;
                }
                else
                {
                    toolStripButton1.Enabled = false;
                    toolStripButton2.Enabled = false;
                }

                Program.getCsvVars(curTmp);
                Program.getSettings(t, TrNdSel, curTmp.dgSettings);

            }
            
            toolStrip2.Enabled = true;
            toolStrip3.Enabled = true;
        }


        private void getSignalChildElements(object obj)
        {

            curTempTabPage.dgProps.Columns.Clear();

            curTempTabPage.dgProps.Columns.Add("name","name");
            curTempTabPage.dgProps.Columns.Add("type","type");
            curTempTabPage.dgProps.Columns.Add("descr", "descr");



            curTempTabPage.dgProps.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            curTempTabPage.dgProps.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            curTempTabPage.dgProps.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            curTempTabPage.dgProps.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            Signal s = obj as Signal;
            

            foreach(Signal sg in s.Signals)
            {
                if (sg.HasProperties && sg.HasProperty("1"))
                {

                    curTempTabPage.dgProps.Rows.Add(sg.Name, Program.CDTTypes[sg.getPropertyById("1").Value],sg.getPropertyById("101")!=null? sg.getPropertyById("101").Value:"");
                }
                
            }

            Program.getSettings(s, curTempTabPage.TreeView.SelectedNode, curTempTabPage.dgSettings);
            Program.getVariantsToAdd(s, curTempTabPage);

            if (s.Properties.Count > 0)
            {
                toolStripButton1.Enabled = true;
                toolStripButton2.Enabled = true;
            }
            else
            {
                toolStripButton1.Enabled = false;
                toolStripButton2.Enabled = false;
            }
        }


        private void getSignalProperties(object obj)
        {

            curTempTabPage.dgProps.Columns.Clear();
            DataGridViewComboBoxColumn cmbID = new DataGridViewComboBoxColumn();
            cmbID.HeaderText = "ID";
            DataGridViewComboBoxColumn cmbType = new DataGridViewComboBoxColumn();
            cmbType.HeaderText = "Type";
            foreach (string str in Program.TypeOfProperty.Keys)
            {
                cmbID.Items.Add(str);
            }

            foreach (string str in Program.Types.Values)
            {
                cmbType.Items.Add(str);
            }

            curTempTabPage.dgProps.Columns.Add(cmbID);
            curTempTabPage.dgProps.Columns.Add(cmbType);
            curTempTabPage.dgProps.Columns.Add("Value", "Value");
            curTempTabPage.dgProps.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            curTempTabPage.dgProps.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            curTempTabPage.dgProps.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            curTempTabPage.dgProps.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Signal s = obj as Signal;

            s.Properties.Sort(new PropertyComparer());

            Program.getSettings(s, curTempTabPage.TreeView.SelectedNode, curTempTabPage.dgSettings);
            Program.getProperties(s, curTempTabPage.dgProps, curTempTabPage.TreeView.SelectedNode);


            //  if (curTempTabPage.dgVariants.Rows.Count < 1)
            Program.getVariantsToAdd(s, curTempTabPage);

            if (s.Properties.Count > 0)
            {
                toolStripButton1.Enabled = true;
                toolStripButton2.Enabled = true;
            }
            else
            {
                toolStripButton1.Enabled = false;
                toolStripButton2.Enabled = false;
            }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            
            TreeNode TrNdSel = curTempTabPage.TreeView.SelectedNode;
             if (TrNdSel.Name == "0")
            {
                curTempTabPage.Template.Name = curTempTabPage.dgSettings.Rows[0].Cells[1].Value.ToString();
                TrNdSel.Text = curTempTabPage.Template.Name;
                curTempTabPage.TabPage.Text = curTempTabPage.Template.Name;
                if (oldFileName != curTempTabPage.Template.Name) curTempTabPage.Template.CurPath = null;
                curTempTabPage.Changed = true;
            }
            else
            {
                Element el = Program.getElementById(TrNdSel.Name, tabControl2.SelectedIndex);
                switch (el.GetType().ToString())
                {
                    case "VisualTemplate.Signal":
                        Signal s = el as Signal;
                        Program.setSignal(curTempTabPage);
                        //Program.setSignal(TrNdSel, curTempTabPage.dgSettings.Rows[0].Cells[1].Value.ToString());
                        TrNdSel.Text = s.ToString();
                        break;
                    case "VisualTemplate.Cycle":
                        Program.setCycle(curTempTabPage);
                        Cycle c = el as Cycle;
                        TrNdSel.Text = c.ToString();
                        break;
                }
            }
            if (!curTempTabPage.Changed)
            {
                curTempTabPage.TabPage.Text += "*";
            }
        }

        private void deleteToolStripButton2_Click(object sender, EventArgs e)
        {
            Program.deleteSelected(curTempTabPage);
        }








        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (curTempTabPage.dgProps.CurrentCell.ColumnIndex == 0 && e.Control is ComboBox)
            {
                ComboBox comboBox = e.Control as ComboBox;
                comboBox.SelectedIndexChanged -= LastColumnComboSelectionChanged;
                comboBox.SelectedIndexChanged += LastColumnComboSelectionChanged;
            }
        }

        private void LastColumnComboSelectionChanged(object sender, EventArgs e)
        {
            var currentcell = curTempTabPage.dgProps.CurrentCellAddress;
            var sendingCB = sender as DataGridViewComboBoxEditingControl;
            if (currentcell.X == 0)
            {
                curTempTabPage.dgProps.Rows[currentcell.Y].Cells[1].Value = Program.TypeOfProperty[sendingCB.EditingControlFormattedValue.ToString()];
            }
        }

        private void CellDoubleClick(object sender, EventArgs e)
        {
            var currentcell = curTempTabPage.dgVariants.CurrentCellAddress;

            string vNs = "$" + curTempTabPage.dgVariants.Rows[currentcell.Y].Cells[0].Value.ToString() + "$";

            Clipboard.SetData(DataFormats.Text, (Object)vNs);
            toolStripStatusLabel1.Text = vNs + " - готово для вставки";
        }

        public void StartReplace(string str1, string str2)
        {
          //  Program.ReplaceInTree(treeView1.SelectedNode, str1, str2);
        }



        private void Form1_Load(object sender, EventArgs e)
        {
        //    treeView1.ExpandAll();
        }

        private void bt_Open_Click(object sender, EventArgs e)
        {
          //  Program.loadTemplate(openFileDialog1, treeView1);
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            Program.saveTemplate(saveFileDialog1);
        }

        private void bt_Delete_Click(object sender, EventArgs e)
        {
           // Program.deleteSelected(treeView1.SelectedNode);
        }

        private void bt_AddCycle_Click(object sender, EventArgs e)
        {
           // Program.addCycle(treeView1.SelectedNode);
         //   treeView1.SelectedNode.Expand();
        }

        private void bt_AddSignal_Click(object sender, EventArgs e)
        {

        }




        private void dataGridProps_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            curTempTabPage.dgProps.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgProps_CellDoubleClick);
            if (curTempTabPage.TreeView.SelectedNode.Name == "0")
            {
                Program.setCsvVarPath(curTempTabPage);
                
            }
            else
            {
                Element el = Program.getElementById(curTempTabPage.TreeView.SelectedNode.Name, curTempTabPage.Id);
                if (el is null) return;
                switch (el.GetType().ToString())
                {
                    case "VisualTemplate.Signal":
                        Signal s = el as Signal;
                        Program.setProperties(s, curTempTabPage.dgProps, curTempTabPage.TreeView.SelectedNode);
                        break;
                    case "VisualTemplate.Cycle":
                        Cycle c = el as Cycle;
                        Program.setVariants(c, curTempTabPage.dgProps);
                        break;
                }
            }

            if (!curTempTabPage.Changed)
            {
                curTempTabPage.TabPage.Text += "*";
                curTempTabPage.Changed = true;
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            curTempTabPage.dgProps.CellDoubleClick -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgProps_CellDoubleClick);
        }

        private void dgSettings_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            oldFileName = curTempTabPage.Template.Name;
        }

        //private void bt_AddPoV_Click(object sender, EventArgs e)
        //{
        //    if (treeView1.SelectedNode is null) return;
        //    if (treeView1.SelectedNode.Name == "0")
        //    {
        //        Program.addCsvVar(curTempTabPage);
        //        Program.getCsvVars(curTempTabPage);
        //    }
        //    else
        //    {
        //        Element el = Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex);
        //        if (el is null) return;
        //        switch (el.GetType().ToString())
        //        {
        //            case "VisualTemplate.Signal":
        //                Signal s = el as Signal;
        //                s.Add(new Property("1", "UInt4", "8"));
        //                Program.getProperties(s, dataGridProps, treeView1.SelectedNode);
        //                break;
        //            case "VisualTemplate.Cycle":
        //                Cycle c = el as Cycle;
        //                Program.addVariant(c);
        //              //  c.Add(new Variant("name", "value"));
        //                Program.getVariants(c, dataGridProps);
        //                break;
        //        }
        //    }
        //}

        //private void bt_DeletePoV_Click(object sender, EventArgs e)
        //{

        //    if (treeView1.SelectedNode is null) return;
        //    if (treeView1.SelectedNode.Name == "0")
        //    {
        //        Program.t.CsvVars.RemoveAt(dataGridProps.SelectedCells[0].RowIndex);
        //       // Program.getCsvVars(dataGridProps);
        //    }
        //    else
        //    {

        //        Element el = Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex);
        //        switch (el.GetType().ToString())
        //        {
        //            case "VisualTemplate.Signal":
        //                Signal s = el as Signal;
        //                s.Properties.RemoveAt(dataGridProps.SelectedCells[0].RowIndex);
        //                Program.getProperties(s, dataGridProps, treeView1.SelectedNode);
        //                break;
        //            case "VisualTemplate.Cycle":
        //                Cycle c = el as Cycle;
        //                c.Variatns.RemoveAt(dataGridProps.SelectedCells[0].RowIndex);
        //                Program.getVariants(c, dataGridProps);
        //                break;
        //        }
        //    }
        //}

        private void bt_getCsv_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            if (!(Program.defaultPathOutCsv is null)) saveFileDialog1.InitialDirectory = Program.defaultPathOutCsv;
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Program.globalCsvString = "";
            Program.getCsv(Program.t);

            var fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);
            var sw = new StreamWriter(fs,Encoding.Default);
            Program.globalCsvString = Program.globalCsvString.Trim();
            sw.Write(Program.globalCsvString);
            sw.Close();
            Program.defaultPathOutCsv = Path.GetDirectoryName(saveFileDialog1.FileName);


        }

        private void bt_Copy_Click(object sender, EventArgs e)
        {
            //Element el = Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex);
            //switch (el.GetType().ToString())
            //{
            //    case "VisualTemplate.Signal":
            //        Signal s = el as Signal;
            //        Program.bufElem = s;
            //        break;
            //    case "VisualTemplate.Cycle":
            //        Cycle c = el as Cycle;
            //        Program.bufElem = c;
            //        break;
            //}
        }

        //private void bt_Past_Click(object sender, EventArgs e)
        //{
        //    Element el = Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex);
        //    Element copyEl=null;
        //    //switch (el.GetType().ToString())
        //    //{
        //    //    case "VisualTemplate.Signal":
        //    //        Signal s = el as Signal;


        //    //        //s.Add((Signal)Program.bufSignal.Clone());

        //    //        break;
        //    //    case "VisualTemplate.Cycle":
        //    //        Cycle c = el as Cycle;
        //    //        c.Add((Signal)Program.bufSignal.Clone());
        //    //        //treeView1.Nodes.Clear();
        //    //        //Program.addToTree(Program.t, treeView1);
        //    //        //treeView1.ExpandAll();
        //    //        break;
        //    //}

        //    if (Program.bufElem.GetType().ToString() == "VisualTemplate.Signal")
        //    {
        //        Signal bs = Program.bufElem as Signal;
        //        copyEl = (Signal)bs.Clone();
        //        el.Add((Signal)copyEl);
        //    }
        //    else if (Program.bufElem.GetType().ToString() == "VisualTemplate.Cycle")
        //    {
        //        Cycle bc = Program.bufElem as Cycle;
        //        copyEl = (Cycle)bc.Clone();

        //        el.Add((Cycle)copyEl);
        //        Program.newVarsInDic((Cycle)copyEl);
        //    }

        //  //  Program.addToTree(copyEl, treeView1, treeView1.SelectedNode);

        //   // treeView1.Nodes.Clear();
        //  //  Program.addToTree(Program.t, treeView1);
        // //   treeView1.Nodes[0].Expand();
        //  //  treeView1.ExpandAll();
        //}

        //private void bt_CopyProp_Click(object sender, EventArgs e)
        //{
        //    Element el = Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex);
        //    switch (el.GetType().ToString())
        //    {
        //        case "VisualTemplate.Signal":
        //            Signal s = el as Signal;
        //            Property p = s.Properties[dataGridProps.SelectedCells[0].RowIndex];
        //            Program.bufProp = p;
        //            break;
        //        case "VisualTemplate.Cycle":
        //            Cycle c = el as Cycle;
        //            Variant v = c.Variatns[dataGridProps.SelectedCells[0].RowIndex];
        //            Program.bufVar = v;
        //            break;
        //    }
        //}

        //private void bt_PastProp_Click(object sender, EventArgs e)
        //{
        //    Element el = Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex);
        //    switch (el.GetType().ToString())
        //    {
        //        case "VisualTemplate.Signal":
        //            Signal s = el as Signal;
        //            s.Add((Property)Program.bufProp.Clone());
        //            Program.getProperties(s, dataGridProps, treeView1.SelectedNode);
        //            break;
        //        case "VisualTemplate.Cycle":
        //            Cycle c = el as Cycle;
        //            Variant v = (Variant)Program.bufVar.Clone();
        //            Program.setIdtoVar(v);
        //            c.Add(v);
        //            Program.getVariants(c, dataGridProps);
        //            break;
        //    }
        //}

        //private void bt_PastLink_Click(object sender, EventArgs e)
        //{
        //    Element el = Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex);
        //    switch (el.GetType().ToString())
        //    {
        //        case "VisualTemplate.Signal":
        //            break;
        //        case "VisualTemplate.Cycle":
        //            Cycle c = el as Cycle;
        //            c.Add(Program.bufVar);
        //            Program.bufVar.Link = Program.bufVar.Id;
        //            treeView1.Nodes.Clear();
        //           // Program.addToTree(Program.t, treeView1);
        //            treeView1.Nodes[0].Expand();
        //            break;
        //    }
        //}

        //private void bt_reloadTr_Click(object sender, EventArgs e)
        //{
        //    treeView1.Nodes.Clear();
        //  //  Program.addToTree(Program.t, treeView1);
        //    treeView1.Nodes[0].Expand();
        //}


 
        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F && e.Control)
            {
                FindRep f = new FindRep(this);
                f.Show();
            }
        }

        //private void ctx_copyPath_Click(object sender, EventArgs e)
        //{
        //    Clipboard.SetText(Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex).FullPath);
        //}

        private void copyToolStripButton4_Click(object sender, EventArgs e)
        {
            pastToolStripButton1.Enabled = true;
            Element el = Program.getElementById(curTempTabPage.TreeView.SelectedNode.Name, curTempTabPage.Id);
            if (el is null) return;
            switch (el.GetType().ToString())
            {
                case "VisualTemplate.Signal":
                    Signal s = el as Signal;
                    Program.bufElem = s;
                    break;
                case "VisualTemplate.Cycle":
                    Cycle c = el as Cycle;
                    Program.bufElem = c;
                    break;
            }
        }

        private void pastToolStripButton1_Click(object sender, EventArgs e)
        {
            Element el = Program.getElementById(curTempTabPage.TreeView.SelectedNode.Name, curTempTabPage.Id);
            Element copyEl = null;

            if(curTempTabPage.TreeView.SelectedNode.Name == "0")
            {
                if (Program.bufElem.GetType().ToString() == "VisualTemplate.Cycle")
                {
                    
                    Cycle bc = Program.bufElem as Cycle;
                    copyEl = (Cycle)bc.Clone();
                    curTempTabPage.Template.Cycles.Add((Cycle)copyEl);
                    
                    Program.newVarsInDic((Cycle)copyEl);
                    Program.addToTree(copyEl, curTempTabPage.TreeView, curTempTabPage.Id, curTempTabPage.TreeView.SelectedNode);
                    curTempTabPage.TreeView.SelectedNode.Expand();
                    return;
                }
            }
            //switch (el.GetType().ToString())
            //{
            //    case "VisualTemplate.Signal":
            //        Signal s = el as Signal;


            //        //s.Add((Signal)Program.bufSignal.Clone());

            //        break;
            //    case "VisualTemplate.Cycle":
            //        Cycle c = el as Cycle;
            //        c.Add((Signal)Program.bufSignal.Clone());
            //        //treeView1.Nodes.Clear();
            //        //Program.addToTree(Program.t, treeView1);
            //        //treeView1.ExpandAll();
            //        break;
            //}
            if (el is null) return;
            if (Program.bufElem.GetType().ToString() == "VisualTemplate.Signal")
            {
                Signal bs = Program.bufElem as Signal;
                copyEl = (Signal)bs.Clone();
                el.Add((Signal)copyEl);
            }
            else if (Program.bufElem.GetType().ToString() == "VisualTemplate.Cycle")
            {
                Cycle bc = Program.bufElem as Cycle;
                copyEl = (Cycle)bc.Clone();

                el.Add((Cycle)copyEl);
                Program.newVarsInDic((Cycle)copyEl);
            }
            
            Program.addToTree(copyEl, curTempTabPage.TreeView, curTempTabPage.Id, curTempTabPage.TreeView.SelectedNode);
            curTempTabPage.TreeView.SelectedNode.Expand();
            // curTempTabPage.TreeView.SelectedNode = curTempTabPage.TreeView.SelectedNode.LastNode;

            // treeView1.Nodes.Clear();
            //  Program.addToTree(Program.t, treeView1);
            //   treeView1.Nodes[0].Expand();
            //  treeView1.ExpandAll();
        }

        private void addPropToolStripButton1_Click(object sender, EventArgs e)
        {
            if (curTempTabPage.TreeView.SelectedNode is null) return;
            if (curTempTabPage.TreeView.SelectedNode.Name == "0")
            {
                Program.addCsvVar(curTempTabPage);
                Program.getCsvVars(curTempTabPage);
            }
            else
            {
                Element el = Program.getElementById(curTempTabPage.TreeView.SelectedNode.Name, curTempTabPage.Id);
                if (el is null) return;
                switch (el.GetType().ToString())
                {
                    case "VisualTemplate.Signal":
                        Signal s = el as Signal;
                        s.Add(new Property("1", "UInt4", "8"));
                        Program.getProperties(s, curTempTabPage.dgProps, curTempTabPage.TreeView.SelectedNode);
                        break;
                    case "VisualTemplate.Cycle":
                        Cycle c = el as Cycle;
                        Program.addVariant(c);
                        //  c.Add(new Variant("name", "value"));
                        Program.getVariants(c, curTempTabPage.dgProps);
                        break;
                }
            }

            toolStripButton1.Enabled = true;
            toolStripButton2.Enabled = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (curTempTabPage.TreeView.SelectedNode is null) return;
            if (curTempTabPage.TreeView.SelectedNode.Name == "0")
            {
                curTempTabPage.Template.CsvVars.RemoveAt(curTempTabPage.dgProps.SelectedCells[0].RowIndex);
                Program.getCsvVars(curTempTabPage);
                if (curTempTabPage.Template.CsvVars.Count < 1)
                {
                    toolStripButton1.Enabled = false;
                    toolStripButton2.Enabled = false;
                }
            }
            else
            {

                Element el = Program.getElementById(curTempTabPage.TreeView.SelectedNode.Name, curTempTabPage.Id);
                switch (el.GetType().ToString())
                {
                    case "VisualTemplate.Signal":
                        Signal s = el as Signal;
                        s.Properties.RemoveAt(curTempTabPage.dgProps.SelectedCells[0].RowIndex);
                        Program.getProperties(s, curTempTabPage.dgProps, curTempTabPage.TreeView.SelectedNode);
                        if (s.Properties.Count < 1)
                        {
                            toolStripButton1.Enabled = false;
                            toolStripButton2.Enabled = false;
                        }
                        break;
                    case "VisualTemplate.Cycle":
                        Cycle c = el as Cycle;
                        c.Variatns.RemoveAt(curTempTabPage.dgProps.SelectedCells[0].RowIndex);
                        Program.getVariants(c, curTempTabPage.dgProps);
                        if (c.Variatns.Count < 1)
                        {
                            toolStripButton1.Enabled = false;
                            toolStripButton2.Enabled = false;
                        }
                        break;
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Element el = Program.getElementById(curTempTabPage.TreeView.SelectedNode.Name, curTempTabPage.Id);
            switch (el.GetType().ToString())
            {
                case "VisualTemplate.Signal":
                    Signal s = el as Signal;
                    Property p = s.Properties[curTempTabPage.dgProps.SelectedCells[0].RowIndex];
                    Program.bufProp = p;
                    break;
                case "VisualTemplate.Cycle":
                    Cycle c = el as Cycle;
                    Variant v = c.Variatns[curTempTabPage.dgProps.SelectedCells[0].RowIndex];
                    Program.bufVar = v;
                    break;
            }
            toolStripButton3.Enabled = true;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Element el = Program.getElementById(curTempTabPage.TreeView.SelectedNode.Name, curTempTabPage.Id);
            switch (el.GetType().ToString())
            {
                case "VisualTemplate.Signal":
                    Signal s = el as Signal;
                    s.Add((Property)Program.bufProp.Clone());
                    Program.getProperties(s, curTempTabPage.dgProps, curTempTabPage.TreeView.SelectedNode);
                    break;
                case "VisualTemplate.Cycle":
                    Cycle c = el as Cycle;
                    Variant v = (Variant)Program.bufVar.Clone();
                    Program.setIdtoVar(v);
                    c.Add(v);
                    Program.getVariants(c, curTempTabPage.dgProps);
                    break;

            }
            toolStripButton1.Enabled = true;
            toolStripButton2.Enabled = true;

        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if(curTempTabPage.Template.CurPath is null)
            {
                saveFileDialog1.Filter = "jvt files (*.jvt)|*.jvt|All files (*.*)|*.*";
                saveFileDialog1.FileName = curTempTabPage.Template.Name;
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                curTempTabPage.Template.CurPath = saveFileDialog1.FileName;
            }
            Program.saveTemplate(curTempTabPage);
            curTempTabPage.TabPage.Text = curTempTabPage.Template.Name;
            curTempTabPage.Changed = false;
        }

        private void outToolStripButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            if (!(Program.defaultPathOutCsv is null)) saveFileDialog1.InitialDirectory = Program.defaultPathOutCsv;
            saveFileDialog1.FileName = curTempTabPage.Template.Name + "_out";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Program.globalCsvString = "";
            Program.getCsv(curTempTabPage.Template);

            var fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);
            var sw = new StreamWriter(fs, Encoding.Default);
            Program.globalCsvString = Program.globalCsvString.Trim();
            sw.Write(Program.globalCsvString);
            sw.Close();
            Program.defaultPathOutCsv = Path.GetDirectoryName(saveFileDialog1.FileName);
        }



        private void dgSettings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != 3 || e.ColumnIndex != 2) return;
            Element el = Program.getElementById(curTempTabPage.TreeView.SelectedNode.Name, curTempTabPage.Id);
            Program.updateVarsFromCsv(curTempTabPage, el as Cycle);
            Program.getVariants(el as Cycle, curTempTabPage.dgProps);
        }

        private void dgProps_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 4) return;
            string csvName = curTempTabPage.dgProps.Rows[e.RowIndex].Cells[0].Value.ToString();
            

            new CSVEdit(curTempTabPage, csvName);


            //Element el = Program.getElementById(curTempTabPage.TreeView.SelectedNode.Name, curTempTabPage.Id);
            //Program.updateVarsFromCsv(curTempTabPage, el as Cycle);
            //Program.getVariants(el as Cycle, curTempTabPage.dgProps);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(splitContainer1.Panel1Collapsed)
            {

                splitContainer1.Panel1Collapsed = false;
            }
            else
            {
                splitContainer1.Panel1Collapsed = true;
            }
                
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (Program.MasterFile.curPath is null)
            {
                saveFileDialog1.Filter = "master file (*.xmlvt)|*.xmlvt|All files (*.*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
            }
            Program.saveMaster(saveFileDialog1.FileName);

        }

        private void treeMasterView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeMasterView.SelectedNode.Name == "") return;
            tabControl2.SelectedIndex = int.Parse(treeMasterView.SelectedNode.Name);
 
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
 
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            if (!(Program.defaultPathOutCsv is null)) saveFileDialog1.InitialDirectory = Program.defaultPathOutCsv;
            saveFileDialog1.FileName = curTempTabPage.Template.Name + "_n_etc_out";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            Program.globalCsvString = "";

            foreach (TempTabPage tmp in Program.MasterFile.tempTabPagesDic.Values)
            {
                Program.getCsv(tmp.Template);
            }

            var fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);
            var sw = new StreamWriter(fs, Encoding.Default);
            Program.globalCsvString = Program.globalCsvString.Trim();
            sw.Write(Program.globalCsvString);
            sw.Close();
            Program.defaultPathOutCsv = Path.GetDirectoryName(saveFileDialog1.FileName);
            
        }



        private void getForSignalExceltoolStripButton5_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            if (!(Program.defaultPathOutCsv is null)) saveFileDialog1.InitialDirectory = Program.defaultPathOutCsv;
            saveFileDialog1.FileName = curTempTabPage.Template.Name + "_out";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Program.globalCsvString = "";
            Program.getForExcel(curTempTabPage.Template);

            var fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);
            var sw = new StreamWriter(fs, Encoding.Default);
            Program.globalCsvString = Program.globalCsvString.Trim();
            sw.Write(Program.globalCsvString);
            sw.Close();
            Program.defaultPathOutCsv = Path.GetDirectoryName(saveFileDialog1.FileName);

            if(MessageBox.Show(this,"Готово. Открыть файл?","Выполнено",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                ProcessStartInfo pci = new ProcessStartInfo("Excel.exe");
                pci.Arguments = '"' + saveFileDialog1.FileName + '"';
                Process.Start(pci);
            }
        }

        private void dublToolStripButton3_Click(object sender, EventArgs e)
        {
            Element el = Program.getElementById(curTempTabPage.TreeView.SelectedNode.Name, curTempTabPage.Id);
            Element dublEl = null;
            if (el is null) return;
            if (el.Parent is null) return;
            switch (el.GetType().ToString())
            {
                case "VisualTemplate.Signal":
                    Signal s = el as Signal;
                    dublEl = (Signal)s.Clone();
                    el.Parent.Signals.Add((Signal)dublEl);
                    dublEl.restoreParent(el.Parent);
                    Program.addToTree(dublEl, curTempTabPage.TreeView, curTempTabPage.Id, curTempTabPage.TreeView.SelectedNode.Parent);
                    //curTempTabPage.TreeView.SelectedNode.Expand();
                    break;
                case "VisualTemplate.Cycle":
                    Cycle c = el as Cycle;
                    dublEl = (Cycle)c.Clone();
                    el.Parent.Cycles.Add((Cycle)dublEl);
                    dublEl.restoreParent(el.Parent);
                    Program.addToTree(dublEl, curTempTabPage.TreeView, curTempTabPage.Id, curTempTabPage.TreeView.SelectedNode.Parent);
                    
                    //curTempTabPage.TreeView.SelectedNode.Expand();
                    break;
            }


    }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            curTempTabPage.TreeView.SelectedNode = e.Node;
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.deleteSelected(curTempTabPage);
        }

        private void папкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addElemnt(curTempTabPage);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void циклToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addCycle(curTempTabPage);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void int1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addElemnt(curTempTabPage, Program.CDTTypes.FirstOrDefault(x => x.Value == "Int1").Key);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void uInt1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addElemnt(curTempTabPage, Program.CDTTypes.FirstOrDefault(x => x.Value == "UInt1").Key);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void int2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addElemnt(curTempTabPage, Program.CDTTypes.FirstOrDefault(x => x.Value == "Int2").Key);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void uInt2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addElemnt(curTempTabPage, Program.CDTTypes.FirstOrDefault(x => x.Value == "UInt2").Key);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void int4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addElemnt(curTempTabPage, Program.CDTTypes.FirstOrDefault(x => x.Value == "Int4").Key);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void uInt4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addElemnt(curTempTabPage, Program.CDTTypes.FirstOrDefault(x => x.Value == "UInt4").Key);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void int8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addElemnt(curTempTabPage, Program.CDTTypes.FirstOrDefault(x => x.Value == "Int8").Key);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void uInt8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addElemnt(curTempTabPage, Program.CDTTypes.FirstOrDefault(x => x.Value == "UInt8").Key);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void floatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addElemnt(curTempTabPage, Program.CDTTypes.FirstOrDefault(x => x.Value == "Float").Key);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void doubleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addElemnt(curTempTabPage, Program.CDTTypes.FirstOrDefault(x => x.Value == "Double").Key);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void boolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addElemnt(curTempTabPage, Program.CDTTypes.FirstOrDefault(x => x.Value == "Bool").Key);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void stringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addElemnt(curTempTabPage, Program.CDTTypes.FirstOrDefault(x => x.Value == "String").Key);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pastToolStripButton1.Enabled = true;
            Element el = Program.getElementById(curTempTabPage.TreeView.SelectedNode.Name, curTempTabPage.Id);
            if (el is null) return;
            switch (el.GetType().ToString())
            {
                case "VisualTemplate.Signal":
                    Signal s = el as Signal;
                    Program.bufElem = s;
                    break;
                case "VisualTemplate.Cycle":
                    Cycle c = el as Cycle;
                    Program.bufElem = c;
                    break;
            }
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Element el = Program.getElementById(curTempTabPage.TreeView.SelectedNode.Name, curTempTabPage.Id);
            Element copyEl = null;

            if (curTempTabPage.TreeView.SelectedNode.Name == "0")
            {
                if (Program.bufElem.GetType().ToString() == "VisualTemplate.Cycle")
                {

                    Cycle bc = Program.bufElem as Cycle;
                    copyEl = (Cycle)bc.Clone();
                    curTempTabPage.Template.Cycles.Add((Cycle)copyEl);

                    Program.newVarsInDic((Cycle)copyEl);
                    Program.addToTree(copyEl, curTempTabPage.TreeView, curTempTabPage.Id, curTempTabPage.TreeView.SelectedNode);
                    curTempTabPage.TreeView.SelectedNode.Expand();
                    return;
                }
            }

            if (el is null) return;
            if (Program.bufElem.GetType().ToString() == "VisualTemplate.Signal")
            {
                Signal bs = Program.bufElem as Signal;
                copyEl = (Signal)bs.Clone();
                el.Add((Signal)copyEl);
            }
            else if (Program.bufElem.GetType().ToString() == "VisualTemplate.Cycle")
            {
                Cycle bc = Program.bufElem as Cycle;
                copyEl = (Cycle)bc.Clone();

                el.Add((Cycle)copyEl);
                Program.newVarsInDic((Cycle)copyEl);
            }

            Program.addToTree(copyEl, curTempTabPage.TreeView, curTempTabPage.Id, curTempTabPage.TreeView.SelectedNode);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void дублироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Element el = Program.getElementById(curTempTabPage.TreeView.SelectedNode.Name, curTempTabPage.Id);
            Element dublEl = null;
            if (el is null) return;
            if (el.Parent is null) return;
            switch (el.GetType().ToString())
            {
                case "VisualTemplate.Signal":
                    Signal s = el as Signal;
                    dublEl = (Signal)s.Clone();
                    el.Parent.Signals.Add((Signal)dublEl);
                    dublEl.restoreParent(el.Parent);
                    Program.addToTree(dublEl, curTempTabPage.TreeView, curTempTabPage.Id, curTempTabPage.TreeView.SelectedNode.Parent);
                    //curTempTabPage.TreeView.SelectedNode.Expand();
                    break;
                case "VisualTemplate.Cycle":
                    Cycle c = el as Cycle;
                    dublEl = (Cycle)c.Clone();
                    el.Parent.Cycles.Add((Cycle)dublEl);
                    dublEl.restoreParent(el.Parent);
                    Program.addToTree(dublEl, curTempTabPage.TreeView, curTempTabPage.Id, curTempTabPage.TreeView.SelectedNode.Parent);

                    //curTempTabPage.TreeView.SelectedNode.Expand();
                    break;
            }
        }



        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox ts = sender as ToolStripComboBox;

            switch(ts.SelectedIndex)
            {
                case 0:
                    curPropShowMode = PropShowMode.showProperties;
                    
                    break;
                case 1:
                    curPropShowMode = PropShowMode.showChildSignals;
                    break;
            }
            treeView1_AfterSelect(null, null);
        }
    }
}
