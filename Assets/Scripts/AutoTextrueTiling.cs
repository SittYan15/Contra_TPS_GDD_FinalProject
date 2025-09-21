using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class AutoTextureTiling : MonoBehaviour
{
    public Vector2 baseTiling = Vector2.one; // tiling for scale = (1,1,1)

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateTiling();
    }

    void UpdateTiling()
    {
        // Get the scale of the object in world space
        Vector3 scale = transform.lossyScale;

        // Apply tiling relative to object scale
        rend.material.mainTextureScale = new Vector2(
            baseTiling.x * scale.x,
            baseTiling.y * scale.z  // use Z for depth instead of Y
        );
    }

    // If you want it to update dynamically in the editor
#if UNITY_EDITOR
    void Update()
    {
        UpdateTiling();
    }
#endif
}
