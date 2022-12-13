using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections.Specialized;

namespace FileApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //data to use
            string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_xx/sampleData.txt";
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_xx/aocData.txt"; 

            //problem part
            int partNum = 1;
            //int partNum = 2;

            try
            {
                using (StreamReader sr = new StreamReader(dataLocation))
                {
                    string data;
                    string output = "";

                    while ((data = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine("data: {0}", data);
                        //part 1 solution
                        if (partNum == 1)
                        {

                            //part 2 solution
                        }
                        else if (partNum == 2)
                        {

                        }
                    }

                    Console.WriteLine("output: {0}", output);
                }

            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }
}