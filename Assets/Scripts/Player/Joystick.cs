using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Canvas canvas; // UI 캔버스 참조
    public RectTransform frame;
    public RectTransform handle;

    private float handleRange = 130;
    private Vector3 input;
    private Vector2 initialTouchPos;

    public float Horizontal { get { return input.x; } }
    public float Vertical { get { return input.y; } }

    void Start()
    {
        canvas = FindObjectOfType<Canvas>(); // 캔버스 찾기
        this.transform.localPosition = Vector3.zero;
        frame.localPosition = Vector3.zero;
    }
    void Update()
    {
        /*
        // 마우스 왼쪽 버튼 클릭
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            SetJoystickPosition(mousePosition);
        }

        // 터치 입력
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;
            SetJoystickPosition(touchPosition);
        }
      */
    }

    private void SetJoystickPosition(Vector2 screenPosition)
    {
        // 스크린 좌표를 캔버스 좌표계로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.worldCamera,
            out Vector2 canvasPosition
        );

        // 조이스틱 위치 설정
        transform.localPosition = canvasPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("2");
        Vector2 localVector = GetInputPosition(eventData) - initialTouchPos;

        if (localVector.magnitude < handleRange)
        {
            handle.localPosition = localVector;
        }
        else
        {
            handle.localPosition = localVector.normalized * handleRange;
        }

        input = localVector;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.localPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("1");
        initialTouchPos = GetInputPosition(eventData);
        handle.localPosition = Vector2.zero;
        OnDrag(eventData);
    }

    private Vector2 GetInputPosition(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(frame, eventData.position, eventData.pressEventCamera, out localPoint);
        return localPoint;
    }
}