using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.UI;

public class ManagerPractica : MonoBehaviour
{
    [Header("Componentes AR")]
    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager;

    [Header("Elementos de la Interfaz (UI)")]
    public TextMeshProUGUI textoContadorPlanos;
    public Button botonBorrar;
    public TMP_Dropdown dropdownPrefabs;

    [Header("Modelos 3D (Prefabs)")]
    public GameObject[] prefabsParaInstanciar;

    // Lista para guardar los objetos y poder borrarlos
    private List<GameObject> objetosCreados = new List<GameObject>();
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        botonBorrar.onClick.AddListener(BorrarObjetos);
    }

    void Update()
    {
        // Texto en tiempo real con el numero de planos
        textoContadorPlanos.text = "Planos detectados: " + planeManager.trackables.count;

        // Detecta si el usuario toca la pantalla
        if (Pointer.current == null || !Pointer.current.press.wasPressedThisFrame) return;

        Vector2 touchPosition = Pointer.current.position.ReadValue();

        // Instanciar al interactuar con planos
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // Combo para elegir entre diferentes prefabs
            int indiceSeleccionado = dropdownPrefabs.value;

            if (prefabsParaInstanciar.Length > 0)
            {
                GameObject nuevoObjeto = Instantiate(prefabsParaInstanciar[indiceSeleccionado], hitPose.position, hitPose.rotation);
                objetosCreados.Add(nuevoObjeto);
            }
        }
    }

    // Botón de borrar que elimine los prefabs instanciados
    public void BorrarObjetos()
    {
        foreach (GameObject obj in objetosCreados)
        {
            Destroy(obj);
        }
        objetosCreados.Clear();
    }
}