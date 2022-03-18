using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2DController : MonoBehaviour
{
    public float Speed = 6f;
    public float JumpForce = 5.3f;
    public float WallXJumpForce = 6f;
    public float WallYJumpForce = 15f;
    public float wallCheckDistance = 0.42f;
    public bool debug;

    private float _horizontal;
    private bool _jumping;
    private bool _grounded;
    private bool _leftWall;
    private bool _rightWall;
    private bool mirandoDerecha = true;

    private Rigidbody2D _rigidBody2D;
    private CircleCollider2D _circleCollider2D;
    public GameManager gameManager;
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private LayerMask wallLayerMask;

    private Animator animator;
    public GameObject[] players;

    bool OnWall(float distance)
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(this.transform.position, Vector2.left, -distance, wallLayerMask);
        bool isHit = raycastHit.collider != null;

        if (debug)
        {
            Color rayColor = Color.red;
            if (isHit)
            {
                rayColor = Color.green;
            }
            Debug.DrawRay(this.transform.position, new Vector2(distance, 0), rayColor);
        }

        return isHit;
    }


    bool OnGround()
    {
        float extraHeight = 0.1f;
        float adjustOnGroundWidth = 0.1f;
        Bounds colliderBounds = _circleCollider2D.bounds;

        RaycastHit2D raycastHit = Physics2D.BoxCast(colliderBounds.center, new Vector3(colliderBounds.size.x - adjustOnGroundWidth, colliderBounds.size.y, colliderBounds.size.z), 0f, Vector2.down, extraHeight, platformLayerMask);
        bool isHit = raycastHit.collider != null;

        if (debug)
        {
            Color rayColor = Color.red;
            if (isHit)
            {
                rayColor = Color.green;
            }
            Debug.DrawRay(colliderBounds.center + new Vector3(colliderBounds.extents.x - adjustOnGroundWidth, 0), Vector2.down * (colliderBounds.extents.y + extraHeight), rayColor);
            Debug.DrawRay(colliderBounds.center - new Vector3(colliderBounds.extents.x - adjustOnGroundWidth, 0), Vector2.down * (colliderBounds.extents.y + extraHeight), rayColor);
            Debug.DrawRay(colliderBounds.center - new Vector3(colliderBounds.extents.x - adjustOnGroundWidth, colliderBounds.extents.y + extraHeight), Vector2.right * (2 * (colliderBounds.extents.x - adjustOnGroundWidth)), rayColor);
        }

        return isHit;
    }

    void Move()
    {
        transform.position += new Vector3(_horizontal, 0, 0) * Time.fixedDeltaTime * Speed;

        if (Input.GetButton("Jump") && _leftWall)
        {
            _rigidBody2D.AddForce(new Vector2(WallXJumpForce, WallYJumpForce), ForceMode2D.Impulse);
        }
        else if (Input.GetButton("Jump") && _rightWall)
        {
            _rigidBody2D.AddForce(new Vector2(-WallXJumpForce, WallYJumpForce), ForceMode2D.Impulse);
        }
        else if (_jumping)
        {
            _rigidBody2D.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            _jumping = false;
        }
    }

    private void Start()
    {
        _rigidBody2D = this.GetComponent<Rigidbody2D>();
        _circleCollider2D = this.GetComponent<CircleCollider2D>();

        animator = GetComponent<Animator>();
        timeR1.instanciar.iniciarTiempo();
        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    void Update()
    {

        _horizontal = Input.GetAxis("Horizontal");
        _grounded = OnGround();
        _leftWall = OnWall(-wallCheckDistance);
        _rightWall = OnWall(wallCheckDistance);

        animator.SetBool("isRunning", _horizontal != 0f);
        GestionarOrientacion(_horizontal);

        if (Input.GetButton("Jump") && _grounded)
        {
            _jumping = true;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void GestionarOrientacion(float inputMovimiento)
    {
        // Si se cumple condición
        if ((mirandoDerecha == true && inputMovimiento < 0) || (mirandoDerecha == false && inputMovimiento > 0))
        {
            // Ejecutar código de volteado
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        FindStartPos();
        players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 1)
        {
            Destroy(players[1]);
        }
    }

    void FindStartPos()
    {
        transform.position = GameObject.FindWithTag("StartPos").transform.position;
    }

}
