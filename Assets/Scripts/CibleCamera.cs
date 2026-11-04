using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gère les déplacement de la cible de caméra. La caméra est une Cinemachine qui a comme cible ce gameObject.
/// </summary>
public class CibleCamera : MonoBehaviour
{
    [Header("Paramètres de déplacement")]
    [SerializeField, Tooltip("Vitesse de déplacement en m/s")]
    private float vitesseDeplacement;

    [SerializeField, Tooltip("Vitesse de rotation en deg/sec")]
    private float vitesseRotation;

    [SerializeField, Tooltip("Vitesse inclinaison en deg/sec")]
    private float vitesseInclinaison;

    [SerializeField, Tooltip("Angles d'inclinaison limites de la caméra. Doit être plus petit que le premier angle ou plus grand que le second")]
    private Vector2 limitesInclinaison;

    [SerializeField, Tooltip("Vitesse zoom de la caméra")]
    private float vitesseZoom;

    [SerializeField, Tooltip("Limites du zoom de la caméra")]
    private Vector2 limitesZoom;

    [Header("Références aux objets de jeu")]
    [SerializeField, Tooltip("Le PlayerInput qui gère les actions de la personne qui joue")]
    private PlayerInput controles;

    [SerializeField, Tooltip("Zone de confinement de la caméra")]
    private BoxCollider volumeCamera;

    [SerializeField, Tooltip("La caméra qui suit la cible")]
    private CinemachineCamera cameraGeree;

    // Variables privées pour la gestion du déplacement
    /// <summary>
    /// Le deplacement actuel de la caméra
    /// </summary>
    private Vector2 deplacement;

    /// <summary>
    /// La rotation actuelle de la caméra
    /// </summary>
    private float rotation;

    /// <summary>
    /// L'inclinaison actuelle de la caméra
    /// </summary>
    private float inclinaison;

    /// <summary>
    /// Le zoom actuel de la caméra
    /// </summary>
    private float zoom;

    private void Start()
    {
        // Action de déplacement
        InputAction actionDeplacement = controles.actions.FindAction("player/DeplacerCamera");
        actionDeplacement.performed += CommencerDeplacement;
        actionDeplacement.canceled += TerminerDeplacement;

        InputAction actionRotation = controles.actions.FindAction("player/TournerCamera");
        actionRotation.performed += CommencerRotation;
        actionRotation.canceled += TerminerRotation;

        InputAction actionInclinaison = controles.actions.FindAction("player/InclinerCamera");
        actionInclinaison.performed += CommencerInclinaison;
        actionInclinaison.canceled += TerminerInclinaison;

        InputAction actionZoom = controles.actions.FindAction("player/ZoomerCamera");
        actionZoom.performed += CommencerZoom;
        actionZoom.canceled += TerminerZoom;
    }

    private void Update()
    {
        DeplacerCamera();
        TournerCamera();
        InclinerCamera();
        ZoomerCamera();
    }

    private void OnDestroy()
    {
        if (controles == null || controles.actions == null) { return; }

        // Retire les callbacks des actions pour éviter les fuites de mémoire
        InputAction actionDeplacement = controles.actions.FindAction("player/DeplacerCamera");
        actionDeplacement.performed -= CommencerDeplacement;
        actionDeplacement.canceled -= TerminerDeplacement;

        InputAction actionRotation = controles.actions.FindAction("player/TournerCamera");
        actionRotation.performed -= CommencerRotation;
        actionRotation.canceled -= TerminerRotation;

        InputAction actionInclinaison = controles.actions.FindAction("player/InclinerCamera");
        actionInclinaison.performed -= CommencerInclinaison;
        actionInclinaison.canceled -= TerminerInclinaison;

        InputAction actionZoom = controles.actions.FindAction("player/ZoomerCamera");
        actionZoom.performed -= CommencerZoom;
        actionZoom.canceled -= TerminerZoom;
    }

    #region Déplacement
    /// <summary>
    /// Commence le déplacement de la caméra
    /// </summary>
    /// <param name="contexte">Information du callback de l'action</param>
    private void CommencerDeplacement(InputAction.CallbackContext contexte)
    {
        deplacement = vitesseDeplacement * contexte.ReadValue<Vector2>().normalized;
    }

    /// <summary>
    /// Termine le déplacement de la caméra
    /// </summary>
    /// <param name="contexte">Information du callback de l'action</param>
    private void TerminerDeplacement(InputAction.CallbackContext contexte)
    {
        deplacement = Vector2.zero;
    }

    /// <summary>
    /// Gère le déplacement de la caméra en fonction de l'input du joueur et des limites du volume de la caméra
    /// </summary>
    private void DeplacerCamera()
    {
        if (deplacement.sqrMagnitude > 0.0f)
        {
            Vector3 prochainDeplacement = transform.right * deplacement.x +
            transform.forward * deplacement.y;

            prochainDeplacement = Vector3.Scale(prochainDeplacement, new Vector3(1.0f, 0.0f, 1.0f));
            prochainDeplacement *= Time.deltaTime;

            Vector3 prochainePosition = transform.position + prochainDeplacement;

            if (volumeCamera.bounds.Contains(prochainePosition))
            {
                transform.position = prochainePosition;
            }
        }
    }
    #endregion

    #region Rotation
    /// <summary>
    /// Commence la rotation de la caméra
    /// </summary>
    /// <param name="contexte">Information du callback de l'action</param>
    private void CommencerRotation(InputAction.CallbackContext contexte)
    {
        rotation = vitesseRotation * contexte.ReadValue<float>();
    }

    /// <summary>
    /// Termine la rotation de la caméra
    /// </summary>
    /// <param name="contexte">Information du callback de l'action</param>
    private void TerminerRotation(InputAction.CallbackContext contexte)
    {
        rotation = 0.0f;
    }

    /// <summary>
    /// Effectue la rotation de la caméra
    /// </summary>
    private void TournerCamera()
    {
        // Important de tourner dans l'espace du monde pour ne pas avoir d'interaction avec l'inclinaison de la caméra
        transform.Rotate(new Vector3(0.0f, rotation * Time.deltaTime, 0.0f), Space.World);
    }
    #endregion

    #region Inclinaison 
    /// <summary>
    /// Commence l'inclinaison de la caméra
    /// </summary>
    /// <param name="contexte">Information du callback de l'action</param>
    private void CommencerInclinaison(InputAction.CallbackContext contexte)
    {
        inclinaison = vitesseInclinaison * contexte.ReadValue<float>();
    }

    /// <summary>
    /// Termine l'inclinaison de la caméra
    /// </summary>
    /// <param name="contexte">Information du callback de l'action</param>
    private void TerminerInclinaison(InputAction.CallbackContext contexte)
    {
        inclinaison = 0.0f;
    }

    /// <summary>
    /// Applique l'inclinaison de la caméra en fonction de l'input du joueur et des limites d'inclinaison
    /// </summary>
    private void InclinerCamera()
    {
        float angle = (transform.localEulerAngles.x + inclinaison * Time.deltaTime) % 360;

        if (angle < limitesInclinaison.x || angle > limitesInclinaison.y)
        {
            transform.Rotate(new Vector3(inclinaison * Time.deltaTime, 0.0f, 0.0f), Space.Self);
        }
    }
    #endregion

    #region Zoom
    /// <summary>
    /// Commence le zoom de la caméra
    /// </summary>
    /// <param name="contexte">Information du callback de l'action</param>
    private void CommencerZoom(InputAction.CallbackContext contexte)
    {
        zoom = vitesseZoom * contexte.ReadValue<float>();
    }

    /// <summary>
    /// Termine le zoom de la caméra    
    /// </summary>
    /// <param name="contexte">Information du callback de l'action</param>
    private void TerminerZoom(InputAction.CallbackContext contexte)
    {
        zoom = 0.0f;
    }

    /// <summary>
    /// Applique le zoom de la caméra en fonction de l'input du joueur
    /// </summary>
    private void ZoomerCamera()
    {
        CinemachinePositionComposer positionComposer = cameraGeree.GetComponent<CinemachinePositionComposer>();
        Vector3 offsetCamera = positionComposer.TargetOffset + positionComposer.TargetOffset.normalized * zoom;
        float distanceCamera = offsetCamera.magnitude;

        if (distanceCamera >= limitesZoom.x && distanceCamera <= limitesZoom.y)
        {
            positionComposer.TargetOffset = offsetCamera;
        }
    }
    #endregion
}