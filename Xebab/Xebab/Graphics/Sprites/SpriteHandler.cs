using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Xebab.Helpers;

namespace Xebab.Graphics.Sprites
{
	public class SpriteHandler
	{
		//Queue of sprites to be added
		private Queue<Sprite> spritesToAdd;
		//Queue of sprites to be removed
		private Queue<Sprite> spritesToRemove;

		//Unsorted collection of scene sprites
		public LinkedList<Sprite> Sprites { get; private set; }

		public SpriteHandler()
		{
			spritesToRemove = new Queue<Sprite>();
			spritesToAdd = new Queue<Sprite>();

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
		}

		private void DeleteSprite(Sprite sprite)
		{
			Sprites.Remove(sprite);
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
        }
    }
}
