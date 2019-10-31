using System;
using System.Collections.Generic;
using System.Text;

namespace BandsTest
{
    class Festival
    {
        //Class for the festival. Just needs to hold a name.

        private string festivalName;

        public string FestivalName
        {
            get
            {
                return festivalName;
            }
            set
            {
                festivalName = value;
            }     
        }
    }
}
