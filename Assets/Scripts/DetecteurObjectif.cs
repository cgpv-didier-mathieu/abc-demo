using UnityEngine;

/// <summary>
/// Détecteur d'objectifs pour les ennemis. Lorsqu'un ennemi entre dans le trigger,
/// il est recyclé par le générateur d'ennemis.
/// </summary>
public class DetecteurObjectif : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ennemi ennemi))
        {
            ControleurJeu.Instance.GenerateurEnnemis.RecyclerEnnemi(ennemi);
        }
    }
}