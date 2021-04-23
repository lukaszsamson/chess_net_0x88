using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTest
{
    class HashTable
    {
        public class HashEntry
        {
            public int value;
            public int key;
            public int depth;
            public int age;
            public Move move;
            public Flag flag;
        }
        [Flags]
        public enum Flag
        {
            Unknown = 0x0,
            Exact = 0x3,
            Lower = 0x2,
            Upper = 0x1
        }
        HashEntry[][] table;
        int count;
        public HashTable(int count)
        {
            this.count = count;
            this.table = new HashEntry[2][];
            for (var j = 0; j < 2; j++)
            {
                this.table[j] = new HashEntry[count];
                for (var i = 0; i < count; i++)
                    this.table[j][i] = new HashEntry();
            }
        }
        public void Store(int key, Move move, int value, int depth, int age, Flag flag)
        {
            var index = key % this.count;
                var entry = table[0][index];
                if (entry.key == key)
                {
                    if (entry.depth <= depth)
                    {
                        entry.depth = depth;
                        entry.value = value;
                        entry.move = move;
                        entry.flag = flag;
                    }
                    return;
                }
                else
                {
                    var entry2 = table[1][index];
                    if (entry.depth <= depth || entry.age + 6 <= age)
                    {
                        entry2.key = entry.key;
                        entry2.value = entry.value;
                        entry2.depth = entry.depth;
                        entry2.age = entry.age;
                        entry2.move = entry.move;
                        entry2.flag = entry.flag;

                        entry.key = key;
                        entry.value = value;
                        entry.depth = depth;
                        entry.age = age;
                        entry.move = move;
                        entry.flag = flag;
                    }
                    else
                    {
                        entry2.key = key;
                        entry2.value = value;
                        entry2.depth = depth;
                        entry2.age = age;
                        entry2.move = move;
                        entry2.flag = flag;
                    }
                }
            
        }
        public HashEntry Read(int key)
        {
            var index = key % this.count;
                if (table[0][index].key == key)
                    return table[0][index];
                if (table[1][index].key == key)
                    return table[1][index];
            return null;
        }
    }
}
