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

namespace Mercure._familles
{
    public partial class FamillesForm : Form
    {
        private int sortColumn = -1;

        public FamillesForm()
        {
            InitializeComponent();
        }

        private void FamillesForm_Load(object sender, EventArgs e)
        {
            refreshListView();
            listView.MultiSelect = false;
        }

        private void refreshListView()
        {
            int countRows = Familles.countRows();
            List<Familles> familles = Familles.getListFamilles();

            listView.Clear();

            listView.View = View.Details;
            listView.GridLines = true;
            listView.FullRowSelect = true;

            listView.Columns.Add("Référence");
            listView.Columns.Add("Nom");
            foreach (Familles famille in familles)
            {
                String[] array = new String[2];
                array[0] = famille.RefFamille.ToString();
                array[1] = famille.Nom;

                listView.Items.Add(new ListViewItem(array));
            }
            listView.Refresh();
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
            this.listView.ListViewItemSorter = (IComparer)new ListViewItemComparer(e.Column, listView.Sorting);
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
                int returnVal = -1;
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
                modificationFamille();
            }
        }

        private void listView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                modificationFamille();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                suppressionFamille();
            }
            else if (e.KeyCode == Keys.F5)
            {
                refreshListView();
            }
        }

        private void ajouterUneFamilleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Ajout d'une famille");
            AddOrModifyFamille addFamille = new AddOrModifyFamille(null);
            if (addFamille.ShowDialog() == DialogResult.OK)
                listView.Refresh();
        }

        private void supprimerLaFamilleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            suppressionFamille();
        }

        private void modifierLaFamilleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modificationFamille();
        }

        private void listView_Clic(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Items.Clear();

                contextMenuStrip1.Items.Add("Ajouter une famille", null, ajouterUneFamilleToolStripMenuItem_Click);
                contextMenuStrip1.Items.Add("Supprimer la famille", null, supprimerLaFamilleToolStripMenuItem_Click);
                contextMenuStrip1.Items.Add("Modifier la famille", null, modifierLaFamilleToolStripMenuItem_Click);

                contextMenuStrip1.Show(listView, e.Location);
            }
        }

        private void rechargerF5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshListView();
        }

        private void modificationFamille()
        {
            int i = listView.SelectedIndices[0];
            Console.WriteLine("Modification de la famille " + listView.Items[i].Text);
            Familles famille = new Familles();
            famille.RefFamille = Convert.ToInt32(listView.Items[i].Text);
            famille.loadFromDB();
            AddOrModifyFamille addFamille = new AddOrModifyFamille(famille);
            addFamille.ShowDialog();
        }

        private void suppressionFamille()
        {
            Familles famille = new Familles();
            famille.RefFamille = Convert.ToInt32(listView.Items[listView.SelectedIndices[0]].Text);

            DialogResult dr = MessageBox.Show("Voulez vous supprimer la famille " + famille.RefFamille + " ? ", "Suppression article", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                famille.deleteFromDB();
                listView.Items.Remove(listView.Items[listView.SelectedIndices[0]]);
                listView.Refresh();
            }
        }
    }
}
