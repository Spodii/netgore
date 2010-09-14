namespace DemoGame
{
    /// <summary>
    /// Delegate for handling an event from the <see cref="GameMessageCollection"/>.
    /// </summary>
    /// <param name="oldLanguage">The <see cref="GameMessageCollection"/> for the old language.</param>
    /// <param name="newLanguage">The <see cref="GameMessageCollection"/> for the new language.</param>
    public delegate void GameMessageCollectionChangeLanguageEventHandler(
        GameMessageCollection oldLanguage, GameMessageCollection newLanguage);
}