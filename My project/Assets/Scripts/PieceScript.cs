using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceScript : MonoBehaviour
{
    //Indique si c'est la pi�ce manipul� par le joueur ou non
    private bool virgin = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //si la pi�ce sort de la zone de jeu, elle dispara�t.
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Si la pi�ce en touche une autre, on joue la prochaine
        if (collision.collider.transform.parent != this.transform.parent)
            tryNextPiece();
    }

    private void OnTriggerEnter(Collider other)
    {
        //si la pi�ce sort de la zone de jeu, elle dispara�t.
        if (other.gameObject.tag == "DeadZone")
        {
            trydestroy();
        }
    }

    public void tryNextPiece()
    {
        if (virgin)
        {
            virgin = false;
            //we keep control of the piece a bit after colision
            //StartCoroutine(keepControl());
            SceneManager.Instance.nextPiece();
        }
    }

    /*IEnumerator keepControl()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.Instance.nextPiece();
    }*/

        public void trydestroy()
    {
        //on indique qu'on ne veux plus la jouer
        tryNextPiece();
        if (!virgin)
        {
            Destroy(this.gameObject);
        }
    }
}
