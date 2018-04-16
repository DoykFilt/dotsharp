using System;
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
    public partial class AddOrModifySousFamille : Form
    {
        private SousFamilles sFamille;

        public AddOrModifySousFamille(SousFamilles SFamille = null)
        {
            InitializeComponent();
            sFamille = SFamille;
            updateComboFamille();

            if (sFamille != null)
            {
                Text = "Modification d'une sous-famille";
                textBoxNom.Text = sFamille.Nom;
            }
            else
            {
                Text = "Ajout d'une sous-famille";
            }
        }


        private void ButtonVal_Click(object sender, EventArgs e)
        {
            if (CheckValidData())
            {
                if (sFamille != null)
                {
                    DialogResult result;
                    result = MessageBox.Show("Etes vous sur de vouloir modifier cette sous-famille ?", "Attention : modification d'une sous-famille existante", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        sFamille.Nom = textBoxNom.Text;

                        sFamille.updateInDB();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    DialogResult result;
                    result = MessageBox.Show("Etes vous sur de vouloir ajouter cette sous-famille ?", "Attention : ajout d'une nouvelle sous-famille", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        sFamille = new SousFamilles();
                        sFamille.Nom = textBoxNom.Text;

                        if (SousFamilles.getRefSousFamilleFromName(sFamille.Nom) == -1)
                        {
                            sFamille.saveInDB();
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Impossible d'ajouter cette sous-famille", "Attention : ajout d'une sous-famille existante");
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

            if (String.IsNullOrEmpty(textBoxNom.Text))
                validation = false;

            return validation;
        }

        public void updateComboFamille()
        {
            List<Familles> listeFamille = Familles.getListFamilles();
            for (int i = 0; i < listeFamille.Count(); i++)
            {
                comboBoxFamille.Items.Add(listeFamille[i].Nom);
                if (sFamille != null)
                {
                    Familles famille = new Familles();
                    famille.RefFamille = sFamille.RefFamille;
                    famille.loadFromDB();
                    if (listeFamille[i].Nom == famille.Nom)
                        comboBoxFamille.SelectedItem = comboBoxFamille.Items[i];
                }
            }
            comboBoxFamille.Refresh();
        }
    }
}
