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
            string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_19/sampleData.txt";
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_19/aocData.txt"; 

            //problem part
            int partNum = 1;
            //int partNum = 2;

            List<Blueprint> blueprintList = new List<Blueprint>();

            try
            {
                using (StreamReader sr = new StreamReader(dataLocation))
                {
                    string data;
                    string output = "";

                    while ((data = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine(data);
                        Blueprint bluePrint = new Blueprint();
                        string[] bluePrintData = data.Split(": ");

                        //get blurpint id
                        string[] idData = bluePrintData[0].Split(" ");
                        int id = int.Parse(idData[1]);
                        bluePrint.id = id;
                        Console.WriteLine("Blueprint id:{0}", id);

                        string[] robotSpecs = bluePrintData[1].Trim().Split(". ");
                        Console.WriteLine("Robot {0}", bluePrintData[0]);

                        //get oreRobot
                        string[] oreRobotData = robotSpecs[0].Trim().Split(" ");
                        Robot oreRobot = new Robot();
                        oreRobot.ore = int.Parse(oreRobotData[4]);
                        bluePrint.oreRobot = oreRobot;
                        Console.WriteLine("Ore Robot ore:{0}", oreRobot.ore);

                        //get clayRobot
                        string[] clayRobotData = robotSpecs[1].Trim().Split(" ");
                        Robot clayRobot = new Robot();
                        clayRobot.ore = int.Parse(clayRobotData[4]);
                        bluePrint.clayRobot = clayRobot;
                        Console.WriteLine("Clay Robot ore:{0}", clayRobot.ore);

                        //get obsidianRobot
                        string[] obsidianRobotData = robotSpecs[2].Trim().Split(" ");
                        Robot obsidianRobot = new Robot();
                        obsidianRobot.ore = int.Parse(obsidianRobotData[4]);
                        obsidianRobot.clay = int.Parse(obsidianRobotData[7]);
                        bluePrint.obsidianRobot = obsidianRobot;
                        Console.WriteLine("Obsidian Robot ore:{0}\tclay:{1}", obsidianRobot.ore, obsidianRobot.clay);

                        //get geodeRobot
                        string[] geodeRobotData = robotSpecs[3].Trim().Split(" ");
                        Robot geodeRobot = new Robot();
                        geodeRobot.ore = int.Parse(geodeRobotData[4]);
                        geodeRobot.obsidian = int.Parse(geodeRobotData[7]);
                        bluePrint.geodeRobot = geodeRobot;
                        Console.WriteLine("Geode Robot ore:{0}\tobsidian:{1}", geodeRobot.ore, geodeRobot.obsidian);

                        blueprintList.Add(bluePrint);
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            //process the blurprintList
            determineGeodeCountFor24Mins(blueprintList);

        }

        public static void determineGeodeCountFor24Mins(List<Blueprint> blueprintList)
        {
            foreach (Blueprint blueprint in blueprintList)
            {
                int ore = 0;
                int clay = 0;
                int obsidian = 0;
                int geode = 0;
                int oreBot = 1; //initial robot made
                int clayBot = 0;
                int obsidianBot = 0;
                int geodeBot = 0;

                for (int minute = 0; minute < 24; minute++)
                {
                    //intialize
                    bool newOreBot = false;
                    bool newClayBot = false;
                    bool newObsidianBot = false;
                    bool newGeodeBot = false;

                    //build phase
                    Robot geodeRobot = blueprint.geodeRobot;
                    Robot obsidianRobot = blueprint.obsidianRobot;
                    Robot clayRobot = blueprint.clayRobot;
                    Robot oreRobot = blueprint.oreRobot;

                    if (obsidian >= geodeRobot.obsidian)
                    {
                        if (ore >= geodeRobot.ore)
                        {
                            newGeodeBot = true;
                            ore -= geodeRobot.ore;
                            obsidian -= geodeRobot.obsidian;
                        }                        
                    }
                    else if (clay >= obsidianRobot.clay)
                    {
                        if (ore >= obsidianRobot.ore && obsidian < geodeRobot.obsidian)
                        {
                            newObsidianBot = true;
                            ore -= obsidianRobot.ore;
                            clay -= obsidianRobot.clay;
                        }
                    }
                    else if (ore >= clayRobot.ore && clay < obsidianRobot.clay && obsidian < geodeRobot.obsidian)
                    {
                        newClayBot = true;
                        ore -= clayRobot.ore;
                    }
                    else if (ore >= oreRobot.ore && ore < clayRobot.ore && clay < obsidianRobot.clay)
                    {
                        newOreBot = true;
                        ore -= oreRobot.ore;
                    }

                    //collect phase
                    ore += oreBot;
                    clay += clayBot;
                    obsidian += obsidianBot;
                    geode += geodeBot;

                    //set
                    if (newOreBot)
                    {
                        oreBot++;
                    }
                    if (newClayBot)
                    {
                        clayBot++;
                    }
                    if (newObsidianBot)
                    {
                        obsidianBot++;
                    }
                    if (newGeodeBot)
                    {
                        geodeBot++;
                    }

                    Console.WriteLine("minute: {0}", minute + 1);
                    Console.WriteLine("Resource:\tore: {0}\t\tclay: {1}\t\tobsidian: {2}\t\tgeode: {3}", ore, clay, obsidian, geode);
                    Console.WriteLine("Robot:\t\tore: {0}\t\tclay: {1}\t\tobsidian: {2}\t\tgeode: {3}", oreBot, clayBot, obsidianBot, geodeBot);
                }
                Console.WriteLine("total geode for blueprint {0}: {1}", blueprint.id, geode);
            }
        }

        public class Blueprint
        {
            public int id { get; set; }
            public Robot oreRobot { get; set; }
            public Robot clayRobot { get; set; }
            public Robot obsidianRobot { get; set; }
            public Robot geodeRobot { get; set; }
            int geode24minCount { get; set; }
        }

        public class Robot
        {
            public int ore { get; set; }
            public int clay { get; set; }
            public int obsidian { get; set; }
        }
    }
}