using Microsoft.Xna.Framework;

namespace Xebab.Behaviors
{
    /// <summary>
    /// This abstract class defines the base implementation of a Behavior. 
    /// </summary>
    /// <typeparam name="T">The type of the object a behavior applies to.</typeparam>

    public abstract class Behavior<T> : IBehavior where T : IBehavable
    {
        /// <summary>
        /// The object this behavior applies to.
        /// </summary>
        protected T Subject { get; private set; }

        /// <summary>
        /// Indicates if the behavior is functional.
        /// </summary>
        public bool Active { get; private set; }


        public Behavior(T subject)
        {
            this.Subject = subject;
            Initialize();
        }


        /// <summary>
        /// Initialize the state of this behavior. Especially in the
        /// case this behavior's logic is time-dependant this method
        /// is useful to reset its state.
        /// </summary>
        protected virtual void Initialize()
        {
            this.Active = true;
        }

        public void Start()
        {
            this.Active = true;
        }

        public void Stop()
        {
            this.Active = false;
        }

        public void Restart()
        {
            Initialize();
            Start();
        }

        public void Terminate()
        {
            Subject.RemoveBehavior(this);
        }

        /// <summary>
        /// This method contains the specific logic this
        /// behaviors adds to its subject T.
        /// </summary>
        public abstract void Update(GameTime gameTime);

    }
}
