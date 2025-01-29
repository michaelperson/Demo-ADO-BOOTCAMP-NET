using Bootcamp.Net.ADO.APP;
using Microsoft.Data.SqlClient;
using System.Data;

// cmd.ExecuteScalar -> Une seule valeur, équivalent de A1
// cmd.ExecuteNonQuery -> Nombre de lignes affectées
// using(SqlDataRader) -> une ou plusieurs lignes


bool continuer = true;
while (continuer)
{
    Console.WriteLine("1. Afficher les Details studio par nom\n" +
                      "2. Supprimer un jeu\n" +
                      "0. Quitter");
    string choix = Console.ReadLine();
    Console.Clear();


    switch(choix)
    {
        case "0":
            continuer = false;
            break;
        case "1":
            GameCenter.GetStudioByName();
            break;
        case "2":
            GameCenter.DeleteGame();
            break;

        default:
            Console.WriteLine("choix invalide");
            break;
    }

}