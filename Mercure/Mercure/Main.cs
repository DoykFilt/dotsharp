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

namespace Mercure
{
    public partial class Main : Form
    {

        private Hashtable[] groupTables;
        int groupColumn = 0;

        public Main()
        {
            InitializeComponent();
        }

        private void importerDesDonnéesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Integration integrationWindows = new Integration();
            integrationWindows.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            refreshListView();
        }

        private void refreshListView()
        {
            int countRows = Articles.countRows();
            List<Articles> articles = Articles.getListArticles();

            listView.Clear();

            listView.View = View.Details;
            listView.GridLines = true;
            listView.FullRowSelect = true;

            listView.Columns.Add("Référence");
            listView.Columns.Add("Description");
            listView.Columns.Add("Famille");
            listView.Columns.Add("Sous-Famille");
            listView.Columns.Add("Marque");
            listView.Columns.Add("Prix (HT)");
            listView.Columns.Add("Quantité");

            foreach (Articles article in articles)
            {
                SousFamilles sFamille = new SousFamilles();
                sFamille.RefSousFamille = article.RefSousFamille;
                sFamille.loadFromDB();

                Familles famille = new Familles();
                famille.RefFamille = sFamille.RefFamille;
                famille.loadFromDB();

                Marques marque = new Marques();
                marque.RefMarque = article.RefMarque;
                marque.loadFromDB();


                String[] array = new String[7];
                array[0] = article.RefArticle;
                array[1] = article.Description;
                array[2] = famille.Nom;
                array[3] = sFamille.Nom;
                array[4] = marque.Nom;
                array[5] = article.PrixHT.ToString();
                array[6] = article.Quantite.ToString();

                listView.Items.Add(new ListViewItem(array));
            }

            //Groups
            groupTables = new Hashtable[listView.Columns.Count];
            for (int column = 0; column < listView.Columns.Count; column++)
                groupTables[column] = CreateGroupsTable(column);
        }

        private void listView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            int sortColumn = -1;
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
            SetGroups(sortColumn);
            // Set the ListViewItemSorter property to a new ListViewItemComparer
            // object.
            this.listView.ListViewItemSorter = (IComparer) new ListViewItemComparer(e.Column, listView.Sorting);
        }

        // Creates a Hashtable object with one entry for each unique
        // subitem value (or initial letter for the parent item)
        // in the specified column.
        private Hashtable CreateGroupsTable(int column)
        {
            // Create a Hashtable object.
            Hashtable groups = new Hashtable();

            // Iterate through the items in myListView.
            foreach (ListViewItem item in listView.Items)
            {
                // Retrieve the text value for the column.
                string subItemText = item.SubItems[column].Text;

                // Use the initial letter instead if it is the first column.
                if (column == 0)
                {
                    subItemText = subItemText.Substring(0, 1);
                }

                // If the groups table does not already contain a group
                // for the subItemText value, add a new group using the 
                // subItemText value for the group header and Hashtable key.
                if (!groups.Contains(subItemText))
                {
                    groups.Add(subItemText, new ListViewGroup(subItemText,
                        HorizontalAlignment.Left));
                }
            }

            // Return the Hashtable object.
            return groups;
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
        // Sorts ListViewGroup objects by header value.
        private class ListViewGroupSorter : IComparer
        {
            private SortOrder order;

            // Stores the sort order.
            public ListViewGroupSorter(SortOrder theOrder)
            {
                order = theOrder;
            }

            // Compares the groups by header value, using the saved sort
            // order to return the correct value.
            public int Compare(object x, object y)
            {
                int result = String.Compare(
                    ((ListViewGroup)x).Header,
                    ((ListViewGroup)y).Header
                );
                if (order == SortOrder.Ascending)
                {
                    return result;
                }
                else
                {
                    return -result;
                }
            }
        }

        private void SetGroups(int column)
        { 
            // Remove the current groups.
            listView.Groups.Clear();

            // Retrieve the hash table corresponding to the column.
            Hashtable groups = (Hashtable)groupTables[column];

            // Copy the groups for the column to an array.
            ListViewGroup[] groupsArray = new ListViewGroup[groups.Count];
            groups.Values.CopyTo(groupsArray, 0);

            // Sort the groups and add them to myListView.
            Array.Sort(groupsArray, new ListViewGroupSorter(listView.Sorting));
            listView.Groups.AddRange(groupsArray);

            // Iterate through the items in myListView, assigning each 
            // one to the appropriate group.
            foreach (ListViewItem item in listView.Items)
            {
                // Retrieve the subitem text corresponding to the column.
                string subItemText = item.SubItems[column].Text;

                // For the Title column, use only the first letter.
                if (column == 0)
                {
                    subItemText = subItemText.Substring(0, 1);
                }

                // Assign the item to the matching group.
                item.Group = (ListViewGroup)groups[subItemText];
            }

        }

        private void listView_DoubleClic(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int i = listView.SelectedIndices[0];
                Console.WriteLine("Modification de l'article " + listView.Items[i].Text);
            }
        }

        private void listView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = listView.SelectedIndices[0];
                Console.WriteLine("Modification de l'article " + listView.Items[i].Text);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                Articles article = new Articles();
                article.RefArticle = listView.Items[listView.SelectedIndices[0]].Text;

                DialogResult dr = MessageBox.Show("Voulez vous supprimer l'article " + article.RefArticle + " ? ", "Suppression article", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    article.deleteFromDB();
                    listView.Items.Remove(listView.Items[listView.SelectedIndices[0]]);
                    listView.Refresh();
                }
            }
            else if (e.KeyCode == Keys.F5)
            {
                refreshListView();
            }
        }

        private void ajouterUnArticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Ajout d'un article");
        }

        private void supprimerLarticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Articles article = new Articles();
            article.RefArticle = listView.Items[listView.SelectedIndices[0]].Text;

            DialogResult dr = MessageBox.Show("Voulez vous supprimer l'article " + article.RefArticle + " ? ", "Suppression article", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                article.deleteFromDB();
                listView.Items.Remove(listView.Items[listView.SelectedIndices[0]]);
                listView.Refresh();
            }
        }

        private void modifierLarticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = listView.SelectedIndices[0];
            Console.WriteLine("Modification de l'article " + listView.Items[i].Text);
        }

        private void listView_Clic(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Items.Clear();

                contextMenuStrip1.Items.Add("Ajouter un article", null, ajouterUnArticleToolStripMenuItem_Click);
                contextMenuStrip1.Items.Add("Supprimer l'article", null, supprimerLarticleToolStripMenuItem_Click);
                contextMenuStrip1.Items.Add("Modifier l'article", null, modifierLarticleToolStripMenuItem_Click);

                contextMenuStrip1.Show(listView, e.Location);
            }
        }

    }
}
