using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using KamGame;
using KamGame.Converts;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{

    public class Theme
    {
        public Theme(Game2D game) { Game = game; }

        public Game2D Game;
        public string ID;
        public string Title;
        public List<Scene> Scenes { get; private set; }

        public static Theme Load(Game2D game, string themeId)
        {
            var el = game.LoadXml("Themes/" + themeId + "/_theme.xml");
            try
            {
                var theme = new Theme(game)
                {
                    ID = themeId,
                    Title = el.Attr("title"),
                };

                var xpatterns = el.Element("patterns");
                if (xpatterns != null)
                {
                    theme.Patterns.AddRange(xpatterns.Elements().Select(theme.LoadPattern).Where(a => a != null));
                }

                theme.Scenes = el.Elements("scene").Select(a => Scene.Load(theme, a)).ToList();

                return theme;
            }
            catch (Exception ex)
            {
                throw new Exception(@"Theme. Invalid format of file theme.xml. " + ex.Message);
            }
        }


        public Scene StartScene(string sceneId)
        {
            var scene = Scenes.FirstOrDefault(a => string.Equals(a.ID, sceneId, StringComparison.InvariantCultureIgnoreCase));
            if (scene != null)
                scene.Start();
            return scene;
        }


        public static Vector2 PointParse(string value)
        {
            var p = new Vector2();
            var ss = (value ?? "")
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim()).ToArray();
            if (ss.Length > 0) p.X = ss[0].ToFloat();
            if (ss.Length > 1) p.Y = ss[1].ToFloat();
            return p;
        }

        public static Vector2 PointParse(string value, Vector2 defaultValue)
        {
            return value.yes() ? PointParse(value) : defaultValue;
        }

        public readonly List<Pattern> Patterns = new List<Pattern>();

        public abstract class Pattern
        {
            [XmlAttribute("id")]
            public string ID;
        }

        public Pattern LoadPattern(XElement el)
        {
            switch (el.Name.ToString().ToLowerInvariant())
            {
                case "grass":
                    return (Pattern)Deserialize<GroundSprite.Grass.Pattern>(el, new GroundSprite.Grass.Pattern());
                case "treenode":
                    return (Pattern)Deserialize<TreeSprite.TreeSpriteNode.Pattern>(el, new TreeSprite.TreeSpriteNode.Pattern());
                case "wind":
                    return (Pattern)Deserialize<WindController.Pattern>(el, new WindController.Pattern());
            }

            return null;
        }


        public object Deserialize<TPattern>(XElement src, object dest)
            where TPattern : Pattern
        {
            var patternID = src.Attr("pattern");
            var pattern = Patterns.OfType<TPattern>().FirstOrDefault(a => a.ID == patternID);
            var ptype = pattern != null ? pattern.GetType() : null;

            foreach (var dprop in dest.GetType().GetPropertiesAndFields())
            {
                var xmlAttr = dprop.GetAttribute<XmlAttributeAttribute>();
                var value = src.Attr(xmlAttr != null ? xmlAttr.AttributeName : dprop.Name);

                if (value.no())
                {
                    if (pattern == null) continue;
                    var pprop = ptype.GetPropertyOrField(dprop.Name);
                    if (pprop != null)
                        dprop.SetValue(dest, pprop.GetValue(pattern, null), null);
                }
                else
                {
                    if (dprop.ResultType() == typeof(Vector2))
                        dprop.SetValue(dest, PointParse(value), null);
                    else
                        dprop.SetValue(dest, value, null);
                }
            }
            return dest;
        }
    }

}
