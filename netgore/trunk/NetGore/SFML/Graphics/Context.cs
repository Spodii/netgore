using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace SFML
{
    namespace Graphics
    {
        ////////////////////////////////////////////////////////////
        /// <summary>
        /// This class defines
        /// </summary>
        ////////////////////////////////////////////////////////////
        class Context : CriticalFinalizerObject
        {
            ////////////////////////////////////////////////////////////
            static Context ourGlobalContext = null;

            readonly IntPtr myThis = IntPtr.Zero;

            #region Imports

            [DllImport("csfml-window")]
            [SuppressUnmanagedCodeSecurity]
            static extern IntPtr sfContext_Create();

            [DllImport("csfml-window")]
            [SuppressUnmanagedCodeSecurity]
            static extern void sfContext_Destroy(IntPtr View);

            [DllImport("csfml-window")]
            [SuppressUnmanagedCodeSecurity]
            static extern void sfContext_SetActive(IntPtr View, bool Active);

            #endregion

            /// <summary>
            /// Default constructor
            /// </summary>
            ////////////////////////////////////////////////////////////
            public Context()
            {
                myThis = sfContext_Create();
            }

            /// <summary>
            /// Global helper context
            /// </summary>
            ////////////////////////////////////////////////////////////
            public static Context Global
            {
                get
                {
                    if (ourGlobalContext == null)
                        ourGlobalContext = new Context();

                    return ourGlobalContext;
                }
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Finalizer
            /// </summary>
            ////////////////////////////////////////////////////////////
            ~Context()
            {
                sfContext_Destroy(myThis);
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Activate or deactivate the context
            /// </summary>
            /// <param name="active">True to activate, false to deactivate</param>
            ////////////////////////////////////////////////////////////
            public void SetActive(bool active)
            {
                sfContext_SetActive(myThis, active);
            }

            ////////////////////////////////////////////////////////////
        }
    }
}