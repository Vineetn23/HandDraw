using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskSetupManager : MonoBehaviour
{

    public GameObject visual;
    public Transform pivot;
    public Transform creationHand;

    Vector3 startPos;
    bool isUpdateShape;

    public float defaultWidth = 0.3f;
    public float defaultHeight = 0.01f;
    public float heightOffset;

    public GameObject[] objectToSpawnAfter;

    public OVRPassthroughLayer updateShapePassthrough;
    public OVRPassthroughLayer afterUpdateShapePassthrough;

    private Renderer visualRenderer;

    // Start is called before the first frame update
    void Start()
    {
        updateShapePassthrough.hidden= false;
        afterUpdateShapePassthrough.hidden= true;
        visual.SetActive(false);
        foreach (var item in objectToSpawnAfter)
        {
            item.SetActive(false) ;
        }

        visualRenderer = visual.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            updateShapePassthrough.hidden = false;
            afterUpdateShapePassthrough.hidden = true;
            visual.SetActive(true);
            startPos = creationHand.position;
            isUpdateShape = true;
            visualRenderer.enabled = true;

            afterUpdateShapePassthrough.RemoveSurfaceGeometry(visual);
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {            
            isUpdateShape = false;

            foreach(var item in objectToSpawnAfter)
            {
                item.SetActive(true);
            }

            updateShapePassthrough.hidden = true;
            afterUpdateShapePassthrough.hidden = false;
            visualRenderer.enabled = false;
            afterUpdateShapePassthrough.AddSurfaceGeometry(visual);
        }

        if (isUpdateShape)
        {
            UpdateShape();
        }
    }

    public void UpdateShape()
    {
        float distance = Vector3.ProjectOnPlane(creationHand.position - startPos, Vector3.up).magnitude;
        visual.transform.localScale = new Vector3(distance, defaultHeight, defaultWidth);

        pivot.right = Vector3.ProjectOnPlane(creationHand.position - startPos, Vector3.up);

        pivot.position = startPos + pivot.rotation * new Vector3(visual.transform.localScale.x / 2, heightOffset, visual.transform.localScale.z / 2);
    }
}
