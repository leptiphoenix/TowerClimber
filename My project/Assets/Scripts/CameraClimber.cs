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
        this.transform.position = new Vector3(climber.transform.position.x, climber.transform.position.y, this.transform.position.z);
    }
}
