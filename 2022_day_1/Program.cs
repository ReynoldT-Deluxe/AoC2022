using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections.Specialized;

namespace FileApplication {
   class Program {
       public static double processData(int fishCnt, int checkDays, int lanternFishLife) {
            int zeroCnt1; 
            List<double> newFishList1 = new List<double>();
            //Console.WriteLine("newFishList1.Count: {0}", newFishList1.Count);   
            //Console.WriteLine("Fish count after 1 days: {0}", newFishList1.Count);

            for (int day = 1; day <= checkDays; day++) {
                zeroCnt1 = 0;
                //set data for day 1
                if (day == 1){
                    newFishList1.Add(lanternFishLife);
                }

                double[] arr2 = new double[newFishList1.Count];
                newFishList1.CopyTo(arr2); 
                
                for (int x = 0; x < arr2.Length; x++)  {
                    double info1  =  new double();
                    info1 = arr2[x];
                    //check if zero count
                    if (info1 != 0) {
                        //set data to lower by 1
                        info1--;
                    } else if (info1 == 0) {
                        //reset data to 6
                        info1 = 6;
                        zeroCnt1++;
                    }
                    //set the data back to the list
                    arr2[x] = info1;
                }

                //convert array to list
                newFishList1 = arr2.ToList();
                //add a new lanternfish with 8 days
                if (zeroCnt1 > 0) {
                    for (int zC = 0; zC < zeroCnt1; zC++) {
                        newFishList1.Add(8);
                    }
                }
                Console.WriteLine("Fish count for fish {0} after {1} days: {2}", fishCnt, day, newFishList1.Count);
            }

           return newFishList1.Count;
       }
      static void Main(string[] args) {
          
            try {
                using (StreamReader sr = new StreamReader("/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_1/data.txt")) {
                    string data;
                    
                    double currentCalories = 0;
                    double maxCalories = 0;
                    int elfNum = 1;
                    int elfNumMax = 1;

                    while ((data = sr.ReadLine()) != null) {
                        //Console.WriteLine("data: {0}", data);
                        //check data if it's blank 
                        if (data.Length != 0) {
                            currentCalories += Convert.ToDouble(data);
                        //means it is the end of the elf bag data
                        } else {
                            if (currentCalories > maxCalories) {
                                maxCalories = currentCalories;
                                //set elfNum for maxCalories
                                elfNumMax = elfNum;
                            } 
                            //increment elfNum
                            elfNum++;
                            //reset currentCalories
                            currentCalories = 0;
                        }
                        
                        //Console.WriteLine("elfNum: {0}", elfNum);
                        //Console.WriteLine("elfNumMax: {0}", elfNumMax);
                        //Console.WriteLine("currentCalories: {0}", currentCalories);
                        //Console.WriteLine("maxCalories: {0}", maxCalories);
                    }

                    Console.WriteLine("elfNumMax: {0}", elfNumMax);
                    Console.WriteLine("maxCalories: {0}", maxCalories);
                              
                }                    

            } catch (Exception e) {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
      }
   }
}