using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace MemoryMappedFileDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "demo.txt");

            //used to read/write large files
            using(var file = MemoryMappedFile.CreateFromFile(path, FileMode.Create, "mappedName", 1024))
            {
                var valueToWrite = "demo txt content";

                //create ViewAccessor
                var myAccessor = file.CreateViewAccessor();

                myAccessor.WriteArray<byte>(0, Encoding.ASCII.GetBytes(valueToWrite), 0, valueToWrite.Length);

                var readContent = new byte[valueToWrite.Length];
                myAccessor.ReadArray<byte>(0, readContent, 0, valueToWrite.Length);

                var finalValue = Encoding.ASCII.GetString(readContent);
            }
        }
    }
}
