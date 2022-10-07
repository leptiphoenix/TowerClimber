using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    [SerializeField] GameObject SpawnPoint;
    [SerializeField] List<GameObject> PiecePool;

    private int PiecePoolSize;
    private GameObject PieceToPlace;

    private void Awake()
    {
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
        print(PiecePoolSize);
        nextPiece();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextPiece()
    {
        PieceToPlace = Instantiate(PiecePool[Random.Range(0, PiecePoolSize)], SpawnPoint.transform.position, Quaternion.identity);
    }

}
