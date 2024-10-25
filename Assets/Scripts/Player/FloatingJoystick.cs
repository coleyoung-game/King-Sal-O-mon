using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class FloatingJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
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
        canvas = GetComponent<Canvas>(); // 캔버스 가져오기 (수정)
        //frame.gameObject.SetActive(false); // 초기에는 비활성화
    }

    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 또는 터치 입력 처리
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector2 screenPosition = Input.mousePosition;
            if (Input.touchCount > 0)
            {
                screenPosition = Input.GetTouch(0).position;
            }
            ShowJoystick(screenPosition);
        }
    }

    // 조이스틱을 화면에 표시하고 위치 설정
    private void ShowJoystick(Vector2 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.worldCamera,
            out Vector2 canvasPosition
        );

        // 조이스틱 위치 설정 (캔버스 기준)
        transform.localPosition = canvasPosition;
        //frame.gameObject.SetActive(true);
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
        //frame.gameObject.SetActive(false); // 터치 종료 시 비활성화
        SetJoystickColor(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        initialTouchPos = GetInputPosition(eventData);
        // frame.localPosition = initialTouchPos;  // 이 줄을 제거합니다.
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