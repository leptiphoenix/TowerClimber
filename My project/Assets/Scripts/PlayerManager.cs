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

    [SerializeField] List<GameObject> BonusPool;
    [SerializeField] int BonusInterval;


    public int HighScore;
    public int Score;

    private int BonusPoolSize;
    private int bonusTreshold;

    private Rigidbody rb;
    private bool jumping = false;

    private void Awake()
    {
        ctrl = new GameInput();
        ctrl.Climber.Jump.performed += _ => jump();
        //ctrl.Climber.move.performed += ctx => Move(ctx.ReadValue<float>());
    }

    // Start is called before the first frame update
    void Start()
    {
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


        Score = (int) Mathf.Max(rb.transform.position.y, Score);
        if (Score >= bonusTreshold)
        {
            bonusTreshold += BonusInterval;
            Instantiate(BonusPool[Random.Range(0, BonusPoolSize)], new Vector3(Random.Range(-5,5),Score+3,0), Quaternion.identity);
        }
        scoreUI.text = Score.ToString();
    }

    public void jump()
    {
        if (!jumping)
        {
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
        SaveData data = new SaveData(
            (int)Mathf.Max(HighScore, Score)
        );
        SaveSystem.Write(data);
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
}
