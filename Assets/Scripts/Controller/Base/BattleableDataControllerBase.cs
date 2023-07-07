using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BattleableDataControllerBase: IDataController
{
    public readonly static string ConnectionString = "URI=file:" + Application.persistentDataPath;

    protected IDbConnection dbConnection;
    protected IDbCommand dbCommand;
    protected IDataReader dataReader;

    public abstract BattleableVOBase getData();

    public abstract ICollection<BattleableVOBase> UseSelect(string _query);
    public abstract bool UseUpdate(BattleableVOBase status);
    public abstract bool UseUpdate(int id, Vector3 position);
    public abstract bool UseUpdate(string _query);
    public abstract bool UseDelete(string _query);
    public abstract bool UseInsert(string _query);
}
