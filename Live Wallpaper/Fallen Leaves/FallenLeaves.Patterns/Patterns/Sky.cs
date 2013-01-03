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
            sky3 = new Sky { Width = 1.5f, TextureNames = "sky/back03_1, sky/back03_2", RowCount = 2 };
            sky4 = new Sky { Width = 1.5f, TextureNames = "sky/back04_1, sky/back04_2", RowCount = 2 };
            sky4a = new Sky { Width = 1.5f, TextureNames = "sky/back04a_1, sky/back04a_2", RowCount = 2 };
        }

    }
}
