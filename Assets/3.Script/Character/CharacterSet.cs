using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;



public class CharacterSet : NetworkBehaviour
{
    
    #region Network
    [SyncVar(hook = nameof(OnCharacterIndexChanged))]
    public int characterIndex;
    void OnCharacterIndexChanged(int oldIndex, int newIndex)
    {
        Debug.Log($"Character index changed from {oldIndex} to {newIndex}");
        // 캐릭터 외형/능력치 변경 등
        ApplyCharacterSettings(newIndex);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        StartCoroutine(ApplyCharacterSettings(characterIndex)); // 클라이언트 초기화

        owner = GetComponent<CharacterControl>();


    }




    void Start()
    {
        if (!isLocalPlayer) return;
        StartCoroutine(ApplyCharacterSettings(characterIndex));
        
    }
    IEnumerator ApplyCharacterSettings(int index)
    {
        yield return new WaitForSeconds(0.05f);

        characters = GetComponentsInChildren<Animator>(false);
        owner.animator = characters[index];
        Debug.Log($"owner.animator : {owner.animator}");

        
        var characterObject = characters[index].gameObject;
        Transform model = characterObject.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "model");
        Debug.Log($"model : {model}, obj : {characterObject}");

        if (model == null)
            Debug.LogWarning("CharacterControl ] model 없음");
        model.gameObject.SetActive(true);
        CharacterControl characterctrl = GetComponent<CharacterControl>();
        characterctrl.Profile = profiles[index];

        if (owner.ability != null && owner.Profile != null)
        {
            owner.DataSet(owner.Profile);
            foreach (var data in owner.Profile.abilities)
                owner.ability.Add(data, true);
        }


        GetComponent<Rigidbody>().isKinematic = false;

        Transform normal = characterObject.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Normal Collider");
        characterctrl.normalCol = normal.GetComponent<BoxCollider>();

        Transform crawl = characterObject.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Crawl Collider");
        characterctrl.crawlCol = crawl.GetComponent<BoxCollider>();
        // index 기반으로 외형이나 스탯 설정
        //Debug.Log($"{characterIndex} 번째 캐릭터 생성");


    }
    #endregion
    [SerializeField] private List<CharacterProfile> profiles;

    public Animator[] characters;

    public CharacterControl owner;
}