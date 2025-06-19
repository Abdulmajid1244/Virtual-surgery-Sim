using System.Collections;
using UnityEngine;

public class OrganReaction : MonoBehaviour
{
    public Color highlightColor = Color.red;
    public float colorResetDelay = 1f;

    private Color originalColor;
    private Renderer organRenderer;
    private Material organMaterial;

    void Start()
    {
        organRenderer = GetComponent<Renderer>();
        if (organRenderer != null && organRenderer.material != null)
        {
            // Create a new instance of the material so changes are local to this organ
            organMaterial = new Material(organRenderer.material);
            organRenderer.material = organMaterial;

            // For URP Lit Shader: get base color
            if (organMaterial.HasProperty("_BaseColor"))
            {
                originalColor = organMaterial.GetColor("_BaseColor");
            }
            else
            {
                Debug.LogWarning("Material does not use URP Lit Shader with _BaseColor.");
            }
        }
        else
        {
            Debug.LogWarning("OrganReaction: Missing Renderer or Material.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tool"))
        {
            Debug.Log("Tool touched organ: " + collision.gameObject.name);
            ReactToTool();
        }
    }

    void ReactToTool()
    {
        if (organMaterial != null && organMaterial.HasProperty("_BaseColor"))
        {
            organMaterial.SetColor("_BaseColor", highlightColor);
            StopAllCoroutines(); // Stop any running color resets
            StartCoroutine(ResetColorAfterDelay());
        }
    }

    IEnumerator ResetColorAfterDelay()
    {
        yield return new WaitForSeconds(colorResetDelay);
        if (organMaterial != null && organMaterial.HasProperty("_BaseColor"))
        {
            organMaterial.SetColor("_BaseColor", originalColor);
        }
    }
}
