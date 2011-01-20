using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Xebab.Model;
using Xebab.Helpers;

namespace Xebab.Graphics.Sprites
{
	public class SpriteHandler
	{
		//Vertically-sorted collection of scene sprites
		private List<Sprite> spritesDescending;
		//Queue of sprites to be added
		private Queue<Sprite> spritesToAdd;
		//Queue of sprites to be removed
		private Queue<Sprite> spritesToRemove;

		//Unsorted collection of scene sprites
		public LinkedList<Sprite> Sprites { get; private set; }
		public IEnumerable<Sprite> DescendingSprites
		{
			get
			{
				return spritesDescending;
			}
		}

		public SpriteHandler()
		{
			spritesToRemove = new Queue<Sprite>();
			spritesToAdd = new Queue<Sprite>();
			spritesDescending = new List<Sprite>();

			Sprites = new LinkedList<Sprite>();
		}

		public void SpawnSprite(Sprite sprite)
		{
			spritesToAdd.Enqueue(sprite);
		}

		public void RemoveSprite(Sprite sprite)
		{
			spritesToRemove.Enqueue(sprite);
		}

		private void InsertSprite(Sprite sprite)
		{
			Sprites.AddLast(sprite);
			spritesDescending.Add(sprite);
		}

		private void DeleteSprite(Sprite sprite)
		{
			Sprites.Remove(sprite);
			spritesDescending.Remove(sprite);
		}

		public void Update(GameTime gameTime)
		{
			//Update logic of every sprite in the collection
			foreach (Sprite s in Sprites)
				s.Update(gameTime);

			//Add new sprites (if any)
			while (spritesToAdd.Count > 0)
				InsertSprite(spritesToAdd.Dequeue());

			//Remove sprites scheduled for removal (if any)
			while (spritesToRemove.Count > 0)
				DeleteSprite(spritesToRemove.Dequeue());

			//Vertically-sort sprites
			spritesDescending = spritesDescending.OrderBy(s => s.StandPoint.Y).ToList<Sprite>();
        }

		public bool TestCollision(Sprite s1, Sprite s2, CollisionTestMode testMode)
        {
            switch (testMode)
            {
                case CollisionTestMode.Simple:
                    //Simply check if main bounding boxes collide
                    return s1.BoundingBox.ShiftedPosition(s1.Position).Intersects(s2.BoundingBox.ShiftedPosition(s2.Position));
                    
                case CollisionTestMode.BoxSet:
                    //First check whether main bounding boxes collide (fast negative)
					if (!TestCollision(s1, s2, CollisionTestMode.Simple)) return false;
					if (s1.CollisionBoxSet == null || s2.CollisionBoxSet == null) return true;

                    foreach (Rectangle rect1 in s1.CollisionBoxSet)
                    {
                        foreach (Rectangle rect2 in s2.CollisionBoxSet)
                        {
                            if(rect1.ShiftedPosition(s1.Position).Intersects(rect2.ShiftedPosition(s2.Position))) return true;
                        }
                    }
                    return false;

                case CollisionTestMode.Blocks:
                    //Simply check each sprite'e resident blocks
                    foreach (Block b in s1.BlockRegion) //provvisorio 
                    { 
                        if(b.Location.IsInside(s2.BlockRegion)) return true;
                    }
					return false;

				case CollisionTestMode.Distance:
				default:
					throw new NotImplementedException();
            }
        }
    }
}
