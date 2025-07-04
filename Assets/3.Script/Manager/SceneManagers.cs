using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public void LobbyToCharacterRoom()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void CharacterRoomToLobby()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
