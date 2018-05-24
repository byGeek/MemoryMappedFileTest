using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace Test2
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("mmftest", MemoryMappedFileRights.ReadWrite);
            MemoryMappedViewAccessor viewAccessor = mmf.CreateViewAccessor(0, 0, MemoryMappedFileAccess.ReadWrite);


            Mutex mutex = Mutex.OpenExisting("mymutex");
            mutex.WaitOne();  //require mutex to go on

            viewAccessor.Write(1, 1);  //set the second byte to 1

            Console.WriteLine("test2 started");

            viewAccessor.Write(6, System.Diagnostics.Process.GetCurrentProcess().Id);

            mutex.ReleaseMutex();

            viewAccessor.Dispose();
            mmf.Dispose();

            Console.WriteLine("quit myself after 5 seconds...");

            System.Threading.Thread.Sleep(5000);
        }
    }
}
