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

namespace Mercure._familles
{
    public partial class AddOrModifyFamille : Form
    {
        private Familles famille;

        public AddOrModifyFamille(Familles Famille=null)
        {
            InitializeComponent();
            famille = Famille;

            if (famille != null)
            {
                Text = "Modification d'une famille";
                textBoxNom.Text = famille.Nom;
            }
            else
            {
                Text = "Ajout d'une famille";
            }
        }


        private void ButtonVal_Click(object sender, EventArgs e)
        {
            if (CheckValidData())
            {
                if (famille != null)
                {
                    DialogResult result;
                    result = MessageBox.Show("Etes vous sur de vouloir modifier cette famille ?", "Attention : modification d'une famille existante", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        famille.Nom = textBoxNom.Text;

                        famille.updateInDB();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    DialogResult result;
                    result = MessageBox.Show("Etes vous sur de vouloir ajouter cette famille ?", "Attention : ajout d'une nouvelle famille", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        famille = new Familles();
                        famille.Nom = textBoxNom.Text;
                        
                        if (Familles.getRefFamilleFromName(famille.Nom) == -1)
                        {
                            famille.saveInDB();
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Impossible d'ajouter cette famille", "Attention : ajout d'une famille existante");
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
    }
}
