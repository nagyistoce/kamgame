using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;


namespace KamGame.Wallpaper
{

    public abstract class Layer
    {
        public abstract GameComponent NewComponent(Scene scene);

        public static void ApplyPattern(object target, object pattern)
        {
            if (target == null || pattern == null) return;

            var type = target.GetType();
            foreach (var patternProp in pattern.GetType().GetFields())
            {
                var value = patternProp.GetValue(pattern);
                if (value == null) continue;

                var prop = type.GetField(patternProp.Name);
                if (prop == null) continue;

                var valueList = value as IList;
                if (valueList != null)
                {
                    var list = prop.GetValue(target) as IList;
                    if (list != null)
                    {
                        list.Clear();
                        foreach (var value2 in valueList)
                        {
                            list.Add(value2);
                        }
                    }
                }
                else
                {
                    prop.SetValue(target, value);
                }
            }
        }

    }


    public abstract class Layer<TPattern> : Layer
        where TPattern : Layer
    {

        private TPattern _pattern;
        public TPattern Pattern
        {
            get { return _pattern; }
            set { if (_pattern != value) ApplyPattern(this, _pattern = value); }
        }

    }


    public class LayerComponent<TLayer> : DrawableGame2DComponent
        where TLayer : Layer
    {
        public LayerComponent(Scene scene, TLayer layer)
            : base(scene.Theme.Game)
        {
            Scene = scene;
            Layer.ApplyPattern(this, layer);
        }

        public Scene Scene { get; private set; }

        public T Load<T>(string assetName)
        {
            return Scene.Load<T>(assetName);
        }

    }

}
