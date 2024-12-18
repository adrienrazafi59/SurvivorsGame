using UnityEngine;
using System.Collections; // Ajoutez ceci pour utiliser IEnumerator et les coroutines

public class Attack : MonoBehaviour
{
    public GameObject hitboxPrefab; // Prefab de la hitbox (doit avoir un BoxCollider en Trigger)
    public LayerMask placementLayer; // Définir quel layer peut recevoir la hitbox
    public Vector3 hitboxSize = new Vector3(3f, 3f, 3f); // Taille du cube de la hitbox (3x3x3)
    private float disappearDelay = 0.1f; // Temps avant que la hitbox disparaisse si elle est vide

    void Update()
    {
        // Vérifier si le bouton gauche de la souris est cliqué
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Click");

            // Créer un rayon de la caméra vers la position de la souris
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Vérifier si le rayon touche un objet
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                // Vérifier si l'objet a le tag "Enemy"
                if (clickedObject.CompareTag("Enemy"))
                {
                    // Détruire l'ennemi si il est cliqué (optionnel)
                    Destroy(clickedObject);
                }
                else if (((1 << hit.collider.gameObject.layer) & placementLayer) != 0)
                {
                    // Instancier la hitbox (cube 3x3x3) à la position du clic
                    GameObject hitbox = Instantiate(hitboxPrefab, hit.point, Quaternion.identity);

                    // Redimensionner la hitbox à 3x3x3
                    hitbox.transform.localScale = hitboxSize;

                    // Ajouter un BoxCollider si la hitbox n'en a pas
                    BoxCollider boxCollider = hitbox.GetComponent<BoxCollider>();
                    if (boxCollider == null)
                    {
                        boxCollider = hitbox.AddComponent<BoxCollider>();
                    }
                    boxCollider.isTrigger = true;

                    // Ajouter le script pour la gestion des ennemis à l'intérieur de la hitbox
                    Hitbox hitboxScript = hitbox.AddComponent<Hitbox>();
                    hitboxScript.SetDisappearDelay(disappearDelay); // Passer le délai de disparition

                    // Lancer la coroutine pour vérifier si la hitbox doit disparaître après un délai
                    StartCoroutine(hitboxScript.CheckForEnemiesAndDisappear());
                }
            }
        }
    }

    // Classe interne pour gérer la hitbox et la détection des ennemis
    public class Hitbox : MonoBehaviour
    {
        private Attack attackScript;
        private bool enemyInside = false;
        private float disappearDelay;

        // Définir le délai de disparition
        public void SetDisappearDelay(float delay)
        {
            disappearDelay = delay;
        }

        // Coroutine pour vérifier s'il y a des ennemis et faire disparaître la hitbox si vide
        public IEnumerator CheckForEnemiesAndDisappear()
        {
            // Attendre le délai de disparition
            yield return new WaitForSeconds(disappearDelay);

            // Si aucun ennemi n'est détecté, détruire la hitbox
            if (!enemyInside)
            {
                Destroy(gameObject);
            }
        }

        // Détecter un ennemi entrant dans la hitbox
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                enemyInside = true;
                // Détruire l'ennemi et la hitbox
                Destroy(other.gameObject);  // Détruire l'ennemi
                Destroy(gameObject);  // Détruire la hitbox
            }
        }

        // Détecter si un ennemi reste dans la hitbox
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                enemyInside = true;
                // Détruire l'ennemi et la hitbox
                Destroy(other.gameObject);  // Détruire l'ennemi
                Destroy(gameObject);  // Détruire la hitbox
            }
        }
    }
}
