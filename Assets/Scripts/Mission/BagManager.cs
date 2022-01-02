using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ItemInfoListener(string itemId, int amount);
public class BagManager : MonoBehaviour
{
    public static BagManager Instance;
    public ItemInfoListener OnGetItem;
    public ItemInfoListener OnRemoveItem;
    public List<ItemBase> List_ItemBases = new List<ItemBase>();
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

    }

    public void GetItem(string itemId, int amount)
    {
        foreach (var item in List_ItemBases)
        {
            if (item.ID == itemId)
            {
                item.curCount += amount;
                if (OnGetItem != null)
                    OnGetItem(itemId, amount);
                return;
            }
        }
        ItemBase tmpItem = InitSources.Instance.GetItembaseByID(itemId);
        tmpItem.curCount += amount;
        List_ItemBases.Add(tmpItem);
        if (OnGetItem != null)
        {
            OnGetItem(itemId, amount);
        }
    }
    public void RemoveItem(string itemId, int amount)
    {
        if (List_ItemBases.Count > 0)
        {
            foreach (var item in List_ItemBases)
            {
                if (item.ID == itemId)
                {
                    Debug.Log(item.ID + " hhhhhhhhhhhhhhh   " + amount);
                    item.curCount -= amount;
                    if (OnRemoveItem != null)
                    {
                        OnRemoveItem(itemId, GetItemAmountByID(itemId));
                    }
                    if (item.curCount <= 0)
                    {
                        Debug.Log("here");
                        List_ItemBases.Remove(item);
                        break;
                    }

                }
            }
        }
        else
        {
            Debug.Log("ERROR:Length of List_ItemBases is 0");
        }
    }
    public int GetItemAmountByID(string itemId)
    {
        for (int i = 0; i < List_ItemBases.Count; i++)
        {
            if (List_ItemBases[i].ID == itemId)
            {
                return List_ItemBases[i].curCount;
            }

        }
        return 0;
    }

    public void SimulateGetItem()
    {
        GetItem("1", 1);
    }
}
