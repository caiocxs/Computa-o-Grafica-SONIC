
using UnityEngine;

public class PlayerMainScript : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float minSpeed = 0f;
    public float maxSpeed = 9f;
    public float speed = 2f;
    public float jumpSpeed = 0f;

    [Header("Configurações de Boost")]
    public float boostForceMultiplier = 2f;
    public float speedAccumulationRate = 1f;
    public float forceAccumulationRate = 1f;

    public float accumulatedSpeed = 0f;
    public float accumulatedForce = 0f;
    private bool isHoldingSpeed = false;

    public Vector3 boostDirection;

    private bool isMoving;
    private bool isRunning;
    private bool isJumping;
    public bool canJump;

    public float maxJumping;
    
    [Header("Referências")]
    public Camera cmr;
    public Vector3 cmrOffset = new Vector3(0.75f, 1.5f, -2.85f);
    public float minCmrX;
    public float maxCmrX;

    private Rigidbody rb;
    private SpriteRenderer sr;
    private Animator an;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();

        rb.freezeRotation = true;

        cmr.transform.position = transform.position + cmrOffset;
        maxCmrX = cmrOffset.x;
    }

    void Update()
    {
        cmr.transform.position = transform.position + cmrOffset;

        HandleHolding();
        if (!isHoldingSpeed)
            HandleWalkingRunning();

        HandleJumping();
    }

    private void HandleWalkingRunning()
    {
        var hor = Input.GetAxis("Horizontal") * speed; // A/D
        var ver = Input.GetAxis("Vertical") * speed;   // W/S
        var movement = new Vector3(hor, 0, ver);

        Vector3 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(movement.x, currentVelocity.y, movement.z);

        if (hor < 0) sr.flipX = true;
        else if (hor > 0) sr.flipX = false;

        isMoving = hor != 0 || ver != 0;
        an.SetBool("isMoving", isMoving);

        isRunning = rb.linearVelocity.magnitude > maxSpeed / 2;
        an.SetBool("isRunning", isRunning);

        HandleSpeedIncreaseDecrease(hor, ver);
    }

    private void HandleSpeedIncreaseDecrease(float hor, float ver)
    {
        if (hor.Equals(0) && ver.Equals(0))
        {
            if (speed > minSpeed)
                speed -= 0.1f;
        }
        else
        {
            if (speed < maxSpeed)
                speed += 0.025f;
            else
                speed -= 0.1f;
        }

        if (hor < 0)
        {
            if (cmrOffset.x > minCmrX)
                cmrOffset.x -= 0.025f;
        }
        else
        {
            if (cmrOffset.x < maxCmrX)
                cmrOffset.x += 0.025f;
        }
    }

    private void HandleHolding()
    {
        isHoldingSpeed = Input.GetKey(KeyCode.LeftControl);
        if (isHoldingSpeed)
        {
            accumulatedSpeed += speedAccumulationRate * Time.deltaTime;
            accumulatedForce += forceAccumulationRate * Time.deltaTime;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            ApplyBoost();
        }

        an.SetBool("isHoldingSpeed", isHoldingSpeed);
    }

    private void ApplyBoost()
    {
        var hor = Input.GetAxis("Horizontal") * speed;

        if (hor == 0) return;

        var directionX = hor < 0 ? -1f : 1f;
        var finalSpeed = speed + accumulatedSpeed;

        boostDirection.x *= directionX;
        speed = finalSpeed;

        //rb.linearVelocity += Vector3.right * directionX * accumulatedForce * boostForceMultiplier;
        rb.AddForce(boostDirection * accumulatedForce * boostForceMultiplier, ForceMode.Impulse);

        accumulatedSpeed = 0f;
        accumulatedForce = 0f;
    }

    private void HandleJumping()
    {
        isJumping = Input.GetKeyDown(KeyCode.Space);
        if(isJumping)
        {
            accumulatedForce += forceAccumulationRate * Time.deltaTime;
            rb.AddForce(Vector3.up * 20f);
        }

        isJumping = Input.GetKeyUp(KeyCode.Space) ;
        if (isJumping)
        {
            canJump = false;
            accumulatedForce = 0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("ground"))
        {
            canJump = true;
            Debug.Log("HEY");
        }
    }
}
