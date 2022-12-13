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
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_08/sampleData.txt";
            string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_08/aocData.txt";

            int[] grid = new int[2];

            try
            {
                //determine grid size
                grid = getGridDimentions(dataLocation);
                //Console.WriteLine("rows: {0}\t columns: {1}", grid[0], grid[1]);
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            List<int[]> gridMatrix = new List<int[]>();
            try
            {
                //insert data into grid matrix
                gridMatrix = getGridMatrix(dataLocation, grid);
                // Console.WriteLine("grid size: {0}", gridMatrix.Count());
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            //iterate thru the grid matrix to find the number of visible trees
            int numberOfVisibleTrees = getNumberOfVisibleTreesFromEdge(gridMatrix, grid);

            Console.WriteLine("The number of visible trees from the edge: {0}", numberOfVisibleTrees);

            //get the scenic score of the ideal spot for the tree house
            //using the same logic above, we can generate the tree with the most visibility
            //then check the score for those
            int score = getHighestScenicScore(gridMatrix, grid);

            Console.WriteLine("The highest scenic score possible for any tree: {0}", score);
        }

        public static int[] getGridDimentions(string dataLocation)
        {

            String data = "";
            int[] specs = new int[2];
            int rows = 0;
            int columns = 0;

            using (StreamReader sr = new StreamReader(dataLocation))
            {
                while ((data = sr.ReadLine()) != null)
                {
                    rows++;
                    //column should not change since it should be the same width althroughout
                    columns = data.Length;
                }

                specs[0] = rows;
                specs[1] = columns;
            }

            return specs;
        }

        public static List<int[]> getGridMatrix(string dataLocation, int[] grid)
        {
            List<int[]> dataGrid = new List<int[]>();
            string data = "";
            int row = 0;

            using (StreamReader sr = new StreamReader(dataLocation))
            {
                while ((data = sr.ReadLine()) != null)
                {
                    //Console.WriteLine("{0}", data);
                    //enter the data to the grid
                    int[] columns = new int[data.Count()];

                    for (int index = 0; index < data.Count(); index++)
                    {
                        columns[index] = int.Parse(data.ElementAt(index).ToString());
                    }

                    dataGrid.Add(columns);
                }
            }

            //print grid
            // foreach (int[] item in dataGrid)
            // {
            //     foreach (int item2 in item)
            //     {
            //         Console.Write("{0}", item2);
            //     }
            //     Console.WriteLine("");
            // }

            return dataGrid;
        }

        public static int getNumberOfVisibleTreesFromEdge(List<int[]> gridMatrix, int[] grid)
        {
            int numberOfVisibleTrees = 0;
            int rows = grid[0];
            int cols = grid[1];

            //calculate the default row of trees at the edge
            //top and bottom = cols * 2
            numberOfVisibleTrees += (cols * 2);
            //left and right = (rows - 2) * 2
            numberOfVisibleTrees += ((rows - 2) * 2);

            //iterate thru the inner matrix
            //row index 1 to n-1
            //column 1 to n-1
            for (int rowIndex = 1; rowIndex < (rows - 1); rowIndex++)
            {
                bool visible = false;

                for (int colIndex = 1; colIndex < (cols - 1); colIndex++)
                {
                    //current position
                    int[] currentPosition = new int[2];
                    currentPosition[0] = rowIndex;
                    currentPosition[1] = colIndex;

                    if (isCurrentPositionVisibleFromAnyEdge(currentPosition, grid, gridMatrix))
                    {
                        numberOfVisibleTrees++;
                    }
                }
            }

            return numberOfVisibleTrees;
        }

        public static int getHighestScenicScore(List<int[]> gridMatrix, int[] grid)
        {
            int score = 0;
            int rows = grid[0];
            int cols = grid[1];
            int currentScore = 0;

            //iterate thru the inner matrix
            //row index 1 to n-1
            //column 1 to n-1
            for (int rowIndex = 1; rowIndex < (rows - 1); rowIndex++)
            {
                bool visible = false;

                for (int colIndex = 1; colIndex < (cols - 1); colIndex++)
                {
                    //current position
                    int[] currentPosition = new int[2];
                    currentPosition[0] = rowIndex;
                    currentPosition[1] = colIndex;
                    currentScore = getScenicScore(currentPosition, grid, gridMatrix);
                    //Console.WriteLine("currentScore: {0}", currentScore);

                    //set score data
                    if (currentScore > score)
                    {
                        score = currentScore;
                    }
                }
            }

            return score;
        }

        public static bool isCurrentPositionVisibleFromAnyEdge(int[] currentPosition, int[] grid, List<int[]> gridMatrix)
        {
            int currentRowIndex = currentPosition[0];
            int currentColIndex = currentPosition[1];
            int minRowIndex = 0;
            int minColIndex = 0;
            int maxRowIndex = grid[0] - 1;
            int maxColIndex = grid[1] - 1;

            //Console.WriteLine("currentRowIndex: {0}\tcurrentColIndex: {1}", currentRowIndex, currentColIndex);
            // Console.WriteLine("minRowIndex: {0}\tmaxRowIndex: {1}", minRowIndex, maxRowIndex);
            // Console.WriteLine("minColIndex: {0}\tmaxColIndex: {1}", minColIndex, maxColIndex);

            int indexHeight = gridMatrix[currentRowIndex][currentColIndex];
            //Console.WriteLine("indexHeight: {0}", indexHeight);

            //check left (negative col; same row)
            bool isLeftVisible = true;
            for (int index = currentColIndex; index >= minColIndex; index--)
            {
                //get next index to the left
                int leftIndex = index - 1;

                //if left index is greater than or equal to zero, process the data
                if (leftIndex >= 0)
                {
                    int[] colData = gridMatrix[currentRowIndex];
                    int treeHeight = colData[leftIndex];
                    //Console.WriteLine("indexHeight: {0}, treeHeight: {1}", indexHeight, treeHeight);

                    //if tree is higher than indexHeight, it not visible
                    if (treeHeight >= indexHeight)
                    {
                        isLeftVisible = false;
                        break;
                    }
                }
            }

            if (isLeftVisible)
            {
                return isLeftVisible;
            }

            //check right
            bool isRightVisible = true;
            for (int index = currentColIndex; index <= maxColIndex; index++)
            {
                //get next index to the right
                int rightIndex = index + 1;

                //if right index is less than or equal to max column index, process the data
                if (rightIndex <= maxColIndex)
                {
                    int[] colData = gridMatrix[currentRowIndex];
                    int treeHeight = colData[rightIndex];
                    //Console.WriteLine("indexHeight: {0}, treeHeight: {1}", indexHeight, treeHeight);

                    //if tree is higher than indexHeight, it not visible
                    if (treeHeight >= indexHeight)
                    {
                        isRightVisible = false;
                        break;
                    }
                }
            }

            if (isRightVisible)
            {
                return isRightVisible;
            }

            //check top
            bool isTopVisible = true;
            for (int index = currentRowIndex; index >= minRowIndex; index--)
            {
                //get next index to the top
                int topIndex = index - 1;

                //if top index is greater than or equal to zero, process the data
                if (topIndex >= 0)
                {
                    int[] colData = gridMatrix[topIndex];
                    int treeHeight = colData[currentColIndex];
                    //Console.WriteLine("indexHeight: {0}, treeHeight: {1}", indexHeight, treeHeight);

                    //if tree is higher than indexHeight, it not visible
                    if (treeHeight >= indexHeight)
                    {
                        isTopVisible = false;
                        break;
                    }
                }
            }

            if (isTopVisible)
            {
                return isTopVisible;
            }

            //check bottom
            bool isBottomVisible = true;
            for (int index = currentRowIndex; index <= maxRowIndex; index++)
            {
                //get next index to the bottom
                int bottomIndex = index + 1;

                //if bottom index is less than or equal to max column index, process the data
                if (bottomIndex <= maxRowIndex)
                {
                    int[] colData = gridMatrix[bottomIndex];
                    int treeHeight = colData[currentColIndex];
                    //Console.WriteLine("indexHeight: {0}, treeHeight: {1}", indexHeight, treeHeight);

                    //if tree is higher than indexHeight, it not visible
                    if (treeHeight >= indexHeight)
                    {
                        isBottomVisible = false;
                        break;
                    }
                }
            }

            if (isBottomVisible)
            {
                return isBottomVisible;
            }

            return false;
        }

        public static int getScenicScore(int[] currentPosition, int[] grid, List<int[]> gridMatrix)
        {
            int currentRowIndex = currentPosition[0];
            int currentColIndex = currentPosition[1];
            int minRowIndex = 0;
            int minColIndex = 0;
            int maxRowIndex = grid[0] - 1;
            int maxColIndex = grid[1] - 1;

            int indexHeight = gridMatrix[currentRowIndex][currentColIndex];

            //check left (negative col; same row)
            int leftCount = 0;
            for (int index = currentColIndex; index >= minColIndex; index--)
            {
                //get next index to the left
                int leftIndex = index - 1;

                //if left index is greater than or equal to zero, process the data
                if (leftIndex >= 0)
                {
                    int[] colData = gridMatrix[currentRowIndex];
                    int treeHeight = colData[leftIndex];
                    //Console.WriteLine("leftCount -> indexHeight: {0}, treeHeight: {1}, leftCount: {2}", indexHeight, treeHeight, leftCount);

                    //count up to the tree that is same height or blocks the view
                    if (treeHeight >= indexHeight)
                    {
                        leftCount++;
                        break;
                    } else {
                        leftCount++;
                    }
                }
            }
            //Console.WriteLine("leftCount -> {0}", leftCount);

            //check right
            int rightCount = 0;
            for (int index = currentColIndex; index <= maxColIndex; index++)
            {
                //get next index to the right
                int rightIndex = index + 1;

                //if right index is less than or equal to max column index, process the data
                if (rightIndex <= maxColIndex)
                {
                    int[] colData = gridMatrix[currentRowIndex];
                    int treeHeight = colData[rightIndex];
                    //Console.WriteLine("rightCount -> indexHeight: {0}, treeHeight: {1}, leftCount: {2}", indexHeight, treeHeight, rightCount);

                    //count up to the tree that is same height or blocks the view
                    if (treeHeight >= indexHeight)
                    {
                        rightCount++;
                        break;
                    } else {
                        rightCount++;
                    }
                }
            }
            //Console.WriteLine("rightCount -> {0}", rightCount);

            //check top
            int topCount = 0;
            for (int index = currentRowIndex; index >= minRowIndex; index--)
            {
                //get next index to the top
                int topIndex = index - 1;

                //if top index is greater than or equal to zero, process the data
                if (topIndex >= 0)
                {
                    int[] colData = gridMatrix[topIndex];
                    int treeHeight = colData[currentColIndex];
                    //Console.WriteLine("topCount -> indexHeight: {0}, treeHeight: {1}, leftCount: {2}", indexHeight, treeHeight, topCount);

                    //count up to the tree that is same height or blocks the view
                    if (treeHeight >= indexHeight)
                    {
                        topCount++;
                        break;
                    } else {
                        topCount++;
                    }
                }
            }
            //Console.WriteLine("topCount -> {0}", topCount);

            //check bottom
            int bottomCount = 0;
            for (int index = currentRowIndex; index <= maxRowIndex; index++)
            {
                //get next index to the bottom
                int bottomIndex = index + 1;

                //if bottom index is less than or equal to max column index, process the data
                if (bottomIndex <= maxRowIndex)
                {
                    int[] colData = gridMatrix[bottomIndex];
                    int treeHeight = colData[currentColIndex];
                    //Console.WriteLine("bottomCount -> indexHeight: {0}, treeHeight: {1}, leftCount: {2}", indexHeight, treeHeight, bottomCount);

                    //count up to the tree that is same height or blocks the view
                    if (treeHeight >= indexHeight)
                    {
                        bottomCount++;
                        break;
                    } else {
                        bottomCount++;
                    }
                }
            }
            //Console.WriteLine("bottomCount -> {0}", bottomCount);

            return leftCount * rightCount * topCount * bottomCount;
        }
    }
}