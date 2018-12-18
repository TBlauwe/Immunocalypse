using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class YellowPageItem
{
    public string key;
    public GameObject sourceObject;
    public readonly YellowPageComponent parentPage;


    public YellowPageItem(YellowPageComponent parentPage, string key, GameObject sourceObject)
    {
        this.parentPage = parentPage;
        if (this.SetKey(key) && this.SetSourceObject(sourceObject))
        {
            //this.parentPage.items.Add(this);
        }
    }

    public string GetKey()
    {
        return this.key;
    }

    public GameObject GetSourceObject()
    {
        return this.sourceObject;
    }

    public YellowPageComponent GetParentPage()
    {
        return this.parentPage;
    }

    public bool SetKey(string key)
    {
        //if (!YellowPageUtils.KeyExists(this.parentPage, key))
        //{
        this.key = key;
        return true;
        //}
        //return false;
    }

    public bool SetSourceObject(GameObject sourceObject)
    {
        if (sourceObject != null)
        {
            //string oldObjectKey = YellowPageUtils.GetKey(this.parentPage, sourceObject);
            //if (oldObjectKey != null)
            //{
            return false;
            //}
        }
        this.sourceObject = sourceObject;
        return true;
    }
}