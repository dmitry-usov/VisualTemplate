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
        private Form1 _main;
        public FindRep(Form1 main)
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
    }
}
