using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using MySql.Data.Common;
using MySql.Data.MySqlClient.Properties;
using MySql.Data.Types;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Summary description for Driver.
    /// </summary>
    class NativeDriver : Driver
    {
        protected Stream baseStream;
        protected ClientFlags connectionFlags;
        protected String encryptionSeed;
        protected int protocol;

        protected MySqlStream stream;
        BitArray nullMap;

        public ClientFlags Flags
        {
            get { return connectionFlags; }
        }

        /// <summary>
        /// Returns true if this connection can handle batch SQL natively
        /// This means MySQL 4.1.1 or later.
        /// </summary>
        public override bool SupportsBatch
        {
            get
            {
                if ((Flags & ClientFlags.MULTI_STATEMENTS) != 0)
                {
                    if (version.isAtLeast(4, 1, 0) && !version.isAtLeast(4, 1, 10))
                    {
                        object qtType = serverProps["query_cache_type"];
                        object qtSize = serverProps["query_cache_size"];
                        if (qtType != null && qtType.Equals("ON") && (qtSize != null && !qtSize.Equals("0")))
                            return false;
                    }
                    return true;
                }
                return false;
            }
        }

        public NativeDriver(MySqlConnectionStringBuilder settings) : base(settings)
        {
            isOpen = false;
        }

        public void Authenticate()
        {
            // write the user id to the auth packet
            stream.WriteString(connectionString.UserID);

            if (version.isAtLeast(4, 1, 1))
                Authenticate411();
            else
                AuthenticateOld();
        }

        /// <summary>
        /// Perform an authentication against a 4.1.1 server
        /// </summary>
        void Authenticate411()
        {
            if ((connectionFlags & ClientFlags.SECURE_CONNECTION) == 0)
                AuthenticateOld();

            stream.Write(Crypt.Get411Password(connectionString.Password, encryptionSeed));
            if ((connectionFlags & ClientFlags.CONNECT_WITH_DB) != 0 && connectionString.Database != null)
                stream.WriteString(connectionString.Database);

            stream.Flush();

            // this result means the server wants us to send the password using
            // old encryption
            stream.OpenPacket();
            if (stream.IsLastPacket)
            {
                stream.StartOutput(0, false);
                stream.WriteString(Crypt.EncryptPassword(connectionString.Password, encryptionSeed.Substring(0, 8), true));
                stream.Flush();
                ReadOk(true);
            }
            else
                ReadOk(false);
        }

        void AuthenticateOld()
        {
            stream.WriteString(Crypt.EncryptPassword(connectionString.Password, encryptionSeed, protocol > 9));
            if ((connectionFlags & ClientFlags.CONNECT_WITH_DB) != 0 && connectionString.Database != null)
                stream.WriteString(connectionString.Database);

            stream.Flush();
            ReadOk(true);
        }

        void CheckEOF()
        {
            if (!stream.IsLastPacket)
                throw new MySqlException("Expected end of data packet");

            stream.ReadByte(); // read off the 254

            if (version.isAtLeast(3, 0, 0) && !version.isAtLeast(4, 1, 0))
                serverStatus = 0;
            if (stream.HasMoreData && version.isAtLeast(4, 1, 0))
            {
                stream.ReadInteger(2);
                serverStatus = (ServerStatusFlags)stream.ReadInteger(2);

                // if we are at the end of this cursor based resultset, then we remove
                // the last row sent status flag so our next fetch doesn't abort early
                // and we remove this command result from our list of active CommandResult objects.
                //                if ((serverStatus & ServerStatusFlags.LastRowSent) != 0)
                //              {
                //                serverStatus &= ~ServerStatusFlags.LastRowSent;
                //              commandResults.Remove(lastCommandResult);
                //        }
            }
        }

        public override void CloseStatement(int id)
        {
            stream.StartOutput(5, true);
            stream.WriteByte((byte)DBCmd.CLOSE_STMT);
            stream.WriteInteger(id, 4);
            stream.Flush();
        }

        public override void Configure(MySqlConnection pconnection)
        {
            base.Configure(pconnection);
            stream.MaxPacketSize = (ulong)maxPacketSize;
            stream.Encoding = Encoding;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (isOpen)
                        ExecuteCommand(DBCmd.QUIT, null, 0);

                    if (stream != null)
                        stream.Close();
                    stream = null;
                }
                catch
                {
                    // we are just going to eat any exceptions
                    // generated here
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// ExecuteCommand does the work of writing the actual command bytes to the writer
        /// We break it out into a function since it is used in several places besides query
        /// </summary>
        /// <param name="cmd">The cmd that we are sending</param>
        /// <param name="bytes">The bytes of the command, can be null</param>
        /// <param name="length">The number of bytes to send</param>
        void ExecuteCommand(DBCmd cmd, byte[] bytes, int length)
        {
            Debug.Assert(length == 0 || bytes != null);

            try
            {
                stream.StartOutput((ulong)length + 1, true);
                stream.WriteByte((byte)cmd);
                if (length > 0)
                    stream.Write(bytes, 0, length);
                stream.Flush();
            }
            catch (MySqlException ex)
            {
                if (ex.IsFatal)
                {
                    isOpen = false;
                    Close();
                }
                throw;
            }
        }

        public override void ExecuteStatement(byte[] bytes)
        {
            ExecuteCommand(DBCmd.EXECUTE, bytes, bytes.Length);
            serverStatus |= ServerStatusFlags.AnotherQuery;
        }

        /// <summary>
        /// FetchDataRow is the method that the data reader calls to see if there is another 
        /// row to fetch.  In the non-prepared mode, it will simply read the next data packet.
        /// In the prepared mode (statementId > 0), it will 
        /// </summary>
        public override bool FetchDataRow(int statementId, int pageSize, int columns)
        {
            /*			ClearFetchedRow();

						if (!commandResults.ContainsKey(statementId)) return false;

						if ( (serverStatus & ServerStatusFlags.LastRowSent) != 0)
							return false;

						stream.StartPacket(9, true);
						stream.WriteByte((byte)DBCmd.FETCH);
						stream.WriteInteger(statementId, 4);
						stream.WriteInteger(1, 4);
						stream.Flush();

						lastCommandResult = statementId;
							*/
            stream.OpenPacket();
            if (stream.IsLastPacket)
            {
                CheckEOF();
                return false;
            }
            nullMap = null;
            if (statementId > 0)
                ReadNullMap(columns);

            return true;
        }

        ~NativeDriver()
        {
            Close();
        }

        MySqlField GetFieldMetaData()
        {
            MySqlField field;

            stream.Encoding = encoding;
            if (version.isAtLeast(4, 1, 0))
                field = GetFieldMetaData41();
            else
            {
                stream.OpenPacket();
                field = new MySqlField(connection)
                        {
                            Encoding = encoding, TableName = stream.ReadLenString(), ColumnName = stream.ReadLenString(),
                            ColumnLength = stream.ReadNBytes()
                        };

                MySqlDbType type = (MySqlDbType)stream.ReadNBytes();
                stream.ReadByte();
                ColumnFlags colFlags;
                if ((Flags & ClientFlags.LONG_FLAG) != 0)
                    colFlags = (ColumnFlags)stream.ReadInteger(2);
                else
                    colFlags = (ColumnFlags)stream.ReadByte();
                field.SetTypeAndFlags(type, colFlags);

                field.Scale = (byte)stream.ReadByte();
                if (!version.isAtLeast(3, 23, 15) && version.isAtLeast(3, 23, 0))
                    field.Scale++;
            }

            return field;
        }

        MySqlField GetFieldMetaData41()
        {
            MySqlField field = new MySqlField(connection);

            stream.OpenPacket();
            field.Encoding = encoding;
            field.CatalogName = stream.ReadLenString();
            field.DatabaseName = stream.ReadLenString();
            field.TableName = stream.ReadLenString();
            field.RealTableName = stream.ReadLenString();
            field.ColumnName = stream.ReadLenString();
            field.OriginalColumnName = stream.ReadLenString();
            stream.ReadByte();
            field.CharacterSetIndex = stream.ReadInteger(2);
            field.ColumnLength = stream.ReadInteger(4);
            MySqlDbType type = (MySqlDbType)stream.ReadByte();
            ColumnFlags colFlags;
            if ((Flags & ClientFlags.LONG_FLAG) != 0)
                colFlags = (ColumnFlags)stream.ReadInteger(2);
            else
                colFlags = (ColumnFlags)stream.ReadByte();

            field.SetTypeAndFlags(type, colFlags);

            field.Scale = (byte)stream.ReadByte();

            if (stream.HasMoreData)
                stream.ReadInteger(2); // reserved

            if (charSets != null && field.CharacterSetIndex != -1)
            {
                CharacterSet cs = CharSetMap.GetChararcterSet(Version, (string)charSets[field.CharacterSetIndex]);
                field.MaxLength = cs.byteCount;
                field.Encoding = CharSetMap.GetEncoding(version, (string)charSets[field.CharacterSetIndex]);
            }

            return field;
        }

        static bool NoServerCheckValidation(object sender, X509Certificate certificate, X509Chain chain,
                                            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public override void Open()
        {
            base.Open();

            // connect to one of our specified hosts
            try
            {
                if (Settings.ConnectionProtocol == MySqlConnectionProtocol.SharedMemory)
                {
                    SharedMemoryStream str = new SharedMemoryStream(Settings.SharedMemoryName);
                    str.Open(Settings.ConnectionTimeout);
                    baseStream = str;
                }
                else
                {
                    string pipeName = Settings.PipeName;
                    if (Settings.ConnectionProtocol != MySqlConnectionProtocol.NamedPipe)
                        pipeName = null;
                    StreamCreator sc = new StreamCreator(Settings.Server, Settings.Port, pipeName);
                    baseStream = sc.GetStream(Settings.ConnectionTimeout);
                }

                if (baseStream == null)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                throw new MySqlException(Resources.UnableToConnectToHost, (int)MySqlErrorCode.UnableToConnectToHost, ex);
            }

            if (baseStream == null)
                throw new MySqlException("Unable to connect to any of the specified MySQL hosts");

            int maxSinglePacket = 255 * 255 * 255;
            stream = new MySqlStream(baseStream, encoding, false);

            // read off the welcome packet and parse out it's values
            stream.OpenPacket();
            protocol = stream.ReadByte();
            string versionString = stream.ReadString();
            version = DBVersion.Parse(versionString);
            threadId = stream.ReadInteger(4);
            encryptionSeed = stream.ReadString();

            if (version.isAtLeast(4, 0, 8))
                maxSinglePacket = (256 * 256 * 256) - 1;

            // read in Server capabilities if they are provided
            serverCaps = 0;
            if (stream.HasMoreData)
                serverCaps = (ClientFlags)stream.ReadInteger(2);
            if (version.isAtLeast(4, 1, 1))
            {
                /* New protocol with 16 bytes to describe server characteristics */
                serverCharSetIndex = stream.ReadInteger(1);

                serverStatus = (ServerStatusFlags)stream.ReadInteger(2);
                stream.SkipBytes(13);
                string seedPart2 = stream.ReadString();
                encryptionSeed += seedPart2;
            }

            // based on our settings, set our connection flags
            SetConnectionFlags();

            stream.StartOutput(0, false);
            stream.WriteInteger((int)connectionFlags, version.isAtLeast(4, 1, 0) ? 4 : 2);

            if (connectionString.UseSSL && (serverCaps & ClientFlags.SSL) != 0)
            {
                stream.Flush();

                StartSSL();

                stream.StartOutput(0, false);
                stream.WriteInteger((int)connectionFlags, version.isAtLeast(4, 1, 0) ? 4 : 2);
            }

            stream.WriteInteger(maxSinglePacket, version.isAtLeast(4, 1, 0) ? 4 : 3);

            if (version.isAtLeast(4, 1, 1))
            {
                stream.WriteByte(8);
                stream.Write(new byte[23]);
            }

            Authenticate();

            // if we are using compression, then we use our CompressedStream class
            // to hide the ugliness of managing the compression
            if ((connectionFlags & ClientFlags.COMPRESS) != 0)
                stream = new MySqlStream(baseStream, encoding, true);

            // give our stream the server version we are connected to.  
            // We may have some fields that are read differently based 
            // on the version of the server we are connected to.
            stream.Version = version;
            stream.MaxBlockSize = maxSinglePacket;

            isOpen = true;
        }

        public override bool Ping()
        {
            try
            {
                ExecuteCommand(DBCmd.PING, null, 0);
                ReadOk(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override int PrepareStatement(string sql, ref MySqlField[] parameters)
        {
            var bytes = encoding.GetBytes(sql);
            ExecuteCommand(DBCmd.PREPARE, bytes, bytes.Length);

            stream.OpenPacket();

            int marker = stream.ReadByte();
            if (marker != 0)
                throw new MySqlException("Expected prepared statement marker");

            int statementId = stream.ReadInteger(4);
            int numCols = stream.ReadInteger(2);
            int numParams = stream.ReadInteger(2);

            stream.ReadInteger(3);
            if (numParams > 0)
            {
                parameters = ReadColumnMetadata(numParams);

                // we set the encoding for each parameter back to our connection encoding
                // since we can't trust what is coming back from the server
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i].Encoding = encoding;
                }
            }

            if (numCols > 0)
            {
                while (numCols-- > 0)
                {
                    stream.OpenPacket();
                    stream.SkipPacket();
                }

                ReadEOF();
            }

            return statementId;
        }

        /// <summary>
        /// Query is the method that is called to send all queries to the server
        /// </summary>
        /// <param name="bytes">The query to send</param>
        /// <param name="length">The length of the query to send</param>
        /// <returns>
        /// -1 for non select queries
        /// >= 0 for select queries
        /// </returns>
        public override void Query(byte[] bytes, int length)
        {
            if (Settings.Logging)
                Logger.LogCommand(DBCmd.QUERY, encoding.GetString(bytes, 0, length));

            // send the command to the server
            ExecuteCommand(DBCmd.QUERY, bytes, length);

            // the server will respond in one of several ways with the first byte indicating
            // the type of response.
            // 0 == ok packet.  This indicates non-select queries
            // 0xff == error packet.  This is handled in stream.OpenPacket
            // > 0 = number of columns in select query
            // We don't actually read the result here since a single query can generate
            // multiple resultsets and we don't want to duplicate code.  See ReadResult
            // Instead we set our internal server status flag to indicate that we have a query waiting.
            // This flag will be maintained by ReadResult
            serverStatus |= ServerStatusFlags.AnotherQuery;
        }

        public override MySqlField[] ReadColumnMetadata(int count)
        {
            var fields = new MySqlField[count];

            for (int i = 0; i < count; i++)
            {
                fields[i] = GetFieldMetaData();
            }

            ReadEOF();
            return fields;
        }

        public override IMySqlValue ReadColumnValue(int index, MySqlField field, IMySqlValue valObject)
        {
            long length = -1;
            bool isNull;

            if (nullMap != null)
                isNull = nullMap[index + 2];
            else
            {
                length = stream.ReadFieldLength();
                isNull = length == -1;
            }

            stream.Encoding = field.Encoding;
            return valObject.ReadValue(stream, length, isNull);
        }

        void ReadEOF()
        {
            stream.OpenPacket();
            CheckEOF();
        }

        void ReadNullMap(int fieldCount)
        {
            // if we are binary, then we need to load in our null bitmap
            nullMap = null;
            var nullMapBytes = new byte[(fieldCount + 9) / 8];
            stream.ReadByte();
            stream.Read(nullMapBytes, 0, nullMapBytes.Length);
            nullMap = new BitArray(nullMapBytes);
        }

        void ReadOk(bool read)
        {
            try
            {
                if (read)
                    stream.OpenPacket();
                byte marker = (byte)stream.ReadByte();
                if (marker != 0)
                    throw new MySqlException("Out of sync with server", true, null);

                stream.ReadFieldLength(); /* affected rows */
                stream.ReadFieldLength(); /* last insert id */
                if (stream.HasMoreData)
                {
                    serverStatus = (ServerStatusFlags)stream.ReadInteger(2);
                    stream.ReadInteger(2); /* warning count */
                    if (stream.HasMoreData)
                        stream.ReadLenString(); /* message */
                }
            }
            catch (MySqlException ex)
            {
                if (ex.IsFatal)
                {
                    isOpen = false;
                    Close();
                }
                throw;
            }
        }

        /// <summary>
        /// ReadResult will attempt to read a single result from the server.  Note that it is not 
        /// reading all the rows of the result set but simple determining what type of result it is
        /// and returning values appropriately.
        /// </summary>
        /// <param name="affectedRows">Set to the number of rows affected in this result, 0 for selects</param>
        /// <param name="lastInsertId">Set to the id of the row inserted by this result, 0 for non-inserts</param>
        /// <returns>Number of columns in the resultset, 0 for non-selects, -1 for no more resultsets</returns>
        public override long ReadResult(ref ulong affectedRows, ref long lastInsertId)
        {
            // if there is not another query or resultset, then return -1
            if ((serverStatus & (ServerStatusFlags.AnotherQuery | ServerStatusFlags.MoreResults)) == 0)
                return -1;

            lastInsertId = -1;
            stream.OpenPacket();

            long fieldCount = stream.ReadFieldLength();
            if (fieldCount > 0)
                return fieldCount;

            if (-1 == fieldCount)
            {
                string filename = stream.ReadString();
                SendFileToServer(filename);

                return ReadResult(ref affectedRows, ref lastInsertId);
            }

            // the code to read last packet will set these server status vars 
            // again if necessary.
            serverStatus &= ~(ServerStatusFlags.AnotherQuery | ServerStatusFlags.MoreResults);
            affectedRows = (ulong)stream.ReadFieldLength();
            lastInsertId = stream.ReadFieldLength();
            if (version.isAtLeast(4, 1, 0))
            {
                serverStatus = (ServerStatusFlags)stream.ReadInteger(2);
                stream.ReadInteger(2);
                if (stream.HasMoreData)
                    stream.ReadLenString();
            }
            return fieldCount;
        }

        public override void Reset()
        {
            stream.StartOutput(0, true);
            stream.WriteByte((byte)DBCmd.CHANGE_USER);
            Authenticate();
        }

        /// <summary>
        /// Sends the specified file to the server. 
        /// This supports the LOAD DATA LOCAL INFILE
        /// </summary>
        /// <param name="filename"></param>
        void SendFileToServer(string filename)
        {
            var buffer = new byte[8196];
            FileStream fs = null;

            try
            {
                fs = new FileStream(filename, FileMode.Open);
                long len = fs.Length;
                while (len > 0)
                {
                    int count = fs.Read(buffer, 4, (int)(len > 8192 ? 8192 : len));
                    stream.SendEntirePacketDirectly(buffer, count);
                    len -= count;
                }
                stream.SendEntirePacketDirectly(buffer, 0);
            }
            catch (Exception ex)
            {
                throw new MySqlException("Error during LOAD DATA LOCAL INFILE", ex);
            }
            finally
            {
                fs.Close();
            }
        }

        /// <summary>
        /// Return the appropriate set of connection flags for our
        /// server capabilities and our user requested options.
        /// </summary>
        void SetConnectionFlags()
        {
            ClientFlags flags = ClientFlags.FOUND_ROWS;

            if (version.isAtLeast(4, 1, 1))
            {
                flags |= ClientFlags.PROTOCOL_41;
                // Need this to get server status values
                flags |= ClientFlags.TRANSACTIONS;

                // user allows/disallows batch statements
                if (connectionString.AllowBatch)
                    flags |= ClientFlags.MULTI_STATEMENTS;

                // We always allow multiple result sets
                flags |= ClientFlags.MULTI_RESULTS;
            }
            else if (version.isAtLeast(4, 1, 0))
                flags |= ClientFlags.RESERVED;

            // if the server allows it, tell it that we want long column info
            if ((serverCaps & ClientFlags.LONG_FLAG) != 0)
                flags |= ClientFlags.LONG_FLAG;

            // if the server supports it and it was requested, then turn on compression
            if ((serverCaps & ClientFlags.COMPRESS) != 0 && connectionString.UseCompression)
                flags |= ClientFlags.COMPRESS;

            if (protocol > 9)
                flags |= ClientFlags.LONG_PASSWORD; // for long passwords
            else
                flags &= ~ClientFlags.LONG_PASSWORD;

            // allow load data local infile
            flags |= ClientFlags.LOCAL_FILES;

            // did the user request an interactive session?
            if (Settings.InteractiveSession)
                flags |= ClientFlags.INTERACTIVE;

            // if the server allows it and a database was specified, then indicate
            // that we will connect with a database name
            if ((serverCaps & ClientFlags.CONNECT_WITH_DB) != 0 && !string.IsNullOrEmpty(connectionString.Database))
                flags |= ClientFlags.CONNECT_WITH_DB;

            // if the server is requesting a secure connection, then we oblige
            if ((serverCaps & ClientFlags.SECURE_CONNECTION) != 0)
                flags |= ClientFlags.SECURE_CONNECTION;

            // if the server is capable of SSL and the user is requesting SSL
            if ((serverCaps & ClientFlags.SSL) != 0 && connectionString.UseSSL)
                flags |= ClientFlags.SSL;

            connectionFlags = flags;
        }

        /// <summary>
        /// Sets the current database for the this connection
        /// </summary>
        /// <param name="dbName"></param>
        public override void SetDatabase(string dbName)
        {
            var dbNameBytes = Encoding.GetBytes(dbName);
            ExecuteCommand(DBCmd.INIT_DB, dbNameBytes, dbNameBytes.Length);
            ReadOk(true);
        }

        public override void SkipColumnValue(IMySqlValue valObject)
        {
            long length = -1;
            if (nullMap == null)
            {
                length = stream.ReadFieldLength();
                if (length == -1)
                    return;
            }
            if (length > -1)
                stream.SkipBytes((int)length);
            else
                valObject.SkipValue(stream);
        }

        public override bool SkipDataRow()
        {
            bool result = true;
            if (!stream.HasMoreData)
                result = FetchDataRow(-1, 0, 0);
            if (result)
                stream.SkipPacket();
            return result;
        }

        void StartSSL()
        {
            RemoteCertificateValidationCallback sslValidateCallback = NoServerCheckValidation;
            SslStream ss = new SslStream(baseStream, true, sslValidateCallback, null);

            X509CertificateCollection certs = new X509CertificateCollection();
            ss.AuthenticateAsClient(String.Empty, certs, SslProtocols.Default, false);
            baseStream = ss;
            stream = new MySqlStream(ss, encoding, false) { SequenceByte = 2 };
        }
    }
}