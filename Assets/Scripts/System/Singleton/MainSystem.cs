using SuperMaxim.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;
using UnityEngine.AddressableAssets;

public class MainSystem : SingletonT<MainSystem>
{
    [SerializeField] private LoadSceneManager m_LoadSceneManager;
    [SerializeField] private AssetController m_AssetController;
    [SerializeField] private SoundController m_SoundController;
    [SerializeField] private PopupController m_PopupController;
    [SerializeField] private LanguageManager m_LanguageManager;
    [SerializeField] private DeviceEnvironmentChecker m_DeviceEnvironmentChecker;
    [SerializeField] private bool m_IsUseAddressable;
    private int m_LanguageIdx = -1;

    private Camera[] m_HandleCameras;
    
    private int m_QualitySettingsIdx = 0;
    private int m_ManipulationIdx = 0;
    private bool m_IsHighFps = false;
    private bool m_IsPause = false;

    public bool IsDeviceEnvironmentCheck = false;
    public bool IsUseAddressable { get { return m_IsUseAddressable; }}
    public SoundController SoundController { get { return m_SoundController; } }
    public PopupController PopupController { get { return m_PopupController; } }
    public LanguageManager LanguageManager { get { return m_LanguageManager; } }
    public float CurrMusicVolume { get { return m_SoundController.Audio_Music.volume; } set { m_SoundController.SetAudioVolume(0, value); } 
    }
    public float CurrSfxVolume { get { return m_SoundController.Audio_Sfx.volume; } set { m_SoundController.SetAudioVolume(1, value); } }
    /// <summary>
    /// 0 : Not Connected, 1 : LTE/5G, 2 : Wifi
    /// </summary>
    public int NetworkState { get; set; }
    public int QualitySettingsIdx { get { return m_QualitySettingsIdx; } 
        set
        {
            m_QualitySettingsIdx = value;
            QualitySettings.SetQualityLevel(m_QualitySettingsIdx);
        }
    }
    public int ManipulationIdx { get { return m_ManipulationIdx; }  set { m_ManipulationIdx = value; } }
    public int LanguageIdx { get { return m_LanguageIdx; } }
    public bool IsHighFps { get { return m_IsHighFps; } 
        set 
        { 
            m_IsHighFps = value;
            Application.targetFrameRate = m_IsHighFps ? 60 : 30;
        } 
    }
    public bool IsPause { get { return m_IsPause; } 
        private set 
        {
            m_IsPause = value;
        } 
    }



    private void Awake()
    {
        if (m_Instance != this && m_Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        m_HandleCameras = new Camera[1] { m_PopupController.POPUP_CAMERA };
    }
    private void OnEnable()
    {
        Messenger.Default.Subscribe<Payload_SetOverlayCamera>(SetOverlayCamera, SetOverlayCamera_Predicate);
        Messenger.Default.Subscribe<Payload_GamePause>(OnGamePause);
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.5f);
#if UNITY_ANDROID && !UNITY_EDITOR
        if (m_LanguageIdx == -1)
        {
            if (Application.systemLanguage == SystemLanguage.Korean)
                m_LanguageIdx = 0;
            else if (Application.systemLanguage == SystemLanguage.English)
                m_LanguageIdx = 1;
        }
#elif UNITY_EDITOR
        m_LanguageIdx = 0;
#endif
        //m_LanguageManager.LoadText(false, m_LanguageIdx);
        //yield return m_DeviceEnvironmentChecker.IE_CheckEnviroment();
        //m_AssetController.CheckAssets();
        //yield return new WaitUntil(() => m_AssetController.IsLoaded == true);
        //m_LanguageManager.LoadText(true, m_LanguageIdx);
        //yield return new WaitUntil(() => m_LanguageManager.IsLoaded == true);
        //m_DeviceEnvironmentChecker = null;
        //m_AssetController = null;
        //Messenger.Default.Publish(new Payload_LoadScene(1));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            CheckAssetPopup();
        //if(Input.GetKeyDown(KeyCode.S))
        //{
        //    m_LanguageManager.LoadText(true, 0);
        //}
    }
    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<Payload_SetOverlayCamera>(SetOverlayCamera);
        Messenger.Default.Unsubscribe<Payload_GamePause>(OnGamePause);
    }

    private void SetOverlayCamera(Payload_SetOverlayCamera _Payload)
    {
        var t_Camera = _Payload.MainCamera.GetUniversalAdditionalCameraData();
        int t_Count = m_HandleCameras.Length;
        for (int i = 0; i < t_Count; i++)
            t_Camera.cameraStack.Add(m_HandleCameras[i]);
    }
    private bool SetOverlayCamera_Predicate(Payload_SetOverlayCamera _Payload)
    {
        if (_Payload.MainCamera == null)
            return false;
        else
            return true;
    }

    private void OnGamePause(Payload_GamePause _Payload)
    {
        IsPause = _Payload.IsPause;
    }


    private void CheckAssetPopup()
    {
        //Popup_YN t_Popup = (Popup_YN)Instance.PopupController.CreatePopup_ADR("Popup");
        Popup_YN t_Popup = Instance.PopupController.CreatePopup<Popup_YN>("Popup");
        if (t_Popup == null)
        {
            Debug.LogError($"[Mainsystem]::::[CheckAssetPopup]::::popup is null");
            return;
        }
        t_Popup.SetType(Popup_Type.Jelly);
        t_Popup.OpenPopup();
        t_Popup.SetData("Load Scene Test", "테스트 팝업 \n 테스트 팝업", "Load", ()=> Messenger.Default.Publish(new Payload_LoadScene(1)));
    }

    public void ChangeLanguage(int _LanguageIdx, Action _Act_SetApply = null)
    {
        m_LanguageIdx = _LanguageIdx;
        m_LanguageManager.LoadText(true, m_LanguageIdx, _Act_SetApply);
    }

}
