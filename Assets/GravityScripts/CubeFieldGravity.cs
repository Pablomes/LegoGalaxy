using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFieldGravity : MonoBehaviour
{
    public PlanetInfo planet;

    private int priority;

    // Start is called before the first frame update
    void Start()
    {
        planet = GetComponent<PlanetInfo>();
        priority = planet.priority;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CoGrav"))
        {
            other.GetComponent<GravityController>().FieldList.Add(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CoGrav"))
        {
            other.GetComponent<GravityController>().FieldList.Remove(gameObject);
        }
    }
}
