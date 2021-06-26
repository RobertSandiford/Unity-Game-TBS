using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    Global global;
    FlyCamera gameCamera;

    void Awake() {
    }

    // Start is called before the first frame update
    void Start()
    {
        gameCamera = ((Global)FindObjectOfType<Global>()).gameCamera;
    }

    // Update is called once per frame
    void Update()
    {
        //gameCamera
    }
}
