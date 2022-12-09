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
        //dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_2/sampleData.txt";
        dataLocation = "/Users/T452172/Documents/Personal/Advent_of_Code/2022/AoC2022/2022_day_2/aocData.txt"; 

        //problem part
        //int partNum = 1;
        int partNum = 2;

            try {
                using (StreamReader sr = new StreamReader(dataLocation)) {
                    int score = 0;
                    string data;
                    
                    while ((data = sr.ReadLine()) != null) {
                        //Console.WriteLine("data: {0}", data);
                        //part 1 solution
                        if (partNum == 1) {
                            //find the combination
                            //rock-rock; score + draw; 1 + 3
                            if (data.CompareTo("A X") == 0) {
                                score += 4;
                            //rock-paper; score + won; 2 + 6
                            } else if (data.CompareTo("A Y") == 0) {
                                score += 8;
                            //rock-scissor; score + lost; 3 + 0
                            } else if (data.CompareTo("A Z") == 0) {
                                score += 3;
                            //paper-rock; score + lost; 1 + 0
                            } else if (data.CompareTo("B X") == 0) {
                                score += 1;
                            //paper-paper; score + draw; 2 + 3
                            } else if (data.CompareTo("B Y") == 0) {
                                score += 5;
                            //paper-scissor; score + won; 3 + 6
                            } else if (data.CompareTo("B Z") == 0) {
                                score += 9;
                            //scissor-rock; score + won; 1 + 6
                            } else if (data.CompareTo("C X") == 0) {
                                score += 7;
                            //scissor-paper; score + lost; 2 + 0
                            } else if (data.CompareTo("C Y") == 0) {
                                score += 2;
                            //scissor-scissor; score + draw; 3 + 3
                            } else if (data.CompareTo("C Z") == 0) {
                                score += 6;
                            }

                        //part 2 solution
                        } else if (partNum == 2) {                                
                           //find the combination
                            //rock; lost + scissor; 0 + 3
                            if (data.CompareTo("A X") == 0) {
                                score += 3;
                            //rock; draw + rock; 3 + 1
                            } else if (data.CompareTo("A Y") == 0) {
                                score += 4;
                            //rock; won + paper; 6 + 2
                            } else if (data.CompareTo("A Z") == 0) {
                                score += 8;
                            //paper; lost + rock; 0 + 1
                            } else if (data.CompareTo("B X") == 0) {
                                score += 1;
                            //paper; draw + paper; 3 + 2
                            } else if (data.CompareTo("B Y") == 0) {
                                score += 5;
                            //paper; won + scissor; 6 + 3
                            } else if (data.CompareTo("B Z") == 0) {
                                score += 9;
                            //scissor; lost + paper; 0 + 2
                            } else if (data.CompareTo("C X") == 0) {
                                score += 2;
                            //scissor; draw + scissor; 3 + 3
                            } else if (data.CompareTo("C Y") == 0) {
                                score += 6;
                            //scissor; won + rock; 6 + 1
                            } else if (data.CompareTo("C Z") == 0) {
                                score += 7;
                            }  
                        }
                    }

                    Console.WriteLine("score: {0}", score);
                }                    

            } catch (Exception e) {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
      }
   }
}