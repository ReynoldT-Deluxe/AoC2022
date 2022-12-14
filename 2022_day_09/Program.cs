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
            string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_09/sampleData.txt";
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_xx/aocData.txt"; 

            //problem part
            int partNum = 1;
            //int partNum = 2;

            List<Movement> movements = new List<Movement>();

            try
            {
                using (StreamReader sr = new StreamReader(dataLocation))
                {
                    string data;

                    while ((data = sr.ReadLine()) != null)
                    {
                        Console.WriteLine("data: {0}", data);
                        string[] input = data.Split(" ");
                        movements.Add(new Movement(input[0], int.Parse(input[1])));
                    }
                }

            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            //process the movements
            //assuming that there is a 6X6 space available, where the starting point is 1,1 (avoids calculation errors due to index)
            string[,] movementMap = new string[6, 6];
            Console.WriteLine("movement: {0}", movements.Count);

            processRopeMovement(movementMap, movements);
        }

        public static void processRopeMovement(string[,] movementMap, List<Movement> movements)
        {
            //starting position is assumed to be 5x1
            movementMap[5, 1] = "x";

            int row = 5;
            int col = 1;
            Position initialHeadPosition = new Position(row, col);
            Position initialTailPosition = new Position(row, col);
            Position newHeadPosition;
            Position newTailPosition;

            foreach (Movement moves in movements)
            {
                string direction = moves.direction;
                int space = moves.space;
                row = initialHeadPosition.row;
                col = initialHeadPosition.column;

                //if the head moves to the left, column data is subtracted
                if (direction.CompareTo("L") == 0)
                {
                    newHeadPosition = new Position(row, col - space);
                    newTailPosition = detemineNewTailPosition(initialHeadPosition, moves, initialTailPosition, movementMap);

                    initialHeadPosition = newHeadPosition;
                    initialTailPosition = newTailPosition;
                }

                //head moves to the right
                if (direction.CompareTo("R") == 0)
                {
                    newHeadPosition = new Position(row, col + space);
                    newTailPosition = detemineNewTailPosition(initialHeadPosition, moves, initialTailPosition, movementMap);

                    initialHeadPosition = newHeadPosition;
                    initialTailPosition = newTailPosition;
                }

                //head moves up
                if (direction.CompareTo("U") == 0)
                {
                    newHeadPosition = new Position(row - space, col);
                    newTailPosition = detemineNewTailPosition(initialHeadPosition, moves, initialTailPosition, movementMap);

                    initialHeadPosition = newHeadPosition;
                    initialTailPosition = newTailPosition;
                }

                //head moves down
                //head moves up
                if (direction.CompareTo("D") == 0)
                {
                    newHeadPosition = new Position(row + space, col);
                    newTailPosition = detemineNewTailPosition(initialHeadPosition, moves, initialTailPosition, movementMap);

                    initialHeadPosition = newHeadPosition;
                    initialTailPosition = newTailPosition;
                }
            }
        }

        public static void printMovementMap(string[,] movementMap)
        {
            //print movementMap
            for (int r = 0; r < 6; r++)
            {
                for (int c = 0; c < 6; c++)
                {
                    string data = movementMap[r, c];
                    Console.Write(data);
                }
                Console.WriteLine("");
            }
        }

        public static Position detemineNewTailPosition(Position initialHeadPosition, Movement moves, Position initialTailPosition, string[,] movementMap)
        {
            Console.WriteLine("movetail: {0}', {1}", moves.direction, moves.space);

            if (moves.direction.CompareTo("L") == 0)
            {
                if (moves.space >= 1)
                {
                    //tail moves to the right of the head and movement is recorded for all columns from head position
                    for (int index = initialHeadPosition.column; index > (initialHeadPosition.column - moves.space); index--)
                    {
                        //Console.WriteLine("index: {0}; check: {1}; iteratae: {0}", index, initialHeadPosition.column - moves.space, index-1);
                        if ((initialHeadPosition.row != initialTailPosition.row)
                            && (initialHeadPosition.column == initialTailPosition.column
                                || (initialHeadPosition.column - initialTailPosition.column) == -1
                                || (initialHeadPosition.column - initialTailPosition.column) == 1))
                        {
                            //don't mark
                            continue;
                        }
                        else
                        {
                            movementMap[initialHeadPosition.row, index] = "x";
                            printMovementMap(movementMap);
                        }
                    }
                    return new Position(initialHeadPosition.row, initialHeadPosition.column + 1 - moves.space);
                }
                else if (moves.space == 1)
                {
                    //tail does not move
                    return initialTailPosition;
                }
            }

            if (moves.direction.CompareTo("R") == 0)
            {
                if (moves.space >= 1)
                {
                    //tail moves to the left of the head and movement is recorded for all columns from head position
                    for (int index = initialHeadPosition.column; index < (initialHeadPosition.column + moves.space); index++)
                    {
                        if ((initialHeadPosition.row != initialTailPosition.row)
                            && (initialHeadPosition.column == initialTailPosition.column
                                || (initialHeadPosition.column - initialTailPosition.column) == -1
                                || (initialHeadPosition.column - initialTailPosition.column) == 1))
                        {
                            //don't mark
                            continue;
                        }
                        else
                        {
                            movementMap[initialHeadPosition.row, index] = "x";
                            printMovementMap(movementMap);
                        }
                    }
                    return new Position(initialHeadPosition.row, initialHeadPosition.column - 1 + moves.space);
                }
                else if (moves.space == 1)
                {
                    //tail does not move
                    return initialTailPosition;
                }
            }

            if (moves.direction.CompareTo("U") == 0)
            {
                Console.WriteLine("inside U");
                Console.WriteLine("initialHeadPosition.row: {0}", initialHeadPosition.row);
                Console.WriteLine("initialTailPosition.row: {0}", initialTailPosition.row);
                Console.WriteLine("moves.space: {0}", moves.space);
                Console.WriteLine("initialHeadPosition.column: {0}", initialHeadPosition.column);
                Console.WriteLine("initialTailPosition.column: {0}", initialTailPosition.column);

                if (moves.space >= 1)
                {
                    //tail moves to below the head and movement is recorded for all columns from head position
                    for (int index = initialHeadPosition.row; index > (initialHeadPosition.row - moves.space); index--)
                    {
                        Console.WriteLine("index: {0}", index);
                        Console.WriteLine("initialHeadPosition.column: {0}", initialHeadPosition.column);
                        if ((initialHeadPosition.column != initialTailPosition.column))
                        {
                            if (index == initialTailPosition.row
                                                        || index == (initialTailPosition.row + 1)
                                                        || index == (initialTailPosition.row - 1))
                            {
                                //don't mark
                                Console.WriteLine("no mark");
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("else");
                                movementMap[index, initialHeadPosition.column] = "x";
                                printMovementMap(movementMap);
                            }
                        }
                        else
                        {
                            Console.WriteLine("else");
                            movementMap[index, initialHeadPosition.column] = "x";
                            printMovementMap(movementMap);
                        }
                    }
                    return new Position(initialHeadPosition.row + 1 - moves.space, initialHeadPosition.column);
                }
                else if (moves.space == 1)
                {
                    //tail does not move
                    return initialTailPosition;
                }
            }

            if (moves.direction.CompareTo("D") == 0)
            {
                if (moves.space >= 1)
                {
                    //tail moves to below the head and movement is recorded for all columns from head position
                    for (int index = initialHeadPosition.row; index < (initialHeadPosition.row + moves.space); index++)
                    {
                        if ((initialHeadPosition.column != initialTailPosition.column)
                                                    && (initialHeadPosition.row == initialTailPosition.row
                                                        || (initialHeadPosition.row - initialTailPosition.row) == -1
                                                        || (initialHeadPosition.row - initialTailPosition.row) == 1))
                        {
                            //don't mark
                            continue;
                        }
                        else
                        {
                            movementMap[index, initialHeadPosition.column] = "x";
                            printMovementMap(movementMap);
                        }
                    }
                    return new Position(initialHeadPosition.row - 1 + moves.space, initialHeadPosition.column);
                }
                else if (moves.space == 1)
                {
                    //tail does not move
                    return initialTailPosition;
                }
            }

            return initialTailPosition;
        }

        public class Movement
        {
            public string direction;
            public int space;

            public Movement(string direction, int space)
            {
                this.direction = direction;
                this.space = space;
            }
        }

        public class Position
        {
            public int row;
            public int column;

            public Position(int row, int column)
            {
                this.row = row;
                this.column = column;
            }
        }
    }
}