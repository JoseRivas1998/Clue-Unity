using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Vector3 defaultPosition;
    public Vector3 offset;
    public Camera theCamera;

    private GameObject toFollow;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = new Vector3(defaultPosition.x, defaultPosition.y, defaultPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(toFollow == null)
        {
            targetPosition.Set(defaultPosition.x, defaultPosition.y, defaultPosition.z);
        } else
        {
            targetPosition.Set(toFollow.transform.position.x + offset.x, toFollow.transform.position.y + offset.y, toFollow.transform.position.z + offset.z);
        }
        theCamera.transform.position += (targetPosition - theCamera.transform.position) / 25f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0);
        Gizmos.DrawRay(defaultPosition, Vector3.right * 3);
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawRay(defaultPosition, Vector3.up * 3);
        Gizmos.color = new Color(0, 0, 1);
        Gizmos.DrawRay(defaultPosition, Vector3.forward * 3);
    }


    public void StopFollowing()
    {
        toFollow = null;
    }

    public void Follow(GameObject gameObject)
    {
        toFollow = gameObject;
        targetPosition.Set(toFollow.transform.position.x + offset.x, toFollow.transform.position.y + offset.y, toFollow.transform.position.z + offset.z);
    }

    public float DistanceFromTarget()
    {
        return (targetPosition - theCamera.transform.position).magnitude;
    }

}
