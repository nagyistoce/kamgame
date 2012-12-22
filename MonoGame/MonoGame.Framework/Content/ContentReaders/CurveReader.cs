namespace Microsoft.Xna.Framework.Content
{
    internal class CurveReader : ContentTypeReader<Curve>
    {
        protected internal override Curve Read(ContentReader input, Curve existingInstance)
        {
            var curve = existingInstance;
            if (curve == null)
            {
                curve = new Curve();
            }

            curve.PreLoop = (CurveLoopType)input.ReadInt32();
            curve.PostLoop = (CurveLoopType)input.ReadInt32();
            var num6 = input.ReadInt32();

            for (var i = 0; i < num6; i++)
            {
                var position = input.ReadSingle();
                var num4 = input.ReadSingle();
                var tangentIn = input.ReadSingle();
                var tangentOut = input.ReadSingle();
                var continuity = (CurveContinuity)input.ReadInt32();
                curve.Keys.Add(new CurveKey(position, num4, tangentIn, tangentOut, continuity));
            }
            return curve;
        }
    }
}