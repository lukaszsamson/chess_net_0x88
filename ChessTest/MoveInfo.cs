using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTest
{
    class MoveInfo
    {
        public int Enpassant, M50;
        //public PieceType Captured;
        public bool WCR, WCL, BCL, BCR;
        public MoveInfo(Board board)
        {
            this.Enpassant = board.Enpassant;
            this.M50 = board.M50;
            //this.Captured = captured;
            this.WCL = board.WCL;
            this.WCR = board.WCR;
            this.BCL = board.BCL;
            this.BCR = board.BCR;
        }
    }
}
