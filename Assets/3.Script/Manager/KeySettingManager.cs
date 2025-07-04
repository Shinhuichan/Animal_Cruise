using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BindingType // json 문자열로 받아오는데 -> 숫자로 변환
{
    MoveUp = 0,
    MoveDown,
    MoveLeft,
    MoveRight,
    Jump,
    Skill
    

}

[System.Serializable]
public class KeyBindings    //JSON 파일로 저장하기 위해 string으로 받음
{
    public string MoveUp;
    public string MoveDown;
    public string MoveLeft;
    public string MoveRight;
    public string Jump;
    public string Skill;
}

public class KeySettingManager : MonoBehaviour  
{
    public static KeySettingManager Instance; // 싱글톤으로 키세팅매니저 저장

    public Dictionary<BindingType, KeyCode> keyBindings = new Dictionary<BindingType, KeyCode>(); // 새로운 키값 저장

    
    private readonly string savePath = Path.Combine(Application.dataPath,"InputSetting" );
  


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadKeys();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadKeys()
    {

        KeyBindings data;

        if (!File.Exists(savePath))
        {
            System.IO.Directory.CreateDirectory(savePath);
        }

        if (!File.Exists(savePath + "/keysetting.json"))    //제이슨 파일 저장된거 없으면
        {
            data = GetDefaultBindings(); // data를 기본값으로 설정
            SaveKeys(data); // 그걸 파일로 저장
        }
        else
        {
            string json = File.ReadAllText(savePath + "/keysetting.json");
            data = JsonUtility.FromJson<KeyBindings>(json);
        }



        keyBindings[BindingType.MoveUp] = ParseKey(data.MoveUp);
        keyBindings[BindingType.MoveDown] = ParseKey(data.MoveDown);
        keyBindings[BindingType.MoveLeft] = ParseKey(data.MoveLeft);
        keyBindings[BindingType.MoveRight] = ParseKey(data.MoveRight);
        keyBindings[BindingType.Jump] = ParseKey(data.Jump);
        keyBindings[BindingType.Skill] = ParseKey(data.Skill);

     
        
    }

  
    void SaveKeys(KeyBindings data) //제이슨 파일 저장
    {
        string path = savePath + "/keysetting.json";
        string json = JsonUtility.ToJson(data, true);  
        File.WriteAllText(path, json); 
    }

    public void SaveCurrentToJson()  // 현재 바꾼 키 저장
    {
        KeyBindings data = new KeyBindings
        {
            MoveUp = keyBindings[BindingType.MoveUp].ToString(),
            MoveDown = keyBindings[BindingType.MoveDown].ToString(),
            MoveLeft = keyBindings[BindingType.MoveLeft].ToString(),
            MoveRight = keyBindings[BindingType.MoveRight].ToString(),
            Jump = keyBindings[BindingType.Jump].ToString(),
            Skill = keyBindings[BindingType.Skill].ToString()
        };

        SaveKeys(data); // 현재 json 파일 덮어쓰기
    }

    // 초기값으로 리셋, 사용자가 키 셋팅을 실수로 잘못 했을 때 사용
    public void ResetToDefault()    
    {
        KeyBindings defaultData = GetDefaultBindings();

        keyBindings[BindingType.MoveUp] = ParseKey(defaultData.MoveUp);
        keyBindings[BindingType.MoveDown] = ParseKey(defaultData.MoveDown);
        keyBindings[BindingType.MoveLeft] = ParseKey(defaultData.MoveLeft);
        keyBindings[BindingType.MoveRight] = ParseKey(defaultData.MoveRight);
        keyBindings[BindingType.Jump] = ParseKey(defaultData.Jump);
        keyBindings[BindingType.Skill] = ParseKey(defaultData.Skill);


        SaveKeys(defaultData);
    }

    // 기본 키 설정값
    private KeyBindings GetDefaultBindings()
    {
        return new KeyBindings
        {
            MoveUp = "W",
            MoveDown = "S",
            MoveLeft = "A",
            MoveRight = "D",
            Jump = "Space",
            Skill = "LeftControl"
        };
    }

    // 키 중복 검사
    public bool IsKeyAlreadyUsed(KeyCode key, BindingType exceptAction)
    {
        foreach (var pair in keyBindings)
        {
            if (pair.Key != exceptAction && pair.Value == key)
                return true;
        }
        return false;
    }

    // 키 반환
    public KeyCode GetKey(BindingType action)
    {
        return keyBindings.ContainsKey(action) ? keyBindings[action] : KeyCode.None;
    }

    //눌린 키 반환
    public KeyCode PressKeyReturn()
    {
        foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(k))
            {
                return k;
            }
        }
        return KeyCode.None;
    }


    private KeyCode ParseKey(string keyName)
    {
        return (KeyCode) System.Enum.Parse(typeof(KeyCode), keyName);
    }

    

}