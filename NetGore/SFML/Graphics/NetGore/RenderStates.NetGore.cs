using System;
using System.Runtime.InteropServices;

namespace SFML
{
    namespace Graphics
    {
        public partial class RenderStates
        {
            internal MarshalData Marshal(Transform transform)
            {
                MarshalData data = new MarshalData();
                data.blendMode = BlendMode;
                data.transform = Transform * transform;
                data.texture = Texture != null ? Texture.CPointer : IntPtr.Zero;
                data.shader = Shader != null ? Shader.CPointer : IntPtr.Zero;

                return data;
            }
        }
    }
}