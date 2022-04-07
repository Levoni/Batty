using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace Batty_2._0
{
   static class AudioManager
   {

      public static bool Muted { get; set; }

      /// <summary>
      /// Plays melody for beginning of a level.
      /// </summary>
      public static void PlayOpeningSequence()
      {
         if (!Muted)
         {
            Console.Beep(262, 150);
            Console.Beep(262, 150);
            Console.Beep(330, 150);
            Console.Beep(392, 300);
            Thread.Sleep(350);
            Console.Beep(494, 100);
            Console.Beep(523, 450);
         }
      }

      /// <summary>
      /// Plays melody for getting a high score.
      /// </summary>
      public static void PlayHighScore()
      {
         if (!Muted)
         {
            Thread.Sleep(300);

            Console.Beep(311, 200);
            Console.Beep(330, 200);
            Console.Beep(349, 200);
            Console.Beep(370, 600);
         }
      }

      /// <summary>
      /// Plays beep for a ball hitting a block, bar, or screen boundary.
      /// </summary>
      public static void PlayHit()
      {
         if (!Muted)
            Console.Beep(180, 200);
      }

      /// <summary>
      /// Plays melody for losing a life.
      /// </summary>
      public static void PlayLoseLife()
      {
         if (!Muted)
         {
            Console.Beep(262, 200);
            Console.Beep(156, 300);
         }
      }

      /// <summary>
      /// Plays melody for losing the game.
      /// </summary>
      public static void PlayGameOver()
      {
         if (!Muted)
         {
            Thread.Sleep(400);
            Console.Beep(262, 300);
            Console.Beep(247, 300);
            Console.Beep(233, 300);
            Console.Beep(220, 500);
         }
      }

      /// <summary>
      /// Plays beep for incrementing score for remaining time.
      /// </summary>
      public static void PlayTimePoints()
      {
         if (!Muted)
               Console.Beep(220, 150);

         /*
         for (int i = 0; i < 55000000; i++)
         {
            if (i == 0)
            {
               Thread thread = new Thread(new ThreadStart(AudioManager.PlayTimePoints));
               thread.Start();
            }
         }
         */
      }
   }
}
