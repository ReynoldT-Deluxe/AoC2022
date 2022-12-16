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
            string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_11/sampleData.txt";
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_11/aocData.txt";

            //int part = 1;
            int part = 2;

            List<Monkey> monkeyList = new List<Monkey>();
            Monkey monkey = new Monkey();

            try
            {
                using (StreamReader sr = new StreamReader(dataLocation))
                {
                    string data;
                    int monkeyCount = 0;

                    while ((data = sr.ReadLine()) != null)
                    {
                        string trimmedData = data.Trim();
                        //Console.WriteLine(data.Trim());
                        if (trimmedData.StartsWith("Monkey"))
                        {
                            //create a monkey
                            monkey = new Monkey();
                            //monkeyList.Add(monkey);

                            string[] monkeyData = trimmedData.Split(":");
                            string[] numberData = monkeyData[0].Split(" ");
                            monkeyCount = int.Parse(numberData[1]);
                            //Console.WriteLine("monkeyCount: {0}", monkeyCount);
                        }
                        else if (trimmedData.StartsWith("Operation"))
                        {
                            //get the operation and operationBy
                            if (trimmedData.Contains("*"))
                            {
                                monkey.operation = "*";
                            }
                            else if (trimmedData.Contains("+"))
                            {
                                monkey.operation = "+";
                            }

                            string[] operationData = trimmedData.Split(monkey.operation);

                            //operation is equal to old
                            if (operationData[1].Trim().CompareTo("old") == 0)
                            {
                                //set to zero since we don't know what the old value is for now
                                monkey.operationBy = 0;
                            }
                            else
                            {
                                monkey.operationBy = long.Parse(operationData[1].Trim());
                            }
                        }
                        else if (trimmedData.StartsWith("Test"))
                        {
                            string[] testData = trimmedData.Split(" divisible by ");
                            monkey.test = long.Parse(testData[1]);
                        }
                        else if (trimmedData.StartsWith("If true"))
                        {
                            string[] trueData = trimmedData.Split(" throw to monkey ");
                            monkey.truthy = int.Parse(trueData[1]);
                        }
                        else if (trimmedData.StartsWith("If false"))
                        {
                            string[] falseData = trimmedData.Split(" throw to monkey ");
                            monkey.falsy = int.Parse(falseData[1]);
                        }
                        else if (trimmedData.CompareTo("") == 0)
                        {
                            monkeyList.Add(monkey);
                        }
                        else if (trimmedData.StartsWith("Starting items"))
                        {
                            List<long> items = new List<long>();
                            //get starting items
                            string[] itemList = trimmedData.Split("Starting items: ");
                            //Console.WriteLine("itemList[1]: {0}", itemList[1]);
                            if (itemList[1].Contains(","))
                            {
                                //split the commas
                                string[] item = itemList[1].Split(", ");
                                foreach (string x in item)
                                {
                                    items.Add(long.Parse(x));
                                }
                            }
                            else
                            {
                                items.Add(long.Parse(itemList[1]));
                            }

                            monkey.itemsWorryLevels = items;
                        }
                    }
                    //add the last monkey data to the list
                    monkeyList.Add(monkey);
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            //printMonkeyListData(monkeyList);
            long levelOfMonkeyBusiness = 0;

            //for part 1
            if (part == 1)
            {
                levelOfMonkeyBusiness = processMonkeyList(monkeyList, false);
                Console.WriteLine("Level of monkey business after 20 rounds of " +
                    "stuff-slinging simian shenanigans (third of worry): {0}", levelOfMonkeyBusiness);
            }

            //for part 2
            if (part == 2)
            {
                levelOfMonkeyBusiness = processMonkeyList(monkeyList, true);
                Console.WriteLine("Level of monkey business after 10000 rounds of " +
                    "stuff-slinging simian shenanigans (full worry): {0}", levelOfMonkeyBusiness);
            }
        }

        public static void printMonkeyListData(List<Monkey> monkeyList)
        {
            //print monkey data
            //Console.WriteLine(monkeyList.Count);
            for (int count = 0; count < monkeyList.Count; count++)
            {
                //     Console.WriteLine("monkey {0} data", monkeyList.IndexOf(monkeyData));
                //     Console.WriteLine("operation: {0}", monkeyData.operation);
                //     Console.WriteLine("operationBy: {0}", monkeyData.operationBy);
                //     Console.WriteLine("test: {0}", monkeyData.test);
                //     Console.WriteLine("truthy: {0}", monkeyData.truthy);
                //     Console.WriteLine("falsy: {0}", monkeyData.falsy);

                Console.Write("Monkey {0}: ", count);
                foreach (long item in monkeyList[count].itemsWorryLevels)
                {
                    Console.Write("{0} ", item);
                }
                Console.WriteLine("");
                // Console.WriteLine("inspectionCount: {0}", monkeyList[count].inspectionCount);
            }
        }

        public static int processMonkeyList(List<Monkey> monkeyList, bool fullLevelOfWorry)
        {
            int maxRound = 20;
            //iterate thru 20 rounds
            if (fullLevelOfWorry) {
                maxRound = 10000;
            }
            
            for (int round = 0; round < maxRound; round++)
            {
                foreach (Monkey monkey in monkeyList)
                {
                    while (monkey.itemsWorryLevels.Count != 0)
                    {
                        //calculate new Worry = x (old) operation y (operationBy)
                        long x = monkey.itemsWorryLevels[0];
                        long y = monkey.operationBy;
                        if (monkey.operationBy == 0)
                        {
                            y = x;
                        }

                        long updatedWorry = 0;

                        if (monkey.operation.Equals("*"))
                        {
                            updatedWorry = x * y;
                        }
                        else if (monkey.operation.Equals("+"))
                        {
                            updatedWorry = x + y;
                        }

                        //divide worry level by 3 and return the current worry level without the remainder
                        long currentWorryLevel = updatedWorry;
                        if (!fullLevelOfWorry)
                        {
                            currentWorryLevel = (long)Math.Floor((double)updatedWorry / 3);
                        }

                        //test divisibility
                        //divisible
                        if ((currentWorryLevel % monkey.test) == 0)
                        {
                            //truthy
                            monkeyList[monkey.truthy].itemsWorryLevels.Add(currentWorryLevel);
                        }
                        else
                        {
                            //falsy
                            monkeyList[monkey.falsy].itemsWorryLevels.Add(currentWorryLevel);
                        }

                        //remove from original
                        monkey.itemsWorryLevels.RemoveAt(0);
                        //increment the inspection count
                        monkey.inspectionCount++;
                    }
                }

                // Console.WriteLine("Round {0}", round + 1);
                // printMonkeyListData(monkeyList);
            }

            return getLevelOfMonkeyBusiness(monkeyList);
        }

        public static int getLevelOfMonkeyBusiness(List<Monkey> monkeyList)
        {
            //sort descending by inspection count
            List<Monkey> sortedListByInspectionCountDescending = monkeyList.OrderByDescending(info => info.inspectionCount).ToList();
            //get the product of the top two
            // Console.WriteLine("{0}, {1}", sortedListByInspectionCountDescending[0].inspectionCount, 
            //     sortedListByInspectionCountDescending[1].inspectionCount);
            return sortedListByInspectionCountDescending[0].inspectionCount * sortedListByInspectionCountDescending[1].inspectionCount;
        }

        public class Monkey
        {
            public List<long> itemsWorryLevels { get; set; }
            public string operation { get; set; }
            public long operationBy { get; set; }
            public long test { get; set; }
            public int truthy { get; set; }
            public int falsy { get; set; }
            public int inspectionCount { get; set; }
        }
    }
}