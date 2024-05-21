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
    none
}

public class SwipeInputManager : MonoBehaviour
{
    public float SWIPE_THRESHOLD = 25f; // 스와이프 인식 범위

    enum Direction // 방향 enum 설정
    {
        up = 1,
        right,
        down,
        left
    }

    Result Gesture_Result = Result.none; // 제스처 결과
    Vector2 StartPoint; // 시작 지점
    Vector2 EndPoint; // 종료 지점
    Vector2 SwipeVector; // 스와이프 벡터
    public Camera MainCamera; // 메인 카메라

    void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // 터치 입력 처리
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            StartPoint = Touchscreen.current.primaryTouch.startPosition.ReadValue();
        }
        if (Touchscreen.current.primaryTouch.press.isPressed && Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
        {
            EndPoint = Touchscreen.current.primaryTouch.position.ReadValue();

            // 스와이프 벡터 계산
            SwipeVector = EndPoint - StartPoint;

            // 스와이프 거리가 설정한 임계값보다 크면 인식
            if (SwipeVector.magnitude > SWIPE_THRESHOLD)
            {
                RecognizeSwipe();
            }
        }
#else
        // 마우스 입력 처리
        if (Input.GetMouseButtonDown(0)) // 마우스 버튼을 눌렀을 때
        {
            StartPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y); // 시작 지점 설정
        }
        if (Input.GetMouseButtonUp(0)) // 마우스 버튼을 뗐을 때
        {
            EndPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y); // 종료 지점 설정

            // 스와이프 벡터 계산
            SwipeVector = EndPoint - StartPoint;

            // 스와이프 거리가 설정한 임계값보다 크면 인식
            if (SwipeVector.magnitude > SWIPE_THRESHOLD)
            {
                RecognizeSwipe();
            }
        }
#endif
    }

    void RecognizeSwipe()
    {
        // 스와이프 벡터의 각도 계산 (라디안을 각도로 변환)
        float angle = Mathf.Atan2(SwipeVector.y, SwipeVector.x) * Mathf.Rad2Deg;

        // 4방향으로 나누기 위해 각도를 -180부터 180까지로 변환
        if (angle < 0)
        {
            angle += 360;
        }

        // 방향 판단
        if (angle > 45 && angle <= 135) // Up
        {
            Gesture_Result = Result.up;
            Debug.Log("스와이프 방향: Up");
        }
        else if (angle > 135 && angle <= 225) // Left
        {
            Gesture_Result = Result.left;
            Debug.Log("스와이프 방향: Left");
        }
        else if (angle > 225 && angle <= 315) // Down
        {
            Gesture_Result = Result.down;
            Debug.Log("스와이프 방향: Down");
        }
        else // Right
        {
            Gesture_Result = Result.right;
            Debug.Log("스와이프 방향: Right");
        }
    }
}