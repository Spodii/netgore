using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Audio;

namespace DemoGame.Client
{
    public class AudioManager : AudioManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioManager"/> class.
        /// </summary>
        public AudioManager()
            : base(@"Content\Audio\audio.xgs", @"Content\Audio\audio.xwb", @"Content\Audio\audio.xsb")
        {
        }
    }
}
