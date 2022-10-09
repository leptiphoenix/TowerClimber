using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceScript : MonoBehaviour
{
    [SerializeField] Material solidMaterial;

    private bool BeSolid = false;

    //Indique si c'est la pièce manipulé par le constructeur 
    private bool virgin = true;
    //indique si elle a été touchée par le climber
    private bool touched = false;


    [SerializeField] AudioSource piecePlaced;
    [SerializeField] AudioSource playerTouched;

    // Start is called before the first frame update
    void Start()
    {
        piecePlaced = GameObject.Find("pieceplaced").GetComponent<AudioSource>();
        playerTouched = GameObject.Find("touched").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //si la pièce sort de la zone de jeu, elle disparaît.
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Si la pièce en touche une autre, on joue la prochaine
        if (collision.collider.transform.parent != this.transform.parent)
        {
            
            tryNextPiece();
            if (BeSolid)
            {
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
            
        //Si la pièce touche le climber, elle donne une piece suplementaire au climber
        if (collision.gameObject.tag == "Climber" && !touched)
        {
            playerTouched.PlayOneShot(playerTouched.clip);
            SceneManager.Instance.pieceLeft++;
            touched = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //si la pièce sort de la zone de jeu, elle disparaît.
        if (other.gameObject.tag == "DeadZone")
        {
            trydestroy();
        }
        
    }

    public void willBeSolid()
    {
        foreach(MeshRenderer child in this.GetComponentsInChildren<MeshRenderer>())
        {
            child.material = solidMaterial;
        }
        BeSolid = true;
    }

    public void tryNextPiece()
    {
        if (virgin)
        {
            piecePlaced.PlayOneShot(piecePlaced.clip);
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
