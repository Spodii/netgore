namespace NetGore.Graphics
{
    /// <summary>
    /// Enum consisting of the subset of MapRenderLayers for a MapGrh (mostly just for simplification in the editor).
    /// </summary>
    public enum MapGrhRenderLayer : byte
    {
        /// <summary>
        /// Invalid.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Back-most layer for Map sprites. These will appear behind the Character.
        /// </summary>
        Background = MapRenderLayer.SpriteBackground,

        /// <summary>
        /// Draws in background or foreground depending on the position.
        /// </summary>
        Dynamic = MapRenderLayer.Dynamic,

        /// <summary>
        /// Front-most layer for Map sprites. These will appear in front of the Characterm.
        /// </summary>
        Foreground = MapRenderLayer.SpriteForeground,
    }
}