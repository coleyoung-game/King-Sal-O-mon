using SuperMaxim.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using TMPro;

public enum Popup_Type
{
    None = -1,
    Default = 0, 
    Jelly = 1
}

public abstract class Popup : MonoBehaviour
{
    private Popup_Type m_Type;


    public void SetType(Popup_Type _Type = Popup_Type.Default)
    {
        m_Type = _Type;
    }
    

    public void OpenPopup()
    {
        Messenger.Default.Publish(new Payload_GamePause(true));
        switch (m_Type)
        {
            case Popup_Type.Default:
                break;
            case Popup_Type.Jelly:
                StartCoroutine(IE_JellyOpenEffect());
                break;

        }
    }
    public virtual void ClosePopup()
    {
        MainSystem.Instance.SoundController.PlayOneShot("Select");
        Messenger.Default.Publish(new Payload_GamePause(false));
        Addressables.ReleaseInstance(gameObject);
        MainSystem.Instance.PopupController.PopupStackPop(() => Destroy(gameObject));
    }

    private IEnumerator IE_JellyOpenEffect()
    {
        float t_CurrTime = 0.0f;
        float t_MaxTime = 1.0f;

        while(t_CurrTime < t_MaxTime)
        {
            float radius = (2 * Mathf.PI) / 3;
            float result = Mathf.Pow(2, -10 * t_CurrTime) * Mathf.Sin((t_CurrTime * 10 - 0.75f) * radius) + 1;
            transform.localScale = Vector3.one * result;
            yield return null;
            t_CurrTime += Time.deltaTime;
        }

    }


}
