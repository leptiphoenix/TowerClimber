using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    public GameInput ctrl;

    [SerializeField] GameObject SpawnPoint;
    [SerializeField] List<GameObject> PiecePool;

    [SerializeField] float PiecefallSpeed;
    [SerializeField] float fastMultiplier;
    [SerializeField] float PieceTranslateSpeed;
    [SerializeField] TextMeshProUGUI Tips;

    private List<string> tipslist;
    private int tipn;

    public int pieceLeft;
    [SerializeField] TextMeshProUGUI pieceLeftUI;

    
    private int PiecePoolSize;
    private GameObject PieceToPlace;

    private void Awake()
    {
        ctrl = new GameInput();
        ctrl.Constructor.Rotate.performed += _ => rotatePiece();
        ctrl.Constructor.Fast.performed += ctx => MovePieceFaster();
        ctrl.Constructor.PieceTranslate.performed += ctx => TranslatePiece(ctx.ReadValue<float>());
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        PiecePoolSize = PiecePool.Count;

        tipslist = new List<string>();

        tipslist.Add("Tip : Climb as high as you can !");
        tipslist.Add("Tip : When the climber use a new piece, the constructor can put an other one !");
        tipslist.Add("Tip : Cooperate to make the best tower !");
        tipslist.Add("Tip : The climber is lighter than pieces !");
        tipslist.Add("Tip : Poutine is delicious !");
        tipslist.Add("Tip : Share your highscore !");
        tipslist.Add("Tip : You can jump on surfaces up to 45° !");
        tipslist.Add("Tip : Don't fall !");

        tipn = Random.Range(0, tipslist.Count);
        Tips.text = tipslist[tipn];
        StartGame();
    }


    // Update is called once per frame
    void Update()
    {

        //limit piece to place speed
        if (PieceToPlace != null)
            PieceToPlace.GetComponent<Rigidbody>().velocity = PieceToPlace.GetComponent<Rigidbody>().velocity.normalized * Mathf.Min(PieceToPlace.GetComponent<Rigidbody>().velocity.magnitude, PiecefallSpeed);

        pieceLeftUI.text = pieceLeft.ToString();
    }

    private void TranslatePiece(float direction)
    {
        if (PieceToPlace != null)
        {
            if (direction > 0 && PieceToPlace.transform.position.x < 6)
            {
                PieceToPlace.transform.Translate(new Vector3(PieceTranslateSpeed, 0f, 0f), relativeTo: Space.World);
            }
            else if (direction < 0 && PieceToPlace.transform.position.x > -6)
            {
                PieceToPlace.transform.Translate(new Vector3(-PieceTranslateSpeed, 0f, 0f), relativeTo: Space.World);
            }
        }
            
    }
    private void MovePieceFaster()
    {
        if (PieceToPlace != null)
            PieceToPlace.transform.Translate(new Vector3(0f, -fastMultiplier, 0f), relativeTo: Space.World);
    }
    private void rotatePiece()
    {
        if (PieceToPlace != null)
            PieceToPlace.transform.Rotate(new Vector3(0f,0f,90f));
    }

    public void StartGame()
    {
        nextPiece();
    }

    public void nextPiece()
    {
        PieceToPlace = null;
        if (pieceLeft < 1)
        {
            StartCoroutine(waitforclimber());
        }
        else
        {
            PieceToPlace = Instantiate(PiecePool[Random.Range(0, PiecePoolSize)], SpawnPoint.transform.position, Quaternion.identity);
            pieceLeft--;
        }
        
    }

    public void changeinfo()
    {
        tipn = (tipn + 1) % tipslist.Count;
        Tips.text = tipslist[tipn];
    }

    public void makenextPieceSolid()
    {
        PieceToPlace.GetComponent<PieceScript>().willBeSolid();
    }
    IEnumerator waitforclimber()
    {
        yield return new WaitUntil(() => pieceLeft > 0);
        nextPiece();
    }
    private void OnEnable()
    {
        ctrl.Enable();
    }
    private void OnDisable()
    {
        ctrl.Disable();
    }
}
