using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Content
{
    partial class ContentManager
    {

        public void Unload(string assetName)
        {
            object asset;
            if (!loadedAssets.TryGetValue(assetName, out asset)) return;
            var disposable = asset as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
                disposableAssets.Remove(disposable);
            }
            loadedAssets.Remove(assetName);
        }


    }
}
