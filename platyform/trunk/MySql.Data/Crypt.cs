using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using MySql.Data.Common;
using MySql.Data.MySqlClient.Properties;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Summary description for Crypt.
    /// </summary>
    class Crypt
    {
        // private ctor to prevent creating a default one
        Crypt()
        {
        }

        /// <summary>
        /// Encrypts a password using the MySql encryption scheme
        /// </summary>
        /// <param name="password">The password to encrypt</param>
        /// <param name="seed">The encryption seed the server gave us</param>
        /// <param name="new_ver">Indicates if we should use the old or new encryption scheme</param>
        /// <returns></returns>
        public static String EncryptPassword(String password, String seed, bool new_ver)
        {
            long max = 0x3fffffff;
            if (! new_ver)
                max = 0x01FFFFFF;
            if (string.IsNullOrEmpty(password))
                return password;

            var hash_seed = Hash(seed);
            var hash_pass = Hash(password);

            long seed1 = (hash_seed[0] ^ hash_pass[0]) % max;
            long seed2 = (hash_seed[1] ^ hash_pass[1]) % max;
            if (! new_ver)
                seed2 = seed1 / 2;

            var scrambled = new char[seed.Length];
            for (int x = 0; x < seed.Length; x++)
            {
                double r = rand(ref seed1, ref seed2, max);
                scrambled[x] = (char)(Math.Floor(r * 31) + 64);
            }

            if (new_ver)
            {
                /* Make it harder to break */
                char extra = (char)Math.Floor(rand(ref seed1, ref seed2, max) * 31);
                for (int x = 0; x < scrambled.Length; x++)
                {
                    scrambled[x] ^= extra;
                }
            }

            return new string(scrambled);
        }

/*		private void Create41Password( string password )
		{
			SHA1 sha = new SHA1CryptoServiceProvider(); 
			byte[] firstPassBytes = sha.ComputeHash( System.Text.Encoding.Default.GetBytes( password ));

			byte[] salt = packet.GetBuffer();
			byte[] input = new byte[ firstPassBytes.Length + 4 ];
			salt.CopyTo( input, 0 );
			firstPassBytes.CopyTo( input, 4 );
			byte[] outPass = new byte[100];
			byte[] secondPassBytes = sha.ComputeHash( input );

			byte[] cryptSalt = new byte[20];
			Security.ArrayCrypt( salt, 4, cryptSalt, 0, secondPassBytes, 20 );

			Security.ArrayCrypt( cryptSalt, 0, firstPassBytes, 0, firstPassBytes, 20 );

			// send the packet
			packet = CreatePacket(null);
			packet.Write( firstPassBytes, 0, 20 );
			SendPacket(packet);
		}
*/

        /// <summary>
        /// Generate a scrambled password for 4.1.0 using new passwords
        /// </summary>
        /// <param name="password">The password to scramble</param>
        /// <param name="seedBytes">The seedbytes used to scramble</param>
        /// <returns>Array of bytes containing the scrambled password</returns>
        public static byte[] Get410Password(string password, byte[] seedBytes)
        {
            SHA1Hash sha = new SHA1Hash();
            //SHA1 sha = new SHA1CryptoServiceProvider(); 

            // clean it and then digest it
            password = password.Replace(" ", "").Replace("\t", "");
            var passBytes = Encoding.Default.GetBytes(password);
            var firstPass = sha.ComputeHash(passBytes);

            var input = new byte[24];
            Array.Copy(seedBytes, 0, input, 0, 4);
            Array.Copy(firstPass, 0, input, 4, 20);
            var secondPass = sha.ComputeHash(input);

            var scrambledBuff = new byte[20];
            XorScramble(seedBytes, 4, scrambledBuff, 0, secondPass, 20);

            var finalBuff = new byte[20];
            XorScramble(scrambledBuff, 0, finalBuff, 0, firstPass, 20);

            return finalBuff;
        }

        /// <summary>
        /// Returns a byte array containing the proper encryption of the 
        /// given password/seed according to the new 4.1.1 authentication scheme.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static byte[] Get411Password(string password, string seed)
        {
            // if we have no password, then we just return 1 zero byte
            if (password.Length == 0)
                return new byte[1];

            //SHA1 sha = new SHA1CryptoServiceProvider(); 
            SHA1Hash sha = new SHA1Hash();

            var firstHash = sha.ComputeHash(Encoding.Default.GetBytes(password));
            var secondHash = sha.ComputeHash(firstHash);
            var seedBytes = Encoding.Default.GetBytes(seed);

            var input = new byte[seedBytes.Length + secondHash.Length];
            Array.Copy(seedBytes, 0, input, 0, seedBytes.Length);
            Array.Copy(secondHash, 0, input, seedBytes.Length, secondHash.Length);
            var thirdHash = sha.ComputeHash(input);

            var finalHash = new byte[thirdHash.Length + 1];
            finalHash[0] = 0x14;
            Array.Copy(thirdHash, 0, finalHash, 1, thirdHash.Length);

            for (int i = 1; i < finalHash.Length; i++)
            {
                finalHash[i] = (byte)(finalHash[i] ^ firstHash[i - 1]);
            }
            return finalHash;
        }

        /// <summary>
        /// Generates a proper hash for old style 4.1.0 passwords.  This would be used
        /// if a 4.1.0 server contained old 16 byte hashes.
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <param name="seedBytes">Seed bytes received from the server</param>
        /// <returns>Byte array containing the password hash</returns>
        public static byte[] GetOld410Password(string password, byte[] seedBytes)
        {
            var passwordHash = Hash(password);
            string passHex = String.Format(CultureInfo.InvariantCulture, "{0,8:X}{1,8:X}", passwordHash[0], passwordHash[1]);

            var salt = getSaltFromPassword(passHex);

            // compute binary password
            var binaryPassword = new byte[20];
            int offset = 0;
            for (int i = 0; i < 2; i++)
            {
                int val = salt[i];

                for (int t = 3; t >= 0; t--)
                {
                    binaryPassword[t + offset] = (byte)(val % 256);
                    val >>= 8; /* Scroll 8 bits to get next part*/
                }

                offset += 4;
            }

            //SHA1 sha = new SHA1CryptoServiceProvider(); 
            SHA1Hash sha = new SHA1Hash();
            var temp = new byte[8];
            Buffer.BlockCopy(binaryPassword, 0, temp, 0, 8);
            var binaryHash = sha.ComputeHash(temp);

            var scrambledBuff = new byte[20];
            XorScramble(seedBytes, 4, scrambledBuff, 0, binaryHash, 20);

            string scrambleString = Encoding.Default.GetString(scrambledBuff, 0, scrambledBuff.Length).Substring(0, 8);

            var hashPass = Hash(password);
            var hashMessage = Hash(scrambleString);

            const long max = 0x3FFFFFFFL;
            var to = new byte[20];
            int msgPos = 0;
            int msgLength = scrambleString.Length;
            int toPos = 0;
            long seed1 = (hashPass[0] ^ hashMessage[0]) % max;
            long seed2 = (hashPass[1] ^ hashMessage[1]) % max;

            while (msgPos++ < msgLength)
            {
                to[toPos++] = (byte)(Math.Floor(rand(ref seed1, ref seed2, max) * 31) + 64);
            }

            /* Make it harder to break */
            byte extra = (byte)(Math.Floor(rand(ref seed1, ref seed2, max) * 31));

            for (int i = 0; i < 8; i++)
            {
                to[i] ^= extra;
            }

            return to;
        }

        static int[] getSaltFromPassword(String password)
        {
            var result = new int[6];

            if (string.IsNullOrEmpty(password))
                return result;

            int resultPos = 0;
            int pos = 0;

            while (pos < password.Length)
            {
                int val = 0;

                for (int i = 0; i < 8; i++)
                {
                    val = (val << 4) + HexValue(password[pos++]);
                }

                result[resultPos++] = val;
            }

            return result;
        }

        /// <summary>
        /// Hashes a password using the algorithm from Monty's code.
        /// The first element in the return is the result of the "old" hash.
        /// The second element is the rest of the "new" hash.
        /// </summary>
        /// <param name="P">Password to be hashed</param>
        /// <returns>Two element array containing the hashed values</returns>
        static long[] Hash(String P)
        {
            long val1 = 1345345333;
            long val2 = 0x12345671;
            long inc = 7;

            for (int i = 0; i < P.Length; i++)
            {
                if (P[i] == ' ' || P[i] == '\t')
                    continue;
                long temp = (0xff & P[i]);
                val1 ^= (((val1 & 63) + inc) * temp) + (val1 << 8);
                val2 += (val2 << 8) ^ val1;
                inc += temp;
            }

            var hash = new long[2];
            hash[0] = val1 & 0x7fffffff;
            hash[1] = val2 & 0x7fffffff;
            return hash;
        }

        static int HexValue(char c)
        {
            if (c >= 'A' && c <= 'Z')
                return (c - 'A') + 10;
            if (c >= 'a' && c <= 'z')
                return (c - 'a') + 10;
            return c - '0';
        }

        static double rand(ref long seed1, ref long seed2, long max)
        {
            seed1 = (seed1 * 3) + seed2;
            seed1 %= max;
            seed2 = (seed1 + seed2 + 33) % max;
            return (seed1 / (double)max);
        }

        /// <summary>
        /// Simple XOR scramble
        /// </summary>
        /// <param name="from">Source array</param>
        /// <param name="fromIndex">Index inside source array</param>
        /// <param name="to">Destination array</param>
        /// <param name="toIndex">Index inside destination array</param>
        /// <param name="password">Password used to xor the bits</param>
        /// <param name="length">Number of bytes to scramble</param>
        static void XorScramble(byte[] from, int fromIndex, byte[] to, int toIndex, byte[] password, int length)
        {
            // make sure we were called properly
            if (fromIndex < 0 || fromIndex >= from.Length)
                throw new ArgumentException(Resources.IndexMustBeValid, "fromIndex");
            if ((fromIndex + length) > from.Length)
                throw new ArgumentException(Resources.FromAndLengthTooBig, "fromIndex");
            if (from == null)
                throw new ArgumentException(Resources.BufferCannotBeNull, "from");
            if (to == null)
                throw new ArgumentException(Resources.BufferCannotBeNull, "to");
            if (toIndex < 0 || toIndex >= to.Length)
                throw new ArgumentException(Resources.IndexMustBeValid, "toIndex");
            if ((toIndex + length) > to.Length)
                throw new ArgumentException(Resources.IndexAndLengthTooBig, "toIndex");
            if (password == null || password.Length < length)
                throw new ArgumentException(Resources.PasswordMustHaveLegalChars, "password");
            if (length < 0)
                throw new ArgumentException(Resources.ParameterCannotBeNegative, "length");

            // now perform the work
            for (int i = 0; i < length; i++)
            {
                to[toIndex++] = (byte)(from[fromIndex++] ^ password[i]);
            }
        }
    }
}