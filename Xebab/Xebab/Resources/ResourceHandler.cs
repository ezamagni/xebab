using System;
using System.Collections.Generic;
using System.Linq;

namespace Xebab.Resources
{
    public class ResourceHandler
    {
        private List<Resource> resources;

        internal ResourceHandler(List<Resource> resources)
        {
            this.resources = resources;
        }

        public T GetResource<T>(string assetName) where T:Resource
        {
            if (assetName == null)
                throw new ArgumentNullException("assetName");

            T resource = resources.OfType<T>().SingleOrDefault(r => r.AssetName == assetName);

            if (resource == null)
                throw new TextureResourceNotFoundException(assetName);

            return resource;
        }
    }
}
