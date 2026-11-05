using UnityEngine;

[CreateAssetMenu(fileName = "NiveauTour", menuName = "ABC/NiveauTour")]
public class NiveauTour : ScriptableObject
{
    [field: SerializeField] public int Niveau { get; private set; }
    [field: SerializeField] public float Degats { get; private set; }
    [field: SerializeField] public float DelaiAttaque { get; private set; }
    [field: SerializeField, Tooltip("Durée de l'amélioration (secondes)")]
    public float TempsAppliquerAmelioration { get; private set; } = 2f;

    [field: SerializeField, Tooltip("Modèle visuel de la tour à ce niveau")]
    public GameObject ModeleTour { get; private set; }
}