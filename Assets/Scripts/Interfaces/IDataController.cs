using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

interface IDataController
{
    public ICollection<BattleableVOBase> UseSelect(string _query);
    public bool UseUpdate(BattleableVOBase status);
    public bool UseUpdate(int id, Vector3 position);
    public bool UseUpdate(string _query);
    public bool UseDelete(string _query);
    public bool UseInsert(string _query);
}
