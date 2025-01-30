using Bootcamp.Net.ADO.APP;
using Microsoft.Data.SqlClient;
using System.Data;

bool continuer = true;
while (continuer)
{
    Console.WriteLine("1. Afficher les Details studio par nom\n" +
                      "2. Supprimer un jeu\n" +
                      "3. Afficher le détail de tous les jeux à moins de 40€\n" +
                      "4. Afficher les studios indépendants et leur ville classé par le nombre d'employés" +
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
        case "3":
            GameCenter.SelectAllDetailsUnder40();
            break;
        case "4":
            GameCenter.SelectIndependantStudio();
            break;

        default:
            Console.WriteLine("choix invalide");
            break;
    }

}