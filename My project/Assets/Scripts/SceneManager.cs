using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    public GameInput ctrl;

    [SerializeField] GameObject SpawnPoint;
    [SerializeField] List<GameObject> PiecePool;

    [SerializeField] float PiecefallSpeed;
    [SerializeField] float fastMultiplier;
    [SerializeField] float PieceTranslateSpeed;

    private int PiecePoolSize;
    private GameObject PieceToPlace;
    private float maxPiecespeed;

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
        StartGame();
    }


    // Update is called once per frame
    void Update()
    {
        PieceToPlace.GetComponent<Rigidbody>().velocity = PieceToPlace.GetComponent<Rigidbody>().velocity.normalized * Mathf.Min(PieceToPlace.GetComponent<Rigidbody>().velocity.magnitude, PiecefallSpeed);
    }

    private void TranslatePiece(float direction)
    {
        if (direction > 0)
        {
            PieceToPlace.transform.Translate(new Vector3(PieceTranslateSpeed, 0f, 0f),relativeTo:Space.World);
        }
        else if (direction < 0)
        {
            PieceToPlace.transform.Translate(new Vector3(-PieceTranslateSpeed, 0f, 0f), relativeTo: Space.World);
        }
    }
    private void MovePieceFaster()
    {
        PieceToPlace.transform.Translate(new Vector3(0f, -fastMultiplier, 0f), relativeTo: Space.World);
    }
    private void rotatePiece()
    {
        PieceToPlace.transform.Rotate(new Vector3(0f,0f,90f));
    }

    public void StartGame()
    {
        nextPiece();
    }

    public void nextPiece()
    {
        PieceToPlace = Instantiate(PiecePool[Random.Range(0, PiecePoolSize)], SpawnPoint.transform.position, Quaternion.identity);
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
