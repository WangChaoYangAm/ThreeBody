using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 由玩家存储已完成任务和正在进行中的任务ID
/// 该脚本自动将所有进行中的任务进行接取并更新当前任务对应道具数量
/// </summary>
public class PlayerQuestManager : MonoBehaviour
{
    public static PlayerQuestManager Instance;
    //public List<string> List_QuestIds_Completed = new List<string>();
    //public List<string> List_QuestIds_Ongoing = new List<string>();
    [SerializeField]
    private List<Quest> List_Quest_Ongoing = new List<Quest>();
    public Dictionary<string, Quest> List_PlayerQuests = new Dictionary<string, Quest>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //用于读档
    public void Init()
    {
        //Debug.Log(List_Quest_Ongoing2.Count);
        //for (int i = 0; i < List_Quest_Ongoing.Count; i++)
        //{
        //    //AddQuest(List_Quest_Ongoing[i].ID);
        //    Quest tmpQuest = InitSources.Instance.GetQuestByID(List_Quest_Ongoing[i].ID);
        //    tmpQuest.AcceptQuest();
        //    List_Quest_Ongoing.Add(tmpQuest);
        //}
    }
    public void AddQuest(Quest quest)
    {
        //Quest tmpQuest = InitSources.Instance.GetQuestByID(id);
        //tmpQuest.AcceptQuest();
        //List_Quest_Ongoing.Add(tmpQuest);
        if (!List_PlayerQuests.ContainsKey(quest.ID))
        {
            quest.AcceptQuest();
            List_PlayerQuests.Add(quest.ID, quest);
            Debug.Log("<color=red>已接取任务</color>" + quest.ID);
            List_Quest_Ongoing.Add(quest);
        }
    }
    public void CompleteTheQuest(string id)
    {
        if (List_PlayerQuests.ContainsKey(id))
        {
            Quest SelectedQuest = List_PlayerQuests[id];
            for (int i = 0; i < SelectedQuest.CollectObjectives.Count; i++)
            {
                BagManager.Instance.RemoveItem(SelectedQuest.CollectObjectives[i].NeededItemID, SelectedQuest.CollectObjectives[i].MaxAmount);
            }
            for (int i = 0; i < SelectedQuest.QuestReward.Items.Count; i++)
            {
                BagManager.Instance.GetItem(SelectedQuest.QuestReward.Items[i].RewardItem.ID, SelectedQuest.QuestReward.Items[i].Amount);
            }
            List_Quest_Ongoing.Remove(List_PlayerQuests[id]);
            List_PlayerQuests[id].questStatus = QuestStatus.Completed;
        }
    }
}
