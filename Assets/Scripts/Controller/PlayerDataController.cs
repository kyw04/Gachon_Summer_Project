using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

sealed class PlayerDataController : BattleableDataControllerBase
{
    public static string TableName = "PlayerData";

    public PlayerDataController(string dbName)
    {
        Debug.Log(Application.persistentDataPath);
        if (!File.Exists(Application.persistentDataPath + dbName))
            File.Copy(Application.streamingAssetsPath + dbName, Application.persistentDataPath + dbName);
        dbConnection = new SqliteConnection(ConnectionString + dbName);
    }

    public override BattleableVOBase getData()
    {
        BattleableVOBase result = default;

        var tables = UseSelect($"select * from {TableName} where UID = '{SystemInfo.deviceUniqueIdentifier}' ");

        foreach (var table in tables)
        {
            result = table;
        }

     //연동 성공
        return result;
    }

    public override bool UseDelete(string _query)
    {
        throw new NotImplementedException();
    }

    public override ICollection<BattleableVOBase> UseSelect(string _query)
    {

       var result = new List<BattleableVOBase>();
        try
        {
            dbConnection.Open();
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = _query;
            dataReader = dbCommand.ExecuteReader();

            while (dataReader.Read())
            {
                result.Add(
                    //0번은 ID
                    new PlayerVO(
                        dataReader.GetInt32(0),
                        dataReader.GetString(2),
                        dataReader.GetInt32(3),
                        dataReader.GetInt32(4),
                        dataReader.GetInt32(5),
                        dataReader.GetInt32(6),
                        dataReader.GetFloat(7),
                        dataReader.GetString(8),
                        new Vector3(
                            dataReader.GetFloat(9),
                            dataReader.GetFloat(10),
                            dataReader.GetFloat(11)
                            ))
                    );
            }

            if (result.Count == 0) throw new Exception("INVAID_QUERY", new Exception($"Invaild Query : \n \"{_query}\" "));
        }
        catch (Exception e)
        {
            if (e.Message.Equals("INVAID_QUERY"))
            {
                CloseConnection();
                Debug.Log("uid 미발견");
                Debug.Log(UseInsert($"insert into {TableName} (UID, Name) values ('{SystemInfo.deviceUniqueIdentifier}', 'DON')"));

                result = (List<BattleableVOBase>)UseSelect($"select * from {TableName} where uid = '{SystemInfo.deviceUniqueIdentifier}' ");
            }
            else
                Debug.Log("failed " + e.Message);
        }
        finally
        {
            CloseConnection();
        }
        return result;
    }

    public override bool UseUpdate(BattleableVOBase status)
    {
        bool result = true;
        try
        {
            dbConnection.Open();
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText =
                $"update PlayerData set name = '{status.name}'," +
                                      $"maxHealthPoint = {status.maxHealthPoint}," +
                                      $"maxStaminaPoint = {status.maxStaminaPoint}," +
                                      $"attackPoint = {status.attackPoint}," +
                                      $"defencePoint = {status.defencePoint}," +
                                      $"spd = {status.spd}," +
                                      $"modelName = '{status.modelName}'," +
                                      $"x = {status.position.x}, y = {status.position.y}, z = {status.position.z} " +
                                      $"where id = {status.id}";
            dbCommand.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            CloseConnection();
        }
        return result;
    }
    public override bool UseUpdate(int id, Vector3 position)
    {
        bool result = true;
        try
        {
            dbConnection.Open();
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText =
                $"update PlayerData set x = {position.x}, y = {position.y}, z = {position.z} where id = {id}";
            dbCommand.ExecuteNonQuery();
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            CloseConnection();
        }
        return result;
    }
    public override bool UseUpdate(string _query)
    {
        bool result = true;
        try
        {
            dbConnection.Open();
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = _query;
            dbCommand.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            CloseConnection();
        }
        return result;
    }

    public override bool UseInsert(string _query)
    {
        bool result = true;
        try
        {
            dbConnection.Open();
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = _query;
            dbCommand.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            result = false;
        }
        finally
        {
            CloseConnection();
        }
        return result;
    }

    private void CloseConnection()
    {
        dataReader?.Dispose();
        dbCommand.Dispose();
        dbConnection.Close();
    }
}
