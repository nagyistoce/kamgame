using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace KamGame.Wallpapers
{

    public abstract class Layer
    {
        public abstract object NewComponent(Scene scene);

        public static T ApplyPattern<T>(T target, object pattern) where T : class
        {
            if (target == null || pattern == null) return target;

            var type = target.GetType();
            foreach (var patternProp in pattern.GetType().GetFields())
            {
                var value = patternProp.GetValue(pattern);
                if (value == null) continue;

                var prop = type.GetField(patternProp.Name);
                if (prop == null) continue;

                if (value.GetType().IsArray)
                {
                    prop.SetValue(target, value);
                    continue;
                }

                var valueList = value as IList;
                if (valueList != null)
                {
                    var list = prop.GetValue(target) as IList;
                    if (list == null) continue;
                    list.Clear();

                    var olist = list as IListInfo;

                    foreach (var value2 in valueList)
                    {
                        var layer = value2 as Layer;
                        if (layer != null)
                        {
                            var targetLayer = Activator.CreateInstance(olist != null ? olist.ItemType : layer.GetType());
                            ApplyPattern(targetLayer, layer);
                            list.Add(targetLayer);
                        }
                        else
                            list.Add(value2);
                    }
                }
                else
                {
                    if (value is Layer)
                    {
                        ApplyPattern(prop.GetValue(target), value);
                    }
                    else
                    {
                        prop.SetValue(target, value);
                    }
                }
            }

            return target;
        }

        public static T ApplyPatterns<T>(T target, IEnumerable patterns) where T : class
        {
            foreach (var pattern in patterns)
            {
                ApplyPattern(target, pattern);
            }
            return target;
        }
    }


    public abstract class Layer<TPattern> : Layer where TPattern : Layer
    {
        public TPattern Pattern { set { ApplyPattern(this, value); } }
        public IEnumerable<TPattern> Patterns { set { ApplyPatterns(this, value); } }
    }


    public class LayerComponent : DrawableGame2DComponent
    {
        public LayerComponent(Scene scene)
            : base(scene.Theme.Game)
        {
            Scene = scene;
        }

        public Scene Scene { get; private set; }

        public Texture2D LoadTexture(string assetName)
        {
            return Scene.LoadTexture(assetName);
        }

    }

}
