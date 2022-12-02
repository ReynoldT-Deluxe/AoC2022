using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections.Specialized;

namespace FileApplication {
   class Program {
      static void Main(string[] args) {
        string dataLocation = "";
        
        //data to use
        //dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_1/sampleData.txt";
        dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_1/aocData.txt"; 

        //problem part
        //int partNum = 1;
        int partNum = 2;

            try {
                using (StreamReader sr = new StreamReader(dataLocation)) {
                    string data;
                    
                    double currentCalories = 0;
                    double maxCalories = 0;
                    int elfNum = 1;
                    int elfNumMax = 1;
                    double[] calories = new double[3];

                    while ((data = sr.ReadLine()) != null) {
                        //check data if it's blank 
                        if (data.Length != 0) {
                            currentCalories += Convert.ToDouble(data);

                        //means it is the end of the elf bag data
                        } else {
                            //part 1 solution
                            if (partNum == 1) {
                                if (currentCalories > maxCalories) {
                                    maxCalories = currentCalories;
                                    //set elfNum for maxCalories
                                    elfNumMax = elfNum;
                                } 

                            //part 2 solution
                            } else if (partNum == 2) {                                
                                //set the first three as the top 3
                                //add them to a list and sort ascending
                                if (elfNum <= 3) {
                                    if (elfNum == 1) {
                                        calories[0] = currentCalories;
                                    } else if (elfNum == 2) {
                                        calories[1] = currentCalories;
                                    } else if (elfNum == 3) {
                                        calories[2] = currentCalories;
                                    }
                                
                                } else {
                                    // foreach (double cal in calories) {
                                    //     Console.WriteLine("cal: {0}", cal);
                                    // }

                                    bool update = false;
                                    for (int cnt = 0; cnt < calories.Length; cnt++){
                                        double calData = calories[cnt];
                                        if (currentCalories.CompareTo(calData) == 1) {
                                           update = true;
                                        } 
                                    }  

                                    if (update) {
                                         //sort array and replace lowest with new data
                                         Array.Sort(calories);
                                         calories[0] = currentCalories;
                                    }                                   
                                }                           
                            }

                            //increment elfNum
                            elfNum++;
                            //reset currentCalories
                            currentCalories = 0;
                        }
                    }

                    if (partNum == 1) {
                        Console.WriteLine("elfNumMax: {0}", elfNumMax);
                        Console.WriteLine("maxCalories: {0}", maxCalories);
                    } else if (partNum == 2) {                        
                        foreach (double cal in calories) {
                            maxCalories += cal;
                        }
                        Console.WriteLine("Total calories of top 3 bags: {0}", maxCalories);
                    }

                    
                              
                }                    

            } catch (Exception e) {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
      }
   }
}