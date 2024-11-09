using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class FloatingJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Canvas canvas; // UI ĵ���� ����
    public RectTransform frame;
    public RectTransform handle;

    private float handleRange = 130;
    private Vector3 input;
    private Vector2 initialTouchPos;

    public float Horizontal { get { return input.x; } }
    public float Vertical { get { return input.y; } }

    void Start()
    {
        canvas = GetComponent<Canvas>(); // ĵ���� �������� (����)
        //frame.gameObject.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
    }

    void Update()
    {
        // ���콺 ���� ��ư Ŭ�� �Ǵ� ��ġ �Է� ó��
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

    // ���̽�ƽ�� ȭ�鿡 ǥ���ϰ� ��ġ ����
    private void ShowJoystick(Vector2 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.worldCamera,
            out Vector2 canvasPosition
        );

        // ���̽�ƽ ��ġ ���� (ĵ���� ����)
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
        //frame.gameObject.SetActive(false); // ��ġ ���� �� ��Ȱ��ȭ
        SetJoystickColor(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        initialTouchPos = GetInputPosition(eventData);
        // frame.localPosition = initialTouchPos;  // �� ���� �����մϴ�.
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