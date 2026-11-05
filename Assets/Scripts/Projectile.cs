using UnityEngine;

/// <summary>
/// Projectile lancé par la tour pour attaquer les ennemis.
/// </summary>
public class Projectile : MonoBehaviour
{
    // Cible que le projectile doit atteindre
    private Ennemi cible;

    [SerializeField, Tooltip("Vitesse du projectile en m/s")]
    private float vitesse = 5.0f;

    [SerializeField, Tooltip("Temps de vie maximal du projectile en secondes")]
    private float tempsVieMaximal = 5.0f;

    private float tempsVie;

    // Dégâts infligés par le projectile
    private float degats;

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
    /// <param name="degats">Les dégâts infligés par le projectile</param>
    public void Initialiser(Ennemi cible, Vector3 positionInitiale, Quaternion rotationInitiale, float degats)
    {
        this.cible = cible;
        this.degats = degats;
        tempsVie = 0f;
        transform.SetPositionAndRotation(positionInitiale, rotationInitiale);
    }

    private void Update()
    {
        // Détruire le projectile s'il dépasse son temps de vie maximal
        if (tempsVie > tempsVieMaximal)
        {
            PoolProjectiles.Instance.RecyclerProjectile(this);
            return;
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
        if (cible != null && autre.gameObject == cible.gameObject)
        {
            cible.RecevoirDegats(degats);
            PoolProjectiles.Instance.RecyclerProjectile(this);
        }
    }
}