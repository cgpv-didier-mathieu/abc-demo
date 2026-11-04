using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// Génère des ennemis à intervalles réguliers et les place dans la scène.
/// </summary>
public class GenerateurEnnemis : MonoBehaviour
{
    [SerializeField, Tooltip("Emplacement où les ennemis sont générés")]
    private GameObject emplacementGeneration;

    [SerializeField, Tooltip("Destination des ennemis")]
    private GameObject destinationEnnemis;

    [SerializeField, Tooltip("Prefab de l'ennemi à générer")]
    private Ennemi prefabEnnemi;

    [SerializeField, Tooltip("Délai entre chaque vague d'ennemis (en secondes)")]
    private float delaiEntreVague;

    [SerializeField, Tooltip("Nombre d'ennemis par vague")]
    private float tailleVague;

    [SerializeField, Tooltip("Nombre de vagues à générer")]
    private int nombreVagues;

    [SerializeField, Tooltip("Délai entre chaque ennemi généré (en secondes)")]
    private float delaiEntreEnnemi;

    // Est-ce qu'une vague est en cours de génération
    private bool vagueEnCours;

    // Indice de la vague actuelle
    private int indiceVague;

    // Liste des ennemis générés dans la scène
    private List<Ennemi> instancesEnnemis;

    #region Evenements
    public Action<Ennemi> EnnemiRecycle;
    #endregion

    private void Start()
    {
        vagueEnCours = false;
        indiceVague = 0;
        instancesEnnemis = new();
        StartCoroutine(GenererVagueNiveau());
    }

    /// <summary>
    /// Génère les différentes vagues pour un niveau entier
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenererVagueNiveau()
    {
        for (; indiceVague < nombreVagues; indiceVague++)
        {
            StartCoroutine(GenererVague());

            yield return new WaitWhile(() => vagueEnCours);
            yield return new WaitForSeconds(delaiEntreVague);
        }
    }

    /// <summary>
    /// Génère une vague d'ennemis en instanciant le prefab d'ennemi à
    /// l'emplacement de génération et en les initialisant avec la destination.
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenererVague()
    {
        vagueEnCours = true;

        for (int i = 0; i < tailleVague; i++)
        {
            Ennemi ennemi = GenererEnnemi();
            ennemi.Initialiser(emplacementGeneration, destinationEnnemis);

            yield return new WaitForSeconds(delaiEntreEnnemi);
        }

        vagueEnCours = false;
    }

    /// <summary>
    /// Génère un ennemi en utilisant un objet de la liste d'instances si disponible, 
    /// sinon instancie un nouvel ennemi à partir du prefab.
    /// </summary>
    /// <returns>L'instance d'ennemi à utiliser</returns>
    private Ennemi GenererEnnemi()
    {
        if (instancesEnnemis.Count > 0)
        {
            Ennemi instance = instancesEnnemis[0];
            instance.gameObject.SetActive(true);
            instancesEnnemis.RemoveAt(0);

            return instance;
        }

        // On crée un nouvel ennemi si aucun n'est disponible dans la liste
        return Instantiate(prefabEnnemi, transform);
    }

    public void RecyclerEnnemi(Ennemi ennemi)
    {
        instancesEnnemis.Add(ennemi);
        ennemi.gameObject.SetActive(false);
        EnnemiRecycle?.Invoke(ennemi);
    }
}