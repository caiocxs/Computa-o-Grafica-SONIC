using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Rigidbody rb;
    
    [Header("Configuração de KnockBack")]
    public PlayerMainScript playerMainScript;

    [Header("Configuração de KnockBack")]
    public float forceKnockBack = 10f;
    public float stunnedTime = 0.5f;
    public float invincibleTime = 2f; 
    public float yAxisNerf = 0.2f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    private void OnEnemyHit(Collider player)
    {
        var playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerMainScript = player.GetComponent<PlayerMainScript>();
            playerMainScript.SetCanMove(false);

            var direction = player.transform.position - transform.position;

            direction.y *= yAxisNerf;
            direction = direction.normalized;

            playerRb.linearVelocity = Vector3.zero;
            playerRb.AddForce(direction * forceKnockBack, ForceMode.Impulse);

            StartCoroutine(RecoverFromStun());
        }
    }

    IEnumerator RecoverFromStun()
    {
        yield return new WaitForSeconds(stunnedTime);
        playerMainScript.SetCanMove(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("HEY");

            OnEnemyHit(other);
        }
    }

}
