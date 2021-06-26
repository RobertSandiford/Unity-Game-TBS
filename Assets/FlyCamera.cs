using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlyCamera : MonoBehaviour
{
    Global global;

    Vector3 cameraPos;
    Vector3 cameraRotationEuler;

    float startAngle = 40.0f;
    float angle;

    float wheelSensitivity = 1.0f;

    float forwardBackwardsSensitivity = 18.0f;
    float leftRightSensitivity = 18.0f;
    float upDownSensitivity = -4.0f;
    float rotateSensitivity = 36.0f;
    float angleSensitivity = -10.0f;

    float shiftMultiplier = 2.2f;

    float minHeight = 0.5f;
    float maxHeight = 40.0f;

    float scale = 100f;


    //bool locked = false;
    Vector3 savedCameraPosition;

    void Awake() {
        global = (Global)FindObjectOfType<Global>();

        global.cameraObj = gameObject;
    }

    void Start()
    {
        angle = startAngle;
        cameraPos = gameObject.transform.position;
        cameraRotationEuler = transform.rotation.eulerAngles;
        
        cameraRotationEuler.x = angle;
        transform.rotation = Quaternion.Euler(cameraRotationEuler);

        //// scaling
        minHeight *= scale;
        maxHeight *= scale;

        forwardBackwardsSensitivity *= scale;
        leftRightSensitivity *= scale;
        upDownSensitivity *= scale;
    }


    void Update()
    {

        float delta = Time.deltaTime;
        float cameraFacing = cameraRotationEuler.y;
        Vector3 cameraMoveVector = new Vector3();

        bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float shiftMultiplier = (shift) ? this.shiftMultiplier : 1.0f;
        bool control = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

       
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            cameraMoveVector.z += delta * forwardBackwardsSensitivity * shiftMultiplier;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            cameraMoveVector.z -= delta * forwardBackwardsSensitivity * shiftMultiplier;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (control)
            {
                cameraRotationEuler.y -= delta * rotateSensitivity * shiftMultiplier;
            }
            else
            {
                cameraMoveVector.x -= delta * leftRightSensitivity * shiftMultiplier;
            }
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (control)
            {
                cameraRotationEuler.y += delta * rotateSensitivity * shiftMultiplier;
            }
            else
            {
                cameraMoveVector.x += delta * leftRightSensitivity * shiftMultiplier;
            }
        }
        
        float wheelInput = Input.GetAxis("Mouse ScrollWheel") * wheelSensitivity;

        if (control)
        {
            cameraRotationEuler.x += wheelInput * angleSensitivity * shiftMultiplier;
        }
        else
        {
            //float height = cameraPos.y + (wheelInput * upDownSensitivity * shiftMultiplier);
            //cameraMoveVector.y = Mathf.Clamp(height, minHeight, maxHeight);
            cameraMoveVector.y += (wheelInput * upDownSensitivity * shiftMultiplier);
        }


        cameraMoveVector = Quaternion.AngleAxis(cameraFacing, Vector3.up) * cameraMoveVector;
        cameraPos += cameraMoveVector;
        cameraPos.y = Mathf.Clamp(cameraPos.y, minHeight, maxHeight);

        gameObject.transform.position = cameraPos;
        gameObject.transform.rotation = Quaternion.Euler(cameraRotationEuler);


    }

    public float degreesToRadians(float degrees)
    {
        return degrees / 360f * 2f * (float)Math.PI;
    }

    public void CenterCameraOnObject(GameObject gameObject)
    {

        //Vector3 newPosition = cameraPos;


        //newPosition.x = gameObject.transform.position.x;

        cameraPos.x = gameObject.transform.position.x;

        float idealDistance = 16.0f * (float)global.map.scale;
        //float angle = transform.rotation.x;
        //float angle = (50f / 180f * (float)Math.PI); // hardcoding because I don't understand the value returned by the unity rotation

        float angleRadians = degreesToRadians(angle);



        cameraPos.y = gameObject.transform.position.y + (float)Math.Sin(angleRadians) * idealDistance;
        cameraPos.z = gameObject.transform.position.z - (float)Math.Cos(angleRadians) * idealDistance;
        

        //cameraRotationEuler.x = this.angle / 2.0f / (float)Math.PI * 360.0f;

        /*this.angle = startAngle;
        var rotation = transform.rotation.eulerAngles;
        rotation.x = this.angle;
        transform.rotation = Quaternion.Euler(rotation);

       
        transform.position = newPosition;*/
    }

    public void SaveCameraPosition()
    {
        savedCameraPosition = cameraPos;
    }

    public void ResetCameraPosition()
    {
        cameraPos = savedCameraPosition;
    }

    public void LockCamera()
    {
        //locked = true;
    }

    public void UnlockCamera()
    {
        //locked = false;
    }
}