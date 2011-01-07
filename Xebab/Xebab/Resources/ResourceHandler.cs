using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xebab.Model
{
    public class ResourceHandler
    {
        private List<IResource> resources;

        internal ResourceHandler(List<IResource> resources)
        {
            this.resources = resources;
        }

        public T GetResource<T>(string assetName) where T:IResource
        {
            if (assetName == null)
                throw new ArgumentNullException("assetName");

            T resource = resources.OfType<T>().SingleOrDefault(r => r.AssetName == assetName);

            //TODO -oALEX: non ho avuto tempo, creare la classe    TextureResourceNotFoundException : Exception
            if (resource == null)
                throw new TextureResourceNotFoundException(assetName);

            return resource;
        }
    }
}
