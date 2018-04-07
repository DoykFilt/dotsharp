using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mercure
{
    public partial class Integration : Form
    {
        private bool crush; //enable overwrite

        public Integration()
        {
            InitializeComponent();
            this.label2.ForeColor = Color.Red;
            this.label2.Text = " ";
            this.radioButton2.Checked = true;
            this.progressBar1.ForeColor = Color.Plum;
            this.progressBar1.Maximum = 10;
            this.progressBar1.Step = 1;
            this.crush = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (this.textBox1.Text.CompareTo("") == 0)
                this.label2.Text = "Veuillez sélectionner un fichier !!";
            else
            {
                db_management dbmanage = new db_management();
                if (radioButton2.Checked)
                    dbmanage.flushTable();
                dbmanage.integration(this.textBox1.Text);
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
                this.label2.Text = " ";
            }
            else
            {
                this.label2.Text = "Veuillez sélectionner un fichier valide !!";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
