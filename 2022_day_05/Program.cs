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
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_05/sampleData.txt";
            string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_05/aocData.txt"; 

            //problem part
            int partNum = 1;
            //int partNum = 2;

            int maxStacks = 0;
            int stackPositions = 0;

            //get the maximum number of stacks
            try
            {
                string stackData = getMaxStacks(dataLocation);
                string[] data = stackData.Split(",");

                stackPositions = int.Parse(data[0]);
                maxStacks = int.Parse(data[1]);
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The maxStackStream file could not be read:");
                Console.WriteLine(e.Message);
            }

            // get the maximum number of moves
            int moveCount = 0;
            int maxMoves = 0;

            try
            {
                string moveData = getMoveData(dataLocation);
                string[] data = moveData.Split(",");

                moveCount = int.Parse(data[0]);
                maxMoves = int.Parse(data[1]);

            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The moveCountStream file could not be read:");
                Console.WriteLine(e.Message);
            }

            //get the move data
            //three columns are: crates to move, from stack location, to stack location
            int[,] moveArray = new int[moveCount, 3];

            try
            {
                moveArray = getMoveArray(dataLocation, moveArray);

                // for (int row = 0; row < moveCount; row++)
                // {
                //     for (int col = 0; col < 3; col++)
                //     {
                //         Console.Write(moveArray[row, col] + " ");
                //     }
                //     Console.WriteLine();
                // }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The maxMoveStream file could not be read:");
                Console.WriteLine(e.Message);
            }

            //get crate data
            string[] crateData = new string[maxStacks];

            try
            {
                crateData = getCrateData(dataLocation, stackPositions);
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The crateDataStream file could not be read:");
                Console.WriteLine(e.Message);
            }

            //get data bottom up for each stack
            //for the maximum stack rows, we'll assume that it is quadruple the max move count
            string[,] crateStackArray = convertCrateDataToCrateStackArray(crateData, maxMoves, stackPositions, maxStacks);

            if (partNum == 1)
            {
                //process the move array against the crate stack array
                crateStackArray = processMovementsCrateMover9000(crateStackArray, moveArray, moveCount);

                string topCrates = "";
                //get the top stack data
                for (int col = 0; col < maxStacks; col++) {
                    int dataIndex = int.Parse(crateStackArray[0,col]);
                    topCrates += crateStackArray[dataIndex,col];
                }

                Console.WriteLine("top crates: {0}", topCrates);

                // for (int row = 0; row < (stackPositions * 4); row++)
                // {
                //     for (int col = 0; col < maxStacks; col++)
                //     {
                //         Console.Write(crateStackArray[row, col] + " ");
                //     }
                //     Console.WriteLine();
                // }

                //part 2 solution
            }
            else if (partNum == 2)
            {

            }

        }

        public static string getMaxStacks(string dataLocation)
        {
            using (StreamReader stackStream = new StreamReader(dataLocation))
            {
                string stackStreamData;
                int stackNumberPosition = 0;

                while ((stackStreamData = stackStream.ReadLine()) != null)
                {
                    //remove spaces and get the last data
                    string stacks = cleanStreamData(stackStreamData);

                    if (stacks.CompareTo("") != 0 && stacks.All(Char.IsDigit))
                    {
                        return stackNumberPosition + "," + stacks.Substring(stacks.Length - 1);
                    }
                    else
                    {
                        stackNumberPosition++;
                    }
                }
            }

            return "";
        }

        public static string cleanStreamData(string dataStream)
        {
            return dataStream.Replace(" ", "").Replace("[", "").Replace("]", "");
        }

        public static string getMoveData(string dataLocation)
        {
            int moveCount = 0;
            int maxMoves = 0;

            using (StreamReader moveStream = new StreamReader(dataLocation))
            {
                string moveStreamData;

                while ((moveStreamData = moveStream.ReadLine()) != null)
                {
                    if (moveStreamData.StartsWith("move"))
                    {
                        moveCount++;
                        string[] moveCrateData = moveStreamData.Split(" from ");
                        int moves = int.Parse(moveCrateData[0].Split("move ")[1]);

                        if (moves > maxMoves)
                        {
                            maxMoves = moves;
                        }
                    }
                }
            }

            return moveCount + "," + maxMoves;
        }

        public static int[,] getMoveArray(string dataLocation, int[,] moveArray)
        {
            using (StreamReader moveStream = new StreamReader(dataLocation))
            {
                string moveStreamData;
                int rows = 0;

                while ((moveStreamData = moveStream.ReadLine()) != null)
                {
                    if (moveStreamData.StartsWith("move"))
                    {
                        string[] moveCrateData = moveStreamData.Split(" from ");
                        moveArray[rows, 0] = int.Parse(moveCrateData[0].Split("move ")[1]);

                        string[] moveCraneData = moveCrateData[1].Split(" to ");
                        moveArray[rows, 1] = int.Parse(moveCraneData[0]);
                        moveArray[rows, 2] = int.Parse(moveCraneData[1]);
                        rows++;
                    }
                }
            }

            return moveArray;
        }

        public static string[] getCrateData(string dataLocation, int stackPositions)
        {
            string[] crateData = new string[stackPositions];

            using (StreamReader moveStream = new StreamReader(dataLocation))
            {
                string crateStreamData;
                int crateIndex = 0;

                while ((crateStreamData = moveStream.ReadLine()) != null && crateIndex < stackPositions)
                {
                    if (crateStreamData.Length != 0 && !crateStreamData.StartsWith("move"))
                    {
                        crateData[crateIndex] = crateStreamData;
                        crateIndex++;
                    }
                }
            }

            return crateData;
        }

        public static string[,] convertCrateDataToCrateStackArray(string[] crateData, int maxMoves, int stackPositions, int maxStacks)
        {
            // Console.WriteLine("stackPositions: {0}", stackPositions);
            // Console.WriteLine("maxStacks: {0}", maxStacks);
            // Console.WriteLine("maxMoves: {0}", maxMoves);
            
            string[,] crateStackArray = new string[(maxMoves * 4), maxStacks];

            //set count data on row 0
            for (int col = 0; col < (maxStacks); col++)
            {
                crateStackArray[0, col] = "0";
            }

            //get string from crateData indexes, starting from index 1
            int index = 1;
            int crateStackArrayRow = 1;

            //get data from crateData
            for (int stackPositionIndex = (stackPositions - 1); stackPositionIndex >= 0; stackPositionIndex--)
            {
                int stackIndex = 0;

                while (index < crateData[stackPositionIndex].Length)
                {
                    string dataToWrite = crateData[stackPositionIndex][index].ToString();
                    crateStackArray[crateStackArrayRow, stackIndex] = dataToWrite;

                    //increment stack count if data is not blank
                    if (dataToWrite.CompareTo(" ") != 0)
                    {
                        int count = int.Parse(crateStackArray[0, stackIndex]);
                        count++;
                        crateStackArray[0, stackIndex] = count.ToString();
                    }

                    //increment index by 4
                    index = index + 4;
                    //increment array parameters
                    stackIndex++;
                }

                //reset index to 1
                index = 1;
                //increment row
                crateStackArrayRow++;
            }

            // for (int row = 0; row < ((maxMoves * 4)); row++)
            // {
            //     for (int col = 0; col < (maxStacks); col++)
            //     {
            //         Console.Write(crateStackArray[row, col] + " ");
            //     }
            //     Console.WriteLine();
            // }

            return crateStackArray;
        }

        public static string[,] processMovementsCrateMover9000(string[,] crateStackArray, int[,] moveArray, int moveCount)
        {
            //move array cols = crates to move, from stack location, to stack location
            for (int moveIndex = 0; moveIndex < moveCount; moveIndex++)
            {
                int createsToMove = moveArray[moveIndex, 0];
                int stackFrom = moveArray[moveIndex, 1];
                int stackFrom2 = stackFrom - 1;
                int stackTo = moveArray[moveIndex, 2];
                int stackTo2 = stackTo - 1;
                // Console.WriteLine("createsToMove: {0}", createsToMove);
                // Console.WriteLine("stackFrom: {0}", stackFrom);
                // Console.WriteLine("stackTo: {0}", stackTo);
                int currentFromCount = int.Parse(crateStackArray[0, stackFrom2]);
                int currentToCount = int.Parse(crateStackArray[0, stackTo2]);

                int fromIndex = currentFromCount;
                    int toIndex = currentToCount;

                //update the stacks bottom up
                for (int createIndex = 0; createIndex < createsToMove; createIndex++)
                {
                    //get data from last index
                    string fromData = crateStackArray[currentFromCount, stackFrom2];
                    // Console.WriteLine("fromData: {0}", fromData);
                    // Console.WriteLine("currentFromCount: {0}", currentFromCount);
                    // Console.WriteLine("stackFrom2: {0}", stackFrom2);

                    //set data to blank
                    crateStackArray[currentFromCount, stackFrom2] = " ";

                    // for (int row = 0; row < 62; row++)
                    // {
                    //     for (int col = 0; col < 9; col++)
                    //     {
                    //         Console.Write(crateStackArray[row, col] + " ");
                    //     }
                    //     Console.WriteLine();
                    // }

                    //add data to new stack 
                    toIndex++;
                    // Console.WriteLine("currentToCount: {0}", currentToCount);
                    // Console.WriteLine("toIndex: {0}", toIndex);
                    // Console.WriteLine("stackTo2: {0}", stackTo2);
                    crateStackArray[toIndex, stackTo2] = fromData;

                    // for (int row = 0; row < 62; row++)
                    // {
                    //     for (int col = 0; col < 9; col++)
                    //     {
                    //         Console.Write(crateStackArray[row, col] + " ");
                    //     }
                    //     Console.WriteLine();
                    // }

                    //update create stack array counts
                    currentFromCount--;
                    currentToCount++;
                    crateStackArray[0, stackFrom2] = currentFromCount.ToString();
                    crateStackArray[0, stackTo2] = currentToCount.ToString();

                    // for (int row = 0; row < 62; row++)
                    // {
                    //     for (int col = 0; col < 9; col++)
                    //     {
                    //         Console.Write(crateStackArray[row, col] + " ");
                    //     }
                    //     Console.WriteLine();
                    // }
                }
            }

            return crateStackArray;
        }
    }
}