using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Alvo")]
    public Transform player;

    [Header("Configurações de Rotação")]
    public float sensibility = 5.0f;
    public float minX = -20f;
    public float maxX = 30f;
    public float minY = -20f;
    public float maxY = 30f;

    [Header("Configurações de Zoom")]
    public float distance = 5.0f;     
    public float zoomSpeed = 4.0f; 
    public float minZoom = 2.0f;       
    public float maxZoom = 15.0f;      

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        rotationX = transform.eulerAngles.y;
        rotationY = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        if (player == null) return;

        float mouseX = Input.GetAxis("Mouse X") * sensibility;
        float mouseY = Input.GetAxis("Mouse Y") * sensibility;

        rotationX += mouseX;
        rotationX = Mathf.Clamp(rotationX, minX, maxX);

        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        distance -= scroll * zoomSpeed;

        distance = Mathf.Clamp(distance, minZoom, maxZoom);

        Quaternion rotacao = Quaternion.Euler(rotationY, rotationX, 0);

        Vector3 posicaoNegativa = new Vector3(0.0f, 0.0f, -distance);
        Vector3 posicaoFinal = CheckCollision(rotacao * posicaoNegativa + player.position);

        transform.rotation = rotacao;
        transform.position = posicaoFinal;
    }

    Vector3 CheckCollision(Vector3 targetPos)
    {
        RaycastHit hit;
        Vector3 direcao = targetPos - player.position;

        return targetPos;
        if (Physics.Raycast(player.position, direcao.normalized, out hit, distance))
        {
            return hit.point;
        }
    }
}