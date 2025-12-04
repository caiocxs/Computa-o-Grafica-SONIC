using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Rigidbody rb;
    
    [Header("Configuração de Perseguir")]
    public PlayerMainScript playerMainScript;
    public Transform player;

    public float velocity = 4f;
    public float vision = 10f;

    [Header("Configuração de KnockBack")]
    public float forceKnockBack = 10f;
    public float stunnedTime = 0.5f;
    public float invincibleTime = 2f; 
    public float yAxisNerf = 0.2f;

    public float minimumDistance = 0.5f;

    public bool canFly = false;
    private bool gotHit = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!gotHit)
            HandleFollowPlayer();
    }

    private void OnEnemyHit(Collider player)
    {
        var playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null && !gotHit)
        {
            playerMainScript = player.GetComponent<PlayerMainScript>();
            playerMainScript.SetCanMove(false);

            var direction = player.transform.position - transform.position;

            direction.y *= yAxisNerf;
            direction = direction.normalized;

            playerRb.linearVelocity = Vector3.zero;
            playerRb.AddForce(direction * forceKnockBack, ForceMode.Impulse);

            StartCoroutine(RecoverFromStun());
            gotHit = true;
            StartCoroutine(HandleInvincibility());
        }
    }

    IEnumerator RecoverFromStun()
    {
        yield return new WaitForSeconds(stunnedTime);
        playerMainScript.SetCanMove(true);
    }

    private IEnumerator HandleInvincibility()
    {
        yield return new WaitForSeconds(invincibleTime);
        gotHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            OnEnemyHit(other);
        }
    }

    private void HandleFollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= vision && distance > minimumDistance)
        {

            Vector3 playerOnGround = new Vector3(player.position.x, transform.position.y, player.position.z);
            if(canFly)
                transform.LookAt(player);
            else 
                transform.LookAt(playerOnGround);
            
            transform.position += transform.forward * velocity * Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, vision);
    }
}
