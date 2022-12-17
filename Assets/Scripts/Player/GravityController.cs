using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public float GravityModifier = 10f;

    public Transform PlayerTransform;
    public Transform PlayerDirection;
    public Transform MinifigTrans;

    public Rigidbody PlayerRigidbody;

    public List<GameObject> FieldList = new List<GameObject>();

    private GameObject GravitySource = null;

    private int BiggestPriority = -1;

    private Vector3 GravityDirection = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GreatestPriority();
    }

    private void FixedUpdate()
    {
        GravityDirection = Gravity();

        PlayerRigidbody.AddForce(GravityDirection * (GravityModifier * Time.fixedDeltaTime), ForceMode.Acceleration);

        PlayerTransform.rotation = Quaternion.FromToRotation(PlayerTransform.up, -GravityDirection) * PlayerTransform.rotation;
        PlayerDirection.rotation = Quaternion.FromToRotation(PlayerDirection.up, -GravityDirection) * PlayerDirection.rotation;
        MinifigTrans.rotation = Quaternion.FromToRotation(MinifigTrans.up, -GravityDirection) * MinifigTrans.rotation;
    }

    void GreatestPriority()
    {
        bool InSource = false;

        if (GravitySource == null)
        {
            BiggestPriority = -1;
        }
        else
        {
            for (int j = 0; j < FieldList.Count; j++)
            {
                if (FieldList[j] == GravitySource)
                {
                    InSource = true;
                }
            }
            if (InSource)
            {
                BiggestPriority = GravitySource.GetComponent<PlanetInfo>().priority;
            }
            else
            {
                BiggestPriority = -1;
            }
        }

        if (FieldList.Count == 0)
        {
            GravitySource = null;
            return;
        }

        for (int i = 0; i < FieldList.Count; i++)
        {
            if (FieldList[i].GetComponent<PlanetInfo>().priority > BiggestPriority)
            {
                GravitySource = FieldList[i];
            }
            else if (FieldList[i].GetComponent<PlanetInfo>().priority == BiggestPriority)
            {
                float disA = Vector3.Distance(GravitySource.transform.position, PlayerTransform.position);
                float disB = Vector3.Distance(FieldList[i].transform.position, PlayerTransform.position);
                if (disB < disA)
                {
                    GravitySource = FieldList[i];
                }
            }
        }
    }

    Vector3 Gravity()
    {
        if (GravitySource == null) return new Vector3(0f, 0f, 0f);

        if (GravitySource.CompareTag("SphereInGravity"))
        {
            Vector3 grav = (GravitySource.transform.position - transform.position);
            return grav * (1 / Vector3.Magnitude(grav));
        }
        else if (GravitySource.CompareTag("SphereOutGravity"))
        {
            Vector3 grav = -(GravitySource.transform.position - transform.position);
            return grav * (1 / Vector3.Magnitude(grav));
        }
        else if (GravitySource.CompareTag("CubeDownGravity"))
        {
            return -GravitySource.transform.up;
        }
        else if (GravitySource.CompareTag("CubeUpGravity"))
        {
            return GravitySource.transform.up;
        }
        else if (GravitySource.CompareTag("EdgeInGravity"))
        {
            Vector3 grav = (GravitySource.GetComponent<EdgeFieldGravity>().Target.ClosestPoint(transform.position) - transform.position);
            return grav * (1 / Vector3.Magnitude(grav));
        }
        else if (GravitySource.CompareTag("EdgeOutGravity"))
        {
            Vector3 grav = -(GravitySource.GetComponent<EdgeFieldGravity>().Target.ClosestPoint(transform.position) - transform.position);
            return grav * (1 / Vector3.Magnitude(grav));
        }

        return new Vector3 (0f, 0f, 0f);
    }
}
