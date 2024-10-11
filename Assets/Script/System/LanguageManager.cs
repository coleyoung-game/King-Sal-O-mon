using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public class LanguageRawData
{
    public string Code;
    public string Text;
}

public class LanguageManager : MonoBehaviour
{
    /// <summary>
    /// 0 : Korean, 1 : English
    /// </summary>
    private readonly string[] c_LANGUAGE_LOCAL_FILEName = new string[2] { "HB_Local_Korean", "HB_Local_English" };
    private readonly string[] c_LANGUAGE_REMOTE_FILEName = new string[2] { "HB_Remote_Korean.json", "HB_Remote_English.json" };
    private Dictionary<int, string> m_Dic_LoadedTxt = new Dictionary<int, string>();

    private Action m_Act_SetApply = null;

    private TextAsset m_TextAsset = null;

    private bool m_IsLoaded = false;

    public bool IsLoaded { get { return m_IsLoaded; } }

    private void OnloadedSetText(AsyncOperationHandle<TextAsset> obj)
    {
        if(!obj.IsValid())
        {
            Debug.LogError("[LanguageManager]::::[LanguageManager_Completed]::::TextAsset is Invalid.");
            return;
        }
        if(obj.Status ==  AsyncOperationStatus.Failed)
        {
            Debug.LogError("[LanguageManager]::::[LanguageManager_Completed]::::TextAsset load failed.");
            return;
        }
        ConvertToDictionary(obj.Result.text);
        if (m_Act_SetApply != null)
        {
            m_Act_SetApply();
            m_Act_SetApply = null;
        }
    }

    private void ConvertToDictionary(string _RawData)
    {
        LanguageRawData[] t_Rawdata = JsonConvert.DeserializeObject<LanguageRawData[]>(_RawData);
        m_Dic_LoadedTxt = new Dictionary<int, string>();
        int t_TempNumber = -1;
        int t_Count = t_Rawdata.Length;
        for (int i = 0; i < t_Count; i++)
        {
            if (int.TryParse(t_Rawdata[i].Code, out t_TempNumber))
            {
                if (!m_Dic_LoadedTxt.ContainsKey(t_TempNumber))
                    m_Dic_LoadedTxt.Add(t_TempNumber, t_Rawdata[i].Text);
                else
                    Debug.LogError($"[LanguageManager]::::[ConvertToDictionary]::::Key({t_TempNumber}) is already added.");
            }
            else
            {
                Debug.LogError("[LanguageManager]::::[ConvertToDictionary]::::Rawdata Code is Invalid.");
            }
        }
        m_IsLoaded = true;
        m_TextAsset = null;

    }

    public void LoadText(bool _IsRemote, int _Idx, Action _Act_SetApply = null)
    {
        if (_Idx > c_LANGUAGE_REMOTE_FILEName.Length - 1 || _Idx < 0)
        {
            Debug.LogError("[LanguageManager]::::[LoadText]::::_Idx is invalid.");
            return;
        }
        if (_IsRemote)
        {
            m_Act_SetApply = _Act_SetApply;
            Addressables.LoadAssetAsync<TextAsset>(c_LANGUAGE_REMOTE_FILEName[_Idx]).Completed += OnloadedSetText;
        }
        else
        {
            TextAsset t_TestAsset = Resources.Load<TextAsset>(ResourceURL.TextAssetURL + "/" + c_LANGUAGE_LOCAL_FILEName[_Idx]);
            if (t_TestAsset == null)
            {
                Debug.LogError("[LanguageManager]::::[LoadText]::::t_TestAsset is null");
                return;
            }
            ConvertToDictionary(t_TestAsset.text);
            _Act_SetApply?.Invoke();
        }
    }


    public string GetString(int _Code)
    {
        if (!m_Dic_LoadedTxt.ContainsKey(_Code))
        {
            Debug.LogError($"[LanguageManager]::::[GetString]::::Key({_Code}) is invalid.");
            return "";
        }
        return m_Dic_LoadedTxt[_Code];
    }
}
