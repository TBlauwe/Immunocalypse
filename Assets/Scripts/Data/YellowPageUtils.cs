using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class YellowPageUtils
{
    public static string GetKey(YellowPageComponent yellowPageComponent, GameObject sourceObjects)
    {
        foreach (YellowPageItem item in yellowPageComponent.items)
        {
            if (item.GetSourceObject() == null)
            {
                if (sourceObjects == null)
                {
                    item.GetKey();
                }
            }
            else if (item.GetSourceObject().Equals(sourceObjects))
            {
                return item.GetKey();
            }
        }
        return null;
    }

    public static GameObject GetSourceObject(YellowPageComponent yellowPageComponent, string key)
    {
        foreach (YellowPageItem item in yellowPageComponent.items)
        {
            if (item.GetKey().Equals(key))
            {
                return item.GetSourceObject();
            }
        }
        return null;
    }


    public static bool KeyExists(YellowPageComponent yellowPageComponent, string key)
    {
        foreach (YellowPageItem item in yellowPageComponent.items)
        {
            if (item != null && item.GetKey().Equals(key))
            {
                return true;
            }
        }
        return false;
    }

    public static YellowPageItem AddItem(YellowPageComponent yellowPageComponent)
    {
        int id = 0;
        int i = 0;
        while(i < yellowPageComponent.items.Count() && yellowPageComponent.items[i] != null)
        {
            i++;
        }
        if(i < yellowPageComponent.items.Count())
        {
            while (KeyExists(yellowPageComponent, id + ""))
            {
                id++;
            }
            yellowPageComponent.items[i] = new YellowPageItem(yellowPageComponent, id + "", null);
            return yellowPageComponent.items[i];
        }
        return null;
    }

    public static List<string> GetKeys(YellowPageComponent yellowPageComponent)
    {
        List<string> keys = new List<string>();
        foreach (YellowPageItem item in yellowPageComponent.items)
        {
            keys.Add(item.GetKey());
        }
        return keys;
    }

    public static YellowPageItem GetItem(YellowPageComponent yellowPageComponent, string key)
    {
        foreach (YellowPageItem item in yellowPageComponent.items)
        {
            if (item.GetKey().Equals(key))
            {
                return item;
            }
        }
        return null;
    }

    public static List<YellowPageItem> GetItems(YellowPageComponent yellowPageComponent)
    {
        return new List<YellowPageItem>(yellowPageComponent.items);
    }
}
