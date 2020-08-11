using System;
using System.IO;
using System.Threading;

namespace Task_02
{
    class Program
    {
        class CsvToTxt
        {
            private StreamWriter writer;
            private string inFile = @"..\..\..\..\Data\Data8277.csv";
            private string outFile;

            private int stringCounter;
            private string writeString;
            private bool writePermission;
            private int strigBooferCapacity;

            private TimeSpan elapsed;
            private DateTime start;
            public void Convert()
            {
                outFile = inFile.Replace(".csv", ".txt");
                try
                {
                    using (var reader = new StreamReader(inFile))
                    {
                        writer = new StreamWriter(outFile, false);
                        writePermission = true;
                        strigBooferCapacity = 100;
                        string bufferString = null;
                        start = DateTime.Now;
                        while (!reader.EndOfStream)
                        {
                            
                            bufferString += $"{reader.ReadLine()}\n";
                            
                            stringCounter++;

                            if (stringCounter == strigBooferCapacity || reader.EndOfStream)
                            {
                                elapsed = DateTime.Now - start;
                                while (writePermission == false)
                                {
                                    Console.WriteLine("Запись не успевает за чтением");
                                    strigBooferCapacity += 2;
                                }

                                strigBooferCapacity -= 1;
                                Console.WriteLine($"Считано {stringCounter} строк. " +
                                                  $"Потрачено времени {elapsed.Milliseconds}" +
                                                  $" Произвожу запись");
                                
                                start = DateTime.Now;
                                writeString = bufferString;
                                bufferString = null;
                                stringCounter = 0;

                                Thread write = new Thread(Write);
                                write.Start();
                                writePermission = false;
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception(e.Message);
                }


            }

            private void Write()
            {
                try
                {
                    writer.WriteLine(writeString);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception(e.Message);
                }

                elapsed = DateTime.Now - start;
                Console.WriteLine($"Запись произведена." +
                                  $" Потрачено времени " +
                                  $"{elapsed.Milliseconds}");
                writePermission = true;
                start=DateTime.Now;
            }
        }


        static void Main()
        {

            CsvToTxt csvToTxt = new CsvToTxt();

            csvToTxt.Convert();

            Console.ReadLine();
        }
    }
}
