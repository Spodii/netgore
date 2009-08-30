using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Information for a character's body
    /// </summary>
    public class BodyInfo
    {
        public readonly string Body;
        public readonly string Fall;

        /// <summary>
        /// Height of the body collision area in pixels
        /// </summary>
        public readonly float Height;

        /// <summary>
        /// Index of the body info
        /// </summary>
        public readonly BodyIndex Index;

        public readonly string Jump;
        public readonly string Punch;
        public readonly Rectangle PunchRect;
        public readonly string Stand;
        public readonly string Walk;

        /// <summary>
        /// Width of the body collision area in pixels
        /// </summary>
        public readonly float Width;

        /// <summary>
        /// BodyInfo constructor
        /// </summary>
        /// <param name="index">Index of the BodyInfo</param>
        /// <param name="width">Width of the body collision area in pixels</param>
        /// <param name="height">Height of the body collision area in pixels</param>
        public BodyInfo(BodyIndex index, float width, float height, string body, string stand, string walk, string jump,
                        string fall, string punch, Rectangle punchRect)
        {
            Index = index;

            Width = width;
            Height = height;

            Body = body;
            Stand = stand;
            Walk = walk;
            Jump = jump;
            Fall = fall;
            Punch = punch;
            PunchRect = punchRect;
        }

        public static Rectangle GetHitRect(CharacterEntity c, Rectangle rect)
        {
            if (c.Heading == Direction.East)
            {
                int x = (int)(c.Position.X + rect.X);
                int y = (int)(c.Position.Y + rect.Y);
                return new Rectangle(x, y, rect.Width, rect.Height);
            }
            else
            {
                int x = (int)(c.CB.Max.X - rect.X - rect.Width);
                int y = (int)(c.CB.Max.Y - rect.Y - rect.Height);
                return new Rectangle(x, y, rect.Width, rect.Height);
            }
        }

        /// <summary>
        /// Creates the body information from a given file
        /// </summary>
        /// <param name="filePath">Path to the body information file</param>
        /// <returns>An array containing the body information</returns>
        public static BodyInfo[] Load(string filePath)
        {
            MathString ms = new MathString();
            var results = XmlInfoReader.ReadFile(filePath, true);

            // Find the highest index
            BodyIndex highestIndex = new BodyIndex(0);
            foreach (var d in results)
            {
                var currentBodyIndex = d.AsBodyIndex("Body.Index");
                if (highestIndex < currentBodyIndex)
                    highestIndex = currentBodyIndex;
            }

            // Create the return array
            var ret = new BodyInfo[(int)highestIndex + 1];

            // Create the return values
            foreach (var d in results)
            {
                BodyIndex index = d.AsBodyIndex("Body.Index");
                float width = d.AsFloat("Body.Size.Width");
                float height = d.AsFloat("Body.Size.Height");

                // Set the MathString variables
                ms.Variables.Clear();
                ms.Variables.Add("$width", width);
                ms.Variables.Add("$height", height);

                string body = d["Body.Body.SkelBody"];
                string stand = d["Body.Stand.SkelSet"];
                string walk = d["Body.Walk.SkelSet"];
                string jump = d["Body.Jump.SkelSet"];
                string fall = d["Body.Fall.SkelSet"];

                string punch = d["Body.Punch.SkelSet"];
                int punchX = (int)ms.Parse((d["Body.Punch.X"]));
                int punchY = (int)ms.Parse((d["Body.Punch.Y"]));
                int punchWidth = (int)ms.Parse((d["Body.Punch.Width"]));
                int punchHeight = (int)ms.Parse((d["Body.Punch.Height"]));
                Rectangle punchRect = new Rectangle(punchX, punchY, punchWidth, punchHeight);

                ret[(int)index] = new BodyInfo(index, width, height, body, stand, walk, jump, fall, punch, punchRect);
            }

            return ret;
        }
    }
}