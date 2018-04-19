using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using Mercure.modèle;

namespace Mercure
{
    public partial class AddOrModifyArticle : Form
    {
        private float prix = 0;
        private int quantite = 0;
        private Articles article;

        public AddOrModifyArticle(Articles Article=null)
        {
            InitializeComponent();
            article = Article;
            updateComboMarque();
            updateComboFamille();

            if (article != null)
            {
                article = Article;
                Text = "Modification d'un article";
                textBoxRef.Text = article.RefArticle;
                textBoxRef.Enabled = false;
                textBoxDescrip.Text = article.Description;
                numericUpDown1.Value = (decimal)article.PrixHT;
                numericUpDown2.Value = article.Quantite;
            }
            else
            {
                Text = "Ajout d'un article";
            }
        }


        private void ButtonVal_Click(object sender, EventArgs e)
        {
            if (CheckValidData())
            {
                article.RefArticle = textBoxRef.Text;
                if (article != null)
                {
                    DialogResult result;
                    result = MessageBox.Show("Etes vous sur de vouloir modifier cet article?", "Attention : modification d'un article existant", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        article.RefArticle = textBoxRef.Text;
                        article.Description = textBoxDescrip.Text;
                        article.RefMarque = Marques.getRefMarqueFromName(comboBoxMar.SelectedItem.ToString());
                        article.RefSousFamille = SousFamilles.getRefSousFamilleFromName(comboBoxSsFam.SelectedItem.ToString());
                        article.PrixHT = (float)decimal.ToDouble(numericUpDown1.Value);
                        article.Quantite = decimal.ToInt32(numericUpDown2.Value);

                        article.updateInDB();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    DialogResult result;
                    result = MessageBox.Show("Etes vous sur de vouloir ajouter cet article?", "Attention : ajout d'un nouvel article", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        article = new Articles();
                        article.RefArticle = textBoxRef.Text;

                        if (article.loadFromDB() == null)
                        {
                            article.Description = textBoxDescrip.Text;
                            article.RefMarque = Marques.getRefMarqueFromName(comboBoxMar.SelectedItem.ToString());
                            article.RefSousFamille = SousFamilles.getRefSousFamilleFromName(comboBoxSsFam.SelectedItem.ToString());
                            article.PrixHT = prix;
                            article.Quantite = quantite;
                            article.saveInDB();
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Impossible d'ajouter cet article", "Attention : ajout d'un article existant");
                        }
                    }
                }
            }
            else
            {
               MessageBox.Show("Veuillez remplir correctement les champs","Erreur");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }
        
        public Boolean CheckValidData()
        {
            Boolean validation = true;

            if (String.IsNullOrEmpty(textBoxRef.Text))
                validation = false;
            
            if (String.IsNullOrEmpty(textBoxDescrip.Text))
                validation = false;
            
            if (comboBoxMar.SelectedItem == null)
                validation = false;
            
            if (comboBoxFam.SelectedItem == null)
                validation = false;

            if (comboBoxSsFam.SelectedItem == null)
                validation = false;

            return validation;
        }

        public void updateComboMarque()
        {
            List<Marques> listeMarques = Marques.getListMarques();
            for (int i = 0; i < listeMarques.Count(); i++)
            {
                comboBoxMar.Items.Add(listeMarques[i].Nom);
                if (article != null)
                {
                    Marques marque = new Marques();
                    marque.RefMarque = article.RefMarque;
                    marque.loadFromDB();
                    if (listeMarques[i].Nom == marque.Nom)
                        comboBoxMar.SelectedItem = comboBoxMar.Items[i];
                }
            }
            comboBoxMar.Refresh();
        }

        public void updateComboFamille()
        {
            List<Familles> listeFamille = Familles.getListFamilles();
            for (int i = 0; i < listeFamille.Count(); i++)
            {
                comboBoxFam.Items.Add(listeFamille[i].Nom);
                if (article != null)
                {
                    SousFamilles sfamille = new SousFamilles();
                    sfamille.RefSousFamille = article.RefSousFamille;
                    sfamille.loadFromDB();

                    Familles famille = new Familles();
                    famille.RefFamille = sfamille.RefFamille;
                    famille.loadFromDB();
                    if (listeFamille[i].Nom == famille.Nom)
                        comboBoxFam.SelectedItem = comboBoxFam.Items[i];
                }
            }
            comboBoxFam.Refresh();
        }

        public void updateComboSousFamille(String nomFamille)
        {
            comboBoxSsFam.Items.Clear();

            int referenceFamille = Familles.getRefFamilleFromName(nomFamille);
            List<SousFamilles> sousFamilles = SousFamilles.getListSousFamillesFromFamilleRef(referenceFamille);
            for (int i = 0; i < sousFamilles.Count(); i++)
            {
                comboBoxSsFam.Items.Add(sousFamilles[i].Nom);
                if (article != null)
                {
                    SousFamilles sfamille = new SousFamilles();
                    sfamille.RefSousFamille = article.RefSousFamille;
                    sfamille.loadFromDB();
                    if (sousFamilles[i].Nom == sfamille.Nom)
                        comboBoxSsFam.SelectedItem = comboBoxSsFam.Items[i];
                }
            }
            comboBoxSsFam.Refresh();
        }

        private void comboBoxFam_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateComboSousFamille(comboBoxFam.SelectedItem.ToString());
        }
    }
}
