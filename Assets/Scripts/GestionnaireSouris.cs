using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gestionnaire par défaut des événements de la souris. Lorsque le gestionnaire est actif, il vérifie si le clic tombe sur certains
/// objets et les affiche dans l'interface 
/// </summary>
public class GestionnaireSouris : MonoBehaviour, IGestionnaireSouris
{
    // Objet actuellement sélectionné par la souris
    private GameObject objetActif;

    // Evénement déclenché lorsqu'un objet est sélectionné par la souris. Le premier paramètre est l'objet précédemment sélectionné,
    // le second est l'objet actuellement sélectionné.
    public event Action<GameObject, GameObject> ObjetSelectionne;

    private void OnDestroy()
    {
        DesabonnerEvenements();
    }

    ///Abonne l’action "ClicGauche" à l’événement <c>performed</c> pour déclencher <c>DetecterObjet</c> lors d’un clic
    /// gauche.
    public void PrendreControleSouris()
    {
        InputActionAsset actions = ControleurJeu.Instance.Controles.actions;

        actions["ClicGauche"].performed += DetecterObjet;
        actions["ClicDroit"].performed += LibererObjetActif;
    }

    /// <inheritdoc/>

    public void LibererControleSouris()
    {
        DesabonnerEvenements();
    }

    /// <summary>
    /// Désabonne les événements de clic gauche et droit pour éviter les fuites de mémoire et les comportements inattendus.
    /// </summary>
    private void DesabonnerEvenements()
    {
        if (ControleurJeu.Instance == null || ControleurJeu.Instance.Controles == null)
        {
            return;
        }

        InputActionAsset actions = ControleurJeu.Instance.Controles.actions;

        if (actions != null)
        {
            actions["ClicGauche"].performed -= DetecterObjet;
            actions["ClicDroit"].performed -= LibererObjetActif;
        }
    }

    /// <summary>
    /// Détecte l'objet sur lequel la souris a cliqué et effectue une action en conséquence. 
    /// </summary>
    /// <param name="context">Le contexte de l'action</param>
    private void DetecterObjet(InputAction.CallbackContext context)
    {
        Ray ray = ControleurJeu.Instance.CameraPrincipale.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Selectionnable")))
        {
            // Si l'objet est une tour
            if (hit.collider.TryGetComponent<Tour>(out Tour tour))
            {
                SelectionnerObjet(tour.gameObject);
            }
        }
    }

    /// <summary>
    /// Met à jour l’objet actif et déclenche l’événement de changement de sélection avec l’objet précédemment actif et
    /// le nouvel objet actif.
    /// </summary>
    /// <remarks>L’événement de sélection est invoqué uniquement s’il existe au moins un abonné.</remarks>
    /// <param name="gameObject">Objet à définir comme sélection active.</param>
    private void SelectionnerObjet(GameObject gameObject)
    {
        GameObject objetPrecedent = objetActif;
        objetActif = gameObject;
        ObjetSelectionne?.Invoke(objetPrecedent, objetActif);
    }

    /// <summary>
    /// Libère ce qui est sélectionné comme objet
    /// </summary>
    /// <param name="gameObject">Le contexte de l'action</param>
    private void LibererObjetActif(InputAction.CallbackContext context)
    {
        GameObject objetPrecedent = objetActif;
        objetActif = null;
        ObjetSelectionne?.Invoke(objetPrecedent, null);

    }
}