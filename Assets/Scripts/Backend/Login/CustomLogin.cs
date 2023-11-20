using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class CustomLogin : MonoBehaviour
{
    UiManager UiManager => UiManager.Instance;

    private string currentInput = "";

    void Update()
    {
        // 키보드 입력을 받음
        if (Input.anyKeyDown)
        {
            string inputKey = Input.inputString;

            if (inputKey.Length == 1)
            {
                // 단일 문자를 입력하면 현재 입력에 추가
                currentInput += inputKey;
            }
            else if (inputKey == "\b")
            {
                // 백스페이스를 누르면 문자열 초기화
                currentInput = "";
            }

            // 'login' 입력 시
            if (currentInput == "login")
            {
                OnInputLogin();
                currentInput = "";
            }

            // 'admin' 입력 시
            if (currentInput == "admin")
            {
                OnInputAdmin();
                currentInput = "";
            }
        }
    }

    void OnInputLogin()
    {
        // 'login' 입력 시 호출할 동작
        Debug.Log("Login input detected!");

        UiManager.SetStartUi(false);
        UiManager.SetLoginUi(true);
    }

    void OnInputAdmin()
    {
        var bro = Backend.BMember.CustomLogin("pqowi2000", "101010");

        if (bro.IsSuccess())
        {
            UiManager.SetStartUi(false);
            UiManager.SetGameUi(true);
        }
    }
}
