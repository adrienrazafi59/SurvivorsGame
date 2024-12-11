using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 50f; 
    private Transform cameraTransform; 
    private Vector3 cameraOffset; 

    void Start()
    {
        cameraTransform = Camera.main.transform;

        if (cameraTransform == null)
        {
            Debug.LogError("Aucune caméra principale trouvée ! Vérifiez que votre caméra est bien taguée 'Main Camera'.");
            return;
        }
        cameraOffset = cameraTransform.position - transform.position;
    }

    void Update()
    {
        float moveX = Input.GetKey(KeyCode.A) ? -1 : (Input.GetKey(KeyCode.D) ? 1 : 0);
        float moveZ = Input.GetKey(KeyCode.W) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0);

        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        if (cameraTransform != null)
        {
            cameraTransform.position = transform.position + cameraOffset;

            cameraTransform.rotation = Quaternion.Euler(46.854f, 0f, 0f);
        }
    }
}
