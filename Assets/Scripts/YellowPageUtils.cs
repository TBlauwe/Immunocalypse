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
            if (item.GetKey().Equals(key))
            {
                return true;
            }
        }
        return false;
    }

    public static YellowPageItem AddItem(YellowPageComponent yellowPageComponent)
    {
        int id = 0;
        while (KeyExists(yellowPageComponent, id + ""))
        {
            id++;
        }
        return new YellowPageItem(yellowPageComponent, id + "", null);
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
