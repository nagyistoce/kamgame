using Microsoft.Xna.Framework.Graphics;


namespace Microsoft.Xna.Framework.Content
{
    internal class TextureCubeReader : ContentTypeReader<TextureCube>
    {
        protected internal override TextureCube Read(ContentReader reader, TextureCube existingInstance)
        {
            var surfaceFormat = (SurfaceFormat)reader.ReadInt32();
            var size = reader.ReadInt32();
            var levels = reader.ReadInt32();
            var textureCube = new TextureCube(reader.GraphicsDevice, size, levels > 1, surfaceFormat);
            for (var face = 0; face < 6; face++)
            {
                for (var i = 0; i < levels; i++)
                {
                    var faceSize = reader.ReadInt32();
                    var faceData = reader.ReadBytes(faceSize);
                    textureCube.SetData((CubeMapFace)face, i, null, faceData, 0, faceSize);
                }
            }
            return textureCube;
        }
    }
}