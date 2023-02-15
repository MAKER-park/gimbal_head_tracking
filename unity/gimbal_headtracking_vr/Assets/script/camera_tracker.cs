using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_tracker : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 3;
    public Vector3 offset;

    float eulerAngX;
    float eulerAngY;
    float eulerAngZ;

    void tracking_motion(){
        eulerAngX = target.localEulerAngles.x;
        eulerAngY = target.localEulerAngles.y;
        eulerAngZ = target.localEulerAngles.z;
        Debug.Log("eulerAngX : " + eulerAngX);
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        transform.position = smoothedPosition;
        transform.rotation = Quaternion.Euler(-eulerAngX, -eulerAngY, 0);    
        }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tracking_motion();
    }
}
