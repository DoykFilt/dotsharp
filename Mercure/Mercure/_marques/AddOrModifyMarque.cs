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

namespace Mercure._marques
{
    public partial class AddOrModifyMarque : Form
    {
        private Marques marque;

        public AddOrModifyMarque(Marques Marque=null)
        {
            InitializeComponent();
            marque = Marque;

            if (marque != null)
            {
                Text = "Modification d'une marque";
                textBoxNom.Text = marque.Nom;
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
                if (marque != null)
                {
                    DialogResult result;
                    result = MessageBox.Show("Etes vous sur de vouloir modifier cette marque ?", "Attention : modification d'une marque existante", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        marque.Nom = textBoxNom.Text;

                        marque.updateInDB();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    DialogResult result;
                    result = MessageBox.Show("Etes vous sur de vouloir ajouter cette marque ?", "Attention : ajout d'une nouvelle marque", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        marque = new Marques();
                        marque.Nom = textBoxNom.Text;
                        
                        if (Marques.getRefMarqueFromName(marque.Nom) == -1)
                        {
                            marque.saveInDB();
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Impossible d'ajouter cette marque", "Attention : ajout d'une marque existante");
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
