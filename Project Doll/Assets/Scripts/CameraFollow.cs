using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject cameraTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // follow target
        transform.position = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y, transform.position.z);
    }
}
