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
using System.Data.SqlClient;
using System.Xml;
using System.Media;
using Mercure.modèle;

namespace Mercure
{
    public partial class Integration : Form
    {
        public Integration()
        {
            InitializeComponent();
            this.checkBox1.Checked = true;
            this.progressBar1.ForeColor = Color.Plum;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Value = 0;
            this.progressBar1.Step = 1;
        }

        //Boutton d'intégration
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.CompareTo("") == 0)
            {
                SystemSounds.Beep.Play();
                this.label3.Text = "Veuillez sélectionner un fichier !!";
            }
            else
            {
                if (checkBox1.Checked)
                    flushTables();
                integration(this.textBox1.Text);
            }
        }

        //Boutton de recherche de fichier
        private void button2_Click(object sender, EventArgs e)
        {
            String path;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Sélectionner le fichier xml";
            ofd.Filter = "Fichiers XML (*.XML)|*.XML";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path = ofd.FileName;
                this.textBox1.Text = path;
            }
            else
            {
                SystemSounds.Beep.Play();
                this.label3.Text = "Veuillez sélectionner un fichier valide !!";
            }
        }

        public void integration(String text)
        {
            int countFailedArticle = 0;
            this.progressBar1.Value = 0;

            Console.WriteLine("Lecture du fichier xml " + text);
            label3.Text = "Lecture du fichier XML..";
            label3.Update();

            XmlDocument doc = new XmlDocument();
            doc.Load(text);

            XmlNode node = doc.DocumentElement;
            XmlNodeList nodeList = node.SelectNodes("/materiels/article");

            label3.Text = nodeList.Count + " articles ont été détectés, intégration dans la base de données..";
            label3.Update();
            Console.WriteLine(nodeList.Count + " articles ont été détectés");
            this.progressBar1.Maximum = nodeList.Count;

            for (int i = 0; i < nodeList.Count; i++)
            {
                String description = nodeList[i].SelectNodes("description").Item(0).InnerText;
                String refArticle = nodeList[i].SelectNodes("refArticle").Item(0).InnerText;
                String marque = nodeList[i].SelectNodes("marque").Item(0).InnerText;
                String famille = nodeList[i].SelectNodes("famille").Item(0).InnerText;
                String sousFamille = nodeList[i].SelectNodes("sousFamille").Item(0).InnerText;
                float prixHT = float.Parse(nodeList[i].SelectNodes("prixHT").Item(0).InnerText);

                Articles article = new Articles(refArticle);
                if (article.loadFromDB() == null)
                {
                    Marques marques = new Marques();
                    marques.Nom = marque;
                    marques.saveInDB();
                    Familles familles = new Familles();
                    familles.Nom = famille;
                    familles.saveInDB();
                    SousFamilles sfamilles = new SousFamilles();
                    sfamilles.Nom = sousFamille;
                    sfamilles.RefFamille = familles.RefFamille;
                    sfamilles.saveInDB();

                    article.Description = description;
                    article.Quantite = 1;
                    article.PrixHT = prixHT;
                    article.RefMarque = marques.RefMarque;
                    article.RefSousFamille = sfamilles.RefSousFamille;
                    if(!article.saveInDB())
                        countFailedArticle++;

                }
                else
                {
                    article.Quantite = article.Quantite + 1;
                    article.updateInDB();
                }
                progressBar1.PerformStep();
            }
            SystemSounds.Beep.Play();
            label3.Text = "Intégration terminée. " + countFailedArticle + " articles n'ont pas été intégrés";
            label3.Update();
        }

        public void flushTables()
        {
            Articles.flushTable();
            SousFamilles.flushTable();
            Familles.flushTable();
            Marques.flushTable();
        }
    }
}
