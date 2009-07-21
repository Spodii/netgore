namespace DemoGame.Client
{
    /// <summary>
    /// Delegate for when an IDrawableEntity's MapRenderLayer is changed
    /// </summary>
    /// <param name="drawableEntity">IDrawableEntity that changed their MapRenderLayer</param>
    /// <param name="oldLayer">The previous value of MapRenderLayer</param>
    public delegate void MapRenderLayerChange(IDrawableEntity drawableEntity, MapRenderLayer oldLayer);
}