using UnityEngine;
using Meta.XR.BuildingBlocks;
using System.Collections;

public class Menue : MonoBehaviour
{
    [SerializeField] private GameObject devMapper;
    [SerializeField] private GameObject playMapper;
    [SerializeField] private GameObject SampleSpatialAnchorController;

    private SpatialAnchorLoaderBuildingBlock anchorLoader;

    void Awake()
    {
        anchorLoader = SampleSpatialAnchorController.GetComponent<SpatialAnchorLoaderBuildingBlock>();
    }

    public void OnRayClickDEV()
    {
        Debug.Log("CLICK DEV FUNKTIONIERT");
        devMapper.SetActive(true);
        playMapper.SetActive(false);

        var anchors = FindObjectsOfType<OVRSpatialAnchor>();

        foreach (var anchor in anchors)
        {
            var renderers = anchor.GetComponentsInChildren<Renderer>();
            foreach (var r in renderers)
            {
                r.enabled = true;
            }
        }

        if (anchorLoader != null)
        {
            anchorLoader.LoadAnchorsFromDefaultLocalStorage();
        }

        gameObject.SetActive(false);
    }

    public void OnRayClickPLAY()
    {
        Debug.Log("CLICK PLAY FUNKTIONIERT");
        devMapper.SetActive(false);
        playMapper.SetActive(true);


        gameObject.SetActive(false);
    }

    public void ShowMenue()
    {
        gameObject.SetActive(true);
    }

}