using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Common;


namespace LabProductLine.RobotArmDigitalTwin
{
public class ArmAngleUIManager : MonoSingleton<ArmAngleUIManager>
{
    [Header("UI  Basic  Settings")]
    public GameObject armAngleUI_Prefab;
    public SerializableDictionary<int, Transform> armIndex2TF;
    public Dictionary<int, TMP_Text> armIndex2Text;

    private AuboController auboController;
    public override void Init()
    {
        base.Init();
        armIndex2Text = new Dictionary<int, TMP_Text>();
        auboController = FindObjectOfType<AuboController>();
    }

    private void Start()
    {
        if (armIndex2TF.keys.Count != armIndex2TF.values.Count) return;
        for (int i = 0; i < armIndex2TF.keys.Count; i++)
        {
            GameObject go = GenerateArmAngleUI(armIndex2TF.values[i].position);
            go.GetComponent<ArmAngleUI>().UIPoint = armIndex2TF.values[i];
            armIndex2Text.Add(armIndex2TF.keys[i], go.transform.GetChild(0).GetComponent<TMP_Text>());
        }
    }

    public GameObject GenerateArmAngleUI(Vector3 point)
    {
        GameObject go = Instantiate(armAngleUI_Prefab, point, Quaternion.identity, transform);
        return go;
    }

    private void Update()
    {
        if(auboController.RobotJoint!=null)
        {
            for (int i = 0; i < armIndex2TF.keys.Count; i++)
            {
                armIndex2Text[i+1].text = string.Format("轴{0}角度为：\n" + "{1}",i+1,auboController.RobotJoint[i]);
            }
        }
    }

}
}
