using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xebab.Model;
using Microsoft.Xna.Framework.Graphics;

namespace Xebab.Graphics.Font
{
    public class FontResource : IResource
    {
        public SpriteFont Font { get; internal set; }
        public string AssetName { get; internal set; }
    }
}
