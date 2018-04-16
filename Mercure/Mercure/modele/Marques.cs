using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Mercure.modèle
{
    public class Marques
    {
        static int idMarque = 0;
        private int refMarque;
        private String nom;

        public Marques()
        {
            idMarque++;
            this.refMarque = -1;
            this.nom = null;
        }

        public Marques(int r, String n)
        {
            idMarque++;
            this.refMarque = r;
            this.nom = n;
        }

        static public int loadLastId()
        {
            db_management db = db_management.Instance;
            try
            {
                SQLiteConnection connection = db.openConnection();

                String squery = "SELECT RefMarque, MAX(RefMarque) FROM Marques";
                SQLiteCommand commande = new SQLiteCommand(squery, connection);
                SQLiteDataReader reader = commande.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int id;
                    if (reader[0].GetType() != typeof(DBNull))
                        id = (int)reader[0];
                    else id = 0;
                    reader.Close();
                    db.closeConnection();
                    idMarque = id;
                    return id;
                }
                else
                {
                    reader.Close();
                    db.closeConnection();
                    return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Marques/loadLastId");
                db.closeConnection();
                return 0;
            }
        }

        public int saveInDB()
        {
            db_management db = db_management.Instance;
            try{
                SQLiteConnection connection = db.openConnection();

                Console.WriteLine("Insertion Marque");
                string squery = "SELECT RefMarque FROM Marques WHERE Nom = @Nom";
                SQLiteCommand commande = new SQLiteCommand(squery, connection);
                commande.Parameters.AddWithValue("@Nom", nom);
                SQLiteDataReader reader = commande.ExecuteReader();
                if (reader.HasRows)
                {
                    Console.WriteLine("L'objet existe déjà ! refMarque mis à jour");
                    reader.Read();
                    this.refMarque = (int)reader[0];
                    reader.Close();
                    db.closeConnection();
                    return this.refMarque;
                }
                else
                {
                    refMarque = idMarque;
                    idMarque++;

                    squery = "INSERT INTO Marques (RefMarque, Nom) VALUES (@RefMarque, @Nom)";
                    commande = new SQLiteCommand(squery, connection);
                    commande.Parameters.Add(new SQLiteParameter("@RefMarque", refMarque));
                    commande.Parameters.Add(new SQLiteParameter("@Nom", nom));

                    commande.ExecuteNonQuery();
                    reader.Close();
                    db.closeConnection();
                    return refMarque;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Marques/saveInDB");
                db.closeConnection();
            }
            return -1;
        }

        public int updateInDB()
        {
            db_management db = db_management.Instance;
            try
            {
                SQLiteConnection connection = db.openConnection();

                Console.WriteLine("modification Marque");
                string squery = "UPDATE Marques SET Nom = @Nom WHERE RefMarque = @RefMarque";
                SQLiteCommand commande = new SQLiteCommand(squery, connection);
                commande.Parameters.AddWithValue("@Nom", nom);
                commande.Parameters.AddWithValue("@RefMarque", refMarque);
                commande.ExecuteNonQuery();

                db.closeConnection();
                return this.refMarque;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Marques/updateInDB");
                db.closeConnection();
            }
            return -1;
        }

        public int loadFromDB()
        {
            db_management db = db_management.Instance;
            try{
                if (refMarque == -1)
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

                    squery = "SELECT * FROM Marques WHERE RefMarque = @RefMarque";
                    commande = new SQLiteCommand(squery, connection);
                    commande.Parameters.Add(new SQLiteParameter("@RefMarque", refMarque));

                    reader = commande.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        Nom = (String)reader[1];
                        reader.Close();
                        db.closeConnection();
                        return this.refMarque;
                    }
                    else
                    {
                        Console.WriteLine("Erreur, la marque avec la référence " + refMarque + " n'existe pas");
                        reader.Close();
                        db.closeConnection();
                        return -1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Marques/loadFromDB");
                db.closeConnection();
            }
            return -1;
        }

        public bool deleteFromDB()
        {
            db_management db = db_management.Instance;
            try
            {
                SQLiteConnection connection = db.openConnection();
                SQLiteCommand commande;
                String squery;
                squery = "DELETE FROM Marques WHERE RefMarque = @RefMarque";
                commande = new SQLiteCommand(squery, connection);
                commande.Parameters.Add(new SQLiteParameter("@RefMarque", refMarque));

                commande.ExecuteNonQuery();
                db.closeConnection();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Marques/deleteFromDB");
                db.closeConnection();
                return false;
            }
        }

        public static void flushTable()
        {
            db_management db = db_management.Instance;
            try{
                SQLiteConnection connection = db.openConnection();
                SQLiteCommand commande;
                String squery = "DELETE FROM Marques";
                commande = new SQLiteCommand(squery, connection);
                commande.ExecuteNonQuery();
                db.closeConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Marques/flushTable");
                db.closeConnection();
            }
        }

        static public int getRefMarqueFromName(String name)
        {
            db_management db = db_management.Instance;
            try
            {
                SQLiteConnection connection = db.openConnection();

                String squery = "SELECT RefMarque FROM Marques WHERE Nom = @Nom";
                SQLiteCommand commande = new SQLiteCommand(squery, connection);
                commande.Parameters.Add(new SQLiteParameter("@Nom", name));
                SQLiteDataReader reader = commande.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int reference;
                    if (reader[0].GetType() != typeof(DBNull))
                        reference = Convert.ToInt32(reader.GetInt64(0));
                    else reference = -1;
                    reader.Close();
                    db.closeConnection();
                    return reference;
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
                Console.WriteLine(e.Message + " | In Marques/getRefMarqueFromName");
                db.closeConnection();
                return -1;
            }
        }

        public static List<Marques> getListMarques()
        {
            db_management db = db_management.Instance;
            List<Marques> list = new List<Marques>();

            try
            {
                SQLiteConnection connection = db.openConnection();
                SQLiteCommand commande;
                SQLiteDataReader reader;
                string squery;

                squery = "SELECT * FROM Marques";
                commande = new SQLiteCommand(squery, connection);

                reader = commande.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Marques marque = new Marques();

                        marque.refMarque = Convert.ToInt32(reader.GetInt64(0));
                        marque.nom = (String)reader[1];

                        list.Add(marque);
                    }

                    reader.Close();
                    db.closeConnection();
                    return list;
                }
                else
                {
                    Console.WriteLine("Aucune marque dans la base");
                    reader.Close();
                    db.closeConnection();
                    return list;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " | In Marques/getListMarques");
                db.closeConnection();
                return list;
            }
        }

        public static int countRows()
        {
            db_management db = db_management.Instance;
            try
            {
                SQLiteConnection connection = db.openConnection();

                String squery = "SELECT COUNT(RefMarque) FROM Marques";
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
                Console.WriteLine(e.Message + " | In Marques/countRows");
                db.closeConnection();
                return -1;
            }
        }


        public int RefMarque
        {
            get { return refMarque; }
            set { refMarque = value; }
        }

        public String Nom
        {
            get { return nom; }
            set { nom = value; }
        }
    }
}
