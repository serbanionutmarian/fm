using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Schedule
{
    public class Round<T>
    {
        public class Match<T>
        {
            public T Home;

            public T Away;
        }

        public Match<T>[] Matches;
    }
}
