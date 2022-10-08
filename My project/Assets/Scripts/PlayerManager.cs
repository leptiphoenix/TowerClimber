using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameInput ctrl;

    [SerializeField] float moveForce;
    [SerializeField] float jumpForce;
    [SerializeField] float maxsYSpeed;
    [SerializeField] float maxsXSpeed;

    private Rigidbody rb;

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
    }

    public void jump()
    {
        print("jump");
        rb.AddForce(new Vector3(0, jumpForce, 0));
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
