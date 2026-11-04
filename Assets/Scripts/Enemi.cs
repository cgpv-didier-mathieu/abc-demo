using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Ennemi : MonoBehaviour
{
    private GameObject objectif;

    private NavMeshAgent agent;

    [SerializeField, Tooltip("Nombre de points de vie maximaux de l'ennemi")]
    private float vieMaximale;

    // Nombre de points de vie actuels de l'ennemi. Si la valeur tombe à 0, l'ennemi est détruit.
    private float vieActuelle;

    [SerializeField, Tooltip("Barre de vie affichée au-dessus de l'ennemi")]
    private BarreProgression barreVie;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        vieActuelle = vieMaximale;

    }

    public void Initialiser(GameObject emplacementGeneration, GameObject objectif)
    {
        this.objectif = objectif;
        agent.enabled = false;

        // Réinitialise la vie et l'affichage
        vieActuelle = vieMaximale;
        barreVie.MettreAJourProgression(vieActuelle / vieMaximale);

        // Positionnement de l'ennemi
        transform.SetPositionAndRotation(
            emplacementGeneration.transform.position,
            emplacementGeneration.transform.rotation);

        // Démarrage de l'agent
        agent.enabled = true;
        agent.SetDestination(objectif.transform.position);
    }

    public void RecevoirDegats(float degats)
    {
        vieActuelle -= degats;
        barreVie.MettreAJourProgression(vieActuelle / vieMaximale);

        if (vieActuelle <= 0)
        {
            ControleurJeu.Instance.GenerateurEnnemis.RecyclerEnnemi(this);
        }
    }
}