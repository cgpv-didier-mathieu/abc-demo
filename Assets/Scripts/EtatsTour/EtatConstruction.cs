namespace EtatsTour
{
    public class EtatConstruction : EtatTour
    {
        public override EtatTour OnExecuter(Tour tour)
        {
            if (!tour.EstPlacee)
                return this; // reste en construction

            return new EtatAttaque();
        }

        public override void OnSortir(Tour tour)
        {
            base.OnSortir(tour);

            Lanceur lanceur = tour.Lanceur;
            lanceur.gameObject.SetActive(true);

            lanceur.EnnemiEntreZone += tour.OnEnnemiEntreZone;
            lanceur.EnnemiSortDeZone += tour.OnEnnemiSortDeZone;
        }
    }
}