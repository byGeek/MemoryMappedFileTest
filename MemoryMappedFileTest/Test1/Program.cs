using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;
using System.Diagnostics;
using System.Threading;

namespace Test1
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"mmf.data";
            if (!System.IO.File.Exists(filePath))
                System.IO.File.Create(filePath).Close();

            MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(filePath, System.IO.FileMode.Create, "mmftest", 1024 * 1024 * 20, MemoryMappedFileAccess.ReadWrite);
            MemoryMappedViewAccessor viewAccessor = mmf.CreateViewAccessor(0, 0, MemoryMappedFileAccess.ReadWrite);

            bool mutexCreated = false;
            Mutex mutex = new Mutex(true, "mymutex", out mutexCreated);

            if (mutexCreated)
            {
                viewAccessor.Write(0, 1);
                viewAccessor.Write(2, Process.GetCurrentProcess().Id);
            }

            mutex.ReleaseMutex();

            Console.WriteLine("wait process test2 to start...");

            var flag = 0;
            while(true)
            {
                flag =  viewAccessor.ReadByte(1);
                if(flag == 1)
                {
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }

            viewAccessor.Dispose();
            mmf.Dispose();

            Console.WriteLine("process test2 started, quit myself after 5 seconds...");

            System.Threading.Thread.Sleep(5000);



        }
    }
}
