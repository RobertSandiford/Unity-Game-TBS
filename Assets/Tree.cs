using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    public GameObject cameraObj;
    Vector3 rotationEuler;
    //public GameObject cameraObj;
    Vector3 position;

    // Start is called before the first frame update
    void Start() {
        //gameCamera = ((Global)FindObjectOfType<Global>()).gameCamera;
        rotationEuler = gameObject.transform.transform.eulerAngles;
        position = gameObject.transform.position;
        //PI = (float)Math.PI;
        
        FaceToCamera();

        //gameObject.isStatic = true;
    }

    // Update is called once per frame
    void Update() {
        FaceToCamera();
    }

    void FaceToCamera() {
        Vector3 camPos = cameraObj.transform.position;

        gameObject.transform.LookAt(new Vector3(camPos.x, position.y, camPos.z));
    }
}
