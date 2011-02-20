using Microsoft.Xna.Framework;

namespace Xebab.Behaviors
{
    /// <summary>
    /// Objects that implement the IBehavable interface allow subclasses
    /// of Behavior to dynamically change and control their behavior.
    /// </summary>
    
    public interface IBehavable
    {
        void AddBehavior(IBehavior behavior);

        void RemoveBehavior(IBehavior behavior);
    }
}
