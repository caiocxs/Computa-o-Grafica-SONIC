using TreeEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMainScript : MonoBehaviour
{
    private Rigidbody rb;
    private SpriteRenderer sr;
    private int spriteLimit;

    public float speed = 0f;
    public float minSpeed = 0.0f;
    public float maxSpeed = 9.0f;
    public float jumpSpeed = 0f;
    public float currentSpeed = 0f;

    public Camera cmr;
    private Vector3 cmrOffset = new Vector3(0.75f, 1.5f, -2.85f);
    public float minCmrX;
    public float maxCmrX;

    public Sprite[] spritesIdle;
    public Sprite[] spritesWalking;
    public Sprite[] spritesRunning;
    public int currentSprite = 0;
    public float offset = 1.5f;
    public float spriteCounter = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        
        spriteLimit = spritesIdle.Length;
        rb.freezeRotation = true;

        cmr.transform.position = transform.position + cmrOffset;
        maxCmrX = cmrOffset.x;
    }

    void Update()
    {
        cmr.transform.position = transform.position + cmrOffset;
        spriteCounter += Time.deltaTime;
        Moviment();
    }

    private void Moviment()
    {
        var inputHor = Input.GetAxis("Horizontal") * speed; //  a/d
        var inputVer = Input.GetAxis("Vertical") * speed;   //  w/s
        var moviment = new Vector3(inputHor, 0, inputVer);

        HandleSprites(inputHor, inputVer);

        if (inputHor.Equals(0) && inputVer.Equals(0))
        {
            if (speed > minSpeed)
                speed -= 0.1f;
        }
        else
            if (speed < maxSpeed)
                speed += 0.025f;

        if (inputHor < 0)
        {
            sr.flipX = true;

            if (cmrOffset.x > minCmrX)
                cmrOffset.x -= 0.025f;
        }
        else
        {
            sr.flipX = false;
            if (cmrOffset.x < maxCmrX)
                cmrOffset.x += 0.025f;
        }

        Vector3 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(moviment.x, currentVelocity.y, moviment.z);
    }

    private void HandleSprites(float hor, float ver)
    {
        currentSpeed = rb.linearVelocity.magnitude;
        if (spriteCounter > offset)
        {
            spriteCounter = 0.0f;

            if (hor != 0)
            {
                if (ver != 0)
                {
                    WalkOrRunSprite();
                }
                else
                {
                    WalkOrRunSprite();
                }
            }
            else
            {
                if (ver != 0)
                {
                    WalkOrRunSprite();
                }
                else
                {
                    sr.sprite = spritesIdle[currentSprite++];
                }
            }

            if (currentSprite > spriteLimit - 1)
                currentSprite = 0;
        }
    }

    private void WalkOrRunSprite()
    {
        if (currentSpeed > maxSpeed / 2)
            sr.sprite = spritesRunning[currentSprite++];
        else
            sr.sprite = spritesWalking[currentSprite++];
    }
}
