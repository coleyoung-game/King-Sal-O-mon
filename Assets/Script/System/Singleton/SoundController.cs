using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource m_Audio_Music;
    [SerializeField] private AudioSource m_Audio_Sfx;

    private Dictionary<string, AudioClip> m_Dic_LocalClips = new Dictionary<string, AudioClip>();

    public AudioSource Audio_Music { get { return m_Audio_Music; } }
    public AudioSource Audio_Sfx { get { return m_Audio_Sfx; } }


    private void Start()
    {
        LocalSFXLoad();
    }


    /// <summary>
    /// 0 : Music 
    /// 1 : Sfx
    /// </summary>
    public int SetAudioVolume(int _Type, float _Volume)
    {
        switch(_Type)
        {
            case 0:
                m_Audio_Music.volume = _Volume;
                break;
            case 1:
                m_Audio_Sfx.volume = _Volume;
                break;
            default:
                return -1;
        }
        return 0;
    }

    public void LocalSFXLoad()
    {
        AudioClip[] t_Clip = Resources.LoadAll<AudioClip>("Sound");
        int t_Count = t_Clip.Length;
        for (int i = 0; i < t_Count; i++)
            m_Dic_LocalClips.Add(t_Clip[i].name, t_Clip[i]);
    }

    


    /// <summary>
    /// local load
    /// </summary>
    /// <param name="_Name"></param>
    public void PlayOneShot(string _Name)
    {
        //AudioClip t_Clip = Resources.Load<AudioClip>($"Sound/{_Name}");
        //m_Audio_Sfx.PlayOneShot(t_Clip);
        if(!m_Dic_LocalClips.TryGetValue(_Name, out AudioClip clip))
        {
            Debug.LogError($"SoundController::::PlayOneShot::::\"{_Name}\" audiosource is not found.");
            return;
        }
        m_Audio_Sfx.PlayOneShot(m_Dic_LocalClips[_Name]);

    }

    public void PlayMusic(string _Name)
    {
        Addressables.LoadAssetAsync<AudioClip>(_Name).Completed += PlayBackgroundMusic;
    }



    // TODO: Addressable에서 Load하고 실행하니 약간의 딜레이가 발생함. 캐싱 후 실행시키는 방향으로 나아가야 함.
    // 저장 방식 고민 필요(string? int?)
    public void PlaySFX(string _Name)
    {
        Addressables.LoadAssetAsync<AudioClip>(_Name).Completed += PlaySFXSound;
    }
    // TODO: Fade in, Fade Out 효과 개발
    private void PlayBackgroundMusic(AsyncOperationHandle<AudioClip> obj)
    {
        if(Audio_Music.clip == obj.Result)
        {
            Debug.LogError("SoundController::::PlayerBackgroundMusic::::Same Music is already play.");
            return;
        }    
        Audio_Music.clip = obj.Result;
        Audio_Music.Play();
    }
    private void PlaySFXSound(AsyncOperationHandle<AudioClip> obj)
    {
        //Debug.Log($"Play Sound::::Name : {obj.Result.name}");
        Audio_Sfx.PlayOneShot(obj.Result);
    }

    // 테스트 및 검증 필요
    public void LoadSFXSource()
    {
        Addressables.LoadResourceLocationsAsync("SFX").Completed += SoundController_Completed;
    }

    private void SoundController_Completed(AsyncOperationHandle<IList<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation>> obj)
    {
        Addressables.LoadAssetsAsync<AudioClip[]>(obj.Result, LoadSFXAsset);
    }

    private void LoadSFXAsset(AudioClip[] _AudioClip)
    {

    }

}
