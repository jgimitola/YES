using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 5.3f;
    public float JumpForce = 5.3f;
    public bool debug;

    private float _horizontal;
    private bool _jumping;
    private bool _grounded = false;

    private Rigidbody2D _rigidBody2D;
    private CircleCollider2D _circleCollider2D;
    [SerializeField] private LayerMask platformLayerMask;

    bool OnGround()
    {
        float extraHeight = 0.1f;
        Bounds colliderBounds = _circleCollider2D.bounds;

        RaycastHit2D raycastHit = Physics2D.BoxCast(colliderBounds.center, colliderBounds.size, 0f, Vector2.down, extraHeight, platformLayerMask);
        bool isHit = raycastHit.collider != null;

        if (debug)
        {
            Color rayColor = Color.red;
            if (isHit)
            {
                rayColor = Color.green;
            }
            Debug.DrawRay(colliderBounds.center + new Vector3(colliderBounds.extents.x, 0), Vector2.down * (colliderBounds.extents.y + extraHeight), rayColor);
            Debug.DrawRay(colliderBounds.center - new Vector3(colliderBounds.extents.x, 0), Vector2.down * (colliderBounds.extents.y + extraHeight), rayColor);
            Debug.DrawRay(colliderBounds.center - new Vector3(colliderBounds.extents.x, colliderBounds.extents.y + extraHeight), Vector2.right * ( 2 * colliderBounds.extents.x), rayColor);
        }

        return isHit;
    }

    void Move()
    {
        transform.position += new Vector3(_horizontal, 0, 0) * Time.fixedDeltaTime * Speed;

        if (_jumping)
        {
            _jumping = false;
            _rigidBody2D.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
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
