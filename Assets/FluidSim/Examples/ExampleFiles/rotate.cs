using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {

    public float rotateSpeed = 1.0f;

    private Vector3 rotateVector = Vector3.zero;

    private Transform myTransform;

	// Use this for initialization
	void Start () {
        myTransform = transform;
    }
	
	// Update is called once per frame
	void Update () {
        rotateVector.z = rotateSpeed * Time.deltaTime;

        myTransform.Rotate(rotateVector);
    }
}
