using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective
{
    [SerializeField]
    string mID;
    public string ID
    {
        get
        {
            return mID;
        }
        set
        {
            mID = value;
        }
    }
    [SerializeField]
    string mDescription;
    public string Description
    {
        get
        {
            return mDescription;
        }
        set
        {
            mDescription = value;
        }
    }
    [SerializeField]
    int mMaxAmount;
    public int MaxAmount
    {
        get
        {
            return mMaxAmount;
        }
        set
        {
            mMaxAmount = value;
        }
    }
    [SerializeField]
    int mCurAmount;
    public int CurAmount
    {
        get
        {
            return mCurAmount;
        }
        set
        {
            mCurAmount = value;
        }
    }
    public bool IsCompleted
    {
        get
        {
            if (mCurAmount >= MaxAmount)
            {
                Debug.Log("mCurAmount " + mCurAmount + " MaxAmount " + MaxAmount);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public virtual void UpdateStatus()
    {
        if (IsCompleted) return;
        CurAmount += 1;
        QuestAcceptPanelManager.Instance.ShowDescription();
    }

}
[System.Serializable]
public class CollectObjective : Objective
{
    [SerializeField]
    string mNeededItemID;
    public string NeededItemID
    {
        get
        {
            return mNeededItemID;
        }
        set
        {
            mNeededItemID = value;
        }
    }
    
    public void UpdateCollectAmountUp(string itemID,int amount)
    {
        if (itemID == mNeededItemID)
        {
            for (int i = 0; i < amount; i++)
            {
                UpdateStatus();
            }
        }
    }
    public void UpdateCollectAmountDown(string itemID, int amount)
    {
        if (itemID == mNeededItemID)
        {
            CurAmount = amount;
        }
    }
}
