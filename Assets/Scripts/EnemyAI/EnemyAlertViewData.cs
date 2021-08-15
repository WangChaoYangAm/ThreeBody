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
    /// 该类用于记录视野警戒范围的X/Y半偏角 Z距离和直接进入警戒的AlertDistance
    /// </summary>
    public string Name;
    public float X_Angle;
    public float Y_Angle;
    public float Z_Distance;
    public float AlertDistance;
    public float DefaultRatio;//默认警戒增长比率
}