using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualTemplate
{
    public partial class FindRep : Form
    {
        private MainForm _main;
        public FindRep(MainForm main)
        {
            InitializeComponent();
            _main = main;
        }

        private void bt_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bt_Replace_Click(object sender, EventArgs e)
        {
            _main.StartReplace(textBox1.Text, textBox2.Text);
            MessageBox.Show("Замена выполнена!");
           
        }

        private void FindRep_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
