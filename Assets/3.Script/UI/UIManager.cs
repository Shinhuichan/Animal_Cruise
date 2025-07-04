using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 패널들
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject volumePanel;
    public GameObject keySettingPanel;

    // 로그인 화면 UI
    public Button btnLogin;
    public Button btnRegister;
    public Button btnQuit;
    public InputField inputId;
    public InputField inputPw;
    public Text txtLoginMessage;

    // 회원가입 화면 UI
    public Button btnCheckDuplicate;
    public Button btnConfirmRegister;
    public Text txtDuplicateResult;

    // 회원가입 화면 InputField
    public InputField registerInputId;
    public InputField registerInputPw;


    // 메인 화면 UI
    public Button btnStartGame;
    public Button btnSettings;
    public Button btnLogout;

    // 환경설정 UI
    public Button btnVolume;
    public Button btnKeySetting;
    public Button btnSettingsBack;

    // 볼륨/키 세팅 UI
    public Button btnVolumeBack;
    public Button btnKeyBack;

    private string currentUserId = "";

    public void OnLoginClicked()
    {
        string id = inputId.text.Trim();
        string pw = inputPw.text.Trim();
        txtLoginMessage.text = "";

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
        {
            txtLoginMessage.text = "아이디와 비밀번호를 모두 입력하세요.";
            return;
        }

        if (IsValidUser(id, pw))
        {
            currentUserId = id;
            ShowOnly(mainPanel);
        }
        else
        {
            txtLoginMessage.text = "아이디 또는 비밀번호가 틀렸습니다.";
        }
    }

    public void OnLogoutClicked()
    {
        currentUserId = "";
        inputId.text = "";
        inputPw.text = "";
        ShowOnly(loginPanel);
    }

    public void OnRegisterClicked()
    {
        ShowOnly(registerPanel);
    }

    public void OnSettingsClicked()
    {
        ShowOnly(settingsPanel);
    }

    public void OnVolumeClicked()
    {
        ShowOnly(volumePanel);
    }

    public void OnKeySettingClicked()
    {
        ShowOnly(keySettingPanel);
    }

    public void OnSettingsBackClicked()
    {
        ShowOnly(mainPanel);
    }

    public void OnVolumeBackClicked()
    {
        ShowOnly(settingsPanel);
    }

    public void OnKeyBackClicked()
    {
        ShowOnly(settingsPanel);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    // 추가: 중복 아이디 확인 버튼 이벤트 함수
    public void OnCheckDuplicateClicked()
{
    string idToCheck = registerInputId.text.Trim();  // 수정
    if (string.IsNullOrEmpty(idToCheck))
    {
        txtDuplicateResult.text = "아이디를 입력하세요.";
        return;
    }

    bool isDuplicate = SQLManager.instance.CheckDuplicate(idToCheck);
    txtDuplicateResult.text = isDuplicate ? "이미 존재하는 아이디입니다." : "사용 가능한 아이디입니다.";
}

public void OnConfirmRegisterClicked()
{
    string id = registerInputId.text.Trim();  // 수정
    string pw = registerInputPw.text.Trim();  // 수정

    if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
    {
        txtDuplicateResult.text = "아이디와 비밀번호를 모두 입력하세요.";
        return;
    }

    bool success = SQLManager.instance.Register(id, pw);
    txtDuplicateResult.text = success ? "회원가입 성공!" : "회원가입 실패. 다시 시도하세요.";
    if (success)
    {
        ShowOnly(loginPanel);
    }
}


    void ShowOnly(GameObject target)
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        volumePanel.SetActive(false);
        keySettingPanel.SetActive(false);

        if (target != null)
            target.SetActive(true);
    }

    bool IsValidUser(string id, string pw)
    {
        return SQLManager.instance.Login(id, pw);
    }
}
