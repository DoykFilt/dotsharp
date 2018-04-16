using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mercure.modèle;

namespace Mercure._sousFamilles
{
    public partial class SousFamillesForm : Form
    {
        private int sortColumn = -1;

        public SousFamillesForm()
        {
            InitializeComponent();
        }

        private void SousFamillesForm_Load(object sender, EventArgs e)
        {
            refreshListView();
            listView.MultiSelect = false;
        }

        private void refreshListView()
        {
            int countRows = SousFamilles.countRows();
            List<SousFamilles> sousFamilles = SousFamilles.getListSousFamilles();

            listView.Clear();

            listView.View = View.Details;
            listView.GridLines = true;
            listView.FullRowSelect = true;

            listView.Columns.Add("Référence");
            listView.Columns.Add("Nom");
            listView.Columns.Add("Famille");
            foreach (SousFamilles sousFamille in sousFamilles)
            {
                Familles famille = new Familles();
                famille.RefFamille = sousFamille.RefFamille;
                famille.loadFromDB();

                String[] array = new String[3];
                array[0] = sousFamille.RefSousFamille.ToString();
                array[1] = sousFamille.Nom;
                array[2] = famille.Nom;

                listView.Items.Add(new ListViewItem(array));
            }
        }

        private void listView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            // Determine whether the column is the same as the last column clicked.
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.
                sortColumn = e.Column;
                // Set the sort order to ascending by default.
                listView.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.
                if (listView.Sorting == SortOrder.Ascending)
                    listView.Sorting = SortOrder.Descending;
                else
                    listView.Sorting = SortOrder.Ascending;
            }

            // Call the sort method to manually sort.
            listView.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer
            // object.
            this.listView.ListViewItemSorter = (IComparer) new ListViewItemComparer(e.Column, listView.Sorting);
            listView.Refresh();
        }

        private class ListViewItemComparer : IComparer
        {
            private int col;
            private SortOrder order;

            public ListViewItemComparer()
            {
                col = 0;
                order = SortOrder.Ascending;
            }

            public ListViewItemComparer(int column, SortOrder order)
            {
                col = column;
                this.order = order;
            }

            public int Compare(object x, object y) 
            {
                int returnVal= -1;
                returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
                                        ((ListViewItem)y).SubItems[col].Text);
                // Determine whether the sort order is descending.
                if (order == SortOrder.Descending)
                    // Invert the value returned by String.Compare.
                    returnVal *= -1;
                return returnVal;
            }
        }

        private void listView_DoubleClic(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                modificationSFamille();
            }
        }

        private void listView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                modificationSFamille();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                suppressionSFamille();
            }
            else if (e.KeyCode == Keys.F5)
            {
                refreshListView();
            }
        }

        private void ajouterUneSFamilleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Ajout d'une marque");
            AddOrModifySousFamille addSFamille = new AddOrModifySousFamille(null);
            if(addSFamille.ShowDialog() == DialogResult.OK)
                listView.Refresh();
        }

        private void supprimerLaSFamilleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            suppressionSFamille();
        }

        private void modifierLaSFamilleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modificationSFamille();
        }

        private void listView_Clic(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Items.Clear();

                contextMenuStrip1.Items.Add("Ajouter une sous-famille", null, ajouterUneSFamilleToolStripMenuItem_Click);
                contextMenuStrip1.Items.Add("Supprimer la sous-famille", null, supprimerLaSFamilleToolStripMenuItem_Click);
                contextMenuStrip1.Items.Add("Modifier la sous-famille", null, modifierLaSFamilleToolStripMenuItem_Click);

                contextMenuStrip1.Show(listView, e.Location);
            }
        }

        private void rechargerF5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshListView();
        }

        private void modificationSFamille()
        {
            int i = listView.SelectedIndices[0];
            Console.WriteLine("Modification de la sous-famille " + listView.Items[i].Text);
            SousFamilles sousFamille = new SousFamilles();
            sousFamille.RefSousFamille = Convert.ToInt32(listView.Items[i].Text);
            sousFamille.loadFromDB();
            AddOrModifySousFamille addSFamille = new AddOrModifySousFamille(sousFamille);
            addSFamille.ShowDialog();
        }

        private void suppressionSFamille()
        {
            SousFamilles sousfamille = new SousFamilles();
            sousfamille.RefSousFamille = Convert.ToInt32(listView.Items[listView.SelectedIndices[0]].Text);

            DialogResult dr = MessageBox.Show("Voulez vous supprimer la sous-famille " + sousfamille.RefSousFamille + " ? ", "Suppression sous-famille", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                sousfamille.deleteFromDB();
                listView.Items.Remove(listView.Items[listView.SelectedIndices[0]]);
                listView.Refresh();
            }
        }
    }
}
