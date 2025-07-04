using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logincontroller : MonoBehaviour
{
    public InputField id_i;
    public InputField Password_i;

    [SerializeField] private Text log;

    public GameObject lobbyPanel;    // 로그인 성공 후 보여줄 패널
    public GameObject loginPanel;    // 현재 로그인 패널

    public void LoginBtn()
    {
        if (id_i.text.Equals(string.Empty) || Password_i.text.Equals(string.Empty))
        {
            log.text = "아이디와 비밀번호를 입력하세요";
            return;
        }

        if (SQLManager.instance.Login(id_i.text, Password_i.text))
        {
            User_info info = SQLManager.instance.info;
            Debug.Log($"{info.UID} 로그인 성공");

            // 로그인 패널 닫고 로비 패널 열기
            loginPanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }
        else
        {
            log.text = "아이디 또는 비밀번호가 일치하지 않습니다.";
        }
    }
}
