using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    [SerializeField] List<GameObject> poutinepiece;
    [SerializeField] List<GameObject> poutinepiecesp;

    [SerializeField] GameObject player;
    [SerializeField] GameObject playersp;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("game");
    }

    public void quitGame()
    {

        Application.Quit();
    }

    public void playhover()
    {
        for (int i = 0; i < poutinepiece.Count; i++)
        {
            poutinepiece[i].transform.position = poutinepiecesp[i].transform.position;
            poutinepiece[i].transform.rotation = poutinepiecesp[i].transform.rotation;
        }
        player.transform.position = playersp.transform.position;
    }

}
