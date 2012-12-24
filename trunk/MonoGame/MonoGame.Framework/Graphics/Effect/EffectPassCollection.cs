using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    public class EffectPassCollection : IEnumerable<EffectPass>
    {
        private readonly List<EffectPass> _passes = new List<EffectPass>();

        internal EffectPassCollection()
        {
        }

        internal EffectPassCollection(Effect effect, EffectPassCollection cloneSource)
        {
            foreach (EffectPass pass in cloneSource)
                Add(new EffectPass(effect, pass));
        }

        public EffectPass this[int index]
        {
            get { return _passes[index]; }
        }

        public EffectPass this[string name]
        {
            get
            {
                // TODO: Add a name to pass lookup table.
                foreach (EffectPass pass in _passes)
                {
                    if (pass.Name == name)
                        return pass;
                }
                return null;
            }
        }

        public int Count
        {
            get { return _passes.Count; }
        }

        IEnumerator<EffectPass> IEnumerable<EffectPass>.GetEnumerator()
        {
            return _passes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _passes.GetEnumerator();
        }

        public List<EffectPass>.Enumerator GetEnumerator()
        {
            return _passes.GetEnumerator();
        }

        internal void Add(EffectPass pass)
        {
            _passes.Add(pass);
        }
    }
}