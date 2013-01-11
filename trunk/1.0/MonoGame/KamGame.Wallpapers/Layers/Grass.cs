using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KamGame.Wallpapers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace KamGame.Wallpapers
{
    public class Grass : Layer<Grass>
    {
        public static float DensityFactor = 1;


        public string TextureNames;

        /// <summary>
        /// координаты точки начала травинки (на текстуре). Считается от левого верхнего угла текстуры
        /// </summary>
        public Vector2? BeginPoint;

        /// <summary>
        /// плотность травы (кол-во травинок в пределах экрана)
        /// </summary>
        public int? Density;

        /// <summary>
        /// Масштаб травинки (относительно максимального размера экрана)
        /// </summary>
        public float? MinScale, MaxScale;

        /// <summary>
        /// Максимальный угол наклона
        /// </summary>
        public float? MaxAngle;

        /// <summary>
        /// пределы случайного изменения начального угла наклона каждой травинки
        /// </summary>
        public float? MinRotation, MaxRotation;
        public float? Opacity;

        /// <summary>
        /// коэф-т изменения угола наклона в зависимости от силы ветра. Не влияет на колебания
        /// </summary>
        public float? K0;

        /// <summary>
        /// амплитуда волны колебаний (что проходит по траве через весь экран)
        /// </summary>
        public float? K0w;

        /// <summary>
        ///  период волны колебаний (что проходит по траве через весь экран)
        /// </summary>
        public int? K0p;

        /// <summary>
        /// коэф-т изменения угла наклона  в зависимости от силы ветра. Но он влияет на колебания
        /// </summary>
        public float? minK1, maxK1;

        /// <summary>
        /// коэф-т реакции на изменение ветра (проявляется при резких перепадах
        /// </summary>
        public float? minK2, maxK2;

        /// <summary>
        ///  амплитуда случайных колебаний
        /// </summary>
        public float? minK3, maxK3;

        /// <summary>
        /// период случайных колебаний
        /// </summary>
        public int? minK3p, maxK3p;

        /// <summary>
        /// коэф-т затухания колебаний
        /// </summary>
        public float? minK4, maxK4;

        /// <summary>
        /// коэф-т упругости - чем больше, тем быстрее ветка возвращается к начальному положению
        /// </summary>
        public float? minK5, maxK5;

        public override object NewComponent(Scene scene)
        {
            return ApplyPattern(new GrassPart(), this);
        }
    }


    public class GrassPart
    {
        public Scene Scene { get; set; }
        public GroundSprite Ground { get; set; }

        private List<Herb> Herbs;
        private Texture2D[] Textures;

        public string TextureNames;

        public Vector2 BeginPoint;
        public int Density;
        public float MinScale, MaxScale;
        public float MaxAngle;
        public float MinRotation, MaxRotation;
        public float Opacity = .7f;
        public float K0 = .5f;
        public float K0w = .8f;
        public int K0p = 20;
        public float minK1 = .00015f, maxK1 = .00025f;
        public float minK2 = .15f, maxK2 = .25f;
        public float minK3 = .00125f, maxK3 = .00135f;
        public int minK3p = 50, maxK3p = 100;
        public float minK4 = .012f, maxK4 = .016f;
        public float minK5 = .025f, maxK5 = .035f;


        private Color OpacityColor;

        public void LoadContent()
        {
            var timeScale = Ground.Game.GameTimeScale;
            var accScale = Ground.Game.GameAccelerateScale;
            K0w *= accScale;
            K0p = (int)(K0p * timeScale);
            minK1 *= accScale;
            maxK1 *= accScale;
            minK2 *= accScale;
            maxK2 *= accScale;
            minK3 *= accScale;
            maxK3 *= accScale;
            minK3p = (int)(minK3p * timeScale);
            maxK3p = (int)(maxK3p * timeScale);
            minK4 *= accScale;
            maxK4 *= accScale;
            minK5 *= accScale;
            maxK5 *= accScale;

            var textureNames = (TextureNames ?? "")
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim()).ToArray();
            Textures = new Texture2D[textureNames.Length];

            var i1 = 0;
            foreach (var textureName in textureNames)
            {
                Textures[i1++] = Scene.LoadTexture(textureName);
            }

            var game = Scene.Theme.Game;

            OpacityColor = Ground.Scene.BlackColor * Opacity;

            var count = (int)(Density * Scene.Width * Grass.DensityFactor);
            Herbs = new List<Herb>(count);

            var heights = Ground.Heights ?? new int[0];
            var step = heights != null && heights.Length > 0 ? Ground.WidthPx / Ground.RepeatX / heights.Length : 0;

            for (var i = 0; i < count; i++)
            {
                var h = new Herb
                {
                    Texture = Textures[game.Rand(Textures.Length)],
                    X = game.Rand(Ground.WidthPx),
                    Scale = MinScale + (MaxScale - MinScale) * game.Rand(),
                    Angle0 = MinRotation + (MaxRotation - MinRotation) * game.Rand(),
                    K1 = game.Rand(minK1, maxK1),
                    K2 = game.Rand(minK2, maxK2),
                    K3 = game.Rand(minK3, maxK3),
                    K3p = game.Rand(minK3p, maxK3p),
                    K4 = game.Rand(minK4, maxK4),
                    K5 = game.Rand(minK5, maxK5),
                };

                if (step > 0)
                {
                    var hi0 = (h.X / step) % heights.Length;
                    var hi1 = hi0 < heights.Length - 1 ? hi0 + 1 : 0;
                    var x0 = (h.X / step) * step;
                    var x1 = x0 + step;
                    //
                    h.Y = Ground.HeightPx - (heights[hi0] + (heights[hi1] - heights[hi0]) * (h.X - x0) / (x1 - x0));
                }
                Herbs.Add(h);
            }

        }


        public void Update(int minX, int maxX)
        {
            var game = Ground.Game;
            var wind = Scene.WindStrength;
            var awind = Math.Abs(wind);
            var wind0 = Scene.PriorWindStrength;
            float ticks = game.FrameIndex;

            var windAngle = K0 * MaxAngle * wind;
            var windAngleW = K0w * MaxAngle * wind;
            var k01 = (2 + awind) * Math.PI / (maxX - minX);
            var k0 = -(float)Math.Sign(wind) * ticks / K0p - k01 * minX;
            var k2 = wind - wind0;
            if (game.PriorAcceleration != Vector3.Zero)
                k2 += MathHelper.Clamp((game.Acceleration.X - game.PriorAcceleration.X + game.Acceleration.Y - game.PriorAcceleration.Y) / 50, -.3f, .3f);
            foreach (var h in Herbs)
            {
                if (h.X < minX || h.X > maxX) continue;

                //h.windAngle = windAngle;

                h.angleSpeed += 0
                    + windAngleW * (float)Math.Sin(k01 * h.X + k0)
                    + h.K1 * wind
                    + h.K2 * k2
                    //+ h.K3 * awind * awind * (float)Math.Sin(ticks / h.K3p)
                    - h.K5 * h.Scale * (h.Angle + .3f * windAngle) / MaxAngle;
                h.angleSpeed *= 1 - h.K4;

                //if (h.Angle < -maxAngle && h.angleSpeed < 0 || h.Angle > maxAngle && h.angleSpeed > 0)
                //    h.angleSpeed = 0;
                h.Angle += h.angleSpeed;
                //h.Angle = MathHelper.Clamp(h.Angle, -maxAngle, maxAngle);
            }

        }

        public void Draw(int minX, int maxX)
        {
            var game = Ground.Game;
            var gscale = Ground.Scale;
            foreach (var h in Herbs)
            {
                if (h.X < minX || h.X > maxX) continue;

                game.Draw(h.Texture,
                    h.X * gscale - Ground.Offset,
                    game.ScreenHeight - h.Y * gscale,
                    origin: BeginPoint,
                    scale: h.Scale * game.LandscapeHeight / h.Texture.Height,
                    rotation: h.Angle0 + h.Angle + h.windAngle
                    //, effect: h.Effect
                    , color: OpacityColor
                );
            }
        }


        public class Herb
        {
            public Texture2D Texture;
            public float Scale;
            public int X, Y;
            public float Angle0, Angle;
            public float K1, K2, K3, K4, K5;
            public int K3p;
            public float angleSpeed;
            public float windAngle;
        }

    }
}
