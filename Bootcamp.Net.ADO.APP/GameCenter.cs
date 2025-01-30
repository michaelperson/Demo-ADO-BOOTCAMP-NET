using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Bootcamp.Net.ADO.APP
{
    public static class GameCenter
    {

        private static readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GameCenter;Integrated Security=True;Connect Timeout=60;Encrypt=False;Trust Server Certificate=False";

        public static void GetStudioByName()
        {

            List<string> list = new List<string>();
            Console.WriteLine("Veuillez entrer le nom du studio que vous cherchez : ");
            string studioName = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT * FROM [dbo].[Studio] WHERE Name = @name";
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@name", studioName);


                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["Id"]);
                            string name = Convert.ToString(reader["Name"]);
                            DateTime creationDate = Convert.ToDateTime(reader["CreationDate"]);
                            string city = Convert.ToString(reader["City"]);
                            int employeNbr = Convert.ToInt32(reader["EmployeNbr"]);
                            bool IsIndependant = Convert.ToBoolean(reader["IsIndependant"]);
                            bool IsActive = Convert.ToBoolean(reader["IsActive"]);

                            list.Add($"{id} | {name} | {creationDate} | {city} | {employeNbr} | {IsIndependant} | {IsActive}");
                        }

                        conn.Close();
                    }

                    foreach (string studio in list)
                    {
                        Console.WriteLine(studio);
                    }
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        public static void DeleteGame()
        {
            Console.WriteLine("Veuillez entrer l'id du jeu à supprimer : ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //cmd.CommandText = "DELETE FROM [dbo].[Game] WHERE Id = @Id";
                    //cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "DeleteGame";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows == 1)
                    {
                        Console.WriteLine("le jeu à été supprimé !");
                    }
                    else if (rows > 1)
                    {
                        Console.WriteLine("Oups !");
                        // peut être un rollback ici ?
                    }
                    else
                    {
                        Console.WriteLine("le jeu n'existe pas ou n'a pas pu être supprimé !");
                    }
                }
            }
        }

        public static void SelectAllDetailsUnder40()
        {
            using (DbConnection conn = new SqlConnection(connectionString))
            {
                string queryGame = @"SELECT Id, Title, Description
                                     FROM GameUnder40
                                     ORDER BY Title";
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = queryGame;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 10;
                    try
                    {
                        conn.Open();
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine($@"
                                        -------------------------------------------------------
                                               Titre : {reader["Title"]} ({reader["Id"]})     
                                        -------------------------------------------------------
                                         Description:
                                         ■■■☻☻■■■
                                         {reader["Description"] ?? "♥"}
                                         ■■■☻☻■■■
                                        ========================================================

");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Aucunes données");
                            }
                        }

                    }
                    catch (DbException dbex)
                    {
                        Console.WriteLine($"Exception : {dbex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"C'est le bordel!!! ==> {ex.Message}");
                    }
                    finally 
                    { 
                       if(conn.State != ConnectionState.Closed)
                            conn.Close();
                    }
                }
            
            }
        }

        public static void SelectIndependantStudio()
        {
            //!!!! EN MODE DECONNECTE !!!!!

            string query = @"SELECT Id,  Name, City , EmployeNbr as [#]
                             FROM studio
                             WHERE IsIndependant =1
                             ORDER by EmployeNbr ";

            DataTable IndependantStudio = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (DbDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(IndependantStudio);
                    }

                }
                //Je consomme

                DisplayResult(IndependantStudio);


                //je ferme la connexion
                conn.Close();
            }



        }

        private static void DisplayResult(DataTable independantStudio)
        {
            string Header = @"
             ╔══════╦══════╦═══╦══════════╗
             ║ Name ║ City ║Id ║ #Employee║
             ╠══════╬══════╬═══╬══════════╣
";
            Console.WriteLine(Header);
            foreach (DataRow row in independantStudio.Rows)
            {
                
                Console.WriteLine($"{row["Name"]} | {row["City"]} | {row["Id"]} | {row["#"]}"); 
                
            }
        }
        public static SqlDbType GetSqlDbType(DataColumn column)
        {
            SqlParameter leparam = new SqlParameter();
            leparam.ParameterName =column.ColumnName;
            leparam.TypeName = column.DataType.ToString();


            return leparam.SqlDbType;
        }
    }


}
