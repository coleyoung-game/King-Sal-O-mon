using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using UnityEngine.UI;

public class AssetController : MonoBehaviour
{
    [SerializeField] private string[] m_CheckLabel;
    [SerializeField] private Image m_Img_CurrDownLoading;
    [SerializeField] private Text m_Txt_CurrDownLoading;

 
    private List<string> m_UpdateList;
    private long m_DownloadSize;
    private bool m_IsLoaded = false;
    private bool m_IsLoadAtlas = false;

    
    private WaitForSeconds m_CompleteDownWaitTime = new WaitForSeconds(.5f);
    private IEnumerator IE_CheckAssetsHandle = null;
    private IEnumerator IE_DownloadAssetHandle = null;
    private IEnumerator IE_ClearCacheHandle = null;
    public bool IsLoaded { get { return m_IsLoaded; } }

    private void Start()
    {
        SpriteAtlasManager.atlasRequested += RequestAtlas;
    }
    private async void RequestAtlas(string _Key, Action<SpriteAtlas> _Callback)
    {
        if (m_IsLoadAtlas)
            return;
        var t_LoadAtlas = Addressables.LoadAssetAsync<SpriteAtlas>(_Key);
        await t_LoadAtlas.Task;
        if(t_LoadAtlas.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("[AssetController]::::[RequestAtlas]::::atlas asset load faild.");
            return;
        }
        if(t_LoadAtlas.Result == null)
        {
            Debug.LogError("[AssetController]::::[RequestAtlas]::::atlas asset is null.");
            return;
        }
        _Callback(t_LoadAtlas.Result);
        m_IsLoadAtlas = true;
    }

    public void CheckAssets()
    {
        if (IE_CheckAssetsHandle != null)
            return;
        StartCoroutine(IE_CheckAssetsHandle = IE_CheckAssets()); 
    }



    private IEnumerator IE_CheckUpdate()
    {
        AsyncOperationHandle<List<string>> t_Handle = Addressables.CheckForCatalogUpdates();
        yield return t_Handle;
        m_UpdateList = t_Handle.Result;
    }

    private IEnumerator IE_CheckDownloadSize()
    {
        int t_Count = m_CheckLabel.Length;
        for (int i = 0; i < t_Count; i++)
        {
            AsyncOperationHandle<long> t_Handle = Addressables.GetDownloadSizeAsync(m_CheckLabel[i]);
            yield return t_Handle;
            m_DownloadSize += t_Handle.Result;
        }
    }

    private IEnumerator IE_CheckAssets()
    {
        yield return IE_CheckUpdate();
        if (m_UpdateList == null)
            yield break;
        yield return IE_CheckDownloadSize();
        Popup_YN t_Popup = (Popup_YN)MainSystem.Instance.PopupController.CreatePopup();
        t_Popup.SetType(Popup_Type.Default);
        bool t_Isdownloadable = m_DownloadSize > 0;
        bool t_IsDownload = false;
        bool t_IsUserSelect = false;
        if (t_Isdownloadable)
        {
            if (MainSystem.Instance.NetworkState == 1)
                DataNetworkNoti(() => { t_IsDownload = true; t_IsUserSelect = true; });
            else
                t_Popup.SetData(MainSystem.Instance.LanguageManager.GetString(1), $"{ConvertByte(m_DownloadSize)} {MainSystem.Instance.LanguageManager.GetString(2)} {MainSystem.Instance.LanguageManager.GetString(3)}",
                    MainSystem.Instance.LanguageManager.GetString(100), MainSystem.Instance.LanguageManager.GetString(200), () => { t_IsDownload = true; t_IsUserSelect = true; }, () => { t_IsDownload = false; t_IsUserSelect = true; });
            yield return new WaitUntil(() => t_IsUserSelect == true);
            if (t_IsDownload)
                yield return IE_DownloadAsset();
            else
                yield break;
        }
        else
            t_Popup.SetData(MainSystem.Instance.LanguageManager.GetString(10), MainSystem.Instance.LanguageManager.GetString(11), 
                MainSystem.Instance.LanguageManager.GetString(101), MainSystem.Instance.LanguageManager.GetString(201), () => m_IsLoaded = true, ReleaseAsset);
    }



    private void DataNetworkNoti(Action _AfterAct)
    {
        Popup_YN t_Popup = (Popup_YN)MainSystem.Instance.PopupController.CreatePopup();
        t_Popup.SetType(Popup_Type.Default);
        t_Popup.SetData(MainSystem.Instance.LanguageManager.GetString(1), MainSystem.Instance.LanguageManager.GetString(20), MainSystem.Instance.LanguageManager.GetString(100), MainSystem.Instance.LanguageManager.GetString(200), _AfterAct, () => Application.Quit());
    }

    private IEnumerator IE_DownloadAsset()
    {
        int t_Count = m_CheckLabel.Length;
        for (int i = 0; i < t_Count; i++)
        {
            var t_DownloadCheck = Addressables.DownloadDependenciesAsync(m_CheckLabel[i]);

            while (!t_DownloadCheck.IsDone)
            {
                //Debug.Log($"t_DownloadCheck.PercentComplete_[{i}] : {t_DownloadCheck.PercentComplete}");
                m_Txt_CurrDownLoading.text = $"{(t_DownloadCheck.PercentComplete * 100).ToString("N1")}%";
                m_Img_CurrDownLoading.fillAmount = t_DownloadCheck.PercentComplete;
                yield return null;
            }
            //Addressables.Release(t_DownloadCheck);
        }
        m_Img_CurrDownLoading.fillAmount = 1.0f;
        m_Txt_CurrDownLoading.text = "100%";
        yield return m_CompleteDownWaitTime;
        Popup_YN t_Popup = (Popup_YN)MainSystem.Instance.PopupController.CreatePopup();
        t_Popup.SetType(Popup_Type.Default);
        t_Popup.SetData(MainSystem.Instance.LanguageManager.GetString(10), MainSystem.Instance.LanguageManager.GetString(11), MainSystem.Instance.LanguageManager.GetString(101), MainSystem.Instance.LanguageManager.GetString(201), () => m_IsLoaded = true);
    }

    private void ReleaseAsset()
    {
        if (IE_ClearCacheHandle != null)
            return;
        StartCoroutine(IE_ClearCacheHandle = IE_ClearCache());
    }

    private IEnumerator IE_ClearCache()
    {
        foreach (var tmp in Addressables.ResourceLocators)
        {
            var async = Addressables.ClearDependencyCacheAsync(tmp.Keys, false);
            yield return async;
            Addressables.Release(async);
        }

        Caching.ClearCache();
        Addressables.UpdateCatalogs();
    }

    //**********Utility**********
    private const long c_KB = 1024;
    private const long c_MB = 1048576;
    private const long c_GB = 1073741824;
    private string ConvertByte(long _Byte)
    {
        string t_Result;
        if (_Byte > c_GB)
            t_Result = $"{(_Byte / c_GB).ToString("N2")}GB";
        else if(_Byte > c_MB && _Byte < c_GB)
            t_Result = $"{(_Byte / c_MB).ToString("N2")}MB";
        else if (_Byte > c_KB && _Byte < c_MB)
            t_Result = $"{(_Byte / 1024).ToString("N2")}KB";
        else
            t_Result = $"{_Byte}Bytes";
        return t_Result;
    }


}
