using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 버튼 1개, 2개 유동적으로 사용 가능
/// </summary>
public class Popup_YN : Popup
{
    [SerializeField] private Text m_TitleTxt;
    [SerializeField] private Text m_DiscTxt;
    [SerializeField] private Button m_AcceptBtn;
    [SerializeField] private Text m_AcceptBtnName;
    [SerializeField] private Button m_CancelBtn;
    [SerializeField] private Text m_CancelBtnName;

    public void SetData(string _Title, string _Disc, string _AcceptBtnName, Action _AcceptAct)
    {
        m_CancelBtn.gameObject.SetActive(false);

        Vector2 t_UIAnchorPos = new Vector2(0.5f, 0);
        RectTransform t_CancelBtnRect = m_AcceptBtn.GetComponent<RectTransform>();
        t_CancelBtnRect.anchorMin = t_UIAnchorPos;
        t_CancelBtnRect.anchorMax = t_UIAnchorPos;
        t_CancelBtnRect.pivot = t_UIAnchorPos;
        t_CancelBtnRect.anchoredPosition = new Vector2(0, 200);
        
        m_TitleTxt.text = _Title;
        m_DiscTxt.text = _Disc;
        m_AcceptBtnName.text = _AcceptBtnName;
        m_AcceptBtn.onClick.AddListener(() =>
        {
            _AcceptAct?.Invoke();
            ClosePopup();
        });
        
    }
    public void SetData(string _Title, string _Disc, string _AcceptBtnName, string _CancelBtnName, Action _AcceptAct, Action _CancelAct = null)
    {
        m_TitleTxt.text = _Title;
        m_DiscTxt.text = _Disc;
        m_AcceptBtnName.text = _AcceptBtnName;
        m_CancelBtnName.text = _CancelBtnName;
        m_AcceptBtn.onClick.AddListener(() =>
        {
            ClosePopup();
            _AcceptAct?.Invoke();
        });
        m_CancelBtn.onClick.AddListener(() => 
        {
            _CancelAct?.Invoke();
            ClosePopup();
        });
    }

}
