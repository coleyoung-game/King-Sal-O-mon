using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Canvas canvas;
    public RectTransform frame;
    public RectTransform handle;

    private float handleRange = 130;
    private Vector3 input;
    private Vector2 initialTouchPos;
    private Vector2 touchPosition;
    private Touch touch;

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
        // 마우스 왼쪽 버튼 클릭
        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
            //SetJoystickPosition(touchPosition);
        }

        // 터치 입력
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;
            //SetJoystickPosition(touchPosition);
        }
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

        transform.localPosition = canvasPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
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
        SetJoystickColor(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.localPosition = Vector2.zero;
        SetJoystickColor(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        initialTouchPos = GetInputPosition(eventData);

        SetJoystickPosition(touchPosition);
        frame.localPosition = initialTouchPos;
        handle.localPosition = Vector2.zero;
        OnDrag(eventData);
    }

    private Vector2 GetInputPosition(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(frame, eventData.position, eventData.pressEventCamera, out localPoint);
        return localPoint;
    }

    private void SetJoystickColor(bool isOnDraged)
    {
        Color pointedFrameColor;
        Color pointedHandleColor;

        if (isOnDraged)
        {
            pointedFrameColor = new Color(1, 0, 0, 0.5f);
            pointedHandleColor = new Color(1, 0, 0, 0.6f);
        }
        else
        {
            pointedFrameColor = new Color(1, 1, 1, 0.5f);
            pointedHandleColor = new Color(1, 1, 1, 0.6f);
        }

        this.frame.gameObject.GetComponent<Image>().color = pointedFrameColor;
        this.handle.gameObject.GetComponent<Image>().color = pointedHandleColor;
    }
}