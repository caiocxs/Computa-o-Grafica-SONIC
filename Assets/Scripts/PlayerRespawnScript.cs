using Unity.VisualScripting;
using UnityEngine;

public class PlayerRespawnScript : MonoBehaviour
{
    [Header("Referências")]
    public Transform respawnPoint;

    void Start()
    {
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("water"))
        {
            transform.position = respawnPoint.position;
        }
    }
}