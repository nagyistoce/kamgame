using System.Linq;
using Microsoft.Xna.Framework.Graphics;


namespace Microsoft.Xna.Framework.Content
{
    internal class EffectReader : ContentTypeReader<Effect>
    {
        private static readonly string[] supportedExtensions = new[] { ".fxg" };

        public static string Normalize(string FileName) { return Normalize(FileName, supportedExtensions); }

        private static string TryFindAnyCased(string search, string[] arr, params string[] extensions) { return arr.FirstOrDefault(s => extensions.Any(ext => s.ToLower() == (search.ToLower() + ext))); }

        private static bool Contains(string search, string[] arr) { return arr.Any(s => s == search); }

        protected internal override Effect Read(ContentReader input, Effect existingInstance)
        {
            var count = input.ReadInt32();

            var effect = new Effect(input.GraphicsDevice, input.ReadBytes(count));
            effect.Name = input.AssetName;

            return effect;
        }
    }
}