using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Xml;

namespace Mercure
{
    class db_management
    {
        static int refMarque = 0;
        static int refFamille = 0;
        static int refSousFamille = 0;

        public db_management()
        {
        }

        public void flushTable()
        {
            SQLiteConnection connection = new SQLiteConnection(@"Data Source=Mercure.SQLite;");
            connection.Open();

            String squery;
            SQLiteCommand commande; 

            try
            {
                squery = "DELETE * FROM Articles";
                commande = new SQLiteCommand(squery, connection);
                commande.ExecuteNonQuery();

                squery = "DELETE * FROM Familles";
                commande = new SQLiteCommand(squery, connection);
                commande.ExecuteNonQuery();

                squery = "DELETE * FROM SousFamilles";
                commande = new SQLiteCommand(squery, connection);
                commande.ExecuteNonQuery();

                squery = "DELETE * FROM Marques";
                commande = new SQLiteCommand(squery, connection);
                commande.ExecuteNonQuery();

                Console.WriteLine("Les tables ont été vidé");
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur flush Tables", e.Message);
            }

            connection.Close();

        }


        public void integration(String text)
        {
            SQLiteConnection dbConnection = new SQLiteConnection(@"Data Source=Mercure.SQLite;");
            dbConnection.Open();
            Console.WriteLine("Lecture du fichier xml " + text);
            XmlDocument doc = new XmlDocument();
            doc.Load(text);

            XmlNode node = doc.DocumentElement;
            XmlNodeList nodeList = node.SelectNodes("/materiels/article");

            Console.WriteLine(nodeList.Count + " articles ont été détectés");

            for (int i = 0; i < nodeList.Count; i++)
            {
                String description = nodeList[i].SelectNodes("description").Item(0).InnerText;
                String refArticle = nodeList[i].SelectNodes("refArticle").Item(0).InnerText;
                String marque = nodeList[i].SelectNodes("marque").Item(0).InnerText;
                String famille = nodeList[i].SelectNodes("famille").Item(0).InnerText;
                String sousFamille = nodeList[i].SelectNodes("sousFamille").Item(0).InnerText;
                float prixHT = float.Parse(nodeList[i].SelectNodes("prixHT").Item(0).InnerText);

                insertArticleInDatabase(dbConnection, description, refArticle, marque, famille, sousFamille, prixHT);
            }
            dbConnection.Close();
        }

        public int insertMarque(String marque, SQLiteConnection connection)
        {
            Console.WriteLine("Insertion Marque");
            string squery = "SELECT RefMarque FROM Marques WHERE Nom = @Nom";
            SQLiteCommand commande = new SQLiteCommand(squery, connection);
            commande.Parameters.AddWithValue("@Nom", marque);
            SQLiteDataReader reader = commande.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                return (int)reader[0];
            }
            else
            {
                squery = "INSERT INTO Marques (RefMarque, Nom) VALUES (@RefMarque, @Nom)";
                commande = new SQLiteCommand(squery, connection);
                commande.Parameters.Add(new SQLiteParameter("@RefMarque", refMarque));
                commande.Parameters.Add(new SQLiteParameter("@Nom", marque));

                commande.ExecuteNonQuery();
                refMarque++;
                return refMarque;
            }
        }

        public int insertFamille(String nom, SQLiteConnection connection)
        {
            Console.WriteLine("Insertion Famille");
            string squery = "SELECT RefFamille FROM Familles WHERE Nom = @Nom";
            SQLiteCommand commande = new SQLiteCommand(squery, connection);
            commande.Parameters.Add(new SQLiteParameter("@Nom", nom));
            SQLiteDataReader reader = commande.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                return (int)reader[0];
            }
            else
            {
                squery = "INSERT INTO Familles (RefFamille, Nom) VALUES (@RefFamille, @Nom)";
                commande = new SQLiteCommand(squery, connection);
                commande.Parameters.Add(new SQLiteParameter("@RefFamille", refFamille));
                commande.Parameters.Add(new SQLiteParameter("@Nom", nom));

                commande.ExecuteNonQuery();
                refFamille++;
                return refFamille;
            }
        }

        public int insertSousFamille(int idFamille, String nom, SQLiteConnection connection)
        {
            Console.WriteLine("Insertion Sous-famille");
            string squery = "SELECT RefSousFamille FROM SousFamilles WHERE Nom = @Nom";
            SQLiteCommand commande = new SQLiteCommand(squery, connection);
            commande.Parameters.Add(new SQLiteParameter("@Nom", nom));
            SQLiteDataReader reader = commande.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                return (int)reader[0];
            }
            else
            {
                squery = "INSERT INTO SousFamilles (RefSousFamille, RefFamille, Nom) VALUES (@RefSousFamille, @RefFamille, @Nom)";
                commande = new SQLiteCommand(squery, connection);
                commande.Parameters.Add(new SQLiteParameter("@RefSousFamille", refSousFamille));
                commande.Parameters.Add(new SQLiteParameter("@RefFamille", idFamille));
                commande.Parameters.Add(new SQLiteParameter("@Nom", nom));

                commande.ExecuteNonQuery();
                refSousFamille++;
                return refSousFamille;
            }
        }

        public void insertArticleInDatabase(SQLiteConnection connection, String description, String refArticle, String marque, String famille, String sousFamille, float prixHT)
        {
            int idFamille = 0;
            int idMarque = 0;
            int idSousFamille = 0;
            SQLiteCommand commande;
            string squery;
            SQLiteDataReader reader = null;

            try
            {
                squery = "SELECT * FROM Articles WHERE RefArticle = @RefArticle";
                commande = new SQLiteCommand(squery, connection);
                commande.Parameters.Add(new SQLiteParameter("@RefArticle", refArticle));

                reader = commande.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur Connexion BDD", e.Message);
            }

            if (reader != null && reader.HasRows)
            {
                try
                {
                    squery = "UPDATE Articles Set Quantite = Quantite + 1 WHERE RefArticle = @RefArticle";
                    commande = new SQLiteCommand(squery, connection);
                    commande.Parameters.Add(new SQLiteParameter("@RefArticle", refArticle));

                    commande.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur Update Articles", e.Message);
                }
            }
            else
            {
                try
                {
                    //Ajout de la marque
                    idMarque = insertMarque(marque, connection);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur Insert Marque", e.Message);
                }

                try
                {
                    //Ajout de la famille
                    idFamille = insertFamille(famille, connection);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur Insert Famille", e.Message);
                }

                try
                {
                    //Ajout de la sous-famille
                    idSousFamille = insertSousFamille(idFamille, sousFamille, connection);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur Insert Sous-Famille", e.Message);
                }

                Console.WriteLine("Insertion Article");

                try
                {
                    //Ajout de l'article
                    squery = "INSERT INTO Articles (RefArticle, RefMarque, RefSousFamille, Description, PrixHT, Quantite) Values (@RefArticle, @RefMarque, @RefSousFamille, @Description, @PrixHT, 1)";
                    commande = new SQLiteCommand(squery, connection);
                    commande.Parameters.Add(new SQLiteParameter("@RefArticle", refArticle));
                    commande.Parameters.Add(new SQLiteParameter("@RefMarque", idMarque));
                    commande.Parameters.Add(new SQLiteParameter("@RefSousFamille", idSousFamille));
                    commande.Parameters.Add(new SQLiteParameter("@Description", description));
                    commande.Parameters.Add(new SQLiteParameter("@PrixHT", prixHT));

                    commande.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur Insert Article", e.Message);
                }
            }
        }
    }
}
