using System.Collections;
using CustomInspector;
using UnityEngine;
using UnityEngine.UI;

public class KeySettingData : MonoBehaviour
{
    public BindingType type;

    [SerializeField] private Text Title_t;
    [SerializeField] public UnityEngine.UI.Button btn;
    [SerializeField] private Text btn_text;
    [SerializeField] private Image btn_img;

    [ReadOnly] public KeySettingManager keymanager;

    private KeySettingcontroller keycontroller;
    private Coroutine co;
    public bool isstartCo = false;
    void OnEnable()
    {
        keycontroller = GetComponentInParent<KeySettingcontroller>();


        Title_t = GetComponentInChildren<Text>();
        btn = GetComponentInChildren<UnityEngine.UI.Button>();
        btn_text = btn.GetComponentInChildren<Text>();
        btn_img = btn.GetComponent<Image>();

        Title_t.text = type.ToString() + " : ";

        btn.onClick.AddListener(KeysettingBtn_click);
    }
    void Start()
    {
        keymanager = KeySettingManager.Instance;
        btn_text.text = keymanager.keyBindings[type].ToString();
    }

    public void KeysettingBtn_click()
    {
        //StartCoroutine(keycontroller.Set_current_co(this));
        co = StartCoroutine(btn_click_co());
    }
    public void Stop_co()
    {
        Debug.Log(gameObject.name + "co stop");
        btn_text.text = keymanager.keyBindings[type].ToString();
        StopCoroutine(co);
        isstartCo = false;
    }


    
    IEnumerator btn_click_co()  // 버튼 누를때 코루틴으로 처리
    {
        // yield return ;//이 코루틴이 끝날때까지 기다림...

        while (true)
        {
            isstartCo = true;
            if (Input.anyKeyDown)
            {
                KeyCode k = keymanager.PressKeyReturn();
                Debug.Log(k);
                if (!keymanager.IsKeyAlreadyUsed(k, type) && !((int)k >= 323 && (int)k <= 329))
                {
                    keymanager.keyBindings[type] = k;
                    btn_text.text = keymanager.keyBindings[type].ToString();
                }
                else
                {
                    Debug.Log("이미 사용중입니다.");
                }
                isstartCo = false;
                yield break;
            }
            yield return null;
        }
    }



}