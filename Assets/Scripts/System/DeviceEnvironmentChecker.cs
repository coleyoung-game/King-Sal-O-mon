using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceEnvironmentChecker : MonoBehaviour
{
    private bool m_IsNetworkCheck = false;
    

    private void CheckInternetState()
    {
        MainSystem.Instance.NetworkState = (int)Application.internetReachability;
        if (MainSystem.Instance.NetworkState == 0)
        {
            Popup_YN t_Popup = (Popup_YN)MainSystem.Instance.PopupController.CreatePopup();
            t_Popup.SetType(Popup_Type.Default);
            //t_Popup.SetData(MainSystem.Instance.LanguageManager.GetString(30), MainSystem.Instance.LanguageManager.GetString(31), MainSystem.Instance.LanguageManager.GetString(32), () => Application.Quit());
            t_Popup.SetData("네트워크 안내", "현재 인터넷이 \r\n 연결되있지 않습니다.", "앱 종료", () => Application.Quit());
        }
        else
            m_IsNetworkCheck = true;
    }

    private void CheckPermission()
    {

    }

    public IEnumerator IE_CheckEnviroment()
    {
        CheckInternetState();
        yield return new WaitUntil(() => m_IsNetworkCheck == true);
    }

}
