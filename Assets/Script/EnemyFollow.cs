using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Transform player; // Référence vers l'objet du joueur
    private float speed; // Vitesse aléatoire de l'ennemi
    public float minSpeed = 2f; // Vitesse minimale des ennemis
    public float maxSpeed = 4f; // Vitesse maximale des ennemis
    public float stopDistance = 1.5f; // Distance minimum à laquelle l'ennemi s'arrête
    private Quaternion initialRotation; // Rotation initiale spécifique à chaque modèle
    private float groundHeight = 0f; // Hauteur du sol (y = 0)
    private float heightOffset; // Correction pour la hauteur en fonction du modèle

    void Start()
    {
        // Trouve automatiquement l'objet nommé "Player" dans la scène
        player = GameObject.Find("Player").transform;

        if (player == null)
        {
            Debug.LogError("Aucun objet nommé 'Player' trouvé dans la scène. Assurez-vous que le joueur est bien nommé 'Player'.");
            return;
        }

        // Attribue une vitesse aléatoire à cet ennemi
        speed = Random.Range(minSpeed, maxSpeed);

        // Stocke la rotation initiale de l'ennemi
        initialRotation = transform.rotation;

        // Calcule la hauteur du modèle pour corriger l'enfoncement dans le sol
        heightOffset = GetModelHeight();
    }

    void Update()
    {
        if (player == null) return;

        // Calcul de la distance entre l'ennemi et le joueur
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si l'ennemi est plus loin que stopDistance, il se déplace vers le joueur
        if (distanceToPlayer > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Ajuste la position pour que le mob reste au-dessus du sol
            transform.position = new Vector3(transform.position.x, groundHeight + heightOffset, transform.position.z);

            // Ajuste uniquement l'axe Y pour orienter l'ennemi vers le joueur
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0; // Ignore l'axe vertical
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

            // Combine la rotation initiale avec la nouvelle orientation
            transform.rotation = Quaternion.Euler(
                initialRotation.eulerAngles.x,
                targetRotation.eulerAngles.y,
                initialRotation.eulerAngles.z
            );
        }
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
            height = renderer.bounds.extents.y;
        }

        return height;
    }
}
