using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTest
{
    enum Piece
    {
        O = 0x00,
        P = 0x01,
        N = 0x02,
        B = 0x04,
        R = 0x08,
        Q = 0x0C,
        K = 0x03,
    }

    enum PieceType
    {
        OO = 0x00,
        WP = 0x11,
        WN = 0x12,
        WB = 0x14,
        WR = 0x18,
        WQ = 0x1C,
        WK = 0x13,
        BP = 0x21,
        BN = 0x22,
        BB = 0x24,
        BR = 0x28,
        BQ = 0x2C,
        BK = 0x23,
    }

    enum Color
    {
        O = 0x00,
        W = 0x10,
        B = 0x20,
    }
    class MovegenDirs
    {
        public static int[] RDirs = { -16, 16, -1, 1 };
        public static int[] BDirs = { -17, 17, -15, 15 };
        public static int[] QDirs = { -17, -16, -15, 1, 17, 16, 15, -1 };
        public static int[] NDirs = { 14, 31, 33, 18, -14, -31, -33, -18 };
        public static int[] WPC = { 15, 17 };
        public static int WPM = 16;
        public static int[] BPC = { -17, -15 };
        public static int BPM = -16;
    }

    enum NodeType
    {
        PV = 0x00,
        CUT = 0x01,
        ALL = 0x02,
    }

    class Values
    {
        public const int NONE = -1024 * 2048;
        public const int INF = 1024 * 1024;
        public const int MATE = 1024 * 128;
        public const int DRAW = 0;
        public const int WINDOW = 20;
        public static int PIECEVALUES(PieceType piece)
        {
            switch (piece)
            {
                case PieceType.WP:
                    return 100;
                case PieceType.BP:
                    return -100;
                case PieceType.WN:
                case PieceType.WB:
                    return 300;
                case PieceType.BN:
                case PieceType.BB:
                    return -300;
                case PieceType.WR:
                    return 500;
                case PieceType.BR:
                    return -500;
                case PieceType.WQ:
                    return 900;
                case PieceType.BQ:
                    return -900;
                default:
                    return 0;
            }
        }
        public static int PIECEVALUESABS(PieceType piece)
        {
            switch (piece)
            {
                case PieceType.WP:
                case PieceType.BP:
                    return 100;
                case PieceType.WN:
                case PieceType.WB:
                case PieceType.BN:
                case PieceType.BB:
                    return 300;
                case PieceType.WR:
                case PieceType.BR:
                    return 500;
                case PieceType.WQ:
                case PieceType.BQ:
                    return 900;
                default:
                    return 0;
            }
        }
        
    }
    class Settings
    {
        public const int MoveListSize = 128;
    }
}
