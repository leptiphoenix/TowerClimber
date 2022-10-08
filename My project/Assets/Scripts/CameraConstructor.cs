using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConstructor : MonoBehaviour
{
    [SerializeField] PlayerManager pm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x, pm.Score+8, this.transform.position.z);
    }
}
