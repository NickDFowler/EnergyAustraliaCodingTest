using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BandsTest
{
    class PrintList
    {
        List<Label> labels = new List<Label>();

        public void AddBandsFromList(string fName, string lName, string bName) //This function adds a new entry for the passed Label, Band, and Festival
        {
            bool doesContain = false; //Check if the label already exists. If it doesn't, make a new one
            foreach (Label l in labels)
            {
                if (l.LabelName == lName)
                {
                    doesContain = true;
                }
            }

            if (!doesContain)
            {
                Label newLabel = new Label
                {
                    LabelName = lName
                };
                labels.Add(newLabel);
            }

            foreach (Label l in labels) //Find the label to add the band, then find the band to add the festival
            {
                if (l.LabelName == lName)
                {
                    l.AddBand(bName);

                    foreach (Band b in l.ReturnBands())
                    {
                        if (b.BandName == bName)
                        {
                            b.AddFestival(fName);
                            break;
                        }
                    }

                    break;
                }
            }
        }


        public void PrintBands() //Funtion to print out the band information, in order of Label, Band, Festival
        {
            foreach (Label l in labels)
            {
                Console.WriteLine(l.LabelName);
                foreach (Band b in l.ReturnBands())
                {
                    Console.WriteLine("\t" + b.BandName);
                    if (b.ReturnFestivals().Count > 0)
                    {
                        foreach (Festival f in b.ReturnFestivals())
                        {
                            Console.WriteLine("\t\t" + f.FestivalName);
                        }
                    }
                }
            }
        }

        public string ConcatenateName(string[] s) //Concatenate names with multiple words, add spaces, then trim extra characters
        {
            string name = "";
            for (int i = 1; i < s.Length; i++)
            {
                name += s[i] + " ";
            }
            name = name.Trim('"', ' ');
            return name;

        }

        public void LoadJson() //This function reads in the JSON file and parses the data to add to lists and display.
        {
            char[] delimiters = { ' ', ',', '\n', ':', };
            using (StreamReader sr = new StreamReader("BandsAPI.json"))
            {
                string currentLine = "";
                bool isFest = false;
                bool noFest = false;
                string newFest = "";
                string newLabel = "";
                string newBand = "";
                string[] lineSplit;
                do
                {
                    currentLine = sr.ReadLine();
                    lineSplit = currentLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (lineSplit.Length > 1)
                    {
                        //Check if current line has data or is just a single character
                        if (lineSplit[0] == "\"name\"" && !isFest && !noFest)
                        {
                            /* If the first word is "name", and it's neither already within festival block, or within a block of
                             * bands with no festival, then the current line denotes a festival. Add the name to the festival
                             * variable and set the festival status to true */

                            string s = ConcatenateName(lineSplit);
                            newFest = s;
                            isFest = true;
                        }
                        else if ((lineSplit[0] == "\"name\"" && isFest) || (lineSplit[0] == "\"name\"" && noFest))
                        {
                            /* If the first word is "name", and it's either already within festival block, or within a block of 
                             * bands with no festival, then the current line denotes a band. Add the name to the band variable */

                            string s = ConcatenateName(lineSplit);
                            if (s == "")
                            {
                                s = "No band shown";
                            }
                            newBand = s;
                        }
                        else if (lineSplit[0] == "\"recordLabel\"")
                        {
                            /* If the first word is "recordLabel", then the current line denotes a record label. Add the name
                             * to the label variable then add the new data to the respective lists, as the label always 
                             * appears last */
                            string s = ConcatenateName(lineSplit);
                            if (s == "")
                            {
                                s = "Independant";
                            }
                            newLabel = s;
                            AddBandsFromList(newFest, newLabel, newBand);
                        }
                        else if (lineSplit[0] == "\"bands\"" && !isFest)
                        {
                            /* If the first word is "recordLabel", and it appears outside a festival block, it denotes a block
                             * of bands with no festival. Change the required variable */
                            noFest = true;
                        }
                    }
                    else if (lineSplit[0] == "]")
                    {
                        //This character only appears at the end of a block of bands, so use it to clear all variables
                        isFest = false;
                        noFest = false;
                        newFest = "";
                        newLabel = "";
                        newBand = "";
                    }
                } while (!sr.EndOfStream);
            }
        }

        public static void Main(string[] args)
        {
            PrintList list = new PrintList();
            list.LoadJson();
            list.PrintBands();
        }
    }
}
