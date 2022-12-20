using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualTemplate
{
   public class TempTabPage
    {
        public TabPage TabPage { get; set; }
        public TreeView TreeView { get; set; }
        public DataGridView dgProps { get; set; }
        public DataGridView dgSettings { get; set; }
        public DataGridView dgVariants { get; set; }
        public Template Template { get; set; }
        public ArrayList Elements;
        public int Id { get; set; }
        public bool Changed { get; set; }
        
        public TempTabPage()
        {

        }

        public TempTabPage(string str)
        {
            //Создание страницы
            TabPage = new TabPage(str + "*");
            Changed = false;

             Elements = new ArrayList();

            //Создание сплитконтейнера Для дерева и Для свойств
            SplitContainer sp_TreeNdgv = new SplitContainer()
            {
                Orientation = Orientation.Vertical,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                SplitterDistance = 35
            };
            TabPage.Controls.Add(sp_TreeNdgv);

            //Создание сплитконтейнера для дерева и для настроек
            SplitContainer sp_TreeNSet = new SplitContainer()
            {
                Orientation = Orientation.Horizontal,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                SplitterDistance = 70
            };
            sp_TreeNdgv.Panel1.Controls.Add(sp_TreeNSet);

            //Создание сплитконтейнера для свойств м для списка адресов
            SplitContainer sp_PropsNAddr = new SplitContainer()
            {
                Orientation = Orientation.Vertical,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                SplitterDistance = 100,
            };
            sp_TreeNdgv.Panel2.Controls.Add(sp_PropsNAddr);

            //Создание сплитконтейнера для упоминаний перменных и для переменных 
            SplitContainer sp_LinksNVariants = new SplitContainer()
            {
                Orientation = Orientation.Horizontal,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                SplitterDistance = 100,
            };
            sp_PropsNAddr.Panel2.Controls.Add(sp_LinksNVariants);

            //Дерево
            TreeView = new TreeView()
            {
                Dock = DockStyle.Fill,
                HideSelection = false
            };
            sp_TreeNSet.Panel1.Controls.Add(TreeView);

            //Датагрид настройки
            dgSettings = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                ColumnHeadersVisible = true

            };
            sp_TreeNSet.Panel2.Controls.Add(dgSettings);

            //Датагрид свойства 
            dgProps = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                ColumnHeadersVisible = true
            };
            sp_PropsNAddr.Panel1.Controls.Add(dgProps);

            //Датагрид для добавления перменных
            dgVariants = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                ColumnHeadersVisible = true
            };
            sp_LinksNVariants.Panel1.Controls.Add(dgVariants);



            Template = new Template(str);
        }
    }
}
