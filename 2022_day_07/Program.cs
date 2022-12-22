using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections;

namespace FileApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //data to use
            string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_07/sampleData.txt";
            //string dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_07/aocData.txt";

            //problem part
            int partNum = 1;
            //int partNum = 2;

            List<FileSystemData> fileSystemData = new List<FileSystemData>();

            try
            {
                string output = "";
                //create the file system arraylist
                fileSystemData = generateFileSystemArrayList(dataLocation);
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            //part 1 solution
            if (partNum == 1)
            {
                List<FileSystemData> totalledFileByDirectory = calculateTotalFileSizeByDirectory(fileSystemData);

                // foreach (FileSystemData fsd in totalledFileByDirectory)
                // {
                //     Console.WriteLine("level: {0} \t name: {1} \t type: {2} \t directory: {3} \t size {4}",
                //         fsd.level, fsd.name, fsd.type, fsd.directory, fsd.size);
                // }
                // Console.WriteLine("");

                //get the initial list of directories with size at most 100000
                List<FileSystemData> directoryWithAtMost100K = get100KList(totalledFileByDirectory);

                //get list of directories from the fileSystemData
                List<FileSystemData> directoryList = getDirectoryList(fileSystemData);

                // foreach (FileSystemData fsd in directoryList)
                // {
                //     Console.WriteLine("level: {0} \t name: {1} \t type: {2} \t directory: {3} \t size {4}",
                //         fsd.level, fsd.name, fsd.type, fsd.directory, fsd.size);
                // }

                //calculate the aggregate directory totals based on the heirarchy and add to the 100K list
                aggregateData(totalledFileByDirectory, directoryList, directoryWithAtMost100K);

                //total all the aggregate data as the total size
                Console.WriteLine("The sum total size of 100K sized directories: {0}",
                    getSumTotalOf100KDirectories(directoryWithAtMost100K));

                //part 2 solution
            }
            else if (partNum == 2)
            {

            }

            // Console.WriteLine("output: {0}", output);
        }

        public static List<FileSystemData> generateFileSystemArrayList(string dataLocation)
        {
            List<FileSystemData> fileSystemData = new List<FileSystemData>();

            using (StreamReader sr = new StreamReader(dataLocation))
            {
                //initialize data contents
                int level = 0;
                string name = "";
                string type = "directory";
                string directory = "";
                long size = 0;

                //set root directory data
                name = "root";
                fileSystemData.Add(new FileSystemData(level, name, type, directory, size));

                string data;

                while ((data = sr.ReadLine()) != null)
                {
                    //Console.WriteLine("data: {0}", data);

                    //$ cd *
                    //set directory to *
                    if (data.StartsWith("$ cd"))
                    {
                        string[] cd = data.Split(" ");
                        directory = cd[2];
                    }

                    //ls
                    //increase level to 1
                    if (data.StartsWith("$ ls"))
                    {
                        level++;
                    }

                    //dir *
                    //check if * exists
                    //if it doesn't exist, create a new one
                    //else, do nothing
                    if (data.StartsWith("dir"))
                    {
                        string[] dir = data.Split(" ");
                        string directoryName = dir[1];

                        bool directoryExists = false;
                        //check if the data exists for same directory and level (in case other levels have the same directory name)
                        foreach (FileSystemData fileDataEntry in fileSystemData)
                        {
                            if (fileDataEntry.directory.Equals(directoryName) && fileDataEntry.level.Equals(level))
                            {
                                directoryExists = true;
                                break;
                            }
                        }

                        if (!directoryExists)
                        {
                            type = "directory";
                            name = directoryName;
                            size = 0;
                            fileSystemData.Add(new FileSystemData(level, name, type, directory, size));
                        }
                    }

                    //if starts with a number, it is designated as a file
                    //add the file data
                    if (Char.IsDigit(data.ElementAt(0)))
                    {
                        string[] file = data.Split(" ");
                        string fileSize = file[0];
                        string fileName = file[1];

                        name = fileName;
                        type = "file";
                        size = long.Parse(fileSize);
                        fileSystemData.Add(new FileSystemData(level, name, type, directory, size));
                    }

                    //cd ..
                    //decrease level value
                    if (data.StartsWith("$ cd .."))
                    {
                        level--;
                    }
                }
            }

            //return sorted descending by level
            return fileSystemData.OrderByDescending(info => info.level).ToList();
        }

        public static List<FileSystemData> calculateTotalFileSizeByDirectory(List<FileSystemData> data)
        {
            List<FileSystemData> totalledFileByDirectory = new List<FileSystemData>();

            //iterate thru the fileSystemData and get the data for a specified level
            for (int index = 0; index < data.Count; index++)
            {
                //get the type and verify that it is a file                
                string type = data[index].type;

                if (type.CompareTo("file") == 0)
                {
                    //get data
                    int level = data[index].level;
                    string name = data[index].name;
                    string directory = data[index].directory;
                    long size = data[index].size;

                    //check if the record exists in the totalledFileByDirectory                                               
                    bool dataExists = false;

                    foreach (FileSystemData totals in totalledFileByDirectory)
                    {
                        //if it already exists, update the size value
                        if (level.Equals(totals.level) && directory.CompareTo(totals.directory) == 0)
                        {
                            dataExists = true;
                            totals.size += size;
                        }
                    }

                    //if it doesn't, insert a new record 
                    if (!dataExists)
                    {
                        totalledFileByDirectory.Add(new FileSystemData(level, ("total for directory " + directory), type, directory, size));
                    }
                }
            }

            //return sorted descending by level
            return totalledFileByDirectory.OrderByDescending(info => info.level).ToList();
        }

        public static List<FileSystemData> get100KList(List<FileSystemData> totalledFileByDirectory)
        {
            List<FileSystemData> output100KList = new List<FileSystemData>();

            foreach (FileSystemData data in totalledFileByDirectory)
            {
                if (data.size <= 100000)
                {
                    output100KList.Add(data);
                }
            }

            return output100KList;
        }

        public static List<FileSystemData> getDirectoryList(List<FileSystemData> fileSystemData)
        {
            List<FileSystemData> directoryList = new List<FileSystemData>();

            //iterate thru the fileSystemData and get the data for a specified level
            for (int index = 0; index < fileSystemData.Count; index++)
            {
                //get the type and verify that it is a file                
                string type = fileSystemData[index].type;

                if (type.CompareTo("directory") == 0)
                {
                    directoryList.Add(fileSystemData[index]);
                }
            }

            //return sorted descending by level
            return directoryList.OrderByDescending(info => info.level).ToList();
        }

        public static void aggregateData(List<FileSystemData> totalledFileByDirectory, List<FileSystemData> directoryList,
            List<FileSystemData> directoryWithAtMost100K)
        {
            //iterate thru the totalledFileByDirectory per level, starting from the higest one
            int higestLevel = totalledFileByDirectory[0].level;

            for (int index = higestLevel; index >= 1; index--)
            {
                //Console.WriteLine("currentLevel: {0}", index);
                //get totalledFileByDirectory based on a level
                List<FileSystemData> levelTotalledFileByDirectory = getTotalledFileByDirectoryByLevel(totalledFileByDirectory, index);

                foreach (FileSystemData levelTotalData in levelTotalledFileByDirectory)
                {
                    // Console.WriteLine("totalData -> level: {0} \t name: {1} \t type: {2} \t directory: {3} \t size {4}",
                    //                     totalData.level, totalData.name, totalData.type, totalData.directory, totalData.size);
                    //get the current data                
                    int level = levelTotalData.level;
                    int lowerLevel = level - 1;
                    string dir = levelTotalData.directory;
                    long size = levelTotalData.size;
                    //long updatedSize = 0;
                    string parentDirectory = "";

                    //iterate thru the directory list and check that the current data is a subdirectory
                    //level is not expected to be less than zero for this list
                    foreach (FileSystemData directoryData in directoryList)
                    {
                        // Console.WriteLine("directoryData -> level: {0} \t name: {1} \t type: {2} \t directory: {3} \t size {4}",
                        //                                     directoryData.level, directoryData.name, directoryData.type, directoryData.directory, directoryData.size);
                        // Console.WriteLine("lowerLevel: {0}", lowerLevel);
                        // Console.WriteLine("dir: {0}", dir);
                        if (lowerLevel >= 0 && lowerLevel == directoryData.level && dir.CompareTo(directoryData.name) == 0)
                        {
                            //add the size of the parent directory
                            foreach (FileSystemData parentTotalData in totalledFileByDirectory)
                            {
                                // Console.WriteLine("parentTotalData -> level: {0} \t name: {1} \t type: {2} \t directory: {3} \t size {4}",
                                //                             parentTotalData.level, parentTotalData.name, parentTotalData.type, parentTotalData.directory, parentTotalData.size);
                                if (lowerLevel == parentTotalData.level && parentTotalData.directory.CompareTo(directoryData.directory) == 0)
                                {
                                    parentDirectory = parentTotalData.directory;
                                    //updatedSize = size + parentTotalData.size;                                    
                                    //update the parent
                                    parentTotalData.size += size;
                                    //Console.WriteLine("updatedSize: {0}", parentTotalData.size);
                                    break;
                                }
                            }
                        }
                    }
                }

                //update the directoryWithAtMost100K based on the latest totalledFileByDirectory
                foreach (FileSystemData fsd in totalledFileByDirectory)
                {
                    // Console.WriteLine("update");
                    // Console.WriteLine("fsd -> level: {0} \t name: {1} \t type: {2} \t directory: {3} \t size {4}",
                    //      fsd.level, fsd.name, fsd.type, fsd.directory, fsd.size);

                    bool recordFound = false;

                    //cleanup the list, bottom up to avoid index out of bounds
                    if (fsd.size <= 100000)
                    {
                        //check if the directory already exists in directoryWithAtMost100K
                        //if exists, replace the size
                        for (int index3 = 0; index3 < directoryWithAtMost100K.Count; index3++)
                        {
                            if (fsd.directory.CompareTo(directoryWithAtMost100K[index3].directory) == 0)
                            {
                                recordFound = true;
                                directoryWithAtMost100K[index3].size = fsd.size;
                            }
                        }
                        //else, add to the list
                        if (!recordFound)
                        {
                            directoryWithAtMost100K.Add(new FileSystemData(fsd.level, fsd.name, fsd.type, fsd.directory, fsd.size));
                        }
                    }
                    else if (fsd.size > 100000)
                    {
                        for (int index2 = (directoryWithAtMost100K.Count - 1); index2 >= 0; index2--)
                        {
                            if (fsd.level == directoryWithAtMost100K[index2].level
                                && fsd.directory.CompareTo(directoryWithAtMost100K[index2].directory) == 0)
                            {
                                directoryWithAtMost100K.RemoveAt(index2);
                            }
                        }
                    }

                    // foreach (FileSystemData d100k in directoryWithAtMost100K)
                    // {
                    //     Console.WriteLine("d100k -> level: {0} \t name: {1} \t type: {2} \t directory: {3} \t size {4}",
                    //         d100k.level, d100k.name, d100k.type, d100k.directory, d100k.size);
                    // }
                }
            }
        }

        public static long getSumTotalOf100KDirectories(List<FileSystemData> directoryWithAtMost100K)
        {
            long output = 0;

            foreach (FileSystemData data in directoryWithAtMost100K)
            {
                output += data.size;
            }

            return output;
        }

        public static List<FileSystemData> getTotalledFileByDirectoryByLevel(List<FileSystemData> totalledFileByDirectory, int index)
        {
            List<FileSystemData> outputData = new List<FileSystemData>();

            foreach (FileSystemData item in totalledFileByDirectory)
            {
                if (index == item.level)
                {
                    outputData.Add(item);
                }
            }

            return outputData;
        }
    }

    public class FileSystemData
    {
        public int level;
        public string name;
        public string type;
        public string directory;
        public long size;

        public FileSystemData(int level, string name, string type, string directory, long size)
        {
            this.level = level;
            this.name = name;
            this.type = type;
            this.directory = directory;
            this.size = size;
        }
    }
}