using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class InitSources : MonoBehaviour
{
    public static InitSources Instance;

    public List<ItemBase> List_AllItems = new List<ItemBase>();
    public List<CollectObjective> List_AllCollectObjectives = new List<CollectObjective>();
    public List<Quest> List_AllQuests = new List<Quest>();
    public List<NpcQuestManager> List_AllNpcQuestManagers = new List<NpcQuestManager>();

    public Dictionary<string, int> Dic_QuestFragment = new Dictionary<string, int>();
    public Dictionary<string, NpcQuestManager> Dic_NpcQuestManager = new Dictionary<string, NpcQuestManager>();
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        for (int i = 0; i < List_AllNpcQuestManagers.Count; i++)
        {
            if (!Dic_NpcQuestManager.ContainsKey(List_AllNpcQuestManagers[i].NpcQuestManager_ID))
            {
                Dic_NpcQuestManager.Add(List_AllNpcQuestManagers[i].NpcQuestManager_ID,List_AllNpcQuestManagers[i]);
                //Debug.Log("Had add " + List_AllNpcQuestManagers[i].NpcQuestManager_ID);
            }
            else
            {
                Debug.LogError("It't already Exist this ID of NPC");
            }
        }
        DataSet dataSet_ItemBase = ExcelTool.ReadExcel(Application.dataPath + "/ExcelFile/ItemBases.xlsx");
        for (int i = 1; i < dataSet_ItemBase.Tables[0].Rows.Count; i++)
        {
            ItemBase itemBase = new ItemBase();
            itemBase.ID = dataSet_ItemBase.Tables[0].Rows[i][0].ToString();
            itemBase.Name = dataSet_ItemBase.Tables[0].Rows[i][1].ToString();
            itemBase.Description = dataSet_ItemBase.Tables[0].Rows[i][2].ToString();
            List_AllItems.Add(itemBase);
        }

        DataSet dataSet_CollectObjective = ExcelTool.ReadExcel(Application.dataPath + "/ExcelFile/CollectObjectives.xlsx");
        for (int i = 1; i < dataSet_CollectObjective.Tables[0].Rows.Count; i++)
        {
            CollectObjective co = new CollectObjective();
            co.ID = dataSet_CollectObjective.Tables[0].Rows[i][0].ToString();
            co.Description = dataSet_CollectObjective.Tables[0].Rows[i][1].ToString();
            co.MaxAmount = int.Parse(dataSet_CollectObjective.Tables[0].Rows[i][2].ToString());
            co.CurAmount = int.Parse(dataSet_CollectObjective.Tables[0].Rows[i][3].ToString());
            co.NeededItemID = dataSet_CollectObjective.Tables[0].Rows[i][4].ToString();
            List_AllCollectObjectives.Add(co);
        }

        DataSet dataSet_Quest = ExcelTool.ReadExcel(Application.dataPath + "/ExcelFile/Quests.xlsx");
        for (int i = 0; i < dataSet_Quest.Tables[0].Columns.Count; i++)
        {
            string tmpFragment = dataSet_Quest.Tables[0].Rows[0][i].ToString();
            if (!Dic_QuestFragment.ContainsKey(tmpFragment))//如果字典不存在该字段则添加进去
            {
                Dic_QuestFragment.Add(tmpFragment, i);
            }
            else
            {
                Debug.LogError("Fragment Repeat.Check Excel,please.");
            }
        }
        //foreach (var item in Dic_QuestFragment)
        //{
        //    Debug.Log(item.Key + "  " + item.Value);
        //}
        for (int i = 1; i < dataSet_Quest.Tables[0].Rows.Count; i++)
        {
            Quest quest = new Quest();
            //quest.ID = dataSet_Quest.Tables[0].Rows[i][0].ToString();
            quest.ID = dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["ID"]].ToString();
            quest.Name = dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["Name"]].ToString();
            quest.Description = dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["Description"]].ToString();
            //初始化后置任务列表，然后根据后置任务列表初始化前置任务列表
            if (dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["NextQuestID"]].ToString() != "None")
            {
                string[] nextQuestIDs = dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["NextQuestID"]].ToString().Split(',');
                for (int j = 0; j < nextQuestIDs.Length; j++)
                {
                    quest.NextQuestIds.Add(nextQuestIDs[j]);
                }
            }
            if (dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["CollectGroup"]].ToString() != "")
            {
                string[] collectObjIDs = dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["CollectGroup"]].ToString().Split(',');
                for (int j = 0; j < collectObjIDs.Length; j++)
                {
                    //Debug.Log(collectObjIDs[j]);
                    CollectObjective co = GetCollectObjectiveByID(collectObjIDs[j]);

                    quest.CollectObjectives.Add(co);
                }

            }
            QuestReward questReward = new QuestReward();
            if (dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["RewardMoney"]].ToString() != "")
                questReward.Money = int.Parse(dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["RewardMoney"]].ToString());
            else
                questReward.Money = 0;
            if (dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["RewardEXP"]].ToString() != "")
                questReward.EXP = int.Parse(dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["RewardEXP"]].ToString());
            else
                questReward.EXP = 0;
            if (dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["RewardItemIDs"]].ToString() != "" && dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["RewardItemAmount"]].ToString() != "")
            {
                string[] RewardItemIDs = dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["RewardItemIDs"]].ToString().Split(',');
                string[] RewardItemAmounts = dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["RewardItemAmount"]].ToString().Split(',');
                if (RewardItemIDs.Length != RewardItemAmounts.Length)
                    Debug.LogError(string.Format("奖励道具id长度与数量长度不同，请检查第{0}行", i));
                for (int j = 0; j < RewardItemIDs.Length; j++)
                {
                    ItemBase tmpItem = GetItembaseByID(RewardItemIDs[j]);
                    int Amount = int.Parse(RewardItemAmounts[j]);
                    RewardItemBase rewardItemBase = new RewardItemBase();
                    rewardItemBase.RewardItem = tmpItem;
                    rewardItemBase.Amount = Amount;
                    questReward.Items.Add(rewardItemBase);

                }
            }
            if(dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["PreQuestRelationship"]].ToString() != "")
            {
                switch (dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["PreQuestRelationship"]].ToString())
                {
                    case "and":quest.PreRelationship = PreQuestRelationship.and;break;
                    case "or": quest.PreRelationship = PreQuestRelationship.or;  break;
                    default: Debug.LogError("ERROR:Check the relationship"); break;
                }
            }
            if(dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["NpcIds_GetQuest"]].ToString() != "")
            {
                string[] NpcIDs = dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["NpcIds_GetQuest"]].ToString().Split(',');
                for (int j = 0; j < NpcIDs.Length; j++)
                {
                    if (Dic_NpcQuestManager.ContainsKey(NpcIDs[j]))
                    {
                        if (!Dic_NpcQuestManager[NpcIDs[j]].QuestGroup_Get.Contains(quest.ID))
                        {
                            Dic_NpcQuestManager[NpcIDs[j]].QuestGroup_Get.Add(quest.ID);
                        }
                        else
                        {
                            Debug.Log("It's already exist QuestGroup_Get_ID " + quest.ID);
                        }
                    }
                    else
                    {
                        Debug.LogError("ERROR:Don't exist this NpcQuestManager_ID "+ NpcIDs[j]);
                    }
                }
            }
            if (dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["NpcIds_SubmitQuest"]].ToString() != "")
            {
                string[] NpcIDs = dataSet_Quest.Tables[0].Rows[i][Dic_QuestFragment["NpcIds_SubmitQuest"]].ToString().Split(',');
                for (int j = 0; j < NpcIDs.Length; j++)
                {
                    if (Dic_NpcQuestManager.ContainsKey(NpcIDs[j]))
                    {
                        if (!Dic_NpcQuestManager[NpcIDs[j]].QuestGroup_Submit.Contains(quest.ID))
                        {
                            Dic_NpcQuestManager[NpcIDs[j]].QuestGroup_Submit.Add(quest.ID);
                        }
                        else
                        {
                            Debug.Log("It's already exist QuestGroup_Submit_ID " + quest.ID);
                        }
                    }
                    else
                    {
                        Debug.LogError("ERROR:Don't exist this NpcQuestManager_ID " + NpcIDs[j]);
                    }
                }
            }
            //quest.Init(); 
            quest.QuestReward = questReward;
            List_AllQuests.Add(quest);
        }

        SortQuest();//根据id排序
        InitPreQuests();

    }
    #region Old Check Function
    public ItemBase GetItembaseByID(string id)
    {
        foreach (var item in List_AllItems)
        {
            if (item.ID == id)
            {
                ItemBase tmpItem = new ItemBase();
                tmpItem.ID = item.ID;
                tmpItem.Name = item.Name;
                tmpItem.Description = item.Description;
                tmpItem.ItemType = ItemTypes.Medicine;
                return tmpItem;
            }
        }
        return null;

    }
    public CollectObjective GetCollectObjectiveByID(string id)
    {
        foreach (var Co in List_AllCollectObjectives)
        {
            if (Co.ID == id)
            {
                CollectObjective tmpCo = new CollectObjective();
                tmpCo.ID = Co.ID;
                tmpCo.Description = Co.Description;
                tmpCo.MaxAmount = Co.MaxAmount;
                tmpCo.CurAmount = Co.CurAmount;
                tmpCo.NeededItemID = Co.NeededItemID;
                return tmpCo;
            }
        }
        return null;

    }

    public Quest GetQuestByID(string id)
    {
        foreach (Quest quest in List_AllQuests)
        {
            if (quest.ID == id)
            {
                Quest tmpQuest = new Quest();
                tmpQuest.ID = quest.ID;
                tmpQuest.Name = quest.Name;
                tmpQuest.Description = quest.Description;
                tmpQuest.NextQuestIds = quest.NextQuestIds;
                foreach (CollectObjective co in quest.CollectObjectives)
                {
                    CollectObjective tmpCo = new CollectObjective();
                    tmpCo.ID = co.ID;
                    tmpCo.Description = co.Description;
                    tmpCo.MaxAmount = co.MaxAmount;
                    tmpCo.CurAmount = co.CurAmount;
                    tmpCo.NeededItemID = co.NeededItemID;
                    tmpQuest.CollectObjectives.Add(tmpCo);
                }
                tmpQuest.Init();
                tmpQuest.QuestReward = quest.QuestReward;//此处其实并不严谨，应该效仿tmpQuest获取CollectObjective，但由于不需要更改数量，故无所谓
                tmpQuest.PreRelationship = quest.PreRelationship;
                tmpQuest.PreQuestIds = quest.PreQuestIds;
                return tmpQuest;
            }
        }
        return null;
    }
    #endregion
    #region 折半查找 仅限自动初始化前置任务
    /// <summary>
    /// 折半查找任务，根据ID，仅限自动初始化前置任务
    /// 因为要修改源任务
    /// </summary>
    /// <returns></returns>
    private Quest GetQuestByID_Half(string id)
    {
        int low = 0;
        int height = List_AllQuests.Count - 1;
        for (int i = 0; i < List_AllQuests.Count; i++)
        {
            int mid = (low + height) / 2;
            //Debug.Log("执行次数 "+ mid);
            if (List_AllQuests[mid].ID == id)
            {
                return List_AllQuests[mid];
            }
            else if (int.Parse(List_AllQuests[mid].ID) > int.Parse(id))
            {
                height = mid - 1;
            }
            else
            {
                low = mid + 1;
            }
        }
        return null;


    }
    #endregion
    /// <summary>
    /// 根据任务ID号排序
    /// </summary>
    private void SortQuest()
    {
        for (int i = 0; i < List_AllQuests.Count - 1; i++)
        {
            for (int j = 0; j < List_AllQuests.Count - 1 - i; j++)
            {
                if (int.Parse(List_AllQuests[j].ID) > int.Parse(List_AllQuests[j + 1].ID))
                {
                    Quest tmpQuest = new Quest();
                    tmpQuest = List_AllQuests[j];
                    List_AllQuests[j] = List_AllQuests[j + 1];
                    List_AllQuests[j + 1] = tmpQuest;
                }
            }

        }
    }
    /// <summary>
    /// 初始化前置任务列表
    /// </summary>
    private void InitPreQuests()
    {
        //GetQuestByID_Half("1006");
        for (int i = 0; i < List_AllQuests.Count; i++)
        {
            for (int j = 0; j < List_AllQuests[i].NextQuestIds.Count; j++)
            {
                string tmpNextQuestID = List_AllQuests[i].NextQuestIds[j];
                //Debug.Log(tmpNextQuestID);
                Quest tmpQuest = GetQuestByID_Half(tmpNextQuestID);//要更新前置任务的任务
                if (!HavePreQuestId(tmpQuest, List_AllQuests[i].ID))
                {
                    tmpQuest.PreQuestIds.Add(List_AllQuests[i].ID);
                }
            }

        }
    }
    /// <summary>
    /// 初始化前置任务列表时，检查是否存在对应ID
    /// </summary>
    /// <param name="quest"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool HavePreQuestId(Quest quest, string id)
    {
        for (int i = 0; i < quest.PreQuestIds.Count; i++)
        {
            if (quest.PreQuestIds[i] == id)
                return true;
        }
        return false;
    }

}
