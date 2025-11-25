using TreeEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMainScript : MonoBehaviour
{
    private Camera cmr;
    
    public float speed = 0f;
    public float minSpeed = 0.0f;
    public float maxSpeed = 9.0f;
    public float jumpSpeed = 0f;

    public Vector3 cmrOffset = new Vector3(0.75f, 1.5f, -2.85f);
    public float minCmrX;
    public float maxCmrX;

    public bool canJump = false;

    void Start()
    {
        cmr.transform.position = transform.position + cmrOffset;
        maxCmrX = cmrOffset.x;
    }

    void Update()
    {
        cmr.transform.position = transform.position + cmrOffset;
        Moviment();
        Jump();
    }

    private void Moviment()
    {
        var inputHor = Input.GetAxis("Horizontal") * speed; //  a/d
        var inputVer = Input.GetAxis("Vertical") * speed;   //  w/s

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
            if (cmrOffset.x > minCmrX)
                cmrOffset.x -= 0.025f;
        }
        else
        {
            if (cmrOffset.x < maxCmrX)
                cmrOffset.x += 0.025f;
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    private void Jump()
    {

    }
}
