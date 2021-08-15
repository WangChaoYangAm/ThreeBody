using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyAssets/EnemyAlertViewData")]
public class EnemyAlertViewData : ScriptableObject
{
    public List<AlertViewConfig> AlertViewConfig = new List<AlertViewConfig>();
}
[System.Serializable]
public class AlertViewConfig
{
    /// <summary>
    /// �������ڼ�¼��Ұ���䷶Χ��X/Y��ƫ�� Z�����ֱ�ӽ��뾯���AlertDistance
    /// </summary>
    public string Name;
    public float X_Angle;
    public float Y_Angle;
    public float Z_Distance;
    public float AlertDistance;
    public float DefaultRatio;//Ĭ�Ͼ�����������
}