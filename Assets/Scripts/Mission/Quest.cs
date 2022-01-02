using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest 
{
    public string ID;
    public string Name;
    public string Description;
    public QuestStatus questStatus;

    [SerializeField]
    private List<string> mPreQuestIds = new List<string>();
    public List<string> PreQuestIds
    {
        get
        {
            return mPreQuestIds;
        }
        set
        {
            mPreQuestIds = value;
        }
    }
    [SerializeField]
    private List<string> mNextQuestIds = new List<string>();
    public List<string> NextQuestIds
    {
        get
        {
            return mNextQuestIds;
        }
        set
        {
            mNextQuestIds = value;
        }
    }

    [SerializeField]
    List<Objective> mObjectives = new List<Objective>();
    public List<Objective> Objectives
    {
        get
        {
            return mObjectives;
        }
        set
        {
            mObjectives = value;
        }
    }
    [SerializeField]
    List<CollectObjective> mCollectObjectives = new List<CollectObjective>();
    public List<CollectObjective> CollectObjectives
    {
        get
        {
            return mCollectObjectives;
        }
        set
        {
            mCollectObjectives = value;
        }
    }
    [SerializeField]
    QuestReward mQuestReward = new QuestReward();
    public QuestReward QuestReward
    {
        get
        {
            return mQuestReward;
        }
        set
        {
            mQuestReward = value;
        }
    }
    [SerializeField]
    PreQuestRelationship mPreRelationship;
    public PreQuestRelationship PreRelationship
    {
        get
        {
            return mPreRelationship;
        }
        set
        {
            mPreRelationship = value;
        }
    }
    public Quest()
    {
        ID = "None";
        Name = "None";
        Description = "None";
        questStatus = QuestStatus.NotAccepted;
        //NextQuestID = "None";
    }
    public void Init()
    {
        foreach (CollectObjective co in CollectObjectives)
        {
            mObjectives.Add(co);
        }
    }
    public void AcceptQuest()
    {
        if (questStatus == QuestStatus.NotAccepted)
        {
            questStatus = QuestStatus.Ongoing;
            foreach (var o in mObjectives)
            {
                if(o is CollectObjective)
                {
                    CollectObjective co = o as CollectObjective;
                    BagManager.Instance.OnGetItem += co.UpdateCollectAmountUp;
                    BagManager.Instance.OnRemoveItem += co.UpdateCollectAmountDown;
                    Debug.Log("执行此处");
                    co.UpdateCollectAmountUp(co.NeededItemID, BagManager.Instance.GetItemAmountByID(co.NeededItemID));
                }
            }
        }
    }
    public void SelectThisQuest()
    {
        QuestAcceptPanelManager.Instance.SelectedQuest = this;
    }
    public bool IsCompleted
    {
        get
        {
            if(questStatus == QuestStatus.Ongoing)
            {
                foreach (Objective o in mObjectives)
                {
                    if (o.IsCompleted == false)
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}
/// <summary>
/// 我阐述一种功能实现方式，每个人物上都有字段 “前置任务”和 “后置任务”
/// 每个任务完成的时候，将该任务ID存储到公用已完成任务列表中
/// 然后触发后置任务，如果该后置任务的ID不在已完成任务列表中，则触发，如果在，那就不触发该任务
/// 比如上面的图 CFG已经完成。那么再触发D，由于F已经完成，那么就不触发F。
/// 类似的，完成A 之后正常触发E，然后E完成后由于G已经完成，就不重复触发G了
/// </summary>
public enum QuestStatus
{
    NotAccepted,
    Ongoing,
    Completed,
    Abandoned
}
public enum PreQuestRelationship
{
    and,
    or
}