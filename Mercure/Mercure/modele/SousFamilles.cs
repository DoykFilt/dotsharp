using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Mercure.modèle
{
    class SousFamilles
    {
        static int idSousFamille = 0;
        private int refSousFamille;
        private int refFamille;
        private String nom;
        
        public SousFamilles()
        {
            idSousFamille = loadLastId();
            idSousFamille++;
            this.refSousFamille = -1;
            this.nom = null;
        }
        public SousFamilles(int r, int rs, String n)
        {
            idSousFamille = loadLastId();
            idSousFamille++;
            this.refFamille = r;
            this.refSousFamille = rs;
            this.nom = n;
        }

        private int loadLastId()
        {
            db_management db = db_management.Instance;
            try
            {
                SQLiteConnection connection = db.openConnection();

                String squery = "SELECT RefSousFamille, MAX(RefSousFamille) FROM SousFamilles";
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
                Console.WriteLine(e.Message + " | In SousFamilles/loadLastId");
                db.closeConnection();
                return -1;
            }
        }

        public int saveInDB()
        {
            db_management db = db_management.Instance;
            try
            {
                SQLiteConnection connection = db.openConnection();

                Console.WriteLine("Insertion Sous-famille");
                string squery = "SELECT RefSousFamille FROM SousFamilles WHERE Nom = @Nom AND RefFamille = @RefFamille";
                SQLiteCommand commande = new SQLiteCommand(squery, connection);
                commande.Parameters.Add(new SQLiteParameter("@Nom", nom));
                commande.Parameters.Add(new SQLiteParameter("@RefFamille", refFamille));
                SQLiteDataReader reader = commande.ExecuteReader();
                if (reader.HasRows)
                {
                    Console.WriteLine("L'objet existe déjà ! refSousFamille mis à jour");
                    reader.Read();
                    refSousFamille = (int)reader[0];
                    reader.Close();
                    db.closeConnection();
                    return refSousFamille;
                }
                else
                {
                    refSousFamille = idSousFamille;
                    idSousFamille++;
                    squery = "INSERT INTO SousFamilles (RefSousFamille, RefFamille, Nom) VALUES (@RefSousFamille, @RefFamille, @Nom)";
                    commande = new SQLiteCommand(squery, connection);
                    commande.Parameters.Add(new SQLiteParameter("@RefSousFamille", refSousFamille));
                    commande.Parameters.Add(new SQLiteParameter("@RefFamille", refFamille));
                    commande.Parameters.Add(new SQLiteParameter("@Nom", nom));

                    commande.ExecuteNonQuery();
                    db.closeConnection();
                    return refSousFamille;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In SousFamilles/saveInDB");
                db.closeConnection();
            }
            return -1;
        }

        public int loadFromDB()
        {
            db_management db = db_management.Instance;
            try{
                if (refSousFamille == -1)
                {
                    Console.WriteLine("Erreur, ref nulle");
                    db.closeConnection();
                    return -1;
                }
                else
                {
                    SQLiteConnection connection = db.openConnection();
                    SQLiteCommand commande;
                    SQLiteDataReader reader;
                    string squery;

                    squery = "SELECT * FROM SousFamilles WHERE RefSousFamille = @refSousFamille";
                    commande = new SQLiteCommand(squery, connection);
                    commande.Parameters.Add(new SQLiteParameter("@refSousFamille", refSousFamille));

                    reader = commande.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        refFamille = (int)reader[1];
                        nom = (String)reader[2];
                        reader.Close();
                        db.closeConnection();
                        return this.refSousFamille;
                    }
                    else
                    {
                        Console.WriteLine("Erreur, la sous-famille avec la référence " + refSousFamille + " n'existe pas");
                        reader.Close();
                        db.closeConnection();
                        return -1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In SousFamilles/loadFromDB");
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
                String squery = "DELETE FROM SousFamilles";
                commande = new SQLiteCommand(squery, connection);
                commande.ExecuteNonQuery();
                db.closeConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In SousFamilles/flushTable");
                db.closeConnection();
            }
        }

        public int RefSousFamille
        {
            get { return refSousFamille; }
            set { refSousFamille = value; }
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
