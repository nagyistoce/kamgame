using System.Collections;
using System.Collections.Generic;


namespace Microsoft.Xna.Framework.Graphics
{
    public class EffectParameterCollection : IEnumerable<EffectParameter>
    {
        private readonly List<EffectParameter> _parameters = new List<EffectParameter>();

        internal EffectParameterCollection() { }

        internal EffectParameterCollection(EffectParameterCollection cloneSource)
        {
            foreach (var parameter in cloneSource)
                Add(new EffectParameter(parameter));
        }

        public int Count { get { return _parameters.Count; } }

        public EffectParameter this[int index] { get { return _parameters[index]; } }

        public EffectParameter this[string name]
        {
            get
            {
                // TODO: Add a name to parameter lookup table.
                foreach (var parameter in _parameters)
                {
                    if (parameter != null && parameter.Name == name)
                        return parameter;
                }

                return null;
            }
        }

        public IEnumerator<EffectParameter> GetEnumerator() { return _parameters.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return _parameters.GetEnumerator(); }

        internal void Add(EffectParameter param) { _parameters.Add(param); }
    }
}