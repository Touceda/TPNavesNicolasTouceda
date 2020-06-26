using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using Engine;
using Engine.Extensions;

namespace Game
{
    public class Projectile : GameObject
    {
        private Image img;
        private float speed = 700;

        public Projectile()
        {
            img = Properties.Resources.projectile;
            Extent = img.Size;
        }

        public override void Update(float deltaTime)
        {
            if (Position.X > 1600)
            {
                Delete();
            }
            X += speed * deltaTime;          
            CheckForCollision();
        }

        private void CheckForCollision()//Aplicar la misma optimizacion que la nave
        {
            EnemyShip[] EnemyCollisions = AllObjects.Select(m => m as EnemyShip).Where(m => m != null).ToArray();

            if (EnemyCollisions.Count() == 0) return;

            foreach (var enemy in EnemyCollisions)
            {
                if (CollidesWith(enemy))
                {
                    enemy.Explode();
                    Delete();
                }
            }
        }

        public override void DrawOn(Graphics graphics)
        {
            graphics.DrawImage(img, Position);
        }
    }
}
