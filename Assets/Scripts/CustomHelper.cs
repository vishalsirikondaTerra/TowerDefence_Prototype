using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class CustomHelper
{

    public static void LOG(this string msg, GameObject go = null)
    {
        Debug.Log($"[POC] {msg}", go);
    }
    public static MergeSlot GetSlot(this MergeSlot[] mergers, Merger target)
    {
        if (mergers != null)
        {
            foreach (var m in mergers)
            {
                if (m.Match(target))
                {
                    return m;
                }
            }
        }
        return null;
    }
}
