namespace Mercure._familles
{
    partial class FamillesForm
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
            this.listView = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Location = new System.Drawing.Point(15, 12);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(284, 236);
            this.listView.TabIndex = 4;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ColumnClick);
            this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_KeyDown);
            this.listView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_Clic);
            this.listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_DoubleClic);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // FamillesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 278);
            this.Controls.Add(this.listView);
            this.Name = "FamillesForm";
            this.Text = "FamillesForm";
            this.Load += new System.EventHandler(this.FamillesForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}