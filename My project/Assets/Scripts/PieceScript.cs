using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceScript : MonoBehaviour
{

    private bool virgin = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //si la pièce sort de la zone de jeu, elle disparaît.
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.parent != this.transform.parent)
            trytriggerscenemanager();
    }

    private void OnTriggerEnter(Collider other)
    {
        //si la pièce sort de la zone de jeu, elle disparaît.
        if (other.gameObject.tag == "DeadZone")
        {
            trydestroy();
        }
    }

    public void trytriggerscenemanager()
    {
        if (virgin)
        {
            SceneManager.Instance.nextPiece();
            virgin = false;
        }
    }
    
        public void trydestroy()
    {
        //on indique qu'on ne la joue plus
        trytriggerscenemanager();
        if (!virgin)
        {
            Destroy(this.gameObject);
        }
    }
}
