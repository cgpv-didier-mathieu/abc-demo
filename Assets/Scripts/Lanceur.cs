using System;
using UnityEngine;

public class Lanceur : MonoBehaviour
{
    public event Action<Ennemi> EnnemiEntreZone;
    public event Action<Ennemi> EnnemiSortDeZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ennemi ennemi))
            EnnemiEntreZone?.Invoke(ennemi);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Ennemi ennemi))
            EnnemiSortDeZone?.Invoke(ennemi);
    }
}