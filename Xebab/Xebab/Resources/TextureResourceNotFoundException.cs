using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xebab.Model
{
    public class TextureResourceNotFoundException : Exception
    {
        public TextureResourceNotFoundException()
        {
        }

        public TextureResourceNotFoundException(string message)
            : base(message)
        {
        }

        public TextureResourceNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
