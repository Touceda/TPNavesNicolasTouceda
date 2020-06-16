using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Engine.Profiling;

namespace Game
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //Configuracion de siempre
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainScene main = new MainScene();//Creo la Scene y la mando a correr
            Application.Run(main);
            Application.Run(new TallyResults(main.scene.Tally));//Creo la Scene Tally, la mando a correr y le paso info de mi main scene principal
        }
    }
}
