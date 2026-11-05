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

        // On sait que le parent est un bouton, ce code est habituellement risqué à utiliser
        boutonAmelioration = boutonAmeliorationTexte.transform.parent.GetComponent<Button>();
    }

    /// <summary>
    /// Réagit au changement d'objet sélectionné
    /// </summary>
    /// <param name="_">Le vieille objet sélectionné</param>
    /// <param name="nouvelleSelection">Le nouvel objet sélectionné</param>
    public void OnObjetSelectionne(GameObject _, GameObject nouvelleSelection)
    {
        if (nouvelleSelection != null && nouvelleSelection.TryGetComponent(out Tour tour))
        {
            zoneAffichage.SetActive(true);
            AfficherTour(tour);
            tourAffichee = tour;
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
        nomTourTexte.text = tour != null ? tour.NomTour : "NULL";
    }
}