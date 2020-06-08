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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridSettings.Columns.Add("Param", "Наименование");
            dataGridSettings.Columns.Add("Value", "Значение");
            dataGridSettings.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridSettings.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            Program.addToTree(Program.t,treeView1);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeView1.SelectedNode.SelectedImageIndex = treeView1.SelectedNode.ImageIndex;
            object obj = Program.getObjectFromTree(treeView1.SelectedNode.Name);
            if (obj.GetType().ToString() == "VisualTemplate.Cycle")
            {
                dataGridProps.Columns.Clear();
                dataGridProps.Columns.Add("Name", "Name");
                dataGridProps.Columns.Add("Value", "Value");
                dataGridProps.Columns.Add("Step", "Step");
                dataGridProps.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridProps.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridProps.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                Cycle c = obj as Cycle;
                Program.getSettings(c, treeView1.SelectedNode, dataGridSettings);
                Program.getVariants(c, dataGridProps);
            }
            if (obj.GetType().ToString() == "VisualTemplate.Signal")
            {
                 dataGridProps.Columns.Clear();
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

                dataGridProps.Columns.Add(cmbID);
                dataGridProps.Columns.Add(cmbType);
                dataGridProps.Columns.Add("Value", "Value");
                dataGridProps.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridProps.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridProps.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                Signal s = obj as Signal;
                Program.getSettings(s, treeView1.SelectedNode, dataGridSettings);
                Program.getProperties(s, dataGridProps, treeView1.SelectedNode);



                dataGridVariants.Columns.Clear();
                dataGridVariants.Columns.Add("Name", "Name");
                dataGridVariants.Columns.Add("Value", "Value");
                dataGridVariants.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridVariants.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

                Program.getVariants(Program.getParentCycle(s), dataGridVariants);

            }



            if (obj.GetType().ToString() == "VisualTemplate.Template")
            {
                Template t = obj as Template;
                dataGridProps.Columns.Clear();
                dataGridProps.Columns.Add("Name", "Name");
                dataGridProps.Columns.Add("Path", "Path");
                dataGridProps.Columns.Add("Separator", "Separator");
                dataGridProps.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridProps.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridProps.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                DataGridViewComboBoxColumn EncType = new DataGridViewComboBoxColumn();
                EncType.Items.Add("ASCII");
                EncType.Items.Add("Default");
                EncType.Items.Add("UTF8");
                EncType.HeaderText = "Кодировка";
                dataGridProps.Columns.Add(EncType);
                dataGridProps.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

                Program.getCsvVars(dataGridProps);
                Program.getSettings(t, treeView1.SelectedNode, dataGridSettings);
            }
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
            Program.loadTemplate(openFileDialog1, treeView1);
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            Program.saveTemplate(saveFileDialog1);
        }

        private void bt_Delete_Click(object sender, EventArgs e)
        {
            Program.deleteSelected(treeView1.SelectedNode);
        }

        private void bt_AddCycle_Click(object sender, EventArgs e)
        {
            Program.addCycle(treeView1.SelectedNode);
            treeView1.SelectedNode.Expand();
        }

        private void bt_AddSignal_Click(object sender, EventArgs e)
        {
            Program.addSignal(treeView1.SelectedNode);
            treeView1.SelectedNode.Expand();
        }


        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
           
            if (treeView1.SelectedNode.Name == "0")
            {
                Program.t.Name = dataGridSettings.Rows[0].Cells[1].Value.ToString();
                treeView1.SelectedNode.Text = Program.t.Name;
            }
            else
            {
                Element el = Program.getElementById(treeView1.SelectedNode.Name);
                switch (el.GetType().ToString())
                {
                    case "VisualTemplate.Signal":
                        Signal s = el as Signal;
                        Program.setSignal(treeView1.SelectedNode, dataGridSettings.Rows[0].Cells[1].Value.ToString());
                        treeView1.SelectedNode.Text = s.ToString();
                        break;
                    case "VisualTemplate.Cycle":
                        Program.setCycle(treeView1.SelectedNode, dataGridSettings );
                        Cycle c = el as Cycle;
                        treeView1.SelectedNode.Text = c.ToString();
                        break;
                }
            }
        }

        private void dataGridProps_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (treeView1.SelectedNode.Name == "0")
            {
                Program.setCsvVarPath(dataGridProps);
            }
            else
            {
                Element el = Program.getElementById(treeView1.SelectedNode.Name);
                if (el is null) return;
                switch (el.GetType().ToString())
                {
                    case "VisualTemplate.Signal":
                        Signal s = el as Signal;
                        Program.setProperties(s, dataGridProps, treeView1.SelectedNode);
                        break;
                    case "VisualTemplate.Cycle":
                        Cycle c = el as Cycle;
                        Program.setVariants(c, dataGridProps);
                        break;

                }
            }
        }

        private void bt_AddPoV_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode is null) return;
            if (treeView1.SelectedNode.Name == "0")
            {
                Program.addCsvVar();
                Program.getCsvVars(dataGridProps);
            }
            else
            {
                Element el = Program.getElementById(treeView1.SelectedNode.Name);
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
                Program.getCsvVars(dataGridProps);
            }
            else
            {

                Element el = Program.getElementById(treeView1.SelectedNode.Name);
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
            Element el = Program.getElementById(treeView1.SelectedNode.Name);
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

        private void bt_Past_Click(object sender, EventArgs e)
        {
            Element el = Program.getElementById(treeView1.SelectedNode.Name);
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

            Program.addToTree(copyEl, treeView1, treeView1.SelectedNode);

           // treeView1.Nodes.Clear();
          //  Program.addToTree(Program.t, treeView1);
         //   treeView1.Nodes[0].Expand();
          //  treeView1.ExpandAll();
        }

        private void bt_CopyProp_Click(object sender, EventArgs e)
        {
            Element el = Program.getElementById(treeView1.SelectedNode.Name);
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
            Element el = Program.getElementById(treeView1.SelectedNode.Name);
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
            Element el = Program.getElementById(treeView1.SelectedNode.Name);
            switch (el.GetType().ToString())
            {
                case "VisualTemplate.Signal":
                    break;
                case "VisualTemplate.Cycle":
                    Cycle c = el as Cycle;
                    c.Add(Program.bufVar);
                    Program.bufVar.Link = Program.bufVar.Id;
                    treeView1.Nodes.Clear();
                    Program.addToTree(Program.t, treeView1);
                    treeView1.Nodes[0].Expand();
                    break;
            }
        }

        private void bt_reloadTr_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            Program.addToTree(Program.t, treeView1);
            treeView1.Nodes[0].Expand();
        }

        private void button1_Click(object sender, EventArgs e)
        {

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
            Clipboard.SetText(Program.getElementById(treeView1.SelectedNode.Name).FullPath);
        }
    }
}
