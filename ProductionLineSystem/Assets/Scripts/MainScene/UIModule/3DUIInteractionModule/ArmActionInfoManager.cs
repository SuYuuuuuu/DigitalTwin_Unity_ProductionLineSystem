using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEditor;
using UnityEngine;

public class ArmActionInfoManager:SingletonBase<ArmActionInfoManager>
{
    private Dictionary<int,DobotAnimation> armActionDic;

    public ArmActionInfoManager()
    {
        armActionDic = new Dictionary<int, DobotAnimation>();
        DobotAnimation[] actionPanels = (DobotAnimation[])AssetDatabase.LoadAllAssetsAtPath(@"Assets/Resources/ScriptObject");
        foreach (var item in actionPanels)
        {
            if(!armActionDic.ContainsKey(item.animId))
                armActionDic.Add(item.animId,item);
        }

    }

    public DobotAnimation FindDobotAnimById(int id)
    {
        if(armActionDic.ContainsKey(id))
            return armActionDic[id];
        Debug.Log("未查询到该ID的动作信息");
        return null;
    }

}
