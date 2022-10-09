using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public GameInput ctrl;

    [SerializeField] float moveForce;
    [SerializeField] float jumpForce;
    [SerializeField] float maxYSpeed;
    [SerializeField] float maxXSpeed;
    [SerializeField] float XDrag;
    [SerializeField] TextMeshProUGUI scoreUI;
    [SerializeField] TextMeshProUGUI HighscoreUI;

    [SerializeField] AudioSource jumpSound;
    [SerializeField] AudioSource bonusSound;
    [SerializeField] AudioSource bonusSoundpop;
    [SerializeField] AudioSource endgameSound;
    [SerializeField] AudioSource highscoreSound;


    [SerializeField] List<GameObject> BonusPool;
    [SerializeField] int BonusInterval;

    [SerializeField] GameObject panelinfo;

    private bool highscoresoundplayed = false;

    private int HighScore;
    public int Score;

    private int BonusPoolSize;
    private int bonusTreshold;

    private Rigidbody rb;
    private bool jumping = false;

    private void Awake()
    {
        ctrl = new GameInput();
        ctrl.Climber.Jump.performed += _ => jump();
        ctrl.UI.quit.performed += ctx => quit();
        ctrl.UI.hide.performed += ctx => hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Decomment this code to reset highscore 
        //SaveSystem.Write(new SaveData(0));

        rb = this.GetComponent<Rigidbody>();
        SaveData data = SaveSystem.Read();
        if (data != null)
        {
            HighScore = data.HighScore;
        }
        else
        {
            HighScore = 0;
        }
        HighscoreUI.text = HighScore.ToString();

        BonusPoolSize = BonusPool.Count;
        bonusTreshold = BonusInterval;
    }

    // Update is called once per frame
    void Update()
    {

        //move
        if (ctrl.Climber.move.IsPressed())
        {
            rb.AddForce(new Vector3(ctrl.Climber.move.ReadValue<float>() * moveForce, 0, 0));
        }

        

        //limit player speed
        rb.velocity = new Vector3( Mathf.Min(rb.velocity.x, maxXSpeed), Mathf.Min(rb.velocity.y, maxYSpeed), rb.velocity.z);
        rb.velocity = new Vector3(Mathf.Max(rb.velocity.x, -maxXSpeed), Mathf.Max(rb.velocity.y, -maxYSpeed), rb.velocity.z);

        //drag player lateral movement
        float xspeed = rb.velocity.x;
        if (Mathf.Abs(xspeed) > 0.001)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x * XDrag, rb.velocity.y, rb.velocity.z);
        }

        //check to pop bonus
        Score = (int) Mathf.Max(rb.transform.position.y, Score);
        if (Score >= bonusTreshold)
        {
            bonusTreshold += BonusInterval;
            bonusSoundpop.PlayOneShot(bonusSoundpop.clip);
            Instantiate(BonusPool[Random.Range(0, BonusPoolSize)], new Vector3(Random.Range(-5,5),Score+3,0), Quaternion.identity);
        }
        scoreUI.text = Score.ToString();

        //si le highscore est dépasé, jouer une petite musique
        if (Score > HighScore && !highscoresoundplayed)
        {
            highscoresoundplayed = true;
            highscoreSound.PlayOneShot(highscoreSound.clip);
        }
    }

    public void jump()
    {
        if (!jumping)
        {
            jumpSound.PlayOneShot(jumpSound.clip);
            rb.AddForce(new Vector3(0, jumpForce, 0));
            jumping = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //si l'objet que je touche est sous mes pieds et à au moins 45 degré, je peux sauter dessus
        if (collision.contacts[0].normal.y > 0.80f)
        {
            jumping = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //si le joueur touche la zone, la partie est terminée
        if (other.gameObject.tag == "GameOver")
        {
            EndGame();
        }
        //si le joueur touche un bonus on regarde lequel et on applique son effet
        if (other.gameObject.tag == "Bonus")
        {

            bonusSound.PlayOneShot(bonusSound.clip);
            string bonus = other.GetComponent<Bonus>().bonus;
            if (bonus.Equals("PieceUp"))
            {
                SceneManager.Instance.pieceLeft++;
            }
            if (bonus.Equals("Solid"))
            {
                SceneManager.Instance.makenextPieceSolid();
            }
            Destroy(other.gameObject);
        }
    }
    public void EndGame()
    {
        endgameSound.PlayOneShot(endgameSound.clip);
        SaveData data = new SaveData(
            (int)Mathf.Max(HighScore, Score)
        );
        SaveSystem.Write(data);
        SceneManager.Instance.pieceLeft = 0;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        Destroy(transform.GetChild(0).gameObject);
        StartCoroutine(waittorestart());
    }

    IEnumerator waittorestart()
    {
        yield return new WaitForSeconds(3f);
        Destroy(SceneManager.Instance.gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("game");
    }

    private void OnEnable()
    {
        ctrl.Enable();
    }
    private void OnDisable()
    {
        ctrl.Disable();
    }

    private void hide()
    {
        panelinfo.SetActive(!panelinfo.activeSelf);
    }

    private void quit()
    {
        Destroy(SceneManager.Instance.gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
    }
}
