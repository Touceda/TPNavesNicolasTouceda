using Engine;
using Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class SpaceNoise : GameObject
    {
        private Image image; //Conoce su propia imagen
        float speed; //Tiene una velocidad
        float scale; //Tiene una Escala (No se utiliza nunca?)
        bool flipX; //Da la vuelta en X 
        bool flipY; //Da la vuelta en Y 

        public SpaceNoise(Image image, float speed, float scale, bool flipX, bool flipY)
        {
            this.speed = speed;
            this.scale = scale;
            this.flipX = flipX;
            this.flipY = flipY;
            this.image = image;
            Extent = new SizeF(image.Width * scale, image.Height * scale);
        }

        public override void Update(float deltaTime)
        {
            MoveLeft(deltaTime);
            KeepInsideScreen();
        }

        private void MoveLeft(float deltaTime)
        {
            X -= speed * deltaTime;
        }

        private void KeepInsideScreen()
        {
            X = X.Mod(Parent.Width);
            Y = Y.Mod(Parent.Height);
        }

        public override void DrawOn(Graphics graphics)
        {
            FillScreenTiled(graphics);
        }
        
        public void FillScreenTiled(Graphics graphics)
        {
            //Redondeo el numero
            int w = Width.RoundedToInt();
            int h = Height.RoundedToInt();
            int x = Position.X.RoundedToInt();
            int y = Position.Y.RoundedToInt();

            this.image = new Bitmap(image, new Size(w, h));//Ajusto mi imagen, segun mi ancho y alto

            while (x >= Parent.Left) { x -= w; }
            while (y >= Parent.Top) { y -= h; }

            for (int x1 = x; x1 <= Parent.Right; x1 += w)
            {
                for (int y1 = y; y1 <= Parent.Bottom; y1 += h)
                {
                    //if (flipX) { image.RotateFlip(RotateFlipType.RotateNoneFlipX); }//Roto las imagenes
                    //if (flipY) { image.RotateFlip(RotateFlipType.RotateNoneFlipY); }//Roto las imagenes
                    graphics.DrawImage(image, new Point(x1, y1));
                }
            }
        }        
    }
}
