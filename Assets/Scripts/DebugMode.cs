using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

/*
 * やりたいこと
 * どれか一つでもデバッグフラグが立ったらデバッグ中だよと表示させたい
 */


public class DebugMode : MonoBehaviour
{
    private DebugModeList m_debug_mode_list;

    private FieldInfo[] infoArray;
    private List<string> debug_list = new List<string>();

    private void Start()
    {
        m_debug_mode_list = new DebugModeList();

        FieldInfo[] infoArray = m_debug_mode_list.GetType().GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (FieldInfo info in infoArray)
        {
            Debug.Log(info.Name + ": " + info.GetValue(m_debug_mode_list));
            Debug.Log(info.GetValue(m_debug_mode_list) + ": " + info.GetValue(m_debug_mode_list).GetType());
            object temp = info.GetValue(m_debug_mode_list);
            
            if(temp == "false")
            {
                Debug.Log(temp);
            }
        }

    }

    private void Update()
    {
        if (debug_list.Count == 0)
        {
            //foreach (FieldInfo info in infoArray)
            //{
            //    Debug.Log(info.GetValue(this));
            ////  info.GetValue(this)をbool値にしたい
            //    bool temp = System.Convert.ToBoolean(info.GetValue(this));
            //    if (temp)
            //    {
            //        debug_list.Add(info.Name);
            //    }
            //}
        }
        else
        {
            //foreach (FieldInfo info in infoArray)
            //{
            //    //if (debug_list.Contains(info.Name))
            //    //{
            //    //    bool temp = System.Convert.ToBoolean(info.GetValue(this));

            //    //    if (!temp)
            //    //    {
            //    //        debug_list.Remove(info.Name);
            //    //    }
            //    //}
            //}

            DrawDebugMode();
        }
    }

    void DrawDebugMode()
    {
        Debug.Log("debugモードだよ");
    }
}
