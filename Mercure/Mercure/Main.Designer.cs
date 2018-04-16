namespace Mercure
{
    partial class Main
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fichierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importerDesDonnéesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donnéesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rechargerF5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ajouterUnArticleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.marquesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.famillesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.listView = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sousfamillesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fichierToolStripMenuItem,
            this.donnéesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(493, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fichierToolStripMenuItem
            // 
            this.fichierToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importerDesDonnéesToolStripMenuItem});
            this.fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            this.fichierToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.fichierToolStripMenuItem.Text = "Fichier";
            // 
            // importerDesDonnéesToolStripMenuItem
            // 
            this.importerDesDonnéesToolStripMenuItem.Name = "importerDesDonnéesToolStripMenuItem";
            this.importerDesDonnéesToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.importerDesDonnéesToolStripMenuItem.Text = "Importer des données";
            this.importerDesDonnéesToolStripMenuItem.Click += new System.EventHandler(this.importerDesDonnéesToolStripMenuItem_Click);
            // 
            // donnéesToolStripMenuItem
            // 
            this.donnéesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rechargerF5ToolStripMenuItem,
            this.ajouterUnArticleToolStripMenuItem,
            this.marquesToolStripMenuItem,
            this.famillesToolStripMenuItem,
            this.sousfamillesToolStripMenuItem});
            this.donnéesToolStripMenuItem.Name = "donnéesToolStripMenuItem";
            this.donnéesToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.donnéesToolStripMenuItem.Text = "Données";
            // 
            // rechargerF5ToolStripMenuItem
            // 
            this.rechargerF5ToolStripMenuItem.Name = "rechargerF5ToolStripMenuItem";
            this.rechargerF5ToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.rechargerF5ToolStripMenuItem.Text = "Recharger les données (F5)";
            this.rechargerF5ToolStripMenuItem.Click += new System.EventHandler(this.rechargerF5ToolStripMenuItem_Click);
            // 
            // ajouterUnArticleToolStripMenuItem
            // 
            this.ajouterUnArticleToolStripMenuItem.Name = "ajouterUnArticleToolStripMenuItem";
            this.ajouterUnArticleToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.ajouterUnArticleToolStripMenuItem.Text = "Ajouter un article";
            this.ajouterUnArticleToolStripMenuItem.Click += new System.EventHandler(this.ajouterUnArticleToolStripMenuItem_Click_1);
            // 
            // marquesToolStripMenuItem
            // 
            this.marquesToolStripMenuItem.Name = "marquesToolStripMenuItem";
            this.marquesToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.marquesToolStripMenuItem.Text = "Marques";
            this.marquesToolStripMenuItem.Click += new System.EventHandler(this.marquesToolStripMenuItem_Click);
            // 
            // famillesToolStripMenuItem
            // 
            this.famillesToolStripMenuItem.Name = "famillesToolStripMenuItem";
            this.famillesToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.famillesToolStripMenuItem.Text = "Familles";
            this.famillesToolStripMenuItem.Click += new System.EventHandler(this.famillesToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 370);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(493, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // listView
            // 
            this.listView.Location = new System.Drawing.Point(12, 32);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(456, 335);
            this.listView.TabIndex = 2;
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
            // sousfamillesToolStripMenuItem
            // 
            this.sousfamillesToolStripMenuItem.Name = "sousfamillesToolStripMenuItem";
            this.sousfamillesToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.sousfamillesToolStripMenuItem.Text = "Sous-familles";
            this.sousfamillesToolStripMenuItem.Click += new System.EventHandler(this.sousfamillesToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 392);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Gestion des données";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem fichierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importerDesDonnéesToolStripMenuItem;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem donnéesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ajouterUnArticleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rechargerF5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem marquesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem famillesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sousfamillesToolStripMenuItem;
    }
}

