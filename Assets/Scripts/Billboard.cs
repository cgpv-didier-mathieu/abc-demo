using UnityEngine;

/// <summary>
/// Assure que le sprite pointe toujours vers la caméra. Utile pour les sprites 2D dans un monde 3D.
/// </summary>
public class Billboard : MonoBehaviour
{
    [SerializeField, Tooltip("La caméra à regarder")]
    private Camera cameraARegarder;

    private void Start()
    {
        if (cameraARegarder == null)
        {
            cameraARegarder = Camera.main;
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(cameraARegarder.transform, Vector3.up);
    }
}