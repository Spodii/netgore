using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.AI
{
    public interface IAI
    {
        /// <summary>
        /// Gets the <see cref="DynamicEntity"/> that this AI is for.
        /// </summary>
        DynamicEntity Actor { get; }

        /// <summary>
        /// Updates the AI. This is called at most once per frame, but does not require to be called every frame.
        /// </summary>
        void Update();
    }
}
