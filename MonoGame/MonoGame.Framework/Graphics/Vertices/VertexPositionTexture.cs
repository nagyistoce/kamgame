using System.Runtime.InteropServices;


namespace Microsoft.Xna.Framework.Graphics
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPositionTexture : IVertexType
    {
        public Vector3 Position;
        public Vector2 TextureCoordinate;
        public static readonly VertexDeclaration VertexDeclaration;

        public VertexPositionTexture(Vector3 position, Vector2 textureCoordinate)
        {
            Position = position;
            TextureCoordinate = textureCoordinate;
        }

        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }

        public override int GetHashCode()
        {
            // TODO: Fix get hashcode
            return 0;
        }

        public override string ToString() { return string.Format("{{Position:{0} TextureCoordinate:{1}}}", new object[] { Position, TextureCoordinate }); }

        public static bool operator ==(VertexPositionTexture left, VertexPositionTexture right) { return ((left.Position == right.Position) && (left.TextureCoordinate == right.TextureCoordinate)); }

        public static bool operator !=(VertexPositionTexture left, VertexPositionTexture right) { return !(left == right); }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != base.GetType())
            {
                return false;
            }
            return (this == ((VertexPositionTexture)obj));
        }

        static VertexPositionTexture()
        {
            var elements = new[]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
            };
            var declaration = new VertexDeclaration(elements);
            VertexDeclaration = declaration;
        }
    }
}