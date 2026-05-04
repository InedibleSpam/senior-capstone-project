using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InteractableGlow : MonoBehaviour
{
    [Header("Glow Renderer")]
    public Renderer glowRenderer;

    [Header("Idle / Normal Glow")]
    public Color normalColor = new Color(0.2f, 0.7f, 1f, 0.08f);
    public float normalEmissionStrength = 0.6f;

    [Header("Hover Glow")]
    public Color hoverColor = new Color(0.75f, 0.95f, 1f, 0.30f);
    public float hoverEmissionStrength = 1.8f;

    private Material glowMaterial;
    private XRBaseInteractable interactable;

    private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");
    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        if (glowRenderer == null)
        {
            Debug.LogWarning("Glow Renderer is missing on " + gameObject.name);
            return;
        }

        glowMaterial = glowRenderer.material;

        // Make sure the glow starts subtle.
        SetNormalGlow();

        interactable = GetComponent<XRBaseInteractable>();

        if (interactable == null)
        {
            Debug.LogWarning("XR interactable is missing on " + gameObject.name);
            return;
        }

        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
    }

    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.hoverEntered.RemoveListener(OnHoverEntered);
            interactable.hoverExited.RemoveListener(OnHoverExited);
        }
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        SetHoverGlow();
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        SetNormalGlow();
    }

    public void SetNormalGlow()
    {
        ApplyGlow(normalColor, normalEmissionStrength);
    }

    public void SetHoverGlow()
    {
        ApplyGlow(hoverColor, hoverEmissionStrength);
    }

    private void ApplyGlow(Color color, float emissionStrength)
    {
        if (glowMaterial == null) return;

        glowMaterial.SetColor(BaseColorID, color);
        glowMaterial.color = color;
        glowMaterial.SetColor(EmissionColorID, color * emissionStrength);
    }
}