using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;


namespace SharedMemoryTEST
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew("testmap", 10000))
            {
                bool mutexCreated;
                Mutex mutex = new Mutex(true, "testmapmutex", out mutexCreated);
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryWriter writer = new BinaryWriter(stream);

                    Console.Write("Input number =>");
                    int num = int.Parse(Console.ReadLine());

                    writer.Write(num);
                }
                mutex.ReleaseMutex();

                Console.WriteLine("Start Process++ and press ENTER to continue.");
                Console.ReadLine();

                mutex.WaitOne();
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryReader reader = new BinaryReader(stream);
                    Console.WriteLine("Process# says: {0}", reader.ReadInt32());
                    Console.WriteLine("Process++ says: {0}", reader.ReadInt32());
                }
                mutex.ReleaseMutex();
            }
        }
    }
}
