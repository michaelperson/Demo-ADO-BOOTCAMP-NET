using GameManager.Exceptions;
using GameManager.Formatters;
using GameManager.Interfaces;
using GameManager.Tools;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace GameManager
{
    /// <summary>
    /// Classe abstraite gérant les opérations de base de données de manière asynchrone
    /// </summary>
    public abstract class DBDataManager : IDataManager 
    {
        protected readonly DbConnection _connection;

        /// <summary>
        /// Constructeur initialisant la connexion à la base de données
        /// </summary>
        /// <param name="connection">Connexion à la base de données</param>
        protected DBDataManager(DbConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Récupère toutes les données d'une table spécifique
        /// </summary>
        /// <param name="tableName">Nom de la table</param>
        /// <returns>Un DbDataReader contenant les résultats</returns>
        /// <exception cref="DBManagerException">Si le nom de la table est vide</exception>
        /// 
        /// Tests à effectuer:
        /// - Vérifier que la méthode lance une exception si tableName est null ou vide
        /// - Vérifier que la méthode retourne des données pour une table existante
        /// - Vérifier que la connexion est bien fermée en cas d'erreur
        public async Task<DbDataReader> GetAll(string tableName)
        {
            if (string.IsNullOrEmpty(tableName)) throw new DBManagerException("No table name ");
            return await this.GetAsync($"Select * FROM {tableName}");
        }

        /// <summary>
        /// Exécute une requête SELECT asynchrone avec paramètres
        /// </summary>
        /// <param name="query">Requête SQL</param>
        /// <param name="parameters">Paramètres de la requête</param>
        /// <returns>Un DbDataReader contenant les résultats</returns>
        /// <exception cref="DBManagerException">Si la requête n'est pas un SELECT</exception>
        /// 
        /// Tests à effectuer:
        /// - Vérifier que la méthode lance une exception si la requête n'est pas un SELECT
        /// - Vérifier que les paramètres sont correctement ajoutés à la commande
        /// - Vérifier le comportement avec des paramètres null
        /// - Vérifier que la connexion est ouverte pendant l'exécution
        /// - Vérifier que la connexion est fermée après l'utilisation du DataReader
        public async Task<DbDataReader> GetAsync(string query, params (string Name, object Value)[] parameters)
        {
            if (SqlQueryAnalyzer.ContainsOperation(query, SqlQueryAnalyzer.SqlOperationType.Select))
            {
                await _connection.OpenAsync();

                var command = _connection.CreateCommand();
                command.CommandText = query;

                foreach (var (name, value) in parameters)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = name.StartsWith("@") ? name : $"@{name}";
                    parameter.Value = value ?? DBNull.Value;
                    command.Parameters.Add(parameter);
                }

                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            else
            {
                throw new DBManagerException("Invalide Query expression - Only Select statement is permitted");
            }
        }

        /// <summary>
        /// Récupère les données sous forme de liste formatée
        /// </summary>
        /// <param name="query">Requête SQL</param>
        /// <param name="parameters">Paramètres de la requête</param>
        /// <returns>Liste de chaînes formatées contenant les résultats</returns>
        /// 
        /// Tests à effectuer:
        /// - Vérifier le format des chaînes retournées
        /// - Vérifier le comportement avec un jeu de données vide
        /// - Vérifier la gestion des valeurs null dans les colonnes
        /// - Vérifier la conversion des types pour chaque colonne
        /// - Vérifier que la connexion est bien fermée après utilisation
        public async Task<List<string>> GetListAsync(string query, params (string Name, object Value)[] parameters)
        {
            List<string> list = new List<string>();
            if (SqlQueryAnalyzer.ContainsOperation(query, SqlQueryAnalyzer.SqlOperationType.Select))
            {
                using (DbDataReader reader = await GetAsync(query, parameters))
                {
                    while (reader.Read())
                    {
                        int id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0;
                        string name = reader["Name"] != DBNull.Value ? Convert.ToString(reader["Name"]) : string.Empty;
                        DateTime creationDate = reader["CreationDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreationDate"]) : DateTime.MinValue;
                        string city = reader["City"] != DBNull.Value ? Convert.ToString(reader["City"]) : string.Empty;
                        int employeNbr = reader["EmployeNbr"] != DBNull.Value ? Convert.ToInt32(reader["EmployeNbr"]) : 0;
                        bool IsIndependant = reader["IsIndependant"] != DBNull.Value ? Convert.ToBoolean(reader["IsIndependant"]) : false;
                        bool IsActive = reader["IsActive"] != DBNull.Value ? Convert.ToBoolean(reader["IsActive"]) : false;


                        list.Add($"{id} | {name} | {creationDate} | {city} | {employeNbr} | {IsIndependant} | {IsActive}");
                    }
                }
                return list;
            }
            else
            {
                throw new DBManagerException("Invalide Query expression - Only Select statement is permitted");
            }
        }

        /// <summary>
        /// Récupère les données sous forme de tableau formaté
        /// </summary>
        /// <param name="query">Requête SQL</param>
        /// <param name="parameters">Paramètres de la requête</param>
        /// <returns>Chaîne représentant un tableau formaté</returns>
        /// 
        /// Tests à effectuer:
        /// - Vérifier le formatage du tableau avec différentes largeurs de colonnes
        /// - Vérifier le comportement avec un DataTable vide
        /// - Vérifier le message "No data found"
        /// - Vérifier la gestion des caractères spéciaux dans le formatage
        /// - Vérifier que la connexion est fermée après utilisation
        public async Task<string> GetFormattedTableAsync(string query, params (string Name, object Value)[] parameters)
        {
            if (!SqlQueryAnalyzer.ContainsOperation(query, SqlQueryAnalyzer.SqlOperationType.Select))
            {
                throw new DBManagerException("Invalid Query expression - Only Select statement is permitted");
            }

            DataTable dataTable = await LoadDataTableAsync(query, parameters);

            if (dataTable == null || dataTable?.Rows.Count == 0)
                return "No data found.";

            TableFormatter tbf = new TableFormatter(dataTable);
            return tbf.BuildFormattedTable();
        }

        /// <summary>
        /// Charge les données dans un DataTable
        /// </summary>
        /// <param name="query">Requête SQL</param>
        /// <param name="parameters">Paramètres de la requête</param>
        /// <returns>DataTable contenant les résultats</returns>
        /// 
        /// Tests à effectuer:
        /// - Vérifier l'initialisation du DataTable (actuellement il y a un bug car dataTable est null)
        /// - Vérifier le chargement correct des données
        /// - Vérifier la gestion des erreurs pendant le chargement
        /// - Vérifier que la connexion est fermée après utilisation
        private async Task<DataTable> LoadDataTableAsync(string query, params (string Name, object Value)[] parameters)
        {
            
            DataTable dataTable = new DataTable();
            using (DbDataReader reader = await GetAsync(query, parameters))
            {
                dataTable.Load(reader);
            }
            return dataTable;
        }

        /// <summary>
        /// Exécute une requête UPDATE
        /// </summary>
        /// <param name="query">Requête SQL</param>
        /// <param name="parameters">Paramètres de la requête</param>
        /// <returns>Nombre de lignes affectées</returns>
        /// 
        /// Tests à effectuer:
        /// - Vérifier que seules les requêtes UPDATE sont acceptées
        /// - Vérifier le nombre de lignes affectées
        /// - Vérifier la gestion des contraintes de clés étrangères
        /// - Vérifier la gestion des paramètres null
        /// - Vérifier que la connexion est fermée après utilisation
        public async Task<int> ExecuteUpdateAsync(string query, params (string Name, object Value)[] parameters)
        {
            if (SqlQueryAnalyzer.ContainsOperation(query, SqlQueryAnalyzer.SqlOperationType.Update))
            {
                return await ExecuteNonqueryAsync(query, parameters);
            }
            else
            {
                throw new DBManagerException("Invalide Query expression - Only Update is permitted");
            }
        }

        /// <summary>
        /// Exécute une requête DELETE
        /// </summary>
        /// <param name="query">Requête SQL</param>
        /// <param name="parameters">Paramètres de la requête</param>
        /// <returns>Nombre de lignes supprimées</returns>
        /// 
        /// Tests à effectuer:
        /// - Vérifier que seules les requêtes DELETE sont acceptées
        /// - Vérifier le nombre de lignes supprimées
        /// - Vérifier la gestion des contraintes de clés étrangères
        /// - Vérifier la suppression en cascade si configurée
        /// - Vérifier que la connexion est fermée après utilisation
        public async Task<int> ExecuteDeleteAsync(string query, params (string Name, object Value)[] parameters)
        {
            if (SqlQueryAnalyzer.ContainsOperation(query, SqlQueryAnalyzer.SqlOperationType.Delete))
            {
                return await ExecuteNonqueryAsync(query, parameters);
            }
            else
            {
                throw new DBManagerException("Invalide Query expression - Only DELETE statement is permitted");
            }
        }

        /// <summary>
        /// Méthode privée pour exécuter des requêtes non-query (UPDATE, DELETE)
        /// </summary>
        /// <param name="query">Requête SQL</param>
        /// <param name="parameters">Paramètres de la requête</param>
        /// <returns>Nombre de lignes affectées</returns>
        /// 
        /// Tests à effectuer:
        /// - Vérifier la gestion des paramètres
        /// - Vérifier l'ouverture et la fermeture de la connexion
        /// - Vérifier le comportement en cas d'erreur SQL
        /// - Vérifier la gestion des timeouts
        private async Task<int> ExecuteNonqueryAsync(string query, params (string Name, object Value)[] parameters)
        {
            await _connection.OpenAsync();

            using var command = _connection.CreateCommand();
            command.CommandText = query;

            foreach (var (name, value) in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = name.StartsWith("@") ? name : $"@{name}";
                parameter.Value = value ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }

            return await HandleDatabaseOperationAsync(() => command.ExecuteNonQueryAsync(), "Update");

            //return await command.ExecuteNonQueryAsync();
        }


        /// <summary>
        /// Gère les erreurs de base de données de manière centralisée
        /// </summary>
        protected async Task<T> HandleDatabaseOperationAsync<T>(Func<Task<T>> operation, string operationName)
        {
            try
            {
                await EnsureConnectionOpenAsync();
                return await operation.Invoke();
            }
            catch (DBManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DBManagerException($"Erreur lors de l'opération {operationName}", ex);
            }
        }

        /// <summary>
        /// Vérifie l'état de la connexion et la rouvre si nécessaire
        /// </summary>
        protected async Task EnsureConnectionOpenAsync()
        {
            try
            {

                if (_connection.State != ConnectionState.Open)
                {
                    await _connection.OpenAsync();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}