using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemBase
{
    public string ID;
    public string Name;
    public string Description;
    //public int maxCount;
    public int curCount;
    public ItemTypes ItemType;

    public ItemBase()
    {
        ID = "None";
        Name = "None";
        Description = "None";
        //maxCount = 5;
        curCount = 0;
        ItemType = ItemTypes.Medicine;
    }
}
public enum ItemTypes
{
    Medicine,
    Weapon,
    Armor,
    Others
}
[System.Serializable]
public class QuestReward
{
    [SerializeField]
    private int mMoney;
    public int Money
    {
        get
        {
            return mMoney;
        }
        set
        {
            mMoney = value;
        }
    }

    [SerializeField]
    private int mEXP;
    public int EXP
    {
        get
        {
            return mEXP;
        }
        set
        {
            mEXP = value;
        }
    }

    [SerializeField]
    private List<RewardItemBase> mItems = new List<RewardItemBase>();
    public List<RewardItemBase> Items
    {
        get
        {
            return mItems;
        }
        set
        {
            mItems = value;
        }
    }

}
[System.Serializable]
public class RewardItemBase
{
    public ItemBase RewardItem;
    public int Amount;
}