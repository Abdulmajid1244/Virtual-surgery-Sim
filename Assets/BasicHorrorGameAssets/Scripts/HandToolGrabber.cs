using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectiveGrabOnCollision : MonoBehaviour
{
    public Transform grabPoint;           // Assign in Inspector (child of hand)
    private GameObject candidateTool = null;
    private GameObject heldTool = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tool") && heldTool == null)
        {
            candidateTool = other.gameObject;
            Debug.Log("Tool in reach: " + candidateTool.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == candidateTool)
        {
            candidateTool = null;
            Debug.Log("Tool out of reach");
        }
    }

    private void Update()
    {
        // Grab tool with E
        if (heldTool == null && candidateTool != null && Input.GetKeyDown(KeyCode.E))
        {
            GrabTool(candidateTool);
        }

        // Drop tool with Q
        if (heldTool != null && Input.GetKeyDown(KeyCode.Q))
        {
            DropTool();
        }
    }

    void GrabTool(GameObject tool)
    {
        heldTool = tool;

        tool.transform.SetParent(grabPoint);
        tool.transform.localPosition = Vector3.zero;
        tool.transform.localRotation = Quaternion.identity;

        Rigidbody rb = tool.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Debug.Log("Grabbed tool: " + tool.name);
    }

    void DropTool()
    {
        Rigidbody rb = heldTool.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        heldTool.transform.SetParent(null);
        Debug.Log("Dropped tool: " + heldTool.name);

        heldTool = null;
    }
}

