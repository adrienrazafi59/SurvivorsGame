using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public string playerName = "Player"; // Nom du joueur dans la hiérarchie
    public float moveSpeed = 5f;
    public LayerMask groundLayer; // Ajoutez une couche pour le sol
    private Transform player;
    private Rigidbody rb;
    private Quaternion initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialRotation = transform.rotation;

        // Trouve le joueur par son nom dans la hiérarchie
        GameObject playerObject = GameObject.Find(playerName);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene. Make sure there is a GameObject named 'Player'.");
        }
    }

    void Update()
    {
        if (player == null) return;

        // Ajuste la rotation pour regarder le joueur
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0; // Ignore l'axe vertical
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        // Combine la rotation initiale avec la nouvelle orientation
        transform.rotation = Quaternion.Euler(
            initialRotation.eulerAngles.x,
            targetRotation.eulerAngles.y,
            initialRotation.eulerAngles.z
        );

        // Déplace l'ennemi vers le joueur
        Vector3 moveDirection = lookDirection.normalized;
        rb.velocity = moveDirection * moveSpeed;

        // Ajuste la position verticale pour éviter que l'ennemi ne s'enfonce dans le sol
        AdjustHeight();
    }

    void AdjustHeight()
    {
        float groundHeight = 0f; // Hauteur du terrain
        float modelHeight = GetModelHeight();
        transform.position = new Vector3(transform.position.x, groundHeight + modelHeight, transform.position.z);
    }

    // Fonction pour calculer la hauteur du modèle à partir de son collider ou renderer
    private float GetModelHeight()
    {
        float height = 1f; // Valeur par défaut si aucun collider ni renderer n'est trouvé

        // Tente d'utiliser le Collider pour obtenir la hauteur
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            height = collider.bounds.extents.y; // Hauteur depuis le centre jusqu'au sommet
            return height;
        }

        // Si pas de collider, tente d'utiliser le Renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            height = renderer.bounds.extents.y; // Hauteur depuis le centre jusqu'au sommet
        }

        return height;
    }
}
