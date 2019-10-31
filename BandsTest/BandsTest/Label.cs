using System;
using System.Collections.Generic;
using System.Text;

namespace BandsTest
{
    class Label
    {
        //Class for the label. Has a name and a list of signed bands.
        private string labelName;
        private List<Band> bands = new List<Band>();

        public string LabelName
        {
            get
            {
                return labelName;
            }
            set
            {
                labelName = value;
            }

        }

        public void AddBand(string bName) //Function to add band to list. Checks if the passed band name already exists first.
        { 
            bool doesContain = false;
            foreach (Band b in bands)
            {
                if (b.BandName == bName)
                {
                    doesContain = true;
                }
            }

            if (!doesContain)
            {
                Band newBand = new Band
                {
                    BandName = bName
                };
                bands.Add(newBand);
            }
        }

        public List<Band> ReturnBands()
        {
            if(bands.Count != 0)
            {
                return bands;
            }
            else
            {
                return null;
            }
        }

    }
}
