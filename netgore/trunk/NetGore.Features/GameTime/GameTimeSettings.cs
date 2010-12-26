using System;
using System.Diagnostics;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Features.GameTime
{
    /// <summary>
    /// Contains the settings for game time.
    /// </summary>
    public class GameTimeSettings
    {
        /// <summary>
        /// The settings instance.
        /// </summary>
        static GameTimeSettings _instance;

        readonly DateTime _baseTime;
        readonly float _gameTimeMultiplier;
        readonly byte _minAmbient;
        readonly float _minAmbientMultiplier;
        readonly int _nightEndHour;
        readonly int _nightStartHour;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameTimeSettings"/> class.
        /// </summary>
        /// <param name="nightStartHour">The night start hour.</param>
        /// <param name="nightEndHour">The night end hour.</param>
        /// <param name="minAmbientMultiplier">The min ambient multiplier.</param>
        /// <param name="minAmbient">The minimum ambient darkness.</param>
        /// <param name="baseTime">The base time.</param>
        /// <param name="gameTimeMultiplier">The game time multiplier.</param>
        /// <exception cref="ArgumentException">Night must begin on one day, and morning must begin on another day,
        /// just like in real life.</exception>
        public GameTimeSettings(int nightStartHour, int nightEndHour, float minAmbientMultiplier, byte minAmbient,
                                DateTime baseTime, float gameTimeMultiplier)
        {
            if (nightStartHour < nightEndHour)
            {
                throw new ArgumentException(
                    "Night must begin on one day, and morning must begin on another day, just like in real life.",
                    "nightStartHour");
            }

            _nightStartHour = nightStartHour;
            _nightEndHour = nightEndHour;
            _minAmbientMultiplier = minAmbientMultiplier;
            _minAmbient = minAmbient;
            _baseTime = baseTime;
            _gameTimeMultiplier = gameTimeMultiplier;
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> that represents the real-world time at which the game's time
        /// starts. This is the time used for when finding the total real-world minutes that have elapsed. Therefore, it
        /// is only logical that the value is always less than the current time. To make the game time persist, this
        /// value should be set to a constant date (such as the first day of year 2010).
        /// </summary>
        public DateTime BaseTime
        {
            get { return _baseTime; }
        }

        /// <summary>
        /// Gets how much faster the game-time moves than real-world time. A value of 1.0f represents that a
        /// minute in game-time is equal to a minute in real-world time, and 2.0f represents that the game-time
        /// progresses twice as fast as real-world time.
        /// </summary>
        public float GameTimeMultiplier
        {
            get { return _gameTimeMultiplier; }
        }

        /// <summary>
        /// Gets the <see cref="GameTimeSettings"/> instance.
        /// </summary>
        public static GameTimeSettings Instance
        {
            get
            {
                Debug.Assert(_instance != null, "The settings instance should not be null!");
                return _instance;
            }
        }

        /// <summary>
        /// Gets the minimum allowed ambient value for a map when applying the darkness. If the map's unmodified
        /// ambient R, G, or B value is already less than this value, the map's ambient value will be used instead
        /// for the respective color. That is, this value will not make a map lighter than it already is.
        /// </summary>
        public byte MinAmbient
        {
            get { return _minAmbient; }
        }

        /// <summary>
        /// Gets the lowest value that the light multiplier will reach. Note that 0 means that it will be pitch black.
        /// Having below 0 will mean that absolute darkness will last longer, and that we will move into and out
        /// of absolute darkness quicker.
        /// </summary>
        public virtual float MinAmbientMultiplier
        {
            get { return _minAmbientMultiplier; }
        }

        /// <summary>
        /// Gets the hour at which night time ends and the sun has fully risen again (the world is back to full brightness).
        /// </summary>
        public virtual int NightEndHour
        {
            get { return _nightEndHour; }
        }

        /// <summary>
        /// Gets the hour at which night time starts and sun begins to go down (the world starts getting darker).
        /// </summary>
        public virtual int NightStartHour
        {
            get { return _nightStartHour; }
        }

        /// <summary>
        /// Applies the night multiplier to a <see cref="Color"/> to get the new ambient light value.
        /// </summary>
        /// <param name="nightMultiplier">The night multiplier.</param>
        /// <param name="originalColor">The <see cref="Color"/> to apply the <paramref name="nightMultiplier"/> to.</param>
        /// <returns>The new night <see cref="Color"/>.</returns>
        protected virtual Color ApplyNightMultiplier(float nightMultiplier, Color originalColor)
        {
            var r = (byte)(originalColor.R * nightMultiplier).Clamp(MinAmbient, byte.MaxValue);
            var g = (byte)(originalColor.G * nightMultiplier).Clamp(MinAmbient, byte.MaxValue);
            var b = (byte)(originalColor.B * nightMultiplier).Clamp(MinAmbient, byte.MaxValue);

            return new Color(r, g, b);
        }

        /// <summary>
        /// Gets the multiplier value to apply to the ambient light to calculate the new ambient lighting value for
        /// simulating night-time.
        /// </summary>
        /// <param name="time">The current game time.</param>
        /// <returns>The ambient light multiplier.</returns>
        protected virtual float CalculateNightMultiplier(GameDateTime time)
        {
            // Check if we are even in night-time
            if (time.Hour <= NightStartHour && time.Hour >= NightEndHour)
                return 1;

            // Get the light modifier based on the world time. We do this by finding the "percent" of darkness,
            // and multiplying the RGB values of the light by that.

            // How long darkness lasts in minutes (time between nightStartHour and nightEndHour)
            var minutesOfDarkness = (int)((GameDateTime.HoursPerDay - NightStartHour) + NightEndHour);
            minutesOfDarkness *= (int)GameDateTime.MinutesPerHour;
            var halfMinutesOfDarkness = minutesOfDarkness / 2f;

            // How far into the darkness we are with the current time (will be between 0 and hoursOfDarkness)
            int minutesIntoDarkness;
            if (time.Hour > NightStartHour)
                minutesIntoDarkness = time.Hour - NightStartHour;
            else
                minutesIntoDarkness = (int)(GameDateTime.HoursPerDay - NightStartHour) + time.Hour;

            minutesIntoDarkness *= (int)GameDateTime.MinutesPerHour;
            minutesIntoDarkness += time.Minute;

            // Since hoursOfDarkness/2 is the darkest point, get the difference of how many hours we are into
            // darkness and half the total hours of darkness as a percent. This will give us a scale from
            // [-1, 1] where -1 is the start of night, 0 is the darkest point of night, and 1 is the end of night.
            var nightMultiplier = (halfMinutesOfDarkness - minutesIntoDarkness) / halfMinutesOfDarkness;

            // Get the absolute value of the night multiplier so that it instead of -1 to 1, it goes from 1 to 0 to 1
            nightMultiplier = Math.Abs(nightMultiplier);

            // Finally, add the product of the night multiplier to the minMultiplier to "amplify" the darkness
            nightMultiplier += (1 - nightMultiplier) * MinAmbientMultiplier;

            return nightMultiplier;
        }

        /// <summary>
        /// Gets the modified ambient light <see cref="Color"/> for the current <see cref="GameDateTime"/>.
        /// </summary>
        /// <param name="ambient">The ambient light color.</param>
        /// <returns>The modified ambient light.</returns>
        public Color GetModifiedAmbientLight(Color ambient)
        {
            return GetModifiedAmbientLight(ambient, GameDateTime.Now);
        }

        /// <summary>
        /// Gets the modified ambient light <see cref="Color"/> for the specified <see cref="GameDateTime"/>.
        /// </summary>
        /// <param name="ambient">The ambient light color.</param>
        /// <param name="time">The game time to get the ambient light for.</param>
        /// <returns>The modified ambient light for the specified <paramref name="time"/>.</returns>
        public Color GetModifiedAmbientLight(Color ambient, GameDateTime time)
        {
            var multiplier = CalculateNightMultiplier(time);
            var modAmbient = ApplyNightMultiplier(multiplier, ambient);

            return modAmbient;
        }

        /// <summary>
        /// Initializes the <see cref="GameTimeSettings"/>. This must only be called once and called as early as possible.
        /// </summary>
        /// <param name="settings">The settings instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings" /> is <c>null</c>.</exception>
        /// <exception cref="MethodAccessException">This method must be called once and only once.</exception>
        public static void Initialize(GameTimeSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (_instance != null)
                throw new MethodAccessException("This method must be called once and only once.");

            _instance = settings;
        }
    }
}