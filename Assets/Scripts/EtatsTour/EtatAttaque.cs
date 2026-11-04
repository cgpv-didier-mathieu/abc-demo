using UnityEngine;

namespace EtatsTour
{
    public class EtatAttaque : EtatTour
    {
        public override void OnEntrer(Tour tour)
        {
            // Le premier tir part dès qu'une cible est là
            tour.TempsDepuisDerniereAttaque = tour.DelaiAttaque;
        }

        public override EtatTour OnExecuter(Tour tour)
        {
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