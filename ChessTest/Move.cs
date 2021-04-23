using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTest
{
    class Move : IEquatable<Move>
    {
        public int From, To;
        public PieceType Prom;
        public PieceType Moved;
        public PieceType Captured;
        public Move(int from, int to, PieceType prom, PieceType moved, PieceType captured)
        {
            this.From = from;
            this.To = to;
            this.Prom = prom;
            this.Moved = moved;
            this.Captured = captured;
        }
        public int CaptureVale()
        {
            return Values.PIECEVALUESABS(this.Captured) - Values.PIECEVALUESABS(this.Moved) + Values.PIECEVALUESABS(this.Prom);
        }
        public static Dictionary<string, int> string2int;
        public static string[] int2string;
        static Move()
        {
            int2string = new string[128];
            string2int = new Dictionary<string, int>();
            for (int i = 0; i < 8; i++)
            {
                int2string[i * 16 + 0] = "a" + (i + 1).ToString();
                int2string[i * 16 + 1] = "b" + (i + 1).ToString();
                int2string[i * 16 + 2] = "c" + (i + 1).ToString();
                int2string[i * 16 + 3] = "d" + (i + 1).ToString();
                int2string[i * 16 + 4] = "e" + (i + 1).ToString();
                int2string[i * 16 + 5] = "f" + (i + 1).ToString();
                int2string[i * 16 + 6] = "g" + (i + 1).ToString();
                int2string[i * 16 + 7] = "h" + (i + 1).ToString();
            }
            for (int i = 0; i < 128; i++)
                if (!string.IsNullOrEmpty(int2string[i]))
                    string2int.Add(int2string[i], i);
        }

        public static string PromotionString(PieceType p)
        {
            switch (p)
            {
                case PieceType.WQ:
                case PieceType.BQ:
                    return "Q";
                case PieceType.WR:
                case PieceType.BR:
                    return "R";
                case PieceType.WB:
                case PieceType.BB:
                    return "B";
                case PieceType.WN:
                case PieceType.BN:
                    return "N";
                default:
                    return "";
            }
        }

        public override string ToString()
        {
            return int2string[From] + int2string[To] + PromotionString(Prom);
        }

        #region IEquatable<Move> Members

        public bool Equals(Move other)
        {
            return other != null && this.From == other.From && this.To == other.To && this.Prom == other.Prom && this.Moved == other.Moved && this.Captured == other.Captured;
        }

        #endregion
    }
}



