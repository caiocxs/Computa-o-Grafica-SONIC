using UnityEngine;
using UnityEngine.InputSystem.Users;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class SpriteScript : MonoBehaviour
{
    public PlayerMainScript player;

    private Rigidbody rb;
    private SpriteRenderer sr;
    private int spriteLimit;

    private float currentlyOffset;

    public Sprite[] spritesIdle;
    public Sprite[] spritesWalking;
    public Sprite[] spritesRunning;
    public int currentSprite = 0;
    public float offset = 1.5f;
    public float minOffset = 1.0f;

    public float spriteCounter = 0.0f;

    public float maxSpeed = 9.0f;
    public float currentSpeed = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        
        rb.freezeRotation = true;

        spriteLimit = spritesIdle.Length;

        currentlyOffset = offset;
    }

    void Update()
    {
        spriteCounter += Time.deltaTime;
        HandleSprites();
    }

    private void HandleSprites()
    {
        var hor = Input.GetAxis("Horizontal") * player.speed; //  a/d
        var ver = Input.GetAxis("Vertical") * player.speed;   //  w/s
        var moviment = new Vector3(hor, 0, ver);

        Vector3 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(moviment.x, currentVelocity.y, moviment.z);
        if (hor < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        currentSpeed = rb.linearVelocity.magnitude;

        if (spriteCounter > currentlyOffset)
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
                    currentlyOffset = offset;
                }
            }

            if (currentSprite > spriteLimit - 1)
                currentSprite = 0;
        }
    }

    private void WalkOrRunSprite()
    {
        if (currentSpeed > maxSpeed / 2)
        {
            sr.sprite = spritesRunning[currentSprite++];
            if (currentlyOffset > minOffset)
                currentlyOffset -= 0.1f;
        }
        else
        {
            sr.sprite = spritesWalking[currentSprite++];
            currentlyOffset = offset;
        }
    }

}
