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
    public partial class PropEdit : Form
    {
        Property p;

        public PropEdit(Property pd)
        {
            p = pd;
            InitializeComponent();
            myInit();

            cb_ids.SelectedItem = p.Id;
            cb_types.SelectedItem = p.Type;


            //int i = 0;
            //foreach (string cId in cb_ids.Items)
            //{
            //    if (cId == p.Id)
            //    {
            //        cb_ids.SelectedIndex = i;
            //    }
            //    i++;
            //}

            // i = 0;
            //foreach (string cT in cb_types.Items)
            //{
            //    if (cT == p.Type)
            //    {
            //        cb_types.SelectedIndex = i;
            //    }
            //    i++;
            //}

            richTextBox1.Text = p.Value;

        }

        private void myInit()
        {
            foreach (string str in Program.TypeOfProperty.Keys)
            {
                cb_ids.Items.Add(str);
            }

            cb_CDTType.Items.Add("");
            foreach (string str in Program.Types.Values)
            {
                cb_types.Items.Add(str);
            }

            foreach (string str in Program.CDTTypes.Values)
            {
                cb_CDTType.Items.Add(str);
            }


        }

        private void cb_ids_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_types.SelectedItem = Program.TypeOfProperty[cb_ids.SelectedItem.ToString()];
            textBox1.Text = Service.getNameById(cb_ids.SelectedItem.ToString());
            textBox2.Text = Service.getDescrById(cb_ids.SelectedItem.ToString());
            if (cb_ids.SelectedItem.ToString() == "1")
            {
                richTextBox1.Visible = false;
                cb_CDTType.Visible = true;
                if (Program.CDTTypes.ContainsKey(p.Value))
                {
                    cb_CDTType.SelectedItem = Program.CDTTypes[p.Value];
                }
                
            }
            else
            {
                richTextBox1.Visible = true;
                cb_CDTType.Visible = false;
            }
        }

        private void cb_CDTType_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = Program.CDTTypes.FirstOrDefault(x => x.Value == cb_CDTType.SelectedItem.ToString()).Key; 
        }

        private void bt_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bt_ok_Click(object sender, EventArgs e)
        {
            p.Id = cb_ids.SelectedItem.ToString();
            p.Type = cb_types.SelectedItem.ToString();
            p.Value = richTextBox1.Text;
            
        }

    }
}
