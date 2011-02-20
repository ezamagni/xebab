using Microsoft.Xna.Framework;

namespace Xebab.Behaviors
{
    /// <summary>
    /// IBehavior defines an object whose purpose is to dynamically
    /// change or enrich the logic of an IBehavable.
    /// </summary>
    public interface IBehavior
    {
        /// <summary>
        /// Indicates if the behavior is functional.
        /// An inactive behavior doesn't provide any change to T
        /// </summary>
        bool Active { get; }

        /// <summary>
        /// Activate the behavior
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the behavior from receiving
        /// calls to its Update method.
        /// </summary>
        void Stop();

        /// <summary>
        /// Resets the state of the behavior
        /// by calling its Initialize method
        /// </summary>
        void Restart();

        /// <summary>
        /// Removes the behaviour from its 
        /// subject collection.
        /// </summary>
        void Terminate();

        /// <summary>
        /// The specific logic brought by the behavior
        /// </summary>
        /// <param name="gameTime"></param>
        void Update(GameTime gameTime);
    }
}