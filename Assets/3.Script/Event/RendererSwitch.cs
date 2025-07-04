using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;


public class RendererSwitch : MonoBehaviour
{
    public Material originMarterial;
    public Material changeMarterial;

    private CharacterControl player;



    void OnTriggerEnter(Collider other)
    {
        player = other.gameObject.GetComponentInParent<CharacterControl>();
        if (player != null && other.tag == "Player")
        {
             Renderer renderer = GetComponent<Renderer>();

            renderer.material = changeMarterial;
        }
    }

    void OnTriggerExit(Collider other)
    {
     if (player != null && other.tag == "Player")
        {
             Renderer renderer = GetComponent<Renderer>();

            renderer.material = originMarterial;
        }
    }
}
