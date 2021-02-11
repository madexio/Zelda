using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject toFollow;
    Transform followTransform;
    Transform cameraTransform;

    public int top;
    public int bottom;
    public int left;
    public int right;

    void Start()
    {
        followTransform = toFollow.GetComponent<Transform>();
        cameraTransform = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 pos = cameraTransform.position;
        if (followTransform.position.x > left && followTransform.position.x < right)
        {
            pos.x = followTransform.position.x;
        }
        if (followTransform.position.y > bottom && followTransform.position.y < top)
        {
            pos.y = followTransform.position.y;
        }
        pos.z = cameraTransform.position.z;
        cameraTransform.position = pos;
        
    }
}
