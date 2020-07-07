using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualTemplate
{
    public partial class MainForm : Form
    {
        private TempTabPage curTempTabPage;
        public MainForm()
        {
            InitializeComponent();
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
            tabControl2.TabPages.Add(curTempTabPage.TabPage);
            
            Program.addToTree(curTempTabPage.Template, curTempTabPage.TreeView, curTempTabPage.Id);
            curTempTabPage.TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            curTempTabPage.dgSettings.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellEndEdit);
            curTempTabPage.dgProps.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridProps_CellEndEdit);
            curTempTabPage.dgSettings.Columns.Add("Param", "Наименование");
            curTempTabPage.dgSettings.Columns.Add("Value", "Значение");
            curTempTabPage.dgSettings.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            curTempTabPage.dgSettings.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            tabControl2.SelectedIndex = curTempTabPage.Id;
            toolStripStatusLabel1.Text = Program.TemplatesPages.Count.ToString();
            curTempTabPage.TreeView.ImageList = imageListForTree;
            curTempTabPage.Changed = true;
        }

        private void closeToolStripButton_Click(object sender, EventArgs e)
        {
            if (curTempTabPage.Changed)
            {
                DialogResult res = MessageBox.Show(this, "Сохранить изменения перед закрытием?", "Сохранить?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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
            Program.addFolder(curTempTabPage);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }

        private void signalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.addSignal(curTempTabPage);
            curTempTabPage.TreeView.SelectedNode.Expand();
        }


        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            closeToolStripButton.Enabled = true;

            curTempTabPage = Program.CreateNewTemplate();
            curTempTabPage.Id = tabControl2.TabPages.Count;

            if (Program.loadTemplate(openFileDialog1, curTempTabPage) == false)
            {
                Program.TemplatesPages.Remove(curTempTabPage.Id);
                return;
            }



            tabControl2.TabPages.Add(curTempTabPage.TabPage);
            curTempTabPage.TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            curTempTabPage.dgSettings.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellEndEdit);
            curTempTabPage.dgProps.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridProps_CellEndEdit);
            curTempTabPage.dgSettings.Columns.Add("Param", "Наименование");
            curTempTabPage.dgSettings.Columns.Add("Value", "Значение");
            curTempTabPage.dgSettings.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            curTempTabPage.dgSettings.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            curTempTabPage.TreeView.ImageList = imageListForTree;
            curTempTabPage.TabPage.Text = curTempTabPage.Template.Name;
            tabControl2.SelectedIndex = curTempTabPage.Id;
            toolStripStatusLabel1.Text = Program.TemplatesPages.Count.ToString();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            cycleToolStripMenuItem.Enabled = false;
            folderToolStripMenuItem.Enabled = false;
            signalToolStripMenuItem.Enabled = false;
            deleteToolStripButton2.Enabled = true;
            

            TempTabPage curTmp = Program.TemplatesPages[tabControl2.SelectedIndex];
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
                curTmp.dgProps.Columns.Clear();
                curTmp.dgProps.Columns.Add("Name", "Name");
                curTmp.dgProps.Columns.Add("Value", "Value");
                curTmp.dgProps.Columns.Add("Step", "Step");
                curTmp.dgProps.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                curTmp.dgProps.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                curTmp.dgProps.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                Cycle c = obj as Cycle;
                Program.getSettings(c, TrNdSel, curTmp.dgSettings);
                Program.getVariants(c, curTmp.dgProps);

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
                curTmp.dgProps.Columns.Clear();
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

                curTmp.dgProps.Columns.Add(cmbID);
                curTmp.dgProps.Columns.Add(cmbType);
                curTmp.dgProps.Columns.Add("Value", "Value");
                curTmp.dgProps.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                curTmp.dgProps.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                curTmp.dgProps.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                Signal s = obj as Signal;
                Program.getSettings(s, TrNdSel, curTmp.dgSettings);
                Program.getProperties(s, curTmp.dgProps, TrNdSel);

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

                //dataGridVariants.Columns.Clear();
                //dataGridVariants.Columns.Add("Name", "Name");
                //dataGridVariants.Columns.Add("Value", "Value");
                //dataGridVariants.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                //dataGridVariants.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

                //Program.getVariants(Program.getParentCycle(s), dataGridVariants);

            }
            if (obj.GetType().ToString() == "VisualTemplate.Template")
            {
                cycleToolStripMenuItem.Enabled = true;
                deleteToolStripButton2.Enabled = false;

                toolStripButton1.Enabled = false;
                toolStripButton2.Enabled = false;
                toolStripButton3.Enabled = false;

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

                Program.getCsvVars(curTmp);
                Program.getSettings(t, TrNdSel, curTmp.dgSettings);

            }
            
            toolStrip2.Enabled = true;
            toolStrip3.Enabled = true;
        }


        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            
            TreeNode TrNdSel = curTempTabPage.TreeView.SelectedNode;
             if (TrNdSel.Name == "0")
            {
                curTempTabPage.Template.Name = curTempTabPage.dgSettings.Rows[0].Cells[1].Value.ToString();
                TrNdSel.Text = curTempTabPage.Template.Name;
                curTempTabPage.TabPage.Text = curTempTabPage.Template.Name;
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
            if (dataGridProps.CurrentCell.ColumnIndex == 0 && e.Control is ComboBox)
            {
                ComboBox comboBox = e.Control as ComboBox;
                comboBox.SelectedIndexChanged -= LastColumnComboSelectionChanged;
                comboBox.SelectedIndexChanged += LastColumnComboSelectionChanged;
            }
        }

        private void LastColumnComboSelectionChanged(object sender, EventArgs e)
        {
            var currentcell = dataGridProps.CurrentCellAddress;
            var sendingCB = sender as DataGridViewComboBoxEditingControl;
            if (currentcell.X == 0)
            {
                dataGridProps.Rows[currentcell.Y].Cells[1].Value = Program.TypeOfProperty[sendingCB.EditingControlFormattedValue.ToString()];
            }
        }

        public void StartReplace(string str1, string str2)
        {
            Program.ReplaceInTree(treeView1.SelectedNode, str1, str2);
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
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
            treeView1.SelectedNode.Expand();
        }

        private void bt_AddSignal_Click(object sender, EventArgs e)
        {

        }




        private void dataGridProps_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
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

        private void bt_AddPoV_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode is null) return;
            if (treeView1.SelectedNode.Name == "0")
            {
                Program.addCsvVar(curTempTabPage);
                Program.getCsvVars(curTempTabPage);
            }
            else
            {
                Element el = Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex);
                if (el is null) return;
                switch (el.GetType().ToString())
                {
                    case "VisualTemplate.Signal":
                        Signal s = el as Signal;
                        s.Add(new Property("1", "UInt4", "8"));
                        Program.getProperties(s, dataGridProps, treeView1.SelectedNode);
                        break;
                    case "VisualTemplate.Cycle":
                        Cycle c = el as Cycle;
                        Program.addVariant(c);
                      //  c.Add(new Variant("name", "value"));
                        Program.getVariants(c, dataGridProps);
                        break;
                }
            }
        }

        private void bt_DeletePoV_Click(object sender, EventArgs e)
        {

            if (treeView1.SelectedNode is null) return;
            if (treeView1.SelectedNode.Name == "0")
            {
                Program.t.CsvVars.RemoveAt(dataGridProps.SelectedCells[0].RowIndex);
               // Program.getCsvVars(dataGridProps);
            }
            else
            {

                Element el = Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex);
                switch (el.GetType().ToString())
                {
                    case "VisualTemplate.Signal":
                        Signal s = el as Signal;
                        s.Properties.RemoveAt(dataGridProps.SelectedCells[0].RowIndex);
                        Program.getProperties(s, dataGridProps, treeView1.SelectedNode);
                        break;
                    case "VisualTemplate.Cycle":
                        Cycle c = el as Cycle;
                        c.Variatns.RemoveAt(dataGridProps.SelectedCells[0].RowIndex);
                        Program.getVariants(c, dataGridProps);
                        break;
                }
            }
        }

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

        private void bt_Past_Click(object sender, EventArgs e)
        {
            Element el = Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex);
            Element copyEl=null;
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

          //  Program.addToTree(copyEl, treeView1, treeView1.SelectedNode);

           // treeView1.Nodes.Clear();
          //  Program.addToTree(Program.t, treeView1);
         //   treeView1.Nodes[0].Expand();
          //  treeView1.ExpandAll();
        }

        private void bt_CopyProp_Click(object sender, EventArgs e)
        {
            Element el = Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex);
            switch (el.GetType().ToString())
            {
                case "VisualTemplate.Signal":
                    Signal s = el as Signal;
                    Property p = s.Properties[dataGridProps.SelectedCells[0].RowIndex];
                    Program.bufProp = p;
                    break;
                case "VisualTemplate.Cycle":
                    Cycle c = el as Cycle;
                    Variant v = c.Variatns[dataGridProps.SelectedCells[0].RowIndex];
                    Program.bufVar = v;
                    break;
            }
        }

        private void bt_PastProp_Click(object sender, EventArgs e)
        {
            Element el = Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex);
            switch (el.GetType().ToString())
            {
                case "VisualTemplate.Signal":
                    Signal s = el as Signal;
                    s.Add((Property)Program.bufProp.Clone());
                    Program.getProperties(s, dataGridProps, treeView1.SelectedNode);
                    break;
                case "VisualTemplate.Cycle":
                    Cycle c = el as Cycle;
                    Variant v = (Variant)Program.bufVar.Clone();
                    Program.setIdtoVar(v);
                    c.Add(v);
                    Program.getVariants(c, dataGridProps);
                    break;
            }
        }

        private void bt_PastLink_Click(object sender, EventArgs e)
        {
            Element el = Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex);
            switch (el.GetType().ToString())
            {
                case "VisualTemplate.Signal":
                    break;
                case "VisualTemplate.Cycle":
                    Cycle c = el as Cycle;
                    c.Add(Program.bufVar);
                    Program.bufVar.Link = Program.bufVar.Id;
                    treeView1.Nodes.Clear();
                   // Program.addToTree(Program.t, treeView1);
                    treeView1.Nodes[0].Expand();
                    break;
            }
        }

        private void bt_reloadTr_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
          //  Program.addToTree(Program.t, treeView1);
            treeView1.Nodes[0].Expand();
        }


 
        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F && e.Control)
            {
                FindRep f = new FindRep(this);
                f.Show();
            }
        }

        private void ctx_copyPath_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Program.getElementById(treeView1.SelectedNode.Name, tabControl2.SelectedIndex).FullPath);
        }

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
            curTempTabPage.TreeView.SelectedNode = curTempTabPage.TreeView.SelectedNode.LastNode;
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
                saveFileDialog1.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
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
    }
}
