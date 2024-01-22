using UnityEngine;

public interface IMaskRenderer
{
    SpriteRenderer TargetRenderer { get; }
    Texture2D VisibilityMap { get; set; }
    SpriteRenderer MapRenderer { get; set; }
    Texture2D FogTexture { get; set; }
    void Initialise();
    void AddRenderNode(RenderNodeV3 node);
}