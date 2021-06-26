using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shell : MonoBehaviour
{
    public double startTime;
    public Vector3 startPos;
    public Vector3 direction;
    public float speed;
    public Vector3 targetPos;
    public double maxDistance;

    public double speedExponent;
    public float yMultiplier;

    float totalDistance;
    float timeToComplete;

    float initialVerticalVelocity;
    float g = -9.81f * 100f * 0.1f ;

    Vector3 lastPos;

    public void Setup(Vector3 StartPos, Vector3 TargetPos, float Speed, double SpeedExponent = 0.85, float YMultiplier = 0.7f)
    {
        //Debug.Log("Setup");
        //Debug.Log(StartPos);
        //Debug.Log(TargetPos);
        //Debug.Log(Speed);
        startTime = Time.time;
        startPos = StartPos;
        targetPos = TargetPos;
        speed = Speed;
        Vector3 dirVec = (TargetPos - StartPos);
        dirVec.Normalize();
        direction = dirVec;
        maxDistance = Vector3.Distance(StartPos, TargetPos);

        speedExponent = SpeedExponent;
        yMultiplier = YMultiplier;
        
        totalDistance = Vector3.Distance(startPos, targetPos);
        timeToComplete = TimeForCompletion();

        initialVerticalVelocity = (0 - (0.5f * g * (float)Math.Pow(timeToComplete, 2))) / timeToComplete;
        //Debug.Log("<color=green>td: " + totalDistance + "</color>");
        //Debug.Log("<color=green>timeToComplete: " + timeToComplete + "</color>");
        //Debug.Log("<color=green>ivv: " + initialVerticalVelocity + "</color>");

        lastPos = startPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = startPos;
        //Debug.Log("Putting shell at " + startPos.ToString());
    }

    float Displacement(double timeElapsed)
    {
        
        return (float)Math.Pow(timeElapsed * speed, 1.0-((1.0-speedExponent)/100.0) );
    }

    float TimeForCompletion()
    {
        float dist = Vector3.Distance(startPos, targetPos);
        return (float) (Math.Pow(dist, 1.0 / speedExponent) / speed);
    }

    // Update is called once per frame
    void Update()
    {
        double timeElapsed = Time.time - startTime;

        //Debug.Log("TP " + timeElapsed);

        float s = Displacement(timeElapsed);

        //Debug.Log("S " + s);

        //float totalDistance = Vector3.Distance(startPos, targetPos);

        //Debug.Log("TDist " + totalDistance);

        //float timeToComplete = TimeForCompletion();

        //Debug.Log("TimeToComplete " + timeToComplete);

        float distFromCentre = Math.Abs((float)timeElapsed - (timeToComplete / 2f)) / (timeToComplete / 2f);
        distFromCentre = Math.Min(1f, Math.Max(0f, distFromCentre));

        //Debug.Log("distFromCentre " + distFromCentre);

        Vector3 pos = startPos + direction * s;

        //Debug.Log("1-dfc^0.5 " + ((float)Math.Pow(1 - distFromCentre, 0.6)));

        float yDisp = (initialVerticalVelocity * (float)timeElapsed) + (0.5f * g * (float)Math.Pow(timeElapsed, 2));

        //Debug.Log("<color=red>g " + g + "</color>");
        //Debug.Log("<color=red>hgt2 " + (0.5f * g * (float)Math.Pow(timeElapsed, 2)) + "</color>");
        //Debug.Log("<color=red>yDisp " + yDisp + "</color>");

        //pos.y += (float)Math.Pow(1 - distFromCentre, 0.5) * totalDistance * yMultiplier / speed;
        //////pos.y += yDisp * yMultiplier;

        gameObject.transform.position = pos;

        //gameObject.transform.rotation = Quaternion.LookRotation(direction);

        Vector3 dirVec = (pos - lastPos);
        dirVec.Normalize();
        gameObject.transform.rotation = Quaternion.LookRotation(dirVec);

        lastPos = pos;

        if (s > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
