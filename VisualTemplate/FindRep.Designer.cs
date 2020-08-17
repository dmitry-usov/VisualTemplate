namespace VisualTemplate
{
    partial class FindRep
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
            this.bt_Close = new System.Windows.Forms.Button();
            this.bt_Find = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.bt_Replace = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bt_Close
            // 
            this.bt_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Close.Location = new System.Drawing.Point(314, 129);
            this.bt_Close.Name = "bt_Close";
            this.bt_Close.Size = new System.Drawing.Size(75, 23);
            this.bt_Close.TabIndex = 1;
            this.bt_Close.Text = "Закрыть";
            this.bt_Close.UseVisualStyleBackColor = true;
            this.bt_Close.Click += new System.EventHandler(this.bt_Close_Click);
            // 
            // bt_Find
            // 
            this.bt_Find.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Find.Enabled = false;
            this.bt_Find.Location = new System.Drawing.Point(233, 129);
            this.bt_Find.Name = "bt_Find";
            this.bt_Find.Size = new System.Drawing.Size(75, 23);
            this.bt_Find.TabIndex = 2;
            this.bt_Find.Text = "Найти";
            this.bt_Find.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(84, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(305, 20);
            this.textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(84, 38);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(305, 20);
            this.textBox2.TabIndex = 4;
            // 
            // bt_Replace
            // 
            this.bt_Replace.Location = new System.Drawing.Point(12, 129);
            this.bt_Replace.Name = "bt_Replace";
            this.bt_Replace.Size = new System.Drawing.Size(75, 23);
            this.bt_Replace.TabIndex = 5;
            this.bt_Replace.Text = "Заменить";
            this.bt_Replace.UseVisualStyleBackColor = true;
            this.bt_Replace.Click += new System.EventHandler(this.bt_Replace_Click);
            // 
            // FindRep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 159);
            this.Controls.Add(this.bt_Replace);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.bt_Find);
            this.Controls.Add(this.bt_Close);
            this.Name = "FindRep";
            this.Text = "FindRep";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FindRep_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bt_Close;
        private System.Windows.Forms.Button bt_Find;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button bt_Replace;
    }
}