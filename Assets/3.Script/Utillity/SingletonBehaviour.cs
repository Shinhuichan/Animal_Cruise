using UnityEngine;
public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            // 현재 GameManager가 사용된 적이 없다면
            if(_instance == null)
            {
                // 하이어라키에서 GameManager 스크립트를 찾으면 넣습니다.
                _instance = FindFirstObjectByType<T>();
                // 그럼에도 못 찾았으면 하이어라키에서 GameObject를 직접 생성하고 GameManager 스크립트를 붙여주어 만듭니다.
                if(_instance == null)
                {
                   GameObject o = new GameObject(typeof(T).Name);
                   _instance = o.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
    protected abstract bool IsDontDestroy();
    protected virtual void Awake()
    {
        // 파괴되지 않게 Bool값이 true로 설정되어 있으면 Scene 전환에서 파괴되지 않게 해줌.
        if(IsDontDestroy()) DontDestroyOnLoad(this.gameObject);
    }
}
