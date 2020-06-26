using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using Engine;
using Engine.Utils;
using System.Runtime.Remoting.Services;

namespace Game
{
    public class EnemyShip : GameObject
    {
        private static Random rnd = new Random();
        //private static bool flag = false;
        private int shipIndex;
        private EnemyBehavior behavior;
      
        public EnemyShip(int shipIndex, EnemyBehavior behavior)
        {    
            this.shipIndex = shipIndex;
            this.behavior = behavior;
            this.MiImagen = LoadImage() ;//Va a guardar mi Imagen de este objeto particular

            Visible = false;
        }

        public PlayerShip Player
        {
            get
            {
                return AllObjects
                    .Select(obj => obj as PlayerShip)
                    .FirstOrDefault(obj => obj != null);
            }
        }

        public EnemyBehavior Behavior
        {
            get { return behavior; }
        }

        public override void Update(float deltaTime)
        {
            behavior.Update(this, deltaTime);
            Visible = true;
        }

        public void Explode()
        {
            if (rnd.NextDouble() > 0.95)
            {
                PowerUp pup = new PowerUp();
                pup.Center = Center;
                Root.AddChild(pup);
            }
            Explosion.Burst(Parent, Center);
            Delete();
        }


        Image MiImagen;
        public override void DrawOn(Graphics graphics)
        {
            graphics.DrawImage(MiImagen, Bounds);
        }

        private Image LoadImage()
        {
            Image ThisImage = Spritesheet.Ships[shipIndex];
            Extent = new SizeF(ThisImage.Size.Width / 2, ThisImage.Size.Height / 2);
            return ThisImage;

            //Image[] ships = Spritesheet.Load(@"Resources\shipsheetparts.png", new Size(200, 200));
            //ships[shipIndex].RotateFlip(RotateFlipType.Rotate270FlipNone);
            //Image result = ships[shipIndex];
            //Extent = new SizeF(result.Size.Width / 2, result.Size.Height / 2);
            //return result;
        }
    }
}
