using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.RPGComponents;

// TODO: I am quite sure the BodyInfo needs to be cleaned up...

namespace NetGore.RPGComponents
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

        public static Rectangle GetHitRect(CharacterEntityBase c, Rectangle rect)
        {
            if (c.Heading == Direction.East)
            {
                var x = (int)(c.Position.X + rect.X);
                var y = (int)(c.Position.Y + rect.Y);
                return new Rectangle(x, y, rect.Width, rect.Height);
            }
            else
            {
                var x = (int)(c.CB.Max.X - rect.X - rect.Width);
                var y = (int)(c.CB.Max.Y - rect.Y - rect.Height);
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
            var ms = new MathString();
            var results = XmlInfoReader.ReadFile(filePath, true);

            // Find the highest index
            var highestIndex = new BodyIndex(0);
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
                var index = d.AsBodyIndex("Body.Index");
                var width = d.AsFloat("Body.Size.Width");
                var height = d.AsFloat("Body.Size.Height");

                // Set the MathString variables
                ms.Variables.Clear();
                ms.Variables.Add("$width", width);
                ms.Variables.Add("$height", height);

                var body = d["Body.Body.SkelBody"];
                var stand = d["Body.Stand.SkelSet"];
                var walk = d["Body.Walk.SkelSet"];
                var jump = d["Body.Jump.SkelSet"];
                var fall = d["Body.Fall.SkelSet"];

                var punch = d["Body.Punch.SkelSet"];
                var punchX = (int)ms.Parse((d["Body.Punch.X"]));
                var punchY = (int)ms.Parse((d["Body.Punch.Y"]));
                var punchWidth = (int)ms.Parse((d["Body.Punch.Width"]));
                var punchHeight = (int)ms.Parse((d["Body.Punch.Height"]));
                var punchRect = new Rectangle(punchX, punchY, punchWidth, punchHeight);

                ret[(int)index] = new BodyInfo(index, width, height, body, stand, walk, jump, fall, punch, punchRect);
            }

            return ret;
        }
    }
}