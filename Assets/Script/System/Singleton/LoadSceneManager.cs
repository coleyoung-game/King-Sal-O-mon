using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using SuperMaxim.Messaging;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class LoadSceneManager : SingletonT<LoadSceneManager>
{
    [SerializeField] private Camera m_SceneCamera;
    [SerializeField] private GameObject m_BackGround;
    [SerializeField] private Image m_LoadingProgressImg;
    [SerializeField] private Text m_LoadingProgressTxt;
    [SerializeField] private string[] m_SceneNames;
    [SerializeField] private bool m_UseAddressable;

    private IEnumerator IE_LoadingScene_AddressableHandle = null;

    public Camera SceneCamera { get { return m_SceneCamera; } }
    public int LastSceneLoadIdx { get; set; }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<Payload_LoadScene>(OnLoadScene, OnLoadScene_Predicate);
    }
    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<Payload_LoadScene>(OnLoadScene);
    }

    private void OnLoadScene(Payload_LoadScene _Payload)
    {
        //Debug.Log("[LoadSceneManager]::::OnLoadScene");
        if (m_UseAddressable)
        {
            AsyncOperationHandle<SceneInstance> t_LoadScene = Addressables.LoadSceneAsync(m_SceneNames[_Payload.SceneIdx], _Payload.IsAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            if (IE_LoadingScene_AddressableHandle != null)
                return;
            StartCoroutine(IE_LoadingScene_AddressableHandle = IE_LoadingScene_Addressable(t_LoadScene, _Payload.SceneIdx));
        }
        else
        {
            AsyncOperation loadScene = SceneManager.LoadSceneAsync(m_SceneNames[_Payload.SceneIdx], _Payload.IsAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            StartCoroutine(IE_LoadingScene(loadScene, _Payload.SceneIdx));
        }
    }
    private bool OnLoadScene_Predicate(Payload_LoadScene _Payload)
    {
        if (_Payload.SceneIdx < 0 || _Payload.SceneIdx > m_SceneNames.Length - 1 || LastSceneLoadIdx == _Payload.SceneIdx)
            return false;
        else
            return true;
    }
    private IEnumerator IE_LoadingScene(AsyncOperation loadScene, int _LoadSceneIdx)
    {
        m_LoadingProgressImg.fillAmount = 0;
        m_BackGround.SetActive(true);
        while (!loadScene.isDone)
        {
            m_LoadingProgressImg.fillAmount = loadScene.progress;
            m_LoadingProgressTxt.text = (loadScene.progress * 100).ToString() + "%"; 
            yield return new WaitForEndOfFrame();
        }
        m_LoadingProgressImg.fillAmount = 1;
        m_LoadingProgressTxt.text = "100%";
        yield return new WaitForSeconds(.5f);
        LastSceneLoadIdx = _LoadSceneIdx;
        m_BackGround.SetActive(false);
    }

    private IEnumerator IE_LoadingScene_Addressable(AsyncOperationHandle<SceneInstance> _AsyncInstance, int _LoadSceneIdx)
    {
        //Debug.Log("[LoadSceneManager]::::IE_LoadingScene_Addressable");
        m_LoadingProgressImg.fillAmount = 0;
        m_BackGround.SetActive(true);
        while (!_AsyncInstance.IsDone)
        {
            m_LoadingProgressImg.fillAmount = _AsyncInstance.PercentComplete;
            m_LoadingProgressTxt.text = (_AsyncInstance.PercentComplete * 100).ToString("N2") + "%";
            yield return new WaitForEndOfFrame();
        }
        m_LoadingProgressImg.fillAmount = 1;
        m_LoadingProgressTxt.text = "100%";
        yield return new WaitForSeconds(.5f);

        m_BackGround.SetActive(false);
        LastSceneLoadIdx = _LoadSceneIdx;
        IE_LoadingScene_AddressableHandle = null;
    }


}
