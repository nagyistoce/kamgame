using System;
using System.Collections.Generic;
using System.Text;
using KamGame.Wallpapers;


namespace FallenLeaves
{
    partial class FallenLeavesPattern
    {
        public Sky sky2, sky3, sky4, sky4a;

        public void CreateSkys()
        {
            sky2 = new Sky { Width = 1.5f, TextureNames = "sky/back02", Stretch = true, };
            sky3 = new Sky { Width = 1.5f, TextureNames = "sky/back03", BaseVScale = 1.5f };
            sky4 = new Sky { Width = 1.5f, TextureNames = "sky/back04", BaseVScale = 1.5f };
            sky4a = new Sky { Width = 1.5f, TextureNames = "sky/back04a", BaseVScale = 1.5f };
        }

    }
}
