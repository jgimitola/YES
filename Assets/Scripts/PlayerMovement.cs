using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 6f;
    public float JumpForce = 5.3f;
    public float WallXJumpForce = 6f;
    public float WallYJumpForce = 15f;
    public float wallCheckDistance = 0.42f;
    public bool debug;

    private float _horizontal;
    private bool _jumping;
    private bool _grounded = false;
    private bool _leftWall = false;
    private bool _rightWall = false;

    private Rigidbody2D _rigidBody2D;
    private CircleCollider2D _circleCollider2D;
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private LayerMask wallLayerMask;

    /**
     * Check if player is facing a wall with a certain distance
     */
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

    /** 
     * Determines if the player is touching a platform     
     */
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

    /**
     * Move player. Handles X movement, normal jumping and wall jumping.
     */
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

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody2D = this.GetComponent<Rigidbody2D>();
        _circleCollider2D = this.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _grounded = OnGround();
        _leftWall = OnWall(-wallCheckDistance);
        _rightWall = OnWall(wallCheckDistance);

        if (Input.GetButton("Jump") && _grounded)
        {
            _jumping = true;
        }
    }

    public void FixedUpdate()
    {
        Move();
    }
}
