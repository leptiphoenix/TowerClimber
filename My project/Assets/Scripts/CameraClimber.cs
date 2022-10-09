using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClimber : MonoBehaviour
{

    [SerializeField] GameObject climber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(climber.transform.position - transform.position), 5 * Time.deltaTime);
        this.transform.position = new Vector3(climber.transform.position.x*2, climber.transform.position.y+2, this.transform.position.z);
    }

}
