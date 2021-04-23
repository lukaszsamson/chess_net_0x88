using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTest
{
    class History
    {
        int[] counters, hit, total;
        public History()
        {
            counters = new int[0x40 * 128];
        }
        public void GoodMove(Move move, int depth)
        {
            
            var x = (int)move.Moved * 128 + move.To;
            counters[x] += depth * depth;
            if (counters[x] >= 16384)
            {
                var i = counters.Length;
                do
                {
                    --i;
                    counters[i] = (counters[i] + 1) >> 1;
                }
                while (i != 0);
            }
        }
        public int Value(Move move)
        {
            return counters[(int)move.Moved * 128 + move.To];
        }
    }
}

