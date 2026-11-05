/// <summary>
/// Interface pour gérer les interactions de la souris. Cette interface peut être implémentée par des classes qui gèrent les 
/// événements de la souris, tels que les clics et les mouvements pour éviter une collision dans la gestion. La gestion
/// se fait avec le contrôleur de jeu. Les méthodes ne devraient pas être appelées directement par d'autres classes, 
/// mais plutôt via le contrôleur de jeu pour assurer une gestion centralisée des événements de la souris.
/// </summary>
public interface IGestionnaireSouris
{
    /// <summary>
    /// La classe contrôle les événements de la souris
    /// </summary>
    public void PrendreControleSouris();

    /// <summary>
    /// La classe cesse de contrôler les événements de la souris
    /// </summary>
    public void LibererControleSouris();

}