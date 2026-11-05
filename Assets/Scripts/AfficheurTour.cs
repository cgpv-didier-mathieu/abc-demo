using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Affiche une tour dans l'interface
/// </summary>
public class AfficheurTour : MonoBehaviour
{
    [SerializeField, Tooltip("Zone d'affichage de la sélection")]
    private GameObject zoneAffichage;

    [SerializeField, Tooltip("Référence au texte affichant le nom de la tour")]
    TextMeshProUGUI nomTourTexte;

    [SerializeField, Tooltip("Référence au texte affiché dans le bouton d'amélioration")]
    TextMeshProUGUI boutonAmeliorationTexte;

    // Bouton d'amélioration de la tour
    Button boutonAmelioration;

    // Référence à la tour actuellement affichée
    private Tour tourAffichee;

    private void Start()
    {
        ControleurJeu.Instance.GestionnaireSourisParDefaut.ObjetSelectionne += OnObjetSelectionne;

        // Caché tant qu'aucune tour n'est sélectionnée
        zoneAffichage.SetActive(false);

        // On sait que le parent est un bouton, ce code est habituellement risqué à utiliser
        boutonAmelioration = boutonAmeliorationTexte.transform.parent.GetComponent<Button>();
    }

    private void OnDestroy()
    {
        if (ControleurJeu.Instance != null && ControleurJeu.Instance.GestionnaireSourisParDefaut != null)
            ControleurJeu.Instance.GestionnaireSourisParDefaut.ObjetSelectionne -= OnObjetSelectionne;

        if (tourAffichee != null)
            tourAffichee.NiveauChange -= RafraichirTourAffichee;
    }

    // Met à jour l'affichage quand la tour affichée change de niveau
    private void RafraichirTourAffichee()
    {
        if (tourAffichee != null)
            AfficherTour(tourAffichee);
    }

    /// <summary>
    /// Réagit au changement d'objet sélectionné
    /// </summary>
    /// <param name="_">Le vieille objet sélectionné</param>
    /// <param name="nouvelleSelection">Le nouvel objet sélectionné</param>
    public void OnObjetSelectionne(GameObject _, GameObject nouvelleSelection)
    {
        // On cesse d'écouter l'ancienne tour
        if (tourAffichee != null)
            tourAffichee.NiveauChange -= RafraichirTourAffichee;

        if (nouvelleSelection != null && nouvelleSelection.TryGetComponent(out Tour tour))
        {
            zoneAffichage.SetActive(true);
            tourAffichee = tour;
            tourAffichee.NiveauChange += RafraichirTourAffichee;
            AfficherTour(tour);
        }
        else
        {
            zoneAffichage.SetActive(false);
            tourAffichee = null;
        }
    }

    /// <summary>
    /// Lance l'amélioration de la tour affichée
    /// </summary>
    public void Ameliorer()
    {
        if (tourAffichee != null)
        {
            tourAffichee.Ameliorer();
        }
    }

    /// <summary>
    /// Affiche le contenu d'une tour
    /// </summary>
    /// <param name="tour">La tour à afficher</param>
    private void AfficherTour(Tour tour)
    {
        // Met à jour le texte du bouton d'amélioration en fonction du niveau de la tour
        if (tour.Niveau >= Tour.NIVEAU_MAX)
        {
            boutonAmeliorationTexte.text = "Niveau max";
            boutonAmelioration.interactable = false;
        }
        else
        {
            boutonAmeliorationTexte.text = $"Améliorer au niveau {tour.Niveau + 1}";
            boutonAmelioration.interactable = true;
        }

        // Met à jour le texte du nom de la tour
        nomTourTexte.text = string.IsNullOrEmpty(tour.NomTour) ? "Tour" : tour.NomTour;
    }
}