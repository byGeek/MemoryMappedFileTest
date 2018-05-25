using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace mmf_vs_normal_io
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"file.data";
            if (!File.Exists(filePath))
                File.Create(filePath).Close();

            Console.WriteLine("using memory mapped file.");
            var startTime = DateTime.Now;

            MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Create, "filedata", 10 * 1024 * 2014);
            MemoryMappedViewStream viewStream = mmf.CreateViewStream();

            var pos = 0;
            for (int i = 0; i < 10000; i++, pos += 4)
            {
                byte[] array = BitConverter.GetBytes(i);
                viewStream.Write(array, 0, array.Length);

                viewStream.Seek(pos, SeekOrigin.Begin);

                byte[] buffer = new byte[8];

                viewStream.Seek(pos, SeekOrigin.Begin);
                viewStream.Read(buffer, 0, 4);

                //Console.WriteLine("read 4 bytes: {0}", BitConverter.ToInt32(buffer, 0));
            }



            viewStream.Dispose();
            mmf.Dispose();

            var endTime = DateTime.Now;

            Console.WriteLine("Used: {0}", endTime - startTime);

            Console.WriteLine("Used normal IO");

            startTime = DateTime.Now;

            FileStream fs = new FileStream(filePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            BinaryWriter bw = new BinaryWriter(fs);

            pos = 0;
            for (int i = 0; i < 10000; i++, pos += 4)
            {
                bw.Write(i);
                fs.Seek(pos, SeekOrigin.Begin);
                var data = br.ReadInt32();
                //Console.WriteLine("read int: {0}", data);
            }
            br.Close();
            bw.Close();
            fs.Close();

            endTime = DateTime.Now;

            Console.WriteLine("Used:  {0}", endTime - startTime);
        }
    }
}
