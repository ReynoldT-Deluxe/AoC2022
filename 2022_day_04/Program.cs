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
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_04/sampleData.txt";
            string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_04/aocData.txt";

            int maxSections = 0;

            try
            {
                using (StreamReader maxSectionStream = new StreamReader(dataLocation))
                {
                    //determine the max section
                    maxSections = determineMaxSectionValue(maxSectionStream);
                    //Console.WriteLine("maxSections: {0}", maxSections);
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The maxSectionStream file could not be read:");
                Console.WriteLine(e.Message);
            }

            try
            {
                using (StreamReader dataStream = new StreamReader(dataLocation))
                {
                    string data;
                    int fullyContainedCount = 0;
                    int overlapCount = 0;

                    while ((data = dataStream.ReadLine()) != null)
                    {
                        //Console.WriteLine("data: {0}", data);

                        //based on deduction, we need to check the number of like values
                        //if the total like values equal the lower number of assignment, the area is fully contained
                        //get the pair
                        string[] pair = data.Split(',');
                        string[] pair1 = pair[0].Split('-');
                        string[] pair2 = pair[1].Split('-');

                        //process pair1 data
                        int[] pair1Data = new int[maxSections];
                        for (int cnt = 0; cnt < maxSections; cnt++)
                        {
                            if ((cnt + 1) >= int.Parse(pair1[0]) && (cnt + 1) <= int.Parse(pair1[1]))
                            {
                                pair1Data[cnt] = 1;
                            }
                            else
                            {
                                pair1Data[cnt] = 0;
                            }
                        }

                        //process pair2 data
                        int[] pair2Data = new int[maxSections];
                        for (int cnt2 = 0; cnt2 < maxSections; cnt2++)
                        {
                            if ((cnt2 + 1) >= int.Parse(pair2[0]) && (cnt2 + 1) <= int.Parse(pair2[1]))
                            {
                                pair2Data[cnt2] = 1;
                            }
                            else
                            {
                                pair2Data[cnt2] = 0;
                            }
                        }

                        //count same data
                        int sameCount = countSameData(pair1Data, pair2Data);
                        //Console.WriteLine("sameCount: {0}", sameCount);

                        //part 1 solution
                        int pair1Count = pair1Data.Aggregate((a, b) => { return a + b; });
                        //Console.WriteLine("pair1Count: {0}", pair1Count);
                        int pair2Count = pair2Data.Aggregate((a, b) => { return a + b; });
                        //Console.WriteLine("pair2Count: {0}", pair2Count);

                        if (sameCount == pair1Count || sameCount == pair2Count)
                        {
                            fullyContainedCount++;
                        }

                        //part 2 solution
                        if (sameCount > 0)
                        {
                            overlapCount++;
                        }
                    }

                    Console.WriteLine("fullyContainedCount: {0}", fullyContainedCount);
                    Console.WriteLine("overlapCount: {0}", overlapCount);

                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The dataStream file could not be read:");
                Console.WriteLine(e.Message);
            }

        }

        public static int determineMaxSectionValue(StreamReader sr)
        {

            string data;
            int maxSection = 0;

            while ((data = sr.ReadLine()) != null)
            {
                string[] pair = data.Split(',');
                string[] pair1 = pair[0].Split('-');
                string[] pair2 = pair[1].Split('-');

                //it's assumed that the second number is always higher
                if (int.Parse(pair1[1]) > maxSection)
                {
                    maxSection = int.Parse(pair1[1]);
                }
                if (int.Parse(pair2[1]) > maxSection)
                {
                    maxSection = int.Parse(pair2[1]);
                }
            }

            return maxSection;
        }

        public static int countSameData(int[] pair1Data, int[] pair2Data)
        {

            int sameDataCount = 0;

            for (int cnt = 0; cnt < pair1Data.Length; cnt++)
            {
                if (pair1Data[cnt] == 1 && pair2Data[cnt] == 1)
                {
                    sameDataCount++;
                }
            }

            return sameDataCount;
        }
    }
}