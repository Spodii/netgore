using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using NetGore.IO;

namespace DemoGame.Client
{
    class ServerMessage
    {
        public static string Show(byte MessageID, string Parameters)
        {
            switch (MessageID)
	        {
                case 0:
                    return "";
                case 1:
                    return "";
    		    default:
                    return "";
	        }
        }
    }
}
