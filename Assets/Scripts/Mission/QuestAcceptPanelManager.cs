using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 遇到对象时的任务对话框
/// </summary>
public class QuestAcceptPanelManager : MonoBehaviour
{
    public static QuestAcceptPanelManager Instance;
    public Text Context;
    [SerializeField]
    private Quest mSelectedQuest;
    public Quest SelectedQuest
    {
        get
        {
            return mSelectedQuest;
        }
        set
        {
            mSelectedQuest = value;
            //ShowDescription();
        }
    }
    public GameObject Panel_QuestSelect;
    public GameObject Btn_Prefab_Quest;
    public Button Btn_AcceptQuest;
    public Button Btn_SubmitQuest;
    public List<GameObject> Button_Quest = new List<GameObject>();
    public List<Quest> Quests = new List<Quest>();
    //private List<string> List_CompletedQuestId = new List<string>();

    private NpcQuestManager curNpcQuestManager;

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
        Btn_AcceptQuest.onClick.AddListener(AcceptQuest);
        Btn_SubmitQuest.onClick.AddListener(SubmitQuest);
    }

    // Update is called once per frame
    void Update()
    {

    }
    //更改mSelectedQuest时，自动调用？
    public void ShowDescription()
    {
        if (SelectedQuest != null)
        {
            Context.text = string.Format("Name:{0}\nDescription:{1}\n", SelectedQuest.Name, SelectedQuest.Description);
            if (SelectedQuest.questStatus == QuestStatus.NotAccepted)
            {
                Context.text += string.Format("QuestStatus： " + "未领取\n");
            }
            else if (SelectedQuest.questStatus == QuestStatus.Ongoing)
            {
                for (int i = 0; i < SelectedQuest.CollectObjectives.Count; i++)
                {
                    Context.text += SelectedQuest.CollectObjectives[i].IsCompleted ? SelectedQuest.CollectObjectives[i].Description + " 已完成\n" :
                            string.Format("{0}\n  数量{1}/{2}\n", SelectedQuest.CollectObjectives[i].Description, SelectedQuest.CollectObjectives[i].CurAmount, SelectedQuest.CollectObjectives[i].MaxAmount);

                }
            }
            else if (SelectedQuest.questStatus == QuestStatus.Completed)
            {
                Context.text += string.Format("QuestStatus： " + "已提交\n");
            }

        }
        //if (SelectedQuest != null)
        //{
        //    if (PlayerQuestManager.Instance.List_QuestIds_Ongoing.Contains(SelectedQuest.ID))
        //    {

        //    }
        //}
    }
    /// <summary>
    /// 挂载在交接任务按钮上
    /// </summary>
    public void AcceptQuest()
    {
        if (SelectedQuest.questStatus == QuestStatus.NotAccepted)
        {
            //SelectedQuest.AcceptQuest();
            PlayerQuestManager.Instance.AddQuest(SelectedQuest);
            ShowDescription();
            Debug.Log("hahah");
        }

    }

    public void SubmitQuest()
    {
        if (PlayerQuestManager.Instance.List_PlayerQuests.ContainsKey(SelectedQuest.ID))
        {
            if (SelectedQuest.questStatus == QuestStatus.Ongoing && SelectedQuest.IsCompleted)
            {

                PlayerQuestManager.Instance.CompleteTheQuest(SelectedQuest.ID);
                UpdateQuestAcceptPanelManager();
            }
        }
    }
    /// <summary>
    /// 由NpcQuestManager控制
    /// </summary>
    /// <param name="questManager"></param>
    public void SetQuestWindow(NpcQuestManager questManager)
    {
        curNpcQuestManager = questManager;

        UpdateQuestAcceptPanelManager();
    }
    //public bool HadCompleteQuestByID(string questId)
    //{
    //    for (int i = 0; i < PlayerQuestManager.Instance.List_QuestIds_Completed.Count; i++)
    //    {
    //        if (PlayerQuestManager.Instance.List_QuestIds_Completed[i] == questId)
    //            return true;
    //    }
    //    return false;
    //}
    public void UpdateQuestAcceptPanelManager()
    {
        Quests.Clear();
        for (int i = 0; i < curNpcQuestManager.QuestGroup_Get.Count; i++)
        {
            if (PlayerQuestManager.Instance.List_PlayerQuests.ContainsKey(curNpcQuestManager.QuestGroup_Get[i]))
            {
                if (PlayerQuestManager.Instance.List_PlayerQuests[curNpcQuestManager.QuestGroup_Get[i]].questStatus == QuestStatus.Ongoing)
                {
                    Quests.Add(PlayerQuestManager.Instance.List_PlayerQuests[curNpcQuestManager.QuestGroup_Get[i]]);

                }
            }
            else
            {
                Quest tmp = InitSources.Instance.GetQuestByID(curNpcQuestManager.QuestGroup_Get[i]);
                if (tmp.PreQuestIds.Count == 0)
                {
                    Quests.Add(tmp);
                }
                else
                {
                    if(tmp.PreRelationship == PreQuestRelationship.and)
                    {
                        for (int j = 0; j < tmp.PreQuestIds.Count; j++)
                        {
                            if (!PlayerQuestManager.Instance.List_PlayerQuests.ContainsKey(tmp.PreQuestIds[j]))
                            {
                                break;
                            }
                            if (PlayerQuestManager.Instance.List_PlayerQuests[tmp.PreQuestIds[j]].questStatus == QuestStatus.Ongoing)
                            {
                                break;
                            }
                            if (j == tmp.PreQuestIds.Count - 1)
                            {
                                Quests.Add(tmp);
                            }
                        }
                    }
                    else if (tmp.PreRelationship == PreQuestRelationship.or)
                    {
                        for (int j = 0; j < tmp.PreQuestIds.Count; j++)
                        {
                            //Debug.Log("<color=yellow>前置任务ID为</color> " + tmp.PreQuestIds[j]);
                            if (PlayerQuestManager.Instance.List_PlayerQuests.ContainsKey(tmp.PreQuestIds[j]))
                            {
                                Quests.Add(tmp);
                            }
                        }
                    }
                }

                //if (!PlayerQuestManager.Instance.List_PlayerQuests.ContainsKey(curNpcQuestManager.QuestGroup_Get[i]))
                //{
                //    if (tmp.PreQuestIds.Count == 0)
                //    {
                //        Quests.Add(tmp);
                //    }
                //    else
                //    {
                //        if (tmp.PreRelationship == PreQuestRelationship.and)
                //        {
                //            for (int j = 0; j < tmp.PreQuestIds.Count; j++)
                //            {
                //                    Debug.Log(tmp.ID+"   "+ tmp.PreQuestIds[j]+"   "+ j);
                //                //if (PlayerQuestManager.Instance.List_PlayerQuests.ContainsKey(tmp.PreQuestIds[j]) && j == tmp.PreQuestIds.Count-1)
                //                //{
                //                //    Quests.Add(tmp);
                //                //}
                //            }
                //        }
                //        else if (tmp.PreRelationship == PreQuestRelationship.or)
                //        {
                //            for (int j = 0; j < tmp.PreQuestIds.Count; j++)
                //            {
                //                //Debug.Log("<color=yellow>前置任务ID为</color> " + tmp.PreQuestIds[j]);
                //                if (PlayerQuestManager.Instance.List_PlayerQuests.ContainsKey(tmp.PreQuestIds[j]))
                //                {
                //                    Quests.Add(tmp);
                //                }
                //            }
                //        }
                //    }
                //}
            }



            //Quests.Add();
        }
        for (int i = 0; i < Quests.Count; i++)
        {
            if (Button_Quest.Count <= i)
            {
                GameObject tmpBtn_Quest = Instantiate(Btn_Prefab_Quest);
                tmpBtn_Quest.transform.SetParent(Panel_QuestSelect.transform);
                Button_Quest.Add(tmpBtn_Quest);
            }
            Quest tmpQuest = Quests[i];
            Button_Quest[i].SetActive(true);
            Button_Quest[i].GetComponent<Button>().onClick.AddListener(tmpQuest.SelectThisQuest);
            Button_Quest[i].GetComponent<Button>().onClick.AddListener(ShowDescription);
            Button_Quest[i].GetComponentInChildren<Text>().text = Quests[i].Name;
        }
        //关闭多余按钮
        for (int i = Quests.Count; i < Button_Quest.Count; i++)
        {
            Button_Quest[i].SetActive(false);
        }
        Quests[0].SelectThisQuest();
        ShowDescription();
    }
}
