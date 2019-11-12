using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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


        public void LoadJson() //This function reads the JSON text from the given URL and parses the data to add to lists and display.
        {
            using (WebClient wc = new WebClient())
            {
                string json = "";
                do
                {
                    try
                    {
                        json = wc.DownloadString("http://eacodingtest.digital.energyaustralia.com.au/api/v1/festivals");
                    }
                    catch (WebException ex) { }

                } while (json.Equals("") || json.Equals("\"\""));
                List<ReadFestival> festivals = JsonConvert.DeserializeObject<List<ReadFestival>>(json);

                foreach (ReadFestival f in festivals)
                {
                    foreach (ReadBand b in f.Bands)
                    {
                        string s = b.RecordLabel;
                        if (s == null)
                        {
                            s = "Independant";
                        }
                        AddBandsFromList(f.Name, s, b.Name);
                    }
                }
            }
        }

        public static void Main(string[] args)
        {
            PrintList list = new PrintList();
            list.LoadJson();
            list.PrintBands();
        }
    }

    public class ReadFestival //Class for use in deserialising the JSON data
    {
        public string Name { get; set; }
        public List<ReadBand> Bands { get; set; }
    }

    public class ReadBand //Class for use in deserialising the JSON data
    {
        public string Name { get; set; }
        public string RecordLabel { get; set; }
    }
}
