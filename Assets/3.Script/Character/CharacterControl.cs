using UnityEngine;
using Mirror;
using System.Collections;
public class CharacterControl : NetworkBehaviour
{
    [SerializeField] private CharacterProfile profile;
    public CharacterProfile Profile
    {
        get => profile;
        set => profile = value;
    }

    // 땅에 붙어 있는가?
    public bool isGrounded = true;
    public bool isJumping = false;
    public bool canJumping = true;
    public bool isCrowling = false;
    public bool mustCrawling = true;
    [ReadOnly] public Animator animator;
    [ReadOnly] public Rigidbody rb;
    public Collider normalCol;
    public Collider crawlCol;

    [HideInInspector] public Ability_Control ability;

    [HideInInspector] public float moveSpeed =10f;

    [HideInInspector] public float rotateSpeed;
    [HideInInspector] public float jumpForce;
    [HideInInspector] public float jumpDuration;

    void Awake()
    {
        // yield return new WaitForSeconds(0.5f);
        // if(!isLocalPlayer)return;
        if (TryGetComponent(out ability) == false)
            Debug.LogWarning("CharacterControl ] AbilityControl 없음");
        if (TryGetComponent(out rb) == false)
            Debug.LogWarning("CharacterControl ] RigidBody 없음");


    }

    // IEnumerator Start()
    // {
    //     yield return new WaitForSeconds(0.3f);

    //     if (ability != null && Profile != null)
    //     {
    //         DataSet(profile);
    //         foreach (var data in Profile.abilities)
    //             ability.Add(data, true);
    //     }
    //     Debug.Log($"moveSpeed: {moveSpeed},rotateSpeed : {rotateSpeed}, jumpForce : {jumpForce}");
    // }


    public void DataSet(CharacterProfile profile)
    {
        moveSpeed = profile.moveSpeed;
        rotateSpeed = profile.rotateSpeed;
        jumpForce = profile.jumpForce;
        jumpDuration = profile.jumpDuration;
        rb.mass = profile.weight;

    }
    public void Live(bool on)
    {
        // on 데이터에 따라 삶과 죽음을 판단함.
        rb.isKinematic = !on;
        normalCol.isTrigger = !on;
    }

    [Command]
    public void CMDAnimationPlay(string name1, string name2,  float moveSpeed, float targetSpeed, float duration, bool isGrounded)
    {
        AnimateParameter(name1,name2, moveSpeed, targetSpeed, duration, isGrounded);
    }

    [Command]
    public void CMDAnimationTrigger(string name)
    {
        AnimateTrigger(name); 
    }

    [ClientRpc]
    public void AnimateParameter(string name1, string name2,float moveSpeed, float targetSpeed, float duration, bool isGrounded)
    {

        Mathf.Lerp(animator.GetFloat(name), targetSpeed, duration);
        animator.SetBool(name2, isGrounded);
        animator.SetFloat(name1, moveSpeed);
        //animator?.CrossFadeInFixedTime(hash, duration, layer, 0f);
    }

    // [ClientRpc]
    // public void Animate(int hash, float duration, int layer = 0)
    // {
    //     animator?.CrossFadeInFixedTime(hash, duration, layer, 0f);
    // }
    [ClientRpc]
    public void AnimateTrigger(string name)
    {
        if (animator == null) return;


        animator.SetTrigger(name);

    }
}