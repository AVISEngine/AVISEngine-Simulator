﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LIDAR : MonoBehaviour {

    public float maxAngle = 10;
    public float minAngle = -10;
    public int numberOfLayers = 16;
    public int numberOfIncrements = 360;
    public float maxRange = 100f;

    float vertIncrement;
    float azimutIncrAngle;

    [HideInInspector]
    public float[] distances;
    public float[] azimuts;


    // Use this for initialization
    void Start () {
        distances = new float[numberOfLayers* numberOfIncrements];
        azimuts = new float[numberOfIncrements];
        vertIncrement = (float)(maxAngle - minAngle) / (float)(numberOfLayers - 1);
        azimutIncrAngle = (float)(360.0f / numberOfIncrements);
    }

// Update is called once per frame
void FixedUpdate () {
        Vector3 fwd = new Vector3(0, 0, 1);
        Vector3 dir;
        RaycastHit hit;
        int indx = 0;
        float angle;

        //azimut angles
        for (int incr = 0; incr < numberOfIncrements; incr++)
        {
            for (int layer = 0; layer < numberOfLayers; layer++)
            {
                //print("incr "+ incr +" layer "+layer+"\n");
                indx = layer + incr * numberOfLayers;
                angle = minAngle + (float)layer * vertIncrement;
                azimuts[incr] = incr * azimutIncrAngle;
                dir = transform.rotation * Quaternion.Euler(-angle, azimuts[incr], 0)*fwd;
               // print("idx "+ indx +" angle " + angle + "  azimut " + azimut + " quats " + Quaternion.Euler(-angle, azimut, 0) + " dir " + dir+ " fwd " + fwd+"\n");

                if (Physics.Raycast(transform.position, dir, out hit, maxRange))
                {
                    Debug.DrawRay(transform.position +  new Vector3(0,0,0), dir *(float)hit.distance, Color.green);
                    distances[indx] = (float)hit.distance;
                    
                }
                else
                {
                    distances[indx] = 100.0f;
                }
            }
        }

    }
}