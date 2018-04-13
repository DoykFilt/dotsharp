using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Mercure.modèle
{
    class Familles
    {
        static int idFamille = 0;
        private int refFamille;
        private String nom;
        
        public Familles()
        {
            idFamille = loadLastId();
            idFamille++;
            this.refFamille = -1;
            this.nom = null;
        }
        public Familles(int r, String n)
        {
            idFamille = loadLastId();
            idFamille++;
            this.refFamille = r;
            this.nom = n;
        }

        private int loadLastId()
        {
            db_management db = db_management.Instance;
            try
            {
                SQLiteConnection connection = db.openConnection();

                String squery = "SELECT RefFamille, MAX(RefFamille) FROM Familles";
                SQLiteCommand commande = new SQLiteCommand(squery, connection);
                SQLiteDataReader reader = commande.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int id;
                    if (reader[0].GetType() != typeof(DBNull))
                        id = (int)reader[0];
                    else id = -1;
                    reader.Close();
                    db.closeConnection();
                    return id;
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
                Console.WriteLine(e.Message + " | In Familles/loadLastId");
                db.closeConnection();
                return -1;
            }
        }

        public int saveInDB()
        {
            db_management db = db_management.Instance;
            try{
                SQLiteConnection connection = db.openConnection();

                Console.WriteLine("Insertion Famille");
                string squery = "SELECT RefFamille FROM Familles WHERE Nom = @Nom";
                SQLiteCommand commande = new SQLiteCommand(squery, connection);
                commande.Parameters.Add(new SQLiteParameter("@Nom", nom));
                SQLiteDataReader reader = commande.ExecuteReader();
                if (reader.HasRows)
                {
                    Console.WriteLine("L'objet existe déjà ! refFamille mis à jour");
                    reader.Read();
                    refFamille = (int)reader[0];
                    reader.Close();
                    db.closeConnection();
                    return refFamille;
                }
                else
                {
                    this.refFamille = idFamille;
                    idFamille++;
                    squery = "INSERT INTO Familles (RefFamille, Nom) VALUES (@RefFamille, @Nom)";
                    commande = new SQLiteCommand(squery, connection);
                    commande.Parameters.Add(new SQLiteParameter("@RefFamille", refFamille));
                    commande.Parameters.Add(new SQLiteParameter("@Nom", nom));

                    commande.ExecuteNonQuery();
                    reader.Close();
                    db.closeConnection();
                    return refFamille;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Familles/saveInDB");
                db.closeConnection();
            }
            return -1;
        }

        public int loadFromDB()
        {
            db_management db = db_management.Instance;
            try{
                if (refFamille == -1)
                {
                    Console.WriteLine("Erreur, ref nulle");
                    return -1;
                }
                else
                {
                    SQLiteConnection connection = db.openConnection();
                    SQLiteCommand commande;
                    SQLiteDataReader reader;
                    string squery;

                    squery = "SELECT * FROM Familles WHERE RefFamille = @RefFamille";
                    commande = new SQLiteCommand(squery, connection);
                    commande.Parameters.Add(new SQLiteParameter("@RefFamille", refFamille));

                    reader = commande.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        nom = (String)reader[1];
                        reader.Close();
                        db.closeConnection();
                        return this.refFamille;
                    }
                    else
                    {
                        Console.WriteLine("Erreur, la famille avec la référence " + refFamille + " n'existe pas");
                        reader.Close();
                        db.closeConnection();
                        return -1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Familles/loadFromDB");
                db.closeConnection();
            }
            return -1;
        }

        public static void flushTable()
        {
            db_management db = db_management.Instance;
            try{
                SQLiteConnection connection = db.openConnection();
                SQLiteCommand commande;
                String squery = "DELETE FROM Familles";
                commande = new SQLiteCommand(squery, connection);
                commande.ExecuteNonQuery();
                db.closeConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Familles/flushTable");
                db.closeConnection();
            }
        }

        public int RefFamille
        {
            get { return refFamille; }
            set { refFamille = value; }
        }

        public String Nom
        {
            get { return nom; }
            set { nom = value; }
        }
    }
}
