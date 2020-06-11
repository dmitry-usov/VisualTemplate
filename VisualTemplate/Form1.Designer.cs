namespace VisualTemplate
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.ctx_treeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctx_copyPath = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListForTree = new System.Windows.Forms.ImageList(this.components);
            this.dataGridProps = new System.Windows.Forms.DataGridView();
            this.bt_Open = new System.Windows.Forms.Button();
            this.bt_Save = new System.Windows.Forms.Button();
            this.bt_AddCycle = new System.Windows.Forms.Button();
            this.bt_Delete = new System.Windows.Forms.Button();
            this.bt_AddSignal = new System.Windows.Forms.Button();
            this.bt_AddPoV = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.dataGridSettings = new System.Windows.Forms.DataGridView();
            this.bt_DeletePoV = new System.Windows.Forms.Button();
            this.bt_getCsv = new System.Windows.Forms.Button();
            this.bt_Copy = new System.Windows.Forms.Button();
            this.bt_Past = new System.Windows.Forms.Button();
            this.bt_CopyProp = new System.Windows.Forms.Button();
            this.bt_PastProp = new System.Windows.Forms.Button();
            this.bt_PastLink = new System.Windows.Forms.Button();
            this.dataGridVariants = new System.Windows.Forms.DataGridView();
            this.bt_reloadTr = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctx_treeView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridProps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVariants)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.ContextMenuStrip = this.ctx_treeView;
            this.treeView1.HideSelection = false;
            this.treeView1.ImageIndex = 13;
            this.treeView1.ImageList = this.imageListForTree;
            this.treeView1.Location = new System.Drawing.Point(57, 133);
            this.treeView1.Name = "treeView1";
            this.treeView1.PathSeparator = ".";
            this.treeView1.SelectedImageIndex = 13;
            this.treeView1.Size = new System.Drawing.Size(168, 243);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            // 
            // ctx_treeView
            // 
            this.ctx_treeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctx_copyPath});
            this.ctx_treeView.Name = "ctx_treeView";
            this.ctx_treeView.Size = new System.Drawing.Size(167, 26);
            // 
            // ctx_copyPath
            // 
            this.ctx_copyPath.Name = "ctx_copyPath";
            this.ctx_copyPath.Size = new System.Drawing.Size(166, 22);
            this.ctx_copyPath.Text = "Копировать путь";
            this.ctx_copyPath.Click += new System.EventHandler(this.ctx_copyPath_Click);
            // 
            // imageListForTree
            // 
            this.imageListForTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListForTree.ImageStream")));
            this.imageListForTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListForTree.Images.SetKeyName(0, "folder.png");
            this.imageListForTree.Images.SetKeyName(1, "1.png");
            this.imageListForTree.Images.SetKeyName(2, "2.png");
            this.imageListForTree.Images.SetKeyName(3, "3.png");
            this.imageListForTree.Images.SetKeyName(4, "4.png");
            this.imageListForTree.Images.SetKeyName(5, "5.png");
            this.imageListForTree.Images.SetKeyName(6, "6.png");
            this.imageListForTree.Images.SetKeyName(7, "7.png");
            this.imageListForTree.Images.SetKeyName(8, "8.png");
            this.imageListForTree.Images.SetKeyName(9, "9.png");
            this.imageListForTree.Images.SetKeyName(10, "10.png");
            this.imageListForTree.Images.SetKeyName(11, "11.png");
            this.imageListForTree.Images.SetKeyName(12, "12.png");
            this.imageListForTree.Images.SetKeyName(13, "13.png");
            this.imageListForTree.Images.SetKeyName(14, "folderExp.png");
            this.imageListForTree.Images.SetKeyName(15, "icons8-обработать-24.png");
            // 
            // dataGridProps
            // 
            this.dataGridProps.AllowUserToAddRows = false;
            this.dataGridProps.AllowUserToDeleteRows = false;
            this.dataGridProps.AllowUserToResizeRows = false;
            this.dataGridProps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridProps.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridProps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridProps.Location = new System.Drawing.Point(353, 39);
            this.dataGridProps.MultiSelect = false;
            this.dataGridProps.Name = "dataGridProps";
            this.dataGridProps.RowHeadersVisible = false;
            this.dataGridProps.Size = new System.Drawing.Size(624, 200);
            this.dataGridProps.TabIndex = 1;
            this.dataGridProps.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridProps_CellEndEdit);
            this.dataGridProps.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
            this.dataGridProps.LocationChanged += new System.EventHandler(this.LastColumnComboSelectionChanged);
            // 
            // bt_Open
            // 
            this.bt_Open.Location = new System.Drawing.Point(93, 405);
            this.bt_Open.Name = "bt_Open";
            this.bt_Open.Size = new System.Drawing.Size(75, 23);
            this.bt_Open.TabIndex = 2;
            this.bt_Open.Text = "Open";
            this.bt_Open.UseVisualStyleBackColor = true;
            this.bt_Open.Click += new System.EventHandler(this.bt_Open_Click);
            // 
            // bt_Save
            // 
            this.bt_Save.Location = new System.Drawing.Point(174, 405);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(75, 23);
            this.bt_Save.TabIndex = 3;
            this.bt_Save.Text = "Save";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // bt_AddCycle
            // 
            this.bt_AddCycle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_AddCycle.Location = new System.Drawing.Point(397, 387);
            this.bt_AddCycle.Name = "bt_AddCycle";
            this.bt_AddCycle.Size = new System.Drawing.Size(75, 23);
            this.bt_AddCycle.TabIndex = 4;
            this.bt_AddCycle.Text = "Add Cycle";
            this.bt_AddCycle.UseVisualStyleBackColor = true;
            this.bt_AddCycle.Click += new System.EventHandler(this.bt_AddCycle_Click);
            // 
            // bt_Delete
            // 
            this.bt_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_Delete.Location = new System.Drawing.Point(559, 386);
            this.bt_Delete.Name = "bt_Delete";
            this.bt_Delete.Size = new System.Drawing.Size(75, 23);
            this.bt_Delete.TabIndex = 5;
            this.bt_Delete.Text = "Delete";
            this.bt_Delete.UseVisualStyleBackColor = true;
            this.bt_Delete.Click += new System.EventHandler(this.bt_Delete_Click);
            // 
            // bt_AddSignal
            // 
            this.bt_AddSignal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_AddSignal.Location = new System.Drawing.Point(397, 415);
            this.bt_AddSignal.Name = "bt_AddSignal";
            this.bt_AddSignal.Size = new System.Drawing.Size(75, 23);
            this.bt_AddSignal.TabIndex = 6;
            this.bt_AddSignal.Text = "Add Signal";
            this.bt_AddSignal.UseVisualStyleBackColor = true;
            this.bt_AddSignal.Click += new System.EventHandler(this.bt_AddSignal_Click);
            // 
            // bt_AddPoV
            // 
            this.bt_AddPoV.Location = new System.Drawing.Point(353, 245);
            this.bt_AddPoV.Name = "bt_AddPoV";
            this.bt_AddPoV.Size = new System.Drawing.Size(75, 23);
            this.bt_AddPoV.TabIndex = 7;
            this.bt_AddPoV.Text = "Add";
            this.bt_AddPoV.UseVisualStyleBackColor = true;
            this.bt_AddPoV.Click += new System.EventHandler(this.bt_AddPoV_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.RestoreDirectory = true;
            // 
            // dataGridSettings
            // 
            this.dataGridSettings.AllowUserToAddRows = false;
            this.dataGridSettings.AllowUserToDeleteRows = false;
            this.dataGridSettings.AllowUserToResizeRows = false;
            this.dataGridSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridSettings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSettings.Location = new System.Drawing.Point(353, 274);
            this.dataGridSettings.MultiSelect = false;
            this.dataGridSettings.Name = "dataGridSettings";
            this.dataGridSettings.RowHeadersVisible = false;
            this.dataGridSettings.Size = new System.Drawing.Size(237, 107);
            this.dataGridSettings.TabIndex = 8;
            this.dataGridSettings.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellEndEdit);
            // 
            // bt_DeletePoV
            // 
            this.bt_DeletePoV.Location = new System.Drawing.Point(434, 245);
            this.bt_DeletePoV.Name = "bt_DeletePoV";
            this.bt_DeletePoV.Size = new System.Drawing.Size(75, 23);
            this.bt_DeletePoV.TabIndex = 9;
            this.bt_DeletePoV.Text = "Delete";
            this.bt_DeletePoV.UseVisualStyleBackColor = true;
            this.bt_DeletePoV.Click += new System.EventHandler(this.bt_DeletePoV_Click);
            // 
            // bt_getCsv
            // 
            this.bt_getCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_getCsv.Location = new System.Drawing.Point(902, 415);
            this.bt_getCsv.Name = "bt_getCsv";
            this.bt_getCsv.Size = new System.Drawing.Size(75, 23);
            this.bt_getCsv.TabIndex = 10;
            this.bt_getCsv.Text = "getCSV";
            this.bt_getCsv.UseVisualStyleBackColor = true;
            this.bt_getCsv.Click += new System.EventHandler(this.bt_getCsv_Click);
            // 
            // bt_Copy
            // 
            this.bt_Copy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_Copy.Location = new System.Drawing.Point(478, 386);
            this.bt_Copy.Name = "bt_Copy";
            this.bt_Copy.Size = new System.Drawing.Size(75, 23);
            this.bt_Copy.TabIndex = 11;
            this.bt_Copy.Text = "Copy";
            this.bt_Copy.UseVisualStyleBackColor = true;
            this.bt_Copy.Click += new System.EventHandler(this.bt_Copy_Click);
            // 
            // bt_Past
            // 
            this.bt_Past.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_Past.Location = new System.Drawing.Point(478, 415);
            this.bt_Past.Name = "bt_Past";
            this.bt_Past.Size = new System.Drawing.Size(75, 23);
            this.bt_Past.TabIndex = 12;
            this.bt_Past.Text = "Past";
            this.bt_Past.UseVisualStyleBackColor = true;
            this.bt_Past.Click += new System.EventHandler(this.bt_Past_Click);
            // 
            // bt_CopyProp
            // 
            this.bt_CopyProp.Location = new System.Drawing.Point(515, 245);
            this.bt_CopyProp.Name = "bt_CopyProp";
            this.bt_CopyProp.Size = new System.Drawing.Size(75, 23);
            this.bt_CopyProp.TabIndex = 13;
            this.bt_CopyProp.Text = "Copy";
            this.bt_CopyProp.UseVisualStyleBackColor = true;
            this.bt_CopyProp.Click += new System.EventHandler(this.bt_CopyProp_Click);
            // 
            // bt_PastProp
            // 
            this.bt_PastProp.Location = new System.Drawing.Point(596, 245);
            this.bt_PastProp.Name = "bt_PastProp";
            this.bt_PastProp.Size = new System.Drawing.Size(75, 23);
            this.bt_PastProp.TabIndex = 14;
            this.bt_PastProp.Text = "Past";
            this.bt_PastProp.UseVisualStyleBackColor = true;
            this.bt_PastProp.Click += new System.EventHandler(this.bt_PastProp_Click);
            // 
            // bt_PastLink
            // 
            this.bt_PastLink.Location = new System.Drawing.Point(677, 245);
            this.bt_PastLink.Name = "bt_PastLink";
            this.bt_PastLink.Size = new System.Drawing.Size(75, 23);
            this.bt_PastLink.TabIndex = 15;
            this.bt_PastLink.Text = "Past Link";
            this.bt_PastLink.UseVisualStyleBackColor = true;
            this.bt_PastLink.Click += new System.EventHandler(this.bt_PastLink_Click);
            // 
            // dataGridVariants
            // 
            this.dataGridVariants.AllowUserToAddRows = false;
            this.dataGridVariants.AllowUserToDeleteRows = false;
            this.dataGridVariants.AllowUserToResizeRows = false;
            this.dataGridVariants.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridVariants.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridVariants.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridVariants.Location = new System.Drawing.Point(740, 274);
            this.dataGridVariants.MultiSelect = false;
            this.dataGridVariants.Name = "dataGridVariants";
            this.dataGridVariants.RowHeadersVisible = false;
            this.dataGridVariants.Size = new System.Drawing.Size(237, 107);
            this.dataGridVariants.TabIndex = 16;
            // 
            // bt_reloadTr
            // 
            this.bt_reloadTr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_reloadTr.Location = new System.Drawing.Point(559, 415);
            this.bt_reloadTr.Name = "bt_reloadTr";
            this.bt_reloadTr.Size = new System.Drawing.Size(75, 23);
            this.bt_reloadTr.TabIndex = 17;
            this.bt_reloadTr.Text = "RelaodTree";
            this.bt_reloadTr.UseVisualStyleBackColor = true;
            this.bt_reloadTr.Click += new System.EventHandler(this.bt_reloadTr_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(758, 245);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "CopyVars";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(839, 245);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "PastVars";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(200, 100);
            this.tabControl1.TabIndex = 20;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(192, 74);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(989, 24);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 450);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bt_reloadTr);
            this.Controls.Add(this.dataGridVariants);
            this.Controls.Add(this.bt_PastLink);
            this.Controls.Add(this.bt_PastProp);
            this.Controls.Add(this.bt_CopyProp);
            this.Controls.Add(this.bt_Past);
            this.Controls.Add(this.bt_Copy);
            this.Controls.Add(this.bt_getCsv);
            this.Controls.Add(this.bt_DeletePoV);
            this.Controls.Add(this.dataGridSettings);
            this.Controls.Add(this.bt_AddPoV);
            this.Controls.Add(this.bt_AddSignal);
            this.Controls.Add(this.bt_Delete);
            this.Controls.Add(this.bt_AddCycle);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.bt_Open);
            this.Controls.Add(this.dataGridProps);
            this.Controls.Add(this.treeView1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = " ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ctx_treeView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridProps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVariants)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridProps;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button bt_Open;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.Button bt_AddCycle;
        private System.Windows.Forms.Button bt_Delete;
        private System.Windows.Forms.Button bt_AddSignal;
        private System.Windows.Forms.Button bt_AddPoV;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.DataGridView dataGridSettings;
        private System.Windows.Forms.Button bt_DeletePoV;
        private System.Windows.Forms.Button bt_getCsv;
        private System.Windows.Forms.Button bt_Copy;
        private System.Windows.Forms.Button bt_Past;
        private System.Windows.Forms.Button bt_CopyProp;
        private System.Windows.Forms.Button bt_PastProp;
        private System.Windows.Forms.Button bt_PastLink;
        private System.Windows.Forms.DataGridView dataGridVariants;
        private System.Windows.Forms.Button bt_reloadTr;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ContextMenuStrip ctx_treeView;
        private System.Windows.Forms.ToolStripMenuItem ctx_copyPath;
        private System.Windows.Forms.ImageList imageListForTree;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
    }
}

