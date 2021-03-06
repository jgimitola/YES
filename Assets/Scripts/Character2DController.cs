using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2DController : MonoBehaviour
{



    public float velocidad;
    public float fuerzaSalto=5f;

    float limiteSaltos = 2;
    float saltosHechos;
    public float saltosMaximos;
    public LayerMask capaSuelo;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private bool mirandoDerecha = true;
    private float saltosRestantes;
    private Animator animator;
    public GameObject[] players;



    private void Start()
    {
        saltosHechos = 0f;
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        saltosRestantes = saltosMaximos;

        animator = GetComponent<Animator>();
        timeR1.instanciar.iniciarTiempo();
        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        ProcesarMovimiento();
        ProcesarSalto();
    }

    bool EstaEnSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, capaSuelo);
        return raycastHit.collider != null;
    }

    void ProcesarSalto()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isJumping",true);
            if (saltosHechos < limiteSaltos)
            {
                rigidBody.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
                saltosHechos++;
            }
            
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

    void ProcesarMovimiento()
    {
        // L?gica de movimiento
        float inputMovimiento = Input.GetAxis("Horizontal");

        if (inputMovimiento != 0f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        rigidBody.velocity = new Vector2(inputMovimiento * velocidad, rigidBody.velocity.y);

        GestionarOrientacion(inputMovimiento);
    }


    void GestionarOrientacion(float inputMovimiento)
    {
        // Si se cumple condici?n
        if ((mirandoDerecha == true && inputMovimiento < 0) || (mirandoDerecha == false && inputMovimiento > 0))
        {
            // Ejecutar c?digo de volteado
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


    void OnCollisionEnter2D(Collision2D obj)
    {
        if(obj.collider.tag == "Suelo")
        {
            saltosHechos = 0;
        }
    }
}
