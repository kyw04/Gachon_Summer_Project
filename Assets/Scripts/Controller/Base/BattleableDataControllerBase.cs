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

    // 플레이어 데이터 한 개를 가져올 때 쓰는 메소드
    //Data가 복수형이고 Datum이 단수형이래요
    public abstract BattleableVOBase getDatum();

    public abstract ICollection<BattleableVOBase> UseSelect(string _query);
    public abstract bool UseUpdate(BattleableVOBase status);
    public abstract bool UseUpdate(int id, Vector3 position);
    public abstract bool UseUpdate(string _query);
    public abstract bool UseDelete(string _query);
    public abstract bool UseInsert(string _query);
}
