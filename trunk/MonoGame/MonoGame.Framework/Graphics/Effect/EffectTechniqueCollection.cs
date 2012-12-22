using System.Collections;
using System.Collections.Generic;


namespace Microsoft.Xna.Framework.Graphics
{
    public class EffectTechniqueCollection : IEnumerable<EffectTechnique>
    {
        private readonly List<EffectTechnique> _techniques = new List<EffectTechnique>();

        public int Count { get { return _techniques.Count; } }

        internal EffectTechniqueCollection() { }

        internal EffectTechniqueCollection(Effect effect, EffectTechniqueCollection cloneSource)
        {
            foreach (var technique in cloneSource)
                Add(new EffectTechnique(effect, technique));
        }

        public EffectTechnique this[int index] { get { return _techniques[index]; } }

        public EffectTechnique this[string name]
        {
            get
            {
                // TODO: Add a name to technique lookup table.
                foreach (var technique in _techniques)
                {
                    if (technique.Name == name)
                        return technique;
                }

                return null;
            }
        }

        public IEnumerator<EffectTechnique> GetEnumerator() { return _techniques.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return _techniques.GetEnumerator(); }

        internal void Add(EffectTechnique technique) { _techniques.Add(technique); }
    }
}