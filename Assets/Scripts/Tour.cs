using EtatsTour;
using System.Collections.Generic;
using UnityEngine;

public class Tour : MonoBehaviour
{
    [field: SerializeField, Tooltip("Lanceur de projectiles de la tour")]
    public Lanceur Lanceur { get; private set; }

    [field: SerializeField, Tooltip("Dégâts infligés par la tour")]
    public float Degats { get; private set; }

    [field: SerializeField, Tooltip("Délai entre chaque attaque (en secondes)")]
    public float DelaiAttaque { get; private set; }

    public Ennemi CibleActive { get; private set; }
    public float TempsDepuisDerniereAttaque { get; set; }
    public bool EstPlacee { get; private set; }

    private List<Ennemi> ciblesPotentielles;

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

        // Le lanceur reste désactivé tant que la tour n'est pas placée
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

    public void Placer()
    {
        EstPlacee = true;
    }

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

    public void Attaquer()
    {
        Projectile projectile = PoolProjectiles.Instance.GetProjectile();
        projectile.Initialiser(CibleActive, Lanceur.transform.position, Quaternion.identity, Degats);
    }
}