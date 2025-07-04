using UnityEngine;
using CustomInspector;
using System.Collections.Generic;
using System.Linq;



public class Ability_Control : MonoBehaviour
{

    [Space(20), Title("ABILITY SYSTEM", underlined: true, fontSize = 14, alignment = TextAlignment.Center)]
    [HorizontalLine(color:FixedColor.IceWhite), HideField] public bool _l0;
    // 여기서 Ability를 직접 가지고 오면 안됨. Ability의 데이터 정보만을 수정할 뿐.
    // 기능 : 보유 중인 능력
    [SerializeField] List<AbilityData> datas = new List<AbilityData>();
    [ReadOnly] public AbilityFlag flags = AbilityFlag.NONE;
    [ReadOnly] public List<AbilityData> temp;

    [HorizontalLine(color:FixedColor.IceWhite), HideField] public bool _l1;

    // HashSet
        // List와 비슷한 형식의 복수의 자료형을 다룰 때 사용되는 HashSet
        // 자동 정렬이 되는 특징이 있고, 중복이 안된다는 특징이 있음.
            // HashSet<AbilityData>
    // Dictionary
        // Key와 Value를 따로 받아 Key를 받으면 value를 반환하는 특징이 있음.
        // 실제로 Inspector 화면에서 보여지지 않음.
            // Dictionary<AbilityFlag, Ability>
    // 기능 : 사용할 수 있는 능력
    private readonly Dictionary<AbilityFlag, Ability> actives = new Dictionary<AbilityFlag, Ability>();
    // 활성화된 능력만 갱신
    void Update()
    {
        foreach(var a in actives.ToList())
            a.Value?.Update();
    }
    void FixedUpdate()
    {
        foreach(var a in actives.ToList())
            a.Value?.FixedUpdate();
    }

    // 잠재 능력 추가
    public void Add(AbilityData data, bool immediate = false)
    {
        if (datas.Contains(data) == true || data == null)
        {
            Debug.LogWarning("Ability Data가 Null입니다.");
            return;
        }
        // Ability를 추가하기 전에 이미 존재하는 지 확인
        if (datas.Contains(data))
        {
            Debug.LogWarning("해당 Ability가 이미 추가되어있습니다.");
            return;
        }
        
        // Data 상에 추가 및 Ability 생성
        datas.Add(data);
        var ability = data.CreateAbility(GetComponent<CharacterControl>());

        // flags.Add(data.Flag, null);

        // Dictionary의 key값에 따라 value값을 ability으로 넣어줌.
        if (immediate)
        {
            actives[data.Flag] = ability;
            ability.Activate();
        }
    }

    // 잠재 능력 제거
    public void Remove(AbilityData data)
    {
        if (data == null)
        {
            Debug.LogWarning("Ability Data가 Null입니다.");
            return;
        }
        // Ability를 추가하기 전에 이미 존재하는 지 확인
        if (datas.Contains(data) == false)
        {
            Debug.LogWarning("해당 Ability가 이미 제거되어있습니다.");
            return;
        }
        
        actives[data.Flag].Deactivate();

        datas.Remove(data);
        actives.Remove(data.Flag);
    }

    // 모든 잠재능력 제거
    public void RemoveAll() 
    {
        DeActivateAll();

        flags = AbilityFlag.NONE;
        actives.Clear();
        datas.Clear();
    }

    // 비활성화된 능력을 활성화 및 Update 추가
    public void Activate(AbilityFlag flag, bool forceDeacticate, object obj)
    {
        // true라면, 추가하기 전에 현재의 모든 능력을 지우기
        if (forceDeacticate)
            DeActivateAll();
// TEMP
        List<AbilityData> temp = new List<AbilityData>();
        temp.AddRange(datas.GetRange(0, datas.Count));
// TEMP

        foreach(var d in temp)
        {
            if ((d.Flag & flag) == flag)
            {
                if (actives.ContainsKey(flag) == false)
                    actives[flag] = d.CreateAbility(GetComponent<CharacterControl>());
                actives[flag].Activate(obj);
            }
        }
    }

    // 활성화된 능력 비활성화 맟 Update 제거
    public void DeActivate(AbilityFlag flag)
    {
        foreach(var d in datas)
        {
            if ((d.Flag & flag) == flag)
            {
                if (actives.ContainsKey(flag) == true)
                {
                    // flags.Remove(flag, null);
                    actives[flag].Deactivate();
                    actives[flag] = null;
                    actives.Remove(flag);
                }
            }
        }
    }

    // 모든 Ability를 Deactivate
    public void DeActivateAll()
    {
        foreach(var a in actives)
            a.Value.Deactivate();
        actives.Clear();
    }
}