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
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_10/sampleData01.txt";
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_10/sampleData02.txt";
            string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_10/aocData.txt"; 

            List<ProgramInput> programInputs = new List<ProgramInput>();

            try
            {
                programInputs = getProgramInputsFromFile(dataLocation);
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            //cycle thru inputs
            int sumOfSignalStregths = processProgramInputs(programInputs);

            Console.WriteLine("");
            Console.WriteLine("The sum of the six signal strengths: {0}", sumOfSignalStregths);
        }

        public static List<ProgramInput> getProgramInputsFromFile(string dataLocation)
        {
            List<ProgramInput> programInputs = new List<ProgramInput>();

            using (StreamReader sr = new StreamReader(dataLocation))
            {
                string data;

                while ((data = sr.ReadLine()) != null)
                {
                    //Console.WriteLine("data: {0}", data);
                    if (data.StartsWith("noop"))
                    {
                        programInputs.Add(new ProgramInput(data, 0, 1));
                    }
                    else if (data.StartsWith("addx"))
                    {
                        string[] input = data.Split(" ");
                        string command = input[0];
                        int value = int.Parse(input[1]);

                        programInputs.Add(new ProgramInput(command, value, 2));
                    }
                }
            }

            return programInputs;
        }

        public static int processProgramInputs(List<ProgramInput> programInputs)
        {
            //for part 1
            int signalStrength = 0;
            int index = 0;
            int registerValue = 1;
            
            //for part 2
            int crtIndex = 0;
            Sprite sprite = new Sprite(1);

            //Console.WriteLine("sprite: {0},{1},{2}", sprite.position1, sprite.position2, sprite.position3);

            foreach (ProgramInput item in programInputs)
            {
                //Console.WriteLine("command: {0}\tvalue: {1}", item.command, item.value);
                //process noop
                if (item.command.CompareTo("noop") == 0)
                {
                    index++;
                    //draw CRT output
                    //crtIndex = drawCRTOutput(sprite, crtIndex);
                    //process signal strength for specified cycles
                    signalStrength += processSignalStrength(index, registerValue);
                }
                // process addx
                else if (item.command.CompareTo("addx") == 0)
                {
                    for (int cnt = 1; cnt <= 2; cnt++)
                    {
                        index++;
                        //draw CRT output
                        //drawCRTOutput(sprite, crtIndex);
                        //process signal strength for specified cycles
                        signalStrength += processSignalStrength(index, registerValue);                        

                        if (cnt == 2)
                        {
                            registerValue += item.value;
                            sprite.updateSprite(item.value);
                            //Console.WriteLine("sprite: {0},{1},{2}", sprite.position1, sprite.position2, sprite.position3);
                        }
                    }
                }

                //Console.WriteLine("index: {0}\tregisterValue: {1}", index, registerValue);
            }

            return signalStrength;
        }

        public static int processSignalStrength(int index, int registerValue)
        {
            int signalStrength = 0;

            if (index == 20)
            {
                signalStrength += index * registerValue;
            }
            else if (index == 60)
            {
                signalStrength += index * registerValue;
            }
            else if (index == 100)
            {
                signalStrength += index * registerValue;
            }
            else if (index == 140)
            {
                signalStrength += index * registerValue;
            }
            else if (index == 180)
            {
                signalStrength += index * registerValue;
            }
            else if (index == 220)
            {
                signalStrength += index * registerValue;
            }

            return signalStrength;
        }

        public static void drawTheCRTOutput(List<ProgramInput> programInputs)
        {
            int signalStrength = 0;
            int index = 0;
            int registerValue = 1;
            int crtIndex = 0;
            Sprite sprite = new Sprite(1);

            foreach (ProgramInput item in programInputs)
            {
                //Console.WriteLine("command: {0}\tvalue: {1}", item.command, item.value);
                //process noop
                if (item.command.CompareTo("noop") == 0)
                {
                    index++;
                    //process signal strength for specified cycles
                    signalStrength += processSignalStrength(index, registerValue);
                    //draw CRT output
                    drawCRTOutput(sprite, crtIndex);
                }
                // process addx
                else if (item.command.CompareTo("addx") == 0)
                {
                    for (int cnt = 1; cnt <= 2; cnt++)
                    {
                        index++;
                        //process signal strength for specified cycles
                        signalStrength += processSignalStrength(index, registerValue);
                        drawCRTOutput(sprite, crtIndex);

                        if (cnt == 2)
                        {
                            registerValue += item.value;
                            sprite.updateSprite(item.value);
                        }
                    }
                }
            }
        }

        public static bool isCRTIndexInSprite(int crtIndex, Sprite sprite)
        {
            if (crtIndex == sprite.position1 || crtIndex == sprite.position2 || crtIndex == sprite.position3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int drawCRTOutput(Sprite sprite, int crtIndex)
        {
            //draw CRT output
            if (isCRTIndexInSprite(crtIndex, sprite))
            {
                if (crtIndex == 39)
                {
                    Console.WriteLine("#");
                }
                else
                {
                    Console.Write("#");
                }
            }
            else
            {
                if (crtIndex == 39)
                {
                    Console.WriteLine(".");
                }
                else
                {
                    Console.Write(".");
                }
            }

            //increment crtIndex
            crtIndex++;

            //reset to zero if the index is 40
            if (crtIndex == 40)
            {
                crtIndex = 0;
            }

            return crtIndex;
        }

        public class ProgramInput
        {
            public string command { get; set; }
            public int value { get; set; }
            public int cycleCount { get; set; }
            public ProgramInput(string command, int value, int cycleCount)
            {
                this.command = command;
                this.value = value;
                this.cycleCount = cycleCount;
            }
        }
        public class Sprite
        {
            public int position1 { get; set; }
            public int position2 { get; set; }
            public int position3 { get; set; }
            public Sprite(int data)
            {
                this.position1 = data - 1;
                this.position2 = data;
                this.position3 = data + 1;
            }
            public Sprite updateSprite(int value)
            {
                //Console.WriteLine("value: {0}", value);
                this.position1 += value;
                this.position2 += value;
                this.position3 += value;

                return this;
            }
        }
    }
}