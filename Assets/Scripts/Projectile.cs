using UnityEngine;

/// <summary>
/// Projectile lancé par la tour pour attaquer les ennemis.
/// </summary>
public class Projectile : MonoBehaviour
{
    // Cible que le projectile doit atteindre
    private GameObject cible;

    [SerializeField, Tooltip("Vitesse du projectile en m/s")]
    private float vitesse = 5.0f;

    [SerializeField, Tooltip("Temps de vie maximal du projectile en secondes")]
    private float tempsVieMaximal = 5.0f;

    private float tempsVie;

    private void Awake()
    {
        tempsVie = 0.0f;
    }

    /// <summary>
    /// Initialise le projectile avec la cible à atteindre.
    /// </summary>
    /// <param name="cible">La cible que le projectile doit atteindre</param>
    /// <param name="positionInitiale">La position initiale du projectile</param>
    /// <param name="rotationInitiale">La rotation initiale du projectile</param>
    public void Initialiser(GameObject cible, Vector3 positionInitiale, Quaternion rotationInitiale)
    {
        this.cible = cible;
        transform.SetPositionAndRotation(positionInitiale, rotationInitiale);
    }

    private void Update()
    {
        // Détruire le projectile s'il dépasse son temps de vie maximal
        if (tempsVie > tempsVieMaximal)
        {
            PoolProjectiles.Instance.RecyclerProjectile(this);
        }

        if (cible != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, cible.transform.position, vitesse * Time.deltaTime);
        }
        else
        {
            // Si la cible n'existe plus, le projectile continue tout droit
            transform.position += transform.forward * vitesse * Time.deltaTime;
        }

        // Mettre à jour le temps de vie du projectile
        tempsVie += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider autre)
    {
        if (autre.gameObject == cible)
        {
            // Logique de dégâts ou d'effet sur la cible
            Debug.Log("Projectile a touché la cible : " + cible.name);
            Destroy(gameObject); // Détruire le projectile après l'impact
        }
    }
}