using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gère l'affichage d'une barre de progression dans l'interface utilisateur.
public class BarreProgression : MonoBehaviour
{
    [SerializeField, Tooltip("Image représentant la barre de progression")]
    private Image barreInterne;

    [SerializeField, Tooltip("Valeur initiale de la barre de progression")]
    private float valeurInitiale = 0.0f;

    private void Start()
    {
        MettreAJourProgression(valeurInitiale);
    }

    /// <summary>
    /// Met à jour la progression de la barre de progression.
    /// </summary>
    /// <param name="progression">La progression de la barre, comprise entre 0 et 1</param>
    public void MettreAJourProgression(float progression)
    {
        barreInterne.fillAmount = Mathf.Clamp01(progression);
    }
}