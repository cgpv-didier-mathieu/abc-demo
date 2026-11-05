using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gère les opérations générales de contrôle du jeu.
/// </summary>
public class ControleurJeu : MonoBehaviour
{
    /// <summary>
    /// Singleton du contrôleur de jeu.
    /// </summary>
    public static ControleurJeu Instance { get; private set; }

    // Objet responsable du contrôle de la souris dans le jeu.
    private IGestionnaireSouris gestionnaireSouris;

    [field: SerializeField, Tooltip("Référence au gestionnaire de souris par défaut")]
    public GestionnaireSouris GestionnaireSourisParDefaut { get; private set; }

    [field: SerializeField, Tooltip("Référence au PlayerInput pour gérer les entrées")]
    public PlayerInput Controles { get; private set; }

    [field: SerializeField, Tooltip("Référence à la caméra principale")]
    public Camera CameraPrincipale { get; private set; }

    [field: SerializeField, Tooltip("Référence au générateur d'ennemis")]
    public GenerateurEnnemis GenerateurEnnemis { get; private set; }

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
    }

    private void Start()
    {
        // Prendre le contrôle de la souris par défaut au démarrage du jeu
        PrendreControleSouris(GestionnaireSourisParDefaut);
    }

    #region Contrôle de la souris
    /// <summary>
    /// Attribue le contrôle de la souris au gestionnaire spécifié après avoir libéré le gestionnaire actuel, le cas
    /// échéant.
    /// </summary>
    /// <param name="gestionnaire">Gestionnaire qui prend le contrôle de la souris.</param>
    /// <param name="forcerLiberation">Indique si la libération du contrôle doit être forcée avant le transfert.</param>
    public void PrendreControleSouris(IGestionnaireSouris gestionnaire, bool forcerLiberation = true)
    {
        if (gestionnaireSouris != null)
        {
            if (forcerLiberation)
            {
                gestionnaireSouris.LibererControleSouris();
            }
            else
            {
                Debug.LogWarning("Un gestionnaire de souris est déjà actif. Libération forcée non autorisée.");
                return;
            }
        }
        gestionnaireSouris = gestionnaire;
        gestionnaireSouris.PrendreControleSouris();
    }

    /// <summary>
    /// Libère le contrôle de la souris pour le gestionnaire indiqué en paramètre.
    /// </summary>
    /// <param name="gestionnaire">Le gestionnaire à libérer</param>
    public void LibererControleSouris(IGestionnaireSouris gestionnaire)
    {
        if (gestionnaireSouris == gestionnaire)
        {
            gestionnaireSouris.LibererControleSouris();
            gestionnaireSouris = null;

            if (GestionnaireSourisParDefaut != null)
            {
                PrendreControleSouris(GestionnaireSourisParDefaut);
            }
        }
    }
    #endregion
}