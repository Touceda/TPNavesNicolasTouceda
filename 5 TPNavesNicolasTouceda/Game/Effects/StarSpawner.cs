using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Extensions;
using Engine;
using System.Drawing;

namespace Game
{
    public class StarSpawner : GameObject //Esta clase crea las estrellas 
    {
        private Random rnd = new Random();
        private bool firstFrame = true;

        public override void Update(float deltaTime)
        {
            if (firstFrame)
            {
                firstFrame = false;
                FillSpace(1000);
            }

            Left = Parent.Right;
            for (int i = 0; i < 200 * deltaTime; i++)
            {
                SpawnStar();
            }
        }

        private void FillSpace(int numberOfStars)
        {
            for (int i = 0; i < numberOfStars; i++)
            {
                CenterX = rnd.Next(Parent.Left.RoundedToInt(), Parent.Right.RoundedToInt());
                SpawnStar();
            }
        }

        Image StarImag = Properties.Resources.star;
        public void SpawnStar()
        {
            Star star = new Star(StarImag, rnd.Next(300));//Para mejorar puedo guardan un array de de estrellas ya dibujadas y depende el random, le doy una u otra
            star.Center = Center;
            Parent.AddStars(star);
            CenterY = rnd.Next(Parent.Top.RoundedToInt(), Parent.Bottom.RoundedToInt());
        }

    }
}
