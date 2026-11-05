using EtatsTour;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Tour : MonoBehaviour
{
    public const int NIVEAU_MAX = 3;

    [field: SerializeField, Tooltip("Lanceur de projectiles de la tour")]
    public Lanceur Lanceur { get; private set; }

    [SerializeField, Tooltip("Niveau actuel de la tour (NiveauTour1 au départ)")]
    private NiveauTour niveauTour;

    [SerializeField, Tooltip("Parent du modèle visuel de la tour")]
    private Transform conteneurModele;

    // Données issues du niveau
    public int Niveau => niveauTour.Niveau;
    public float Degats => niveauTour.Degats;
    public float DelaiAttaque => niveauTour.DelaiAttaque;

    public string NomTour { get; private set; }

    public Ennemi CibleActive { get; private set; }
    public float TempsDepuisDerniereAttaque { get; set; }
    public bool EstPlacee { get; private set; }

    // Amélioration
    public bool AmeliorationDemandee { get; set; }
    public NiveauTour NiveauAmelioreVers { get; private set; }
    public float TempsAmelioration { get; set; }
    public event Action NiveauChange;

    private List<Ennemi> ciblesPotentielles;
    private GameObject modeleActuel;

    #region Machine à états
    private EtatTour etatActuel;
    private EtatTour prochainEtat;
    #endregion

    private void Awake()
    {
        ciblesPotentielles = new List<Ennemi>();
        TempsDepuisDerniereAttaque = 0f;
        EstPlacee = false;

        prochainEtat = new EtatConstruction();
        etatActuel = null;

        if (Lanceur != null)
            Lanceur.gameObject.SetActive(false);
    }

    private void Start()
    {
        ControleurJeu.Instance.GenerateurEnnemis.EnnemiRecycle += OnEnnemiRecycle;
    }

    private void OnDestroy()
    {
        ControleurJeu controleurJeu = ControleurJeu.Instance;
        if (controleurJeu != null && controleurJeu.GenerateurEnnemis != null)
            controleurJeu.GenerateurEnnemis.EnnemiRecycle -= OnEnnemiRecycle;

        if (Lanceur != null)
        {
            Lanceur.EnnemiEntreZone -= OnEnnemiEntreZone;
            Lanceur.EnnemiSortDeZone -= OnEnnemiSortDeZone;
        }
    }

    private void Update()
    {
        if (prochainEtat != etatActuel)
            prochainEtat.OnEntrer(this);

        etatActuel = prochainEtat;
        prochainEtat = etatActuel.OnExecuter(this);

        if (etatActuel != prochainEtat)
            etatActuel.OnSortir(this);
    }

    public void Nommer(string nom) => NomTour = nom;

    public void Placer()
    {
        EstPlacee = true;
    }

    #region Gestion des cibles
    public void OnEnnemiEntreZone(Ennemi ennemi)
    {
        if (ciblesPotentielles.Contains(ennemi)) return;

        ciblesPotentielles.Add(ennemi);
        if (CibleActive == null)
            CibleActive = ciblesPotentielles[0];
    }

    public void OnEnnemiSortDeZone(Ennemi ennemi)
    {
        ciblesPotentielles.Remove(ennemi);
        if (CibleActive == ennemi)
            ChangerCible();
    }

    private void OnEnnemiRecycle(Ennemi ennemi)
    {
        if (ciblesPotentielles.Remove(ennemi) && CibleActive == ennemi)
            ChangerCible();
    }

    private void ChangerCible()
    {
        CibleActive = ciblesPotentielles.Count > 0 ? ciblesPotentielles[0] : null;
    }
    #endregion

    public void Attaquer()
    {
        Projectile projectile = PoolProjectiles.Instance.GetProjectile();
        projectile.Initialiser(CibleActive, Lanceur.transform.position, Quaternion.identity, Degats);
    }

    #region Amélioration
    public void Ameliorer()
    {
        if (AmeliorationDemandee || Niveau >= NIVEAU_MAX) return;

        NiveauTour prochain = Resources.Load<NiveauTour>($"Niveaux/NiveauTour{Niveau + 1}");
        if (prochain == null)
        {
            Debug.LogWarning($"NiveauTour{Niveau + 1} introuvable dans Resources/Niveaux");
            return;
        }

        NiveauAmelioreVers = prochain;
        AmeliorationDemandee = true;
    }

    public void AppliquerNiveau()
    {
        if (NiveauAmelioreVers == null) return;

        niveauTour = NiveauAmelioreVers;
        NiveauAmelioreVers = null;

        if (modeleActuel != null) Destroy(modeleActuel);
        if (niveauTour.ModeleTour != null && conteneurModele != null)
            modeleActuel = Instantiate(niveauTour.ModeleTour, conteneurModele);

        NiveauChange?.Invoke();
    }
    #endregion
}