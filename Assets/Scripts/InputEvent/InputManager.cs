#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.InputSystem;
#else
using System.Collections;
#endif

using System.Collections.Generic;
using UnityEngine;

public enum Result
{
    up = 0,
    right,
    down,
    left,
    click,
    none
}

public class InputManager : MonoBehaviour
{
    public float SwipeThreshold; // �������� �ν� ����
    public Result Gesture_Result { get; private set; } = Result.none; // ����ó ����� ������Ƽ�� ����

    private Vector2 StartPoint; // ���� ����
    private Vector2 EndPoint; // ���� ����
    private Vector2 SwipeVector; // �������� ����

    void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // ��ġ �Է� ó��
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            StartPoint = Touchscreen.current.primaryTouch.startPosition.ReadValue();
        }
        if (Touchscreen.current.primaryTouch.press.isPressed && Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
        {
            EndPoint = Touchscreen.current.primaryTouch.position.ReadValue();

            // �������� ���� ���
            SwipeVector = EndPoint - StartPoint;

            // �������� �Ÿ��� ������ �Ӱ谪���� ũ�� �ν�
            if (SwipeVector.magnitude > SwipeThreshold)
            {
                RecognizeSwipe();
            }
            else
            {
                Gesture_Result = Result.click; // Ŭ�� �ν�
            }
        }
#else
        // ���콺 �Է� ó��
        if (Input.GetMouseButtonDown(0)) // ���콺 ��ư�� ������ ��
        {
            StartPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y); // ���� ���� ����
        }
        if (Input.GetMouseButtonUp(0)) // ���콺 ��ư�� ���� ��
        {
            EndPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y); // ���� ���� ����

            // �������� ���� ���
            SwipeVector = EndPoint - StartPoint;

            // �������� �Ÿ��� ������ �Ӱ谪���� ũ�� �ν�
            if (SwipeVector.magnitude > SwipeThreshold)
            {
                RecognizeSwipe();
            }
            else
            {
                Gesture_Result = Result.click; // Ŭ�� �ν�
            }
        }
#endif
    }

    void RecognizeSwipe()
    {
        // �������� ������ ���� ��� (������ ������ ��ȯ)
        float angle = Mathf.Atan2(SwipeVector.y, SwipeVector.x) * Mathf.Rad2Deg;

        // 4�������� ������ ���� ������ -180���� 180������ ��ȯ
        if (angle < 0)
        {
            angle += 360;
        }

        // ���� �Ǵ�
        if (angle > 45 && angle <= 135) // Up
        {
            Gesture_Result = Result.up;
            Debug.Log("�������� ����: Up");
        }
        else if (angle > 135 && angle <= 225) // Left
        {
            Gesture_Result = Result.left;
            Debug.Log("�������� ����: Left");
        }
        else if (angle > 225 && angle <= 315) // Down
        {
            Gesture_Result = Result.down;
            Debug.Log("�������� ����: Down");
        }
        else // Right
        {
            Gesture_Result = Result.right;
            Debug.Log("�������� ����: Right");
        }
    }

    public void ResetGestureResult()
    {
        Gesture_Result = Result.none;
    }
}