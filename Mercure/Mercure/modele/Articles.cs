using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace Mercure.modèle
{
    class Articles
    {
        private String refArticle;
        private String description;
        private int refSousFamille;
        private int refMarque;
        private float prixHT;
        private int quantite;

        public Articles()
        {
            this.refArticle = null;
            this.description = null;
        }
        public Articles(String r)
        {
            this.refArticle = r;
            this.description = null;
        }


        public bool saveInDB()
        {
            db_management db = db_management.Instance;
            try{
                SQLiteConnection connection = db.openConnection();
                SQLiteCommand commande;
                String squery;

                squery = "INSERT INTO Articles (RefArticle, RefMarque, RefSousFamille, Description, PrixHT, Quantite) Values (@RefArticle, @RefMarque, @RefSousFamille, @Description, @PrixHT, 1)";
                commande = new SQLiteCommand(squery, connection);
                commande.Parameters.Add(new SQLiteParameter("@RefArticle", refArticle));
                commande.Parameters.Add(new SQLiteParameter("@RefMarque", refMarque));
                commande.Parameters.Add(new SQLiteParameter("@RefSousFamille", refSousFamille));
                commande.Parameters.Add(new SQLiteParameter("@Description", description));
                commande.Parameters.Add(new SQLiteParameter("@PrixHT", prixHT));

                commande.ExecuteNonQuery();
                db.closeConnection();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Articles/saveInDB");
                db.closeConnection();
                return false;
            }
        }

        public bool updateInDB()
        {
            db_management db = db_management.Instance;
            try{
                SQLiteConnection connection = db.openConnection();
                SQLiteCommand commande;
                String squery;
                squery = "UPDATE Articles Set RefMarque = @RefMarque, RefSousFamille = @RefSousFamille, Description = @Description, PrixHT = @PrixHT, Quantite = @Quantite WHERE RefArticle = @RefArticle";
                commande = new SQLiteCommand(squery, connection);
                commande.Parameters.Add(new SQLiteParameter("@RefArticle", refArticle));
                commande.Parameters.Add(new SQLiteParameter("@RefMarque", refMarque));
                commande.Parameters.Add(new SQLiteParameter("@RefSousFamille", refSousFamille));
                commande.Parameters.Add(new SQLiteParameter("@Description", description));
                commande.Parameters.Add(new SQLiteParameter("@PrixHT", prixHT));
                commande.Parameters.Add(new SQLiteParameter("@Quantite", Quantite));

                commande.ExecuteNonQuery();
                db.closeConnection();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Articles/updateInDB");
                db.closeConnection();
                return false;
            }
        }

        public String loadFromDB()
        {
            db_management db = db_management.Instance;
            try
            {
                if (refArticle == null)
                {
                    Console.WriteLine("Erreur, ref nulle");
                    return null;
                }
                else
                {
                    SQLiteConnection connection = db.openConnection();
                    SQLiteCommand commande;
                    SQLiteDataReader reader;
                    string squery;

                    squery = "SELECT * FROM Articles WHERE RefArticle = @RefArticle";
                    commande = new SQLiteCommand(squery, connection);
                    commande.Parameters.Add(new SQLiteParameter("@RefArticle", refArticle));

                    reader = commande.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (reader[0].GetType() != typeof(DBNull))
                        {

                            description = (String)reader[1];
                            refSousFamille = (int)reader[2];
                            refMarque = (int)reader[3];
                            prixHT = (float)reader.GetFloat(4);
                            quantite = (int)reader[5];
                        }

                        reader.Close();
                        db.closeConnection();
                        return this.refArticle;
                    }
                    else
                    {
                        Console.WriteLine("Erreur, l'article avec la référence " + refArticle + " n'existe pas");
                        reader.Close();
                        db.closeConnection();
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Articles/loadFromDB");
                db.closeConnection();
                return null;
            }
        }

        public static bool flushTable()
        {
            db_management db = db_management.Instance;
            try{
                SQLiteConnection connection = db.openConnection();
                SQLiteCommand commande;
                String squery = "DELETE FROM Articles";
                commande = new SQLiteCommand(squery, connection);
                commande.ExecuteNonQuery();
                db.closeConnection();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Articles/flushTable");
                db.closeConnection();
                return false;
            }
        }

        public String RefArticle
        {
            get { return refArticle; }
            set { refArticle = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public int RefSousFamille
        {
            get { return refSousFamille; }
            set { refSousFamille = value; }
        }

        public int RefMarque
        {
            get { return refMarque; }
            set { refMarque = value; }
        }

        public float PrixHT
        {
            get { return prixHT; }
            set { prixHT = value; }
        }

        public int Quantite
        {
            get { return quantite; }
            set { quantite = value; }
        }
    }
}
