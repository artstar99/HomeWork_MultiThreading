using System;
using System.IO;
using System.Runtime.Remoting.Contexts;
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

            private string logFile;
            private string logString;
            private DateTime logTimer;

            private readonly object lockObject = new object();

            private uint stringNumber;
            private int stringCounter;
            private string writeString;
            private bool writePermission;
            private int strigBooferCapacity;
            private bool globalFlag;
            private bool logFileClose;

            private int stringsReaded;

            private TimeSpan elapsed;
            private DateTime start;


            private void GetPathes()
            {
                outFile = inFile.Replace(".csv", ".txt");
                logFile = inFile.Replace(".csv", "_WriteLog.txt");
            }

            public void Convert()
            {
                GetPathes();
                CountStrings();
                Thread consoleThread = new Thread(ShowConsoleInformation);
                //consoleThread.Start();
                writer = new StreamWriter(outFile, false);
                //Thread logFileThread = new Thread(WriteLog);
                //logFileThread.Start();

                try
                {
                    using (var reader = new StreamReader(inFile))
                    {
                        writePermission = true;
                        strigBooferCapacity = 500;
                        string bufferString = null;
                        start = DateTime.Now;
                        while (!(globalFlag = reader.EndOfStream))
                        {
                            
                                bufferString += $"{reader.ReadLine()}\n";
                            

                            stringCounter++;

                            if (stringCounter == strigBooferCapacity || reader.EndOfStream)
                            {
                                elapsed = DateTime.Now - start;
                                while (writePermission == false)
                                {
                                    //lock (lockObject)
                                    //{
                                    //    logString += "Запись не успевает за чтением\n";
                                    //}
                                    strigBooferCapacity += 2;
                                }
                                strigBooferCapacity -= 1;

                                //lock (lockObject)
                                //{
                                //    logString += $"Считано {stringCounter} строк. " +
                                //                 $"Потрачено времени {elapsed.Milliseconds}" +
                                //                 $" Произвожу запись\n";
                                //}

                                start = DateTime.Now;
                              
                                    writeString = bufferString;
                                    bufferString = null;
                                

                                stringsReaded += stringCounter;
                                stringCounter = 0;
                            }

                            Thread write = new Thread(WriteToTxt);
                            write.Start();
                            writePermission = false;
                        }

                        #region Конец считываемого файла
                        start = DateTime.Now;
                        if (bufferString != null)
                            writeString = bufferString;

                        if (stringCounter > 0)
                            stringsReaded += stringCounter;
                        writePermission = false;

                        Thread lastWrite = new Thread(WriteToTxt);
                        lastWrite.Start();

                        while (writePermission == false)
                        {

                        }

                        while (logFileClose)
                        {

                        }
                        #endregion

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception(e.Message);
                }
            }

            private void CountStrings()
            {
                try
                {
                    using (StreamReader sr = new StreamReader(inFile))
                    {
                        while (!sr.EndOfStream)
                        {
                            sr.ReadLine();
                            stringNumber++;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            private void ShowConsoleInformation()
            {
                int secondsIncrement = 3;
                var consoleTimer = DateTime.Now;


                while (!globalFlag)
                {
                    if ((DateTime.Now - consoleTimer >= TimeSpan.FromSeconds(secondsIncrement)))
                    {
                        secondsIncrement += 3;
                        //Thread.Sleep(300);
                        //lock (lockObject)
                        //{
                        Console.Clear();
                        Console.WriteLine($"Общее кол-во строк:\t{stringNumber}");

                        Console.WriteLine($"Обработано строк: \t{stringsReaded}");

                        Console.WriteLine($"Осталось строк:\t{stringNumber - stringsReaded}");
                        Console.WriteLine($@"Времени прошло:{(DateTime.Now - consoleTimer):hh\:mm\:ss}");
                        //}
                    }
                }

                Console.Clear();
                Console.WriteLine(new string('-', 20));
                Console.WriteLine($"Конвертация завершена\n" +
                                  $@"Времени прошло:{(DateTime.Now - consoleTimer):hh\:mm\:ss}");
            }
            /*
            private void WriteLog()
            {
                string logBufferString;
                logTimer = DateTime.Now;
                int secondsIncrement = 10;
                while (!globalFlag)
                {
                    if ((DateTime.Now - logTimer >= TimeSpan.FromSeconds(secondsIncrement))) //проверка что прошло 10 сек

                    {
                        lock (lockObject)
                        {
                            // забираем данные из другого потока из переменной logString
                            logBufferString = logString;
                            logString = null;
                        }

                        secondsIncrement += 10;

                        try
                        {
                            using (StreamWriter logWriter = new StreamWriter(logFile, true))
                            {
                                logWriter.WriteLine($"{DateTime.Now}\n{new string('-', 10)}");
                                logWriter.WriteLine(logBufferString);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }

                // Эти инструкции выполняются после завершения чтения всего файла
                try
                {
                    logBufferString = logString;

                    using (StreamWriter logWriter = new StreamWriter(logFile, true))
                    {
                        logWriter.WriteLine(logBufferString);
                        logWriter.WriteLine($"Конвертация завершена\n" +
                                            $@"Потрачено времени: {DateTime.Now - logTimer:hh\mm\ss}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                logFileClose = true;
            }

            */
            private void WriteToTxt()
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

                //lock (lockObject)
                //{
                //logString += $"Запись произведена." +
                //             $" Потрачено времени " +
                //             $"{elapsed.Milliseconds}\n";
                //}

                writePermission = true;
                start = DateTime.Now;

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
