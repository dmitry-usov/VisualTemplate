using System;
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
    public partial class CSVEdit : Form
    {

        public string csvText;
        CsvVar csv;
        public CSVEdit()
        {
            InitializeComponent();            
        }

        public CSVEdit(TempTabPage tmp, string csvName)
        {
            InitializeComponent();
            loadCsv(tmp, csvName);
            richTextBox1.Text = csvText;
            toDataGrid();
            tabControl1.SelectedIndex = 0;
            Show();
        }

        void loadCsv(TempTabPage tmp, string csvName)
        {

            csv = tmp.Template.getCsvVar(csvName);

            try
            {
                using (StreamReader sr = new StreamReader(Program.getCsvPath(csv, tmp.Template), csv.Encoding))
                {
                    csvText = sr.ReadToEnd();
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        void toDataGrid()
        {
            dataGridView1.Columns.Clear();
            string[] rowsArray = csvText.Split("\n".ToCharArray());
            string[] headerArray = rowsArray[0].Split(csv.Separator);

            for (int h = 0; h < headerArray.Length; h++)
            {
                dataGridView1.Columns.Add("", "");
            }

            for (int r = 0; r< rowsArray.Length; r++)
            {
                dataGridView1.Rows.Add();
                string[] readRowArray = rowsArray[r].Split(csv.Separator);
                for (int c=0; c < readRowArray.Length; c++)
                {
                    dataGridView1.Rows[r].Cells[c].Value = readRowArray[c].Replace("\r","");
                }
            }
        }
    }
}
