using UnityEngine;
using UnityEngine.SceneManagement;

public class SelecteScene : MonoBehaviour
{
    public void SceneLoad(string name)
    {
        SceneManager.LoadScene(name);
    }
}
