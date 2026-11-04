using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Responsable de la création des tours. Elle instancie une tour de niveau 1, la fait suivre la souris, puis la place sur 
/// le terrain lors du clic gauche. Le clic droit annule l'action et détruit la tour.
/// 
/// Si le joueur n'a pas les ressources, la tour n'est pas placée et un message d'erreur est affiché.
/// </summary>
public class FabriqueTour : MonoBehaviour
{
    [SerializeField, Tooltip("Référence aux zones de construction de tour")]
    private List<GameObject> zoneConstructions;

    /// <summary>
    /// Alias pour accès rapide au singleton du contrôleur de jeu.
    /// </summary>
    private ControleurJeu controleurJeu;

    [SerializeField, Tooltip("Le prefab de la tour à instancier")]
    private Tour prefabTour;

    /// <summary>
    /// L'instance de la tour en cours de placement. 
    /// </summary>
    private Tour tourEnPlacement;

    private void Start()
    {
        controleurJeu = ControleurJeu.Instance;

        // Remplit les zones de construction avec les enfants qui ont le tag "ZoneConstruction"
        zoneConstructions = new();

        foreach (Transform enfant in transform)
        {
            if (enfant.CompareTag("ZoneConstruction"))
            {
                zoneConstructions.Add(enfant.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        DesabonnerEvenements();
    }

    #region Gestion des zones de construction
    /// <summary>
    /// Affiche les zones de construction de tour. Ces zones sont activées pour indiquer au joueur où il peut placer une tour.
    /// </summary>
    private void AfficherZonesConstruction()
    {
        // Activer les zones de construction
        foreach (GameObject zone in zoneConstructions)
        {
            zone.SetActive(true);
        }
    }

    /// <summary>
    /// Masque les zones de construction de tour. 
    /// </summary>
    private void MasquerZonesConstruction()
    {
        foreach (GameObject zone in zoneConstructions)
        {
            zone.SetActive(false);
        }
    }
    #endregion

    #region Gestion du placement de la tour

    /// <summary>
    /// Commence le processus de placement d'une tour. Instancie une tour de niveau 1 et la fait suivre la souris.
    /// </summary>
    public void CommencerPlacement()
    {
        tourEnPlacement = Instantiate(prefabTour);
        AfficherZonesConstruction();

        InputActionAsset actions = controleurJeu.Controles.actions;
        actions["ClicGauche"].performed += PlacerTour;
        actions["ClicDroit"].performed += AnnulerPlacement;
        actions["DeplacerSouris"].performed += SuivreSouris;

    }

    /// <summary>
    /// Se désabonne des événements liés au placement de la tour.
    /// </summary>
    private void DesabonnerEvenements()
    {
        if (controleurJeu.Controles == null)
            return;

        InputActionAsset actions = controleurJeu.Controles.actions;
        actions["ClicGauche"].performed -= PlacerTour;
        actions["ClicDroit"].performed -= AnnulerPlacement;
        actions["DeplacerSouris"].performed -= SuivreSouris;
    }

    /// <summary>
    /// Assure que la tour suit la position de la souris sur le terrain. La tour est placée à la position de la souris, 
    /// mais ne peut pas être placée sur des zones interdites.
    /// </summary>
    public void SuivreSouris(InputAction.CallbackContext contexte)
    {
        Vector2 positionSouris = contexte.ReadValue<Vector2>();
        Ray rayon = controleurJeu.CameraPrincipale.ScreenPointToRay(positionSouris);

        if (Physics.Raycast(rayon, out RaycastHit infoRaycast, Mathf.Infinity, LayerMask.GetMask("Carte")))
        {
            tourEnPlacement.transform.position = infoRaycast.point;
        }
    }

    /// <summary>
    /// Assigne la tour à la zone de construction.
    /// </summary>
    public void PlacerTour(InputAction.CallbackContext contexte)
    {
        Vector2 positionSouris = Mouse.current.position.ReadValue();
        Ray rayon = controleurJeu.CameraPrincipale.ScreenPointToRay(positionSouris);

        if (Physics.Raycast(rayon, out RaycastHit infoRaycast, Mathf.Infinity, LayerMask.GetMask("Carte")))
        {
            if (infoRaycast.collider.CompareTag("ZoneConstruction"))
            {
                // Variable locale pour faciliter l'accès
                GameObject zone = infoRaycast.collider.gameObject;

                // Positionne la tour à l'emplacement de la zone de construction.
                tourEnPlacement.transform.position = zone.transform.position;
                tourEnPlacement.Placer();

                // Mets fin au placement de la tour et désactive les zones de construction.
                tourEnPlacement = null;
                MasquerZonesConstruction();

                // On supprime la zone de construction de la liste pour qu'elle ne soit plus affichée.
                zoneConstructions.Remove(zone);

                DesabonnerEvenements();
            }
        }
    }

    /// <summary>
    /// Annule le placement de la tour et détruit l'instance de la tour.
    /// </summary>
    public void AnnulerPlacement(InputAction.CallbackContext contexte)
    {
        if (tourEnPlacement != null)
        {
            Destroy(tourEnPlacement.gameObject);
            tourEnPlacement = null;
        }

        MasquerZonesConstruction();
        DesabonnerEvenements();
    }

    #endregion
}