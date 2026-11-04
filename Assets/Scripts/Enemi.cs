using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Ennemi : MonoBehaviour
{
    private GameObject objectif;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Initialiser(GameObject emplacementGeneration, GameObject objectif)
    {
        this.objectif = objectif;
        agent.enabled = false;

        // Positionnement de l'ennemi
        transform.SetPositionAndRotation(
            emplacementGeneration.transform.position,
            emplacementGeneration.transform.rotation);

        // Démarrage de l'agent
        agent.enabled = true;
        agent.SetDestination(objectif.transform.position);
    }
}