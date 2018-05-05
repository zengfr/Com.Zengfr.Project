using System;
using System.ComponentModel;
using System.Runtime;
using System.Configuration;
using System.Threading;

namespace BrainTechLLC.ThreadSafeObjects
{
   /// <summary>
   /// Basic support for simple spin-lock - for locking tiny sections of code only, avoiding lock()
   /// Uses atomic CompareExchange operation.  NOTE: no deadlock detection, same thread cannot lock an already-locked object
   /// </summary>
   [Serializable, Browsable(true), TypeConverter(typeof(ExpandableObjectConverter))]
   public class Lockable
   {
      private readonly static int SpinCycles = 20;//Properties.Settings.Default.SpinCycles;

      [NonSerialized]
      protected static long _conflicts;

      [NonSerialized]
      protected int _lock;
      
      /// <summary>
      /// Returns the number of lock conflicts that have occurred
      /// </summary>
      public static long Conflicts { get { return _conflicts; } }

      /// <summary>
      /// Aquire the lock
      /// </summary>
      public void AquireLock()
      {
         // Assume that we will grab the lock - call CompareExchange
         if (Interlocked.CompareExchange(ref _lock, 1, 0) == 1)
         {
            int n = 0;

            // Could not grab the lock - spin/wait until the lock looks obtainable
            while (_lock == 1)
            {
               if (n++ > SpinCycles)
               {
                  Interlocked.Increment(ref _conflicts);
                  n = 0;
                  Thread.Sleep(0);
               }
            }

            // Try to grab the lock - call CompareExchange
            while (Interlocked.CompareExchange(ref _lock, 1, 0) == 1)
            {
               n = 0;

               // Someone else grabbed the lock.  Continue to spin/wait until the lock looks obtainable
               while (_lock == 1)
               {
                  if (n++ > SpinCycles)
                  {
                     Interlocked.Increment(ref _conflicts);
                     n = 0;
                     Thread.Sleep(0);
                  }
               }
            }
         }
      }

      /// <summary>
      /// Release the lock
      /// </summary>
      public void ReleaseLock()
      {
         _lock = 0;
      }      
   }
}