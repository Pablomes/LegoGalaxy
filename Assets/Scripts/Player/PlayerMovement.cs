using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    PlayerControls controls;

    public float MaxSpeed = 1f;
    public float Accel = 0.1f;
    public float JumpForce = 1f;
    public float JumpTime = 1f;
    public float SpinForce = 1f;
    public float GroundRayLength = 1f;

    public LayerMask Ground;

    private bool Jumping = false;
    private bool Grounded = false;

    private float Speed;
    private float JumpTimer;

    private Vector2 mov;

    public Rigidbody rb;

    public Transform trans;
    public Transform PlayerDirection;
    public Transform MinifigTrans;

    public Animator anim;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.StartJump.performed += ctx => StartJump();
        controls.Gameplay.EndJump.performed += ctx => EndJump();

        controls.Gameplay.Move.performed += ctx => mov = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => mov = Vector2.zero;

        controls.Gameplay.Spin.performed += ctx => Spin();

        //controls.Gameplay.DisappearTest.performed += ctx => Test();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        JumpTimer = JumpTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (mov != Vector2.zero)
        {
            if (Speed >= MaxSpeed)
            {
                Speed = MaxSpeed;
            }
            else
            {
                Speed += Accel * Time.deltaTime;
            }
        }
        else
        {
            Speed = 0f;
        }
        print(Speed);

        if (Speed > 0)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }

        Move(mov);

        CheckGround();
    }

    private void FixedUpdate()
    {
        //rb.velocity = new Vector3(mov.x, rb.velocity.y, mov.y);

        if (Jumping)
        {
            JumpTimer -= Time.fixedDeltaTime;
            rb.AddForce(PlayerDirection.up * JumpForce, ForceMode.Impulse);

            if (JumpTimer < 0)
            {
                EndJump();
            }
        }
    }

    void Test()
    {
        print("Detected");
    }

    void StartJump()
    {
        Jumping = true;
    }

    void EndJump()
    {
        Jumping = false;
        JumpTimer = JumpTime;
    }

    void Move(Vector2 input)
    {
        
        //NOT WORKING
        Vector3 move = ((input.x * PlayerDirection.right) + (input.y * PlayerDirection.forward)) * Speed * Time.deltaTime;
        MinifigTrans.rotation = Quaternion.FromToRotation(MinifigTrans.forward, move) * MinifigTrans.rotation;
        //print(move);
        trans.Translate(move, Space.World);
        //rb.velocity = new Vector3(rb.velocity.x + move.x, rb.velocity.y + move.y, rb.velocity.z + move.z);// * Time.fixedDeltaTime;
    }
    void Spin()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(PlayerDirection.up * SpinForce);
    }

    void CheckGround()
    {
        if (Physics.Raycast(PlayerDirection.position, -PlayerDirection.up, GroundRayLength, Ground))
        {
            Grounded = true;
            Gizmos.color = Color.green;
            Gizmos.DrawRay(PlayerDirection.position, -PlayerDirection.up * GroundRayLength);
        }
        else
        {
            Grounded = false;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(PlayerDirection.position, -PlayerDirection.up * GroundRayLength);
        }
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    
}
