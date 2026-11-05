using UnityEngine;

namespace EtatsTour
{
    /// <summary>
    /// Etat de la tour lorsqu'elle est en train d'être améliorée
    /// </summary>
    public class EtatAmelioration : EtatTour
    {
        public override void OnEntrer(Tour tour)
        {
            base.OnEntrer(tour);

            tour.TempsAmelioration = 0f;
        }

        public override EtatTour OnExecuter(Tour tour)
        {
            if (tour.TempsAmelioration < tour.NiveauAmelioreVers.TempsAppliquerAmelioration)
            {
                tour.TempsAmelioration += Time.deltaTime;
                return this;
            }

            return new EtatAttaque();
        }

        public override void OnSortir(Tour tour)
        {
            base.OnSortir(tour);

            tour.AppliquerNiveau();
            tour.AmeliorationDemandee = false;
        }
    }
}