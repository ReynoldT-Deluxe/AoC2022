﻿using System;
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
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_3/sampleData.txt";
            string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_3/aocData.txt";

            //problem part
            //int partNum = 1;
            int partNum = 2;

            try
            {
                using (StreamReader sr = new StreamReader(dataLocation))
                {
                    int sumOfPriorities = 0;
                    string data;
                    int groupCnt = 0;
                    System.Text.StringBuilder[] groupData = new System.Text.StringBuilder[3];

                    while ((data = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine("data: {0}", data);
                        //part 1 solution
                        if (partNum == 1)
                        {
                            int contentCount = data.Length / 2;
                            System.Text.StringBuilder compartmentData = new System.Text.StringBuilder(data);

                            //check group 1 vs group 2 compartment data
                            //iterate thru group 1
                            bool loopBreak = false;
                            for (int cnt = 0; cnt < contentCount; cnt++)
                            {
                                //iterate thru group 2
                                for (int cnt2 = 0; cnt2 < contentCount; cnt2++)
                                {
                                    //Console.WriteLine("compartment 1: {0}, compartment 2: {1}", compartmentData[cnt], compartmentData[cnt2 + contentCount]);
                                    if (compartmentData[cnt].CompareTo(compartmentData[cnt2 + contentCount]) == 0)
                                    {
                                        loopBreak = true;
                                        break;
                                    }
                                }

                                //if the same, calculate the priority
                                if (loopBreak)
                                {
                                    sumOfPriorities += calculatePriority(compartmentData[cnt]);
                                    break;
                                }

                            }

                            //part 2 solution
                        }
                        else if (partNum == 2)
                        {
                            //Console.WriteLine("groupCnt: {0}", groupCnt);
                            //get data                   
                            System.Text.StringBuilder compartmentData = new System.Text.StringBuilder(data);
                            groupData[groupCnt] = compartmentData;

                            if (groupCnt != 2)
                            {
                                groupCnt++;
                            }
                            else if (groupCnt == 2)
                            {
                                //process the data
                                sumOfPriorities += calculateGroupPriority(groupData);
                                //restart the group
                                groupCnt = 0;
                            }
                        }
                    }

                    Console.WriteLine("sumOfPriorities: {0}", sumOfPriorities);
                }

            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public static int calculatePriority(char compartmentData)
        {
            //lowerCase
            System.Text.StringBuilder lowerCase = new System.Text.StringBuilder("abcdefghijklmnopqrstuvwxyz");
            //iterate thru lower case
            for (int cnt = 1; cnt < 27; cnt++)
            {
                if (compartmentData.CompareTo(lowerCase[cnt - 1]) == 0)
                {
                    return cnt;
                }
            }

            //upperCase
            System.Text.StringBuilder upperCase = new System.Text.StringBuilder("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            //iterate thru upper case
            for (int cnt2 = 27; cnt2 < 53; cnt2++)
            {
                if (compartmentData.CompareTo(upperCase[cnt2 - 27]) == 0)
                {
                    return cnt2;
                }
            }

            //defaults to return 0
            return 0;
        }

        public static int calculateGroupPriority(System.Text.StringBuilder[] groupData)
        {
            //Console.WriteLine("Calculate group priority");
            //iterate thru first data
            for (int cnt1 = 0; cnt1 < groupData[0].Length; cnt1++)
            {
                //Console.WriteLine("group 0: {0}", groupData[0][cnt1]);
                //iterate thru second data
                for (int cnt2 = 0; cnt2 < groupData[1].Length; cnt2++)
                {
                    //Console.WriteLine("group 1: {0}", groupData[1][cnt2]);
                    if (groupData[0][cnt1].CompareTo(groupData[1][cnt2]) == 0)
                    {
                        //iterate thru third data
                        for (int cnt3 = 0; cnt3 < groupData[2].Length; cnt3++)
                        {
                            //Console.WriteLine("group 2: {0}", groupData[2][cnt3]);
                            if (groupData[0][cnt1].CompareTo(groupData[2][cnt3]) == 0)
                            {
                                return calculatePriority(groupData[0][cnt1]);
                            }
                        }
                    }
                }
            }

            //defaults to return 0
            return 0;
        }
    }
}