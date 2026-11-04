namespace EtatsTour
{
    public abstract class EtatTour
    {
        public virtual void OnEntrer(Tour tour) { }
        public abstract EtatTour OnExecuter(Tour tour);
        public virtual void OnSortir(Tour tour) { }
    }
}