using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 实现思想为存储所有可以接取的任务和可以提交的任务两个列表
/// NPC与玩家交互时，将自身赋值给玩家
/// 玩家根据两个列表比对自身已完成的任务ID
/// 
/// </summary>
public class NpcQuestManager : MonoBehaviour
{
    public string NpcQuestManager_ID;
    public List<string> QuestGroup_Get = new List<string>();
    public List<string> QuestGroup_Submit = new List<string>();

    //public Dictionary<>
    public void MeetNpc()
    {
        QuestAcceptPanelManager.Instance.SetQuestWindow(this);
    }

}
