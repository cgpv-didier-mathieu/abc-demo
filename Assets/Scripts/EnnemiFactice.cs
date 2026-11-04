using UnityEngine;

/// <summary>
/// Ennemi utilisé pour créer la tour. Ne sera pas utilisé dans le jeu final. 
/// </summary>
public class EnnemiFactice : MonoBehaviour
{
    [SerializeField, Tooltip("Vitesse de déplacement en m/s")]
    private float vitesseDeplacement = 2f;

    // Référence au rigidbody de l'ennemi pour appliquer la physique
    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        // Déplacer l'ennemi factice vers l'avant à la vitesse spécifiée
        Vector3 deplacement = transform.forward * vitesseDeplacement * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + deplacement);
    }
}