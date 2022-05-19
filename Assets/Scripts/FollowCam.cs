using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    public float DistanceX = 1f;
    public float DistanceY = 1f;
    public GameObject PlayerObject = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FollowPositionAndRotation()
    {
        Vector3 targetPos = PlayerObject.transform.position + PlayerObject.transform.forward * DistanceX + PlayerObject.transform.up * DistanceY;
      

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * 4f);
        transform.LookAt(PlayerObject.transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FollowPositionAndRotation();
    }
}
