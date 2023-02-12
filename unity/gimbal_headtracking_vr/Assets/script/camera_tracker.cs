using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_tracker : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 3;
    public Vector3 offset;

    [SerializeField]
    float eulerAngX;
    [SerializeField]
    float eulerAngY;
    [SerializeField]
    float eulerAngZ;

    void tracking_motion(){
        eulerAngX = target.localEulerAngles.x;
        eulerAngY = target.localEulerAngles.y;
        eulerAngZ = target.localEulerAngles.z;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        transform.position = smoothedPosition;
        transform.Rotate(eulerAngX, eulerAngY, eulerAngZ, Space.Self);
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
