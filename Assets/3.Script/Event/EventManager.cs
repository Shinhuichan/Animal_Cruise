using UnityEngine;

public class EventManager : MonoBehaviour
{


    #region EVENTS
    [SerializeField] public EventMoveTo eventMoveTo;





    #endregion

    

    public static EventManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    
    

}
