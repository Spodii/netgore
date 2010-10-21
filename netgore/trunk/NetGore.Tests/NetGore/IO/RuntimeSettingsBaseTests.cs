using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.IO
{
    [TestFixture]
    public class RuntimeSettingsBaseTests
    {


        class MySettings : RuntimeSettingsBase, IDisposable
        {
            /// <summary>
            /// Gets the file path to use.
            /// </summary>
            /// <returns>The file path to use.</returns>
            static string GetFilePath()
            {
                return Path.GetTempFileName();
            }

            [SyncValue]
            [DefaultValue(true)]
            [System.ComponentModel.Description("My test desc")]
            public bool A { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="MySettings"/> class.
            /// </summary>
            public MySettings() : base(GetFilePath(), "MySettings", GenericValueIOFormat.Xml)
            {
            }

            /// <summary>
            /// Releases unmanaged resources and performs other cleanup operations before the
            /// <see cref="MySettings"/> is reclaimed by garbage collection.
            /// </summary>
            ~MySettings()
            {
                if (_isDisposed)
                    return;

                HandleDispose();
            }

            bool _isDisposed;

            /// <summary>
            /// Handles disposing.
            /// </summary>
            void HandleDispose()
            {
                // Delete the temp file
                var file = AsRS.FilePath;

                try
                {
                    if (File.Exists(file))
                        File.Delete(file);
                }
                catch (Exception)
                {
                }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_isDisposed)
                    return;

                _isDisposed = true;

                GC.SuppressFinalize(this);

                HandleDispose();
            }
        }
    }
}
