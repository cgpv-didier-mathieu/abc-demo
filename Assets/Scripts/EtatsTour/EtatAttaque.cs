using UnityEngine;

namespace EtatsTour
{
    public class EtatAttaque : EtatTour
    {
        public override EtatTour OnExecuter(Tour tour)
        {
            if (tour.AmeliorationDemandee)
            {
                return new EtatAmelioration();
            }

            if (tour.TempsDepuisDerniereAttaque >= tour.DelaiAttaque
                && tour.CibleActive != null)
            {
                tour.Attaquer();
                tour.TempsDepuisDerniereAttaque = 0f;
            }

            tour.TempsDepuisDerniereAttaque += Time.deltaTime;

            return this;
        }
    }
}