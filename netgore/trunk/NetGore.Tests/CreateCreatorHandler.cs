using System.Linq;

namespace NetGore.Tests
{
    /// <summary>
    /// Handler for getting a <see cref="ReaderWriterCreatorBase"/> instance. 
    /// </summary>
    /// <returns>The <see cref="ReaderWriterCreatorBase"/> instance.</returns>
    delegate ReaderWriterCreatorBase CreateCreatorHandler();
}