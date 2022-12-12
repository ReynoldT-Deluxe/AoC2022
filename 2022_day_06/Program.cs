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
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_06/sampleData01.txt";
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_06/sampleData02.txt";
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_06/sampleData03.txt";
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_06/sampleData04.txt";
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_06/sampleData05.txt";
            string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_06/aocData.txt";

            //problem part
            //int partNum = 1;
            //int partNum = 2;

            try
            {
                using (StreamReader sr = new StreamReader(dataLocation))
                {
                    string data;

                    while ((data = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine("data: {0}", data);
                        //part 1 solution
                        // if (partNum == 1)
                        // {
                        bool isStartPacketFound = findStartOfPacketMarker(data);

                        //part 2 solution
                        // }
                        // else if (partNum == 2)
                        // {
                        //do not process start-of-message marker finder if the start-of-packet
                        if (isStartPacketFound)
                        {
                            findStartOfMessageMarker(data);
                        }
                        //}
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public static bool findStartOfPacketMarker(string data)
        {
            for (int charIndex = 0; charIndex < data.Length; charIndex++)
            {
                //limit to the last 4 characters
                if (charIndex < (data.Length - 4))
                {
                    Char[] charArray = new Char[4];
                    //get four characters from index character
                    charArray[0] = data.ElementAt(charIndex);
                    charArray[1] = data.ElementAt(charIndex + 1);
                    charArray[2] = data.ElementAt(charIndex + 2);
                    charArray[3] = data.ElementAt(charIndex + 3);
                    // Console.WriteLine("{0},{1},{2},{3}", data.ElementAt(charIndex),
                    //     data.ElementAt(charIndex + 1), data.ElementAt(charIndex + 2), data.ElementAt(charIndex + 3));

                    //check if all characters are the same
                    if (charArray.Length != charArray.Distinct().Count())
                    {
                        //there are duplicate present
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("The start-of-packet marker can be found after character {0}", charIndex + 4);
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("The start-of-packet marker cannot be found");
                    return false;
                }
            }

            return false;
        }

        public static void findStartOfMessageMarker(string data)
        {
            for (int charIndex = 0; charIndex < data.Length; charIndex++)
            {
                //limit to the last 14 characters
                if (charIndex < (data.Length - 14))
                {
                    Char[] charArray = new Char[14];
                    //get four characters from index character
                    charArray[0] = data.ElementAt(charIndex);
                    charArray[1] = data.ElementAt(charIndex + 1);
                    charArray[2] = data.ElementAt(charIndex + 2);
                    charArray[3] = data.ElementAt(charIndex + 3);
                    charArray[4] = data.ElementAt(charIndex + 4);
                    charArray[5] = data.ElementAt(charIndex + 5);
                    charArray[6] = data.ElementAt(charIndex + 6);
                    charArray[7] = data.ElementAt(charIndex + 7);
                    charArray[8] = data.ElementAt(charIndex + 8);
                    charArray[9] = data.ElementAt(charIndex + 9);
                    charArray[10] = data.ElementAt(charIndex + 10);
                    charArray[11] = data.ElementAt(charIndex + 11);
                    charArray[12] = data.ElementAt(charIndex + 12);
                    charArray[13] = data.ElementAt(charIndex + 13);
                    // Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}", data.ElementAt(charIndex),
                    //     data.ElementAt(charIndex + 1), data.ElementAt(charIndex + 2), data.ElementAt(charIndex + 3),
                    //     data.ElementAt(charIndex + 4), data.ElementAt(charIndex + 5), data.ElementAt(charIndex + 6),
                    //     data.ElementAt(charIndex + 7), data.ElementAt(charIndex + 8), data.ElementAt(charIndex + 9),
                    //     data.ElementAt(charIndex + 10), data.ElementAt(charIndex + 11), data.ElementAt(charIndex + 12),
                    //     data.ElementAt(charIndex + 13));

                    //check if all characters are the same
                    if (charArray.Length != charArray.Distinct().Count())
                    {
                        //there are duplicate present
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("The start-of-message marker can be found after character {0}", charIndex + 14);
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("The start-of-message marker cannot be found");
                    return;
                }
            }

            return;
        }
    }
}