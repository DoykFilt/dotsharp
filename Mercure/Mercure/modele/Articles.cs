using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace Mercure.modèle
{
    public class Articles
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
            this.quantite = 0;
        }
        public Articles(String r)
        {
            this.refArticle = r;
            this.description = null;
            this.quantite = 0;
        }


        public bool saveInDB()
        {
            db_management db = db_management.Instance;
            try{
                SQLiteConnection connection = db.openConnection();
                SQLiteCommand commande;
                String squery;

                squery = "INSERT INTO Articles (RefArticle, RefMarque, RefSousFamille, Description, PrixHT, Quantite) Values (@RefArticle, @RefMarque, @RefSousFamille, @Description, @PrixHT, @Quantite)";
                commande = new SQLiteCommand(squery, connection);
                commande.Parameters.Add(new SQLiteParameter("@RefArticle", refArticle));
                commande.Parameters.Add(new SQLiteParameter("@RefMarque", refMarque));
                commande.Parameters.Add(new SQLiteParameter("@RefSousFamille", refSousFamille));
                commande.Parameters.Add(new SQLiteParameter("@Description", description));
                commande.Parameters.Add(new SQLiteParameter("@PrixHT", prixHT));
                commande.Parameters.Add(new SQLiteParameter("@Quantite", quantite));

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

        public bool deleteFromDB()
        {
            db_management db = db_management.Instance;
            try
            {
                SQLiteConnection connection = db.openConnection();
                SQLiteCommand commande;
                String squery;
                squery = "DELETE FROM Articles WHERE RefArticle = @RefArticle";
                commande = new SQLiteCommand(squery, connection);
                commande.Parameters.Add(new SQLiteParameter("@RefArticle", refArticle));

                commande.ExecuteNonQuery();
                db.closeConnection();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Articles/deleteFromDB");
                db.closeConnection();
                return false;
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

        public static int countRows()
        {
            db_management db = db_management.Instance;
            try
            {
                SQLiteConnection connection = db.openConnection();

                String squery = "SELECT COUNT(RefArticle) FROM Articles";
                SQLiteCommand commande = new SQLiteCommand(squery, connection);
                SQLiteDataReader reader = commande.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int count;
                    if (reader[0].GetType() != typeof(DBNull))
                        count = Convert.ToInt32(reader.GetInt64(0));
                    else count = -1;
                    reader.Close();
                    db.closeConnection();
                    return count;
                }
                else
                {
                    reader.Close();
                    db.closeConnection();
                    return -1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Articles/countRows");
                db.closeConnection();
                return -1;
            }
        }

        public static List<Articles> getListArticles()
        {
            db_management db = db_management.Instance;
            List<Articles> list = new List<Articles>();

            try
            {
                SQLiteConnection connection = db.openConnection();
                SQLiteCommand commande;
                SQLiteDataReader reader;
                string squery;

                squery = "SELECT * FROM Articles";
                commande = new SQLiteCommand(squery, connection);

                reader = commande.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Articles article = new Articles();

                        article.refArticle = (String)reader[0];
                        article.description = (String)reader[1];
                        article.refSousFamille = (int)reader[2];
                        article.refMarque = (int)reader[3];
                        article.prixHT = (float)reader.GetFloat(4);
                        article.quantite = (int)reader[5];

                        list.Add(article);
                    }

                    reader.Close();
                    db.closeConnection();
                    return list;
                }
                else
                {
                    Console.WriteLine("Aucun article dans la base");
                    reader.Close();
                    db.closeConnection();
                    return list;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Articles/getListArticles");
                db.closeConnection();
                return list;
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
