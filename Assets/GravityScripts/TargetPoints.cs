using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoints : MonoBehaviour
{
    public Transform TargetPoint1;
    public Transform TargetPoint2;

    public Vector3 TargetVector;

    //private Transform[] Targets;

    // Start is called before the first frame update
    void Start()
    {
        TargetPoint1 = gameObject.transform.GetChild(0).gameObject.transform;
        TargetPoint2 = gameObject.transform.GetChild(1).gameObject.transform;

        TargetVector = TargetPoint2.position - TargetPoint1.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
