using UnityEngine;

public class DarkZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            var mesh = other.gameObject.GetComponentInChildren<MeshRenderer>();
            mesh.gameObject.layer = LayerMask.NameToLayer("DarkArea");
        }
        else
        {
            CharacterControl character = other.transform.parent.transform.parent.GetComponent<CharacterControl>();
            if (character.Profile.type == CharacterType.Cat) return;
            var mesh = other.transform.parent.transform.parent.GetComponentInChildren<SkinnedMeshRenderer>();
            mesh.gameObject.layer = LayerMask.NameToLayer("DarkArea");
        }
    }
}