using Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Effects
{
    class ControladorDeSpaceNoice: GameObject
    {
        Image ImgSpaceNoice;
        float speedSpaceNoice;
        float scaleSpaceNoice;
        bool flipXSpaceNoice;
        bool flipYSpaceNoice;

        public ControladorDeSpaceNoice(Image image, float speed, float scale, bool flipX, bool flipY)
        {
            this.speedSpaceNoice = speed;
            this.scaleSpaceNoice = scale;
            this.flipXSpaceNoice = flipX;
            this.flipYSpaceNoice = flipY;
            this.ImgSpaceNoice = image;
        }





        SpaceNoise SN1 = new SpaceNoise(Properties.Resources.space_noise_all_capas, 3 * 5.5f, 2.00f, false, false,1);
        SpaceNoise SN2;
        SpaceNoise SN3;

        //SpaceNoise[] Noices = new SpaceNoise[3];
        public override void Update(float deltaTime)
        {

            if (SN1.X <= 0 && SN1.YaCreeUnaCopia == false)
            {
                SN2 = new SpaceNoise(Properties.Resources.space_noise_all_capas, 3 * 5.5f, 2.00f, false, false, 1600);
                SN1.YaCreeUnaCopia = true;
            }


            if (SN1 != null)
            {
                SN1.Update(deltaTime);
            }

            if (SN2 != null)
            {
                SN2.Update(deltaTime);
            }

            if (SN3 != null)
            {
                SN3.Update(deltaTime);
            }

            //{
            //    SN1.X = 0;
            //    
            //}
            //else
            //{
            //    SN1.Update(deltaTime);
            //}

            ////{
            ////    SN2 = new SpaceNoise(Properties.Resources.space_noise_all_capas, 3 * 5.5f, 2.00f, false, false);
            ////}

        }

        private bool asd = false;
        public override void DrawOn(Graphics graphics)
        {
            if (SN1 != null && asd == false)
            {
                asd = true;
                SN1.DrawOn(graphics);
            }

            if (SN2 != null)
            {
                SN2.DrawOn(graphics);
            }

            if (SN3 != null)
            {
                SN3.DrawOn(graphics);
            }
        }










    }
}
