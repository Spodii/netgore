using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using NetGore.IO;

namespace NetGore.Tests
{
    class IValueReaderWriterTestHelper
    {
        const int _bufferSize = 1024 * 1024;

        /// <summary>
        /// The CreateCreatorHandlers that will be used to create the ReaderWriterCreatorBase instances.
        /// </summary>
        static readonly IEnumerable<CreateCreatorHandler> _createCreators;

        static readonly List<string> _createdTempFiles = new List<string>();

        /// <summary>
        /// Initializes the <see cref="IValueReaderWriterTestHelper"/> class.
        /// </summary>
        static IValueReaderWriterTestHelper()
        {
            // Create the delegates for creating the ReaderWriterCreatorBases
            _createCreators = new CreateCreatorHandler[]
            {
                () => new MemoryBinaryValueReaderWriterCreator(), () => new FileBinaryValueReaderWriterCreator(),
                () => new XmlValueReaderWriterCreator(), () => new BitStreamReaderWriterCreator(),
                () => new BitStreamByteArrayReaderWriterCreator()
            };
        }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="CreateCreatorHandler"/>s.
        /// </summary>
        public static IEnumerable<CreateCreatorHandler> CreateCreators
        {
            get { return _createCreators; }
        }

        /// <summary>
        /// Gets the path for a temp file.
        /// </summary>
        /// <returns>The path for a temp file.</returns>
        /// <exception cref="InvalidOperationException">Too many temp files are out. Make sure they are being released!</exception>
        [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect")]
        static string GetTempFile()
        {
            if (_createdTempFiles.Count > 3)
            {
                // Do a garbage collection to see if there is crap still out there, but waiting to be destructed
                GC.Collect();
                if (_createdTempFiles.Count > 3)
                    throw new InvalidOperationException("Too many temp files are out. Make sure they are being released!");
                else
                    Debug.Fail("Too many objects are using the destructor to clear the temp files. Use IDisposable, damnit!");
            }

            var ret = Path.GetTempFileName();
            _createdTempFiles.Add(ret);
            return ret;
        }

        /// <summary>
        /// Releases a file used with GetTempFile().
        /// </summary>
        /// <param name="filePath">Path to the file to release.</param>
        static void ReleaseFile(string filePath)
        {
            _createdTempFiles.Remove(filePath);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        /// <summary>
        /// BitStream using the ByteArray to transfer data from the reader to writer
        /// </summary>
        class BitStreamByteArrayReaderWriterCreator : ReaderWriterCreatorBase
        {
            readonly BitStream _writeStream = new BitStream(_bufferSize);

            /// <summary>
            /// When overridden in the derived class, gets if name lookup is supported.
            /// </summary>
            public override bool SupportsNameLookup
            {
                get { return false; }
            }

            /// <summary>
            /// When overridden in the derived class, gets if nodes are supported.
            /// </summary>
            public override bool SupportsNodes
            {
                get { return false; }
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueReader instance used to read the values
            /// written by the IValueWriter created with GetWriter().
            /// </summary>
            /// <returns>The IValueWriter instance.</returns>
            public override IValueReader GetReader()
            {
                var buffer = _writeStream.GetBuffer();
                return new BitStream(buffer);
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueWriter instance. This method is always called first.
            /// </summary>
            /// <returns>The IValueReader instance.</returns>
            public override IValueWriter GetWriter()
            {
                return _writeStream;
            }
        }

        /// <summary>
        /// BitStream
        /// </summary>
        class BitStreamReaderWriterCreator : ReaderWriterCreatorBase
        {
            readonly BitStream _stream = new BitStream(_bufferSize);

            /// <summary>
            /// When overridden in the derived class, gets if name lookup is supported.
            /// </summary>
            public override bool SupportsNameLookup
            {
                get { return false; }
            }

            /// <summary>
            /// When overridden in the derived class, gets if nodes are supported.
            /// </summary>
            public override bool SupportsNodes
            {
                get { return false; }
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueReader instance used to read the values
            /// written by the IValueWriter created with GetWriter().
            /// </summary>
            /// <returns>The IValueWriter instance.</returns>
            public override IValueReader GetReader()
            {
                _stream.PositionBits = 0;
                return _stream;
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueWriter instance. This method is always called first.
            /// </summary>
            /// <returns>The IValueReader instance.</returns>
            public override IValueWriter GetWriter()
            {
                return _stream;
            }
        }

        /// <summary>
        /// BinaryValueReader/Writer, using a file.
        /// </summary>
        class FileBinaryValueReaderWriterCreator : ReaderWriterCreatorBase
        {
            readonly string _filePath = GetTempFile();

            /// <summary>
            /// When overridden in the derived class, gets if name lookup is supported.
            /// </summary>
            public override bool SupportsNameLookup
            {
                get { return false; }
            }

            /// <summary>
            /// When overridden in the derived class, gets if nodes are supported.
            /// </summary>
            public override bool SupportsNodes
            {
                get { return true; }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public override void Dispose()
            {
                ReleaseFile(_filePath);
                GC.SuppressFinalize(this);

                base.Dispose();
            }

            ~FileBinaryValueReaderWriterCreator()
            {
                ReleaseFile(_filePath);
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueReader instance used to read the values
            /// written by the IValueWriter created with GetWriter().
            /// </summary>
            /// <returns>The IValueWriter instance.</returns>
            public override IValueReader GetReader()
            {
                return BinaryValueReader.CreateFromFile(_filePath);
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueWriter instance. This method is always called first.
            /// </summary>
            /// <returns>The IValueReader instance.</returns>
            public override IValueWriter GetWriter()
            {
                return BinaryValueWriter.Create(_filePath);
            }
        }

        /// <summary>
        /// BinaryValueReader/Writer, using memory.
        /// </summary>
        class MemoryBinaryValueReaderWriterCreator : ReaderWriterCreatorBase
        {
            readonly BitStream _stream = new BitStream(_bufferSize);

            /// <summary>
            /// When overridden in the derived class, gets if name lookup is supported.
            /// </summary>
            public override bool SupportsNameLookup
            {
                get { return false; }
            }

            /// <summary>
            /// When overridden in the derived class, gets if nodes are supported.
            /// </summary>
            public override bool SupportsNodes
            {
                get { return true; }
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueReader instance used to read the values
            /// written by the IValueWriter created with GetWriter().
            /// </summary>
            /// <returns>The IValueWriter instance.</returns>
            public override IValueReader GetReader()
            {
                _stream.PositionBits = 0;
                return BinaryValueReader.Create(_stream);
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueWriter instance. This method is always called first.
            /// </summary>
            /// <returns>The IValueReader instance.</returns>
            public override IValueWriter GetWriter()
            {
                return BinaryValueWriter.Create(_stream);
            }
        }

        /// <summary>
        /// XmlValueReader/Writer.
        /// </summary>
        class XmlValueReaderWriterCreator : ReaderWriterCreatorBase
        {
            const string _rootNodeName = "TestRootNode";

            readonly string _filePath = GetTempFile();

            /// <summary>
            /// When overridden in the derived class, gets if name lookup is supported.
            /// </summary>
            public override bool SupportsNameLookup
            {
                get { return true; }
            }

            /// <summary>
            /// When overridden in the derived class, gets if nodes are supported.
            /// </summary>
            public override bool SupportsNodes
            {
                get { return true; }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public override void Dispose()
            {
                ReleaseFile(_filePath);
                GC.SuppressFinalize(this);

                base.Dispose();
            }

            ~XmlValueReaderWriterCreator()
            {
                ReleaseFile(_filePath);
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueReader instance used to read the values
            /// written by the IValueWriter created with GetWriter().
            /// </summary>
            /// <returns>The IValueWriter instance.</returns>
            public override IValueReader GetReader()
            {
                return XmlValueReader.CreateFromFile(_filePath, _rootNodeName);
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueWriter instance. This method is always called first.
            /// </summary>
            /// <returns>The IValueReader instance.</returns>
            public override IValueWriter GetWriter()
            {
                return XmlValueWriter.Create(_filePath, _rootNodeName);
            }
        }
    }
}