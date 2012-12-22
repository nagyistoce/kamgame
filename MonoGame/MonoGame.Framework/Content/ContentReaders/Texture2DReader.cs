#region License

/*
MIT License
Copyright ? 2006 The Mono.Xna Team

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

#endregion License


using System;
using Microsoft.Xna.Framework.Graphics;


namespace Microsoft.Xna.Framework.Content
{
    internal class Texture2DReader : ContentTypeReader<Texture2D>
    {
#if ANDROID
        private static readonly string[] supportedExtensions = new[] { ".jpg", ".bmp", ".jpeg", ".png", ".gif" };
#else
        static string[] supportedExtensions = new string[] { ".jpg", ".bmp", ".jpeg", ".png", ".gif", ".pict", ".tga" };
#endif

        internal static string Normalize(string fileName) { return Normalize(fileName, supportedExtensions); }

        protected internal override Texture2D Read(ContentReader reader, Texture2D existingInstance)
        {
            Texture2D texture = null;

            SurfaceFormat surfaceFormat;
            if (reader.version < 5)
            {
                var legacyFormat = (SurfaceFormat_Legacy)reader.ReadInt32();
                switch (legacyFormat)
                {
                    case SurfaceFormat_Legacy.Dxt1:
                        surfaceFormat = SurfaceFormat.Dxt1;
                        break;
                    case SurfaceFormat_Legacy.Dxt3:
                        surfaceFormat = SurfaceFormat.Dxt3;
                        break;
                    case SurfaceFormat_Legacy.Dxt5:
                        surfaceFormat = SurfaceFormat.Dxt5;
                        break;
                    case SurfaceFormat_Legacy.Color:
                        surfaceFormat = SurfaceFormat.Color;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                surfaceFormat = (SurfaceFormat)reader.ReadInt32();
            }

            var width = (reader.ReadInt32());
            var height = (reader.ReadInt32());
            var levelCount = (reader.ReadInt32());

            var convertedFormat = surfaceFormat;
            switch (surfaceFormat)
            {
#if IOS
    // At the moment. If a DXT Texture comes in on iOS, it's really a PVR compressed
    // texture. We need to use this hack until the content pipeline is implemented.
    // For now DXT5 means we're using 4bpp PVRCompression and DXT3 means 2bpp. Look at
    // PvrtcBitmapContent.cs for more information.:
				case SurfaceFormat.Dxt3:
					convertedFormat = SurfaceFormat.RgbaPvrtc2Bpp;
					break;
				case SurfaceFormat.Dxt5:
					convertedFormat = SurfaceFormat.RgbaPvrtc4Bpp;
					break;
#elif ANDROID || PSM
                case SurfaceFormat.Dxt1:
                case SurfaceFormat.Dxt3:
                case SurfaceFormat.Dxt5:
                    convertedFormat = SurfaceFormat.Color;
                    break;
#endif
                case SurfaceFormat.NormalizedByte4:
                    convertedFormat = SurfaceFormat.Color;
                    break;
            }

            if (existingInstance == null)
                texture = new Texture2D(reader.GraphicsDevice, width, height, levelCount > 1, convertedFormat);
            else
                texture = existingInstance;

            for (var level = 0; level < levelCount; level++)
            {
                var levelDataSizeInBytes = (reader.ReadInt32());
                var levelData = reader.ReadBytes(levelDataSizeInBytes);
                var levelWidth = width >> level;
                var levelHeight = height >> level;
                //Convert the image data if required
                switch (surfaceFormat)
                {
#if ANDROID || PSM
                        //no Dxt in OpenGL ES
                    case SurfaceFormat.Dxt1:
                        levelData = DxtUtil.DecompressDxt1(levelData, levelWidth, levelHeight);
                        break;
                    case SurfaceFormat.Dxt3:
                        levelData = DxtUtil.DecompressDxt3(levelData, levelWidth, levelHeight);
                        break;
                    case SurfaceFormat.Dxt5:
                        levelData = DxtUtil.DecompressDxt5(levelData, levelWidth, levelHeight);
                        break;
#endif
                    case SurfaceFormat.Bgr565:
                    {
                        /*
							// BGR -> BGR
							int offset = 0;
							for (int y = 0; y < levelHeight; y++)
							{
								for (int x = 0; x < levelWidth; x++)
								{
									ushort pixel = BitConverter.ToUInt16(levelData, offset);
									pixel = (ushort)(((pixel & 0x0FFF) << 4) | ((pixel & 0xF000) >> 12));
									levelData[offset] = (byte)(pixel);
									levelData[offset + 1] = (byte)(pixel >> 8);
									offset += 2;
								}
							}
							 */
                    }
                        break;
                    case SurfaceFormat.Bgra5551:
                    {
#if OPENGL
                        // Shift the channels to suit OPENGL
                        var offset = 0;
                        for (var y = 0; y < levelHeight; y++)
                        {
                            for (var x = 0; x < levelWidth; x++)
                            {
                                var pixel = BitConverter.ToUInt16(levelData, offset);
                                pixel = (ushort)(((pixel & 0x7FFF) << 1) | ((pixel & 0x8000) >> 15));
                                levelData[offset] = (byte)(pixel);
                                levelData[offset + 1] = (byte)(pixel >> 8);
                                offset += 2;
                            }
                        }
#endif
                    }
                        break;
                    case SurfaceFormat.Bgra4444:
                    {
#if OPENGL
                        // Shift the channels to suit OPENGL
                        var offset = 0;
                        for (var y = 0; y < levelHeight; y++)
                        {
                            for (var x = 0; x < levelWidth; x++)
                            {
                                var pixel = BitConverter.ToUInt16(levelData, offset);
                                pixel = (ushort)(((pixel & 0x0FFF) << 4) | ((pixel & 0xF000) >> 12));
                                levelData[offset] = (byte)(pixel);
                                levelData[offset + 1] = (byte)(pixel >> 8);
                                offset += 2;
                            }
                        }
#endif
                    }
                        break;
                    case SurfaceFormat.NormalizedByte4:
                    {
                        var bytesPerPixel = surfaceFormat.Size();
                        var pitch = levelWidth * bytesPerPixel;
                        for (var y = 0; y < levelHeight; y++)
                        {
                            for (var x = 0; x < levelWidth; x++)
                            {
                                var color = BitConverter.ToInt32(levelData, y * pitch + x * bytesPerPixel);
                                levelData[y * pitch + x * 4] = (byte)(((color >> 16) & 0xff)); //R:=W
                                levelData[y * pitch + x * 4 + 1] = (byte)(((color >> 8) & 0xff)); //G:=V
                                levelData[y * pitch + x * 4 + 2] = (byte)(((color) & 0xff)); //B:=U
                                levelData[y * pitch + x * 4 + 3] = (byte)(((color >> 24) & 0xff)); //A:=Q
                            }
                        }
                    }
                        break;
                }

                texture.SetData(level, null, levelData, 0, levelData.Length);
            }

            return texture;
        }
    }
}