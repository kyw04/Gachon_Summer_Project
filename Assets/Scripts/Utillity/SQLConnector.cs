using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class SQLConnector
{
    public readonly static string ConnectionString = "URI=file:" + Application.streamingAssetsPath;
    public SQLConnector()
    {
        string dbName = "/Battleable.db";

        IDbConnection dbConnection = new SqliteConnection(ConnectionString + dbName);
        dbConnection.Open();
        dbConnection.Close();
        dbConnection.Open();

        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "select * from PlayerData";

        IDataReader dataReader = dbCommand.ExecuteReader();

        while (dataReader.Read())
        {
            var name = dataReader.GetString(0);
            var maxHealthPoint = dataReader.GetInt32(1);
            var maxStaminaPoint = dataReader.GetInt32(2);
            var attackPoint = dataReader.GetInt32(3);
            var defencePoint = dataReader.GetInt32(4);
            var spd = dataReader.GetInt32(5);

            Debug.Log($" {name}\n {maxHealthPoint}\n {maxStaminaPoint}\n {attackPoint}\n {defencePoint}\n {spd}");
            
            //연동 성공
        }
        dbConnection.Close();
    }
}