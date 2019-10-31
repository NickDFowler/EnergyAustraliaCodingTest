using System;
using System.Collections.Generic;
using System.Text;

namespace BandsTest
{
    class Band
    {
        //Class for the band. Has a name and a list of attended festivals.

        private string bandName;
        private List<Festival> festivals = new List<Festival>();

        public string BandName
        {
            get
            {
                return bandName;
            }
            set
            {
                bandName = value;
            }
        }

        public void AddFestival(string fName) //Function to add festival to list. Checks if the passed festival name already exists first.
        {
            bool doesContain = false;
            foreach (Festival f in festivals)
            {
                if (f.FestivalName == fName)
                {
                    doesContain = true;
                }
            }

            if (!doesContain)
            {
                Festival newFest = new Festival
                {
                    FestivalName = fName
                };
                festivals.Add(newFest);
            }
        }

        public List<Festival> ReturnFestivals()
        {
            if (festivals.Count != 0)
            {
                return festivals;
            }
            else
            {
                return null;
            }
        }
    }
}
