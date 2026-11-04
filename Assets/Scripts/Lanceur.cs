using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lanceur d'une tour qui permet de lancer des projectiles vers les ennemis.
/// </summary>
public class Lanceur : MonoBehaviour
{
    // Liste des cibles à portée
    private List<EnnemiFactice> cibles;

    // Cible actuellement attaquée par la tour
    private EnnemiFactice cibleActive;

    [SerializeField, Tooltip("Prefab du projectile de la tour")]
    private Projectile prefabProjectile;

    [SerializeField, Tooltip("Delais entre chaque attaque (en secondes)")]
    private float delaiAttaque;

    private void Awake()
    {
        cibles = new List<EnnemiFactice>();
    }

    /// <summary>
    /// Déclenche l'attaque de la tour en instanciant un projectile et en le dirigeant vers la cible active.
    /// </summary>
    private void Attaquer()
    {
        Projectile projectile = PoolProjectiles.Instance.GetProjectile();
        projectile.Initialiser(cibleActive.gameObject, transform.position, transform.rotation);
    }

    /// <summary>
    /// Coroutine qui gère l'attaque répétée de la tour tant qu'une cible est présente.
    /// </summary>
    /// <returns>IEnumerator pour la coroutine</returns>
    private IEnumerator CoroutineAttaque()
    {
        while (cibleActive != null)
        {
            Attaquer();
            yield return new WaitForSeconds(delaiAttaque);
        }
    }

    /// <summary>
    /// Lorsqu'un objet entre dans le collider de la tour, on vérifie s'il s'agit d'un ennemi et on l'ajoute à la liste des cibles.
    /// </summary>
    /// <param name="autreObjet">Le collider de l'objet entrant</param>
    private void OnTriggerEnter(Collider autreObjet)
    {
        if (autreObjet.TryGetComponent(out EnnemiFactice ennemi))
        {
            Debug.Log("Un ennemi est entré dans la zone de la tour : " + ennemi.name);

            cibles.Add(ennemi);

            // Aucune cible n'est actuellement attaquée, on attaque la première cible qui entre dans la zone
            if (cibleActive == null)
            {
                cibleActive = ennemi;
                StartCoroutine(CoroutineAttaque());
            }
        }
    }

    /// <summary>
    /// Lorsqu'un objet sort du collider de la tour, on vérifie s'il s'agit d'un ennemi et on le retire de la liste des cibles.
    /// </summary>
    /// <param name="autreObjet">Le collider de l'objet sortant</param>
    private void OnTriggerExit(Collider autreObjet)
    {
        if (autreObjet.TryGetComponent(out EnnemiFactice ennemi))
        {
            Debug.Log("Un ennemi est sorti de la zone de la tour : " + ennemi.name);

            cibles.Remove(ennemi);

            // La cible attaquée sort de la zone, on passe à la prochaine cible si elle existe
            if (cibleActive == ennemi)
            {
                if (cibles.Count > 0)
                {
                    cibleActive = cibles[0];
                }
                else
                {
                    cibleActive = null;
                }
            }
        }
    }
}