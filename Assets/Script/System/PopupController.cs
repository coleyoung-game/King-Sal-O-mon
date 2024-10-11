using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PopupController : MonoBehaviour
{
    [SerializeField] private GameObject m_PopupHolder;
    [SerializeField] private Camera m_PopupCamera;
    [SerializeField] private Popup m_LocalPopup;
    [SerializeField] private int m_MaxActiveCount = 4;

    private object m_Lock = new object();
    private Stack<Popup> m_PopupStack = new Stack<Popup>();
    private BitVector32 m_BitVector;
    private List<BitVector32.Section> m_SectionList;

    public GameObject POPUP_HOLDER { get { return m_PopupHolder; } }
    public Camera POPUP_CAMERA { get { return m_PopupCamera; } }

    private bool CheckPopupState() { return m_PopupStack.Count < m_MaxActiveCount; }

    /// <summary>
    /// Local Load
    /// </summary>
    public Popup CreatePopup()
    {
        if (!CheckPopupState())
        {
            Debug.LogError($"[PopupController]::::[CreatePopup_ADR]::::PopupActivateCount is maximum.");
            return null;
        }
        if (m_PopupStack.Count == 0)
            POPUP_HOLDER.SetActive(true);
        Popup popup = Instantiate(m_LocalPopup, POPUP_HOLDER.transform).GetComponent<Popup>();
        m_PopupStack.Push(popup);
        return popup;
    }
    /// <summary>
    /// Resources Load
    /// </summary>
    /// <param name="_PopupName"></param>
    public T CreatePopup<T>(string _PopupName) where T : Popup
    {
        if (!CheckPopupState())
        {
            Debug.LogError($"[PopupController]::::[CreatePopup_ADR]::::PopupActivateCount is maximum.");
            return null;
        }
        if (m_PopupStack.Count == 0)
            POPUP_HOLDER.SetActive(true);
        T t_PopupObj = Instantiate(Resources.Load<GameObject>(ResourceURL.PopupURL + _PopupName), POPUP_HOLDER.transform).GetComponent<T>();
        if (t_PopupObj == null)
        {
            Debug.LogError($"[PopupController]::::[CreatePopup_ADR]::::popup({_PopupName}) is null.");
            return null;
        }
        //Popup t_Popup = t_PopupObj.GetComponent<Popup>();
        m_PopupStack.Push(t_PopupObj);
        return t_PopupObj;
    }
    /// <summary>
    /// Addressable Load
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_PopupName"></param>
    /// <returns></returns>
    public T CreatePopup_ADR<T>(string _PopupName) where T : Popup
    {
        if (!CheckPopupState())
        {
            Debug.LogError($"[PopupController]::::[CreatePopup_ADR]::::PopupActivateCount is maximum.");
            return null;
        }
        if (m_PopupStack.Count == 0)
            POPUP_HOLDER.SetActive(true);
        T t_PopupObj = Addressables.InstantiateAsync(_PopupName, POPUP_HOLDER.transform).WaitForCompletion().GetComponent<T>();
        if (t_PopupObj == null)
        {
            Debug.LogError($"[PopupController]::::[CreatePopup_ADR]::::popup({_PopupName}) is null.");
            return null;
        }
        //Popup t_Popup = t_PopupObj.GetComponent<Popup>();
        m_PopupStack.Push(t_PopupObj);
        return t_PopupObj;
    }

    public void PopupStackPop(Action _Action = null)
    {
        m_PopupStack.Pop();
        if (m_PopupStack.Count < 1)
            POPUP_HOLDER.SetActive(false);
        _Action?.Invoke();
    }

}
