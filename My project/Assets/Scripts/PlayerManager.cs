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
    [SerializeField] float maxsYSpeed;
    [SerializeField] float maxsXSpeed;
    [SerializeField] TextMeshProUGUI scoreUI;
    [SerializeField] TextMeshProUGUI HighscoreUI;

    public int HighScore;
    public int Score;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (ctrl.Climber.move.IsPressed())
        {
            rb.AddForce(new Vector3(ctrl.Climber.move.ReadValue<float>() * moveForce, 0, 0));
        }

        //limit player speed
        rb.velocity = new Vector3( Mathf.Min(rb.velocity.x, maxsXSpeed), Mathf.Min(rb.velocity.y, maxsYSpeed), rb.velocity.z);
        rb.velocity = new Vector3(Mathf.Max(rb.velocity.x, -maxsXSpeed), Mathf.Max(rb.velocity.y, -maxsYSpeed), rb.velocity.z);

        Score = (int) Mathf.Max(rb.transform.position.y, Score);
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
        if (collision.contacts[0].normal.y > 0.85f)
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
