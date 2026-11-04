using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui gère le pool de projectiles pour réutiliser les instances et éviter les instanciations fréquentes.
/// </summary>
public class PoolProjectiles : MonoBehaviour
{
    /// <summary>
    /// Instance unique de la classe PoolProjectiles pour permettre un accès global.
    /// </summary>
    public static PoolProjectiles Instance { get; private set; }

    /// <summary>  
    /// Liste des instances de projectiles disponibles pour le réutiliser
    /// </summary>
    private List<Projectile> instancesDisponibles;

    [SerializeField, Tooltip("Prefab du projectile à utiliser")]
    private Projectile prefabProjectile;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        instancesDisponibles = new List<Projectile>();
    }

    /// <summary>
    /// Récupère un projectile disponible dans la liste ou crée une nouvelle instance si nécessaire.
    /// </summary>
    /// <returns>Une instance de projectile.</returns>
    public Projectile GetProjectile()
    {
        // Récupère un projectile disponible dans la liste
        if (instancesDisponibles.Count > 0)
        {
            Projectile projectile = instancesDisponibles[0];
            instancesDisponibles.RemoveAt(0);
            projectile.gameObject.SetActive(true);
            return projectile;
        }
        else
        {
            // Créez une nouvelle instance de projectile si nécessaire
            Projectile newProjectile = Instantiate(prefabProjectile, transform);
            return newProjectile;
        }
    }

    /// <summary>
    /// Replace un projectile dans la liste des instances disponibles pour le réutiliser plus tard.
    /// </summary>
    /// <param name="projectile">Le projectile à recycler.</param>
    public void RecyclerProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        instancesDisponibles.Add(projectile);
    }
}