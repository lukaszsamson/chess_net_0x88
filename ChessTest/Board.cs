using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTest
{
    partial class Board : ICloneable, IEquatable<Board>
    {
        public const int MAX_HEIGHT = 100;
        PieceType[] Data;
        public bool WCR, WCL, BCR, BCL, WM;
        public int Enpassant, M50, BKSquare, WKSquare;
        public Move[] Killers1;
        public Move[] Killers2;
        public Move[] Killers3;
        public Move[] Killers4;
        public int[] CodesStack;
        public int height;
        public int material;
        public MoveInfo[] stack;
        public int Hash;
        public int age;
        public CheckState[] inCheck;
        public int[][] attackers;
        public Dirs[] pins;
        public List<Move>[] movelists;
        public List<Move>[] captureslists;
        public List<Move>[] quietlists;
        public History history;
        public enum CheckState
        {
            NotChecked = 0x00,
            No = 0x01,
            White = 0x10,
            Black = 0x20,
            Check = 0x30,
        }
        public Board()
        {
            this.movelists = new List<Move>[MAX_HEIGHT];
            this.captureslists = new List<Move>[MAX_HEIGHT];
            this.quietlists = new List<Move>[MAX_HEIGHT];
            for (var i = 0; i < MAX_HEIGHT; i++)
            {
                this.movelists[i] = new List<Move>();
                this.quietlists[i] = new List<Move>();
                this.captureslists[i] = new List<Move>();
            }
            this.Killers1 = new Move[MAX_HEIGHT];
            this.Killers2 = new Move[MAX_HEIGHT];
            this.Killers3 = new Move[MAX_HEIGHT];
            this.Killers4 = new Move[MAX_HEIGHT];
            this.stack = new MoveInfo[MAX_HEIGHT];
            this.Data = new PieceType[128];
            this.CodesStack = new int[256];
            this.inCheck = new CheckState[MAX_HEIGHT];
            this.history = new History();
            //this.attackers = new int[MAX_HEIGHT][];
            
        }
        public void Reset()
        {
            this.material = 0;
            this.height = 0;
            this.age = 0;
            this.inCheck[0] = CheckState.No;
            //this.attackers[0] = new int[0];
            for (var i = 0; i < 8; ++i)
            {
                this.Data[i + 16] = PieceType.WP;
                this.Data[i + 96] = PieceType.BP;
                for (var j = 2; j < 6; ++j)
                    this.Data[j * 16 + i] = 0;
                for (var j = 0; j < 8; ++j)
                    this.Data[8 + i + 16 * j] = 0;
            }
            this.Data[0] = PieceType.WR;
            this.Data[1] = PieceType.WN;
            this.Data[2] = PieceType.WB;
            this.Data[3] = PieceType.WQ;
            this.Data[4] = PieceType.WK;
            this.Data[5] = PieceType.WB;
            this.Data[6] = PieceType.WN;
            this.Data[7] = PieceType.WR;

            this.Data[112] = PieceType.BR;
            this.Data[113] = PieceType.BN;
            this.Data[114] = PieceType.BB;
            this.Data[115] = PieceType.BQ;
            this.Data[116] = PieceType.BK;
            this.Data[117] = PieceType.BB;
            this.Data[118] = PieceType.BN;
            this.Data[119] = PieceType.BR;

            this.WKSquare = 4;
            this.BKSquare = 116;

            this.Enpassant = 128;

            this.WCL = true;
            this.WCR = true;
            this.BCL = true;
            this.BCR = true;

            this.M50 = 0;

            this.WM = true;
            this.CalculateHash();
            this.CodesStack[0] = this.Hash;
        }
        public void GoodMove(Move move, int depth)
        {
            if ((move.Captured | move.Prom) != 0)
                return;

            this.history.GoodMove(move, depth);
        }
        public void AddKiller(Move move)
        {
            //if ((move.Captured | move.Prom) != 0)
            //    return;
#warning Killers sholud be stored after new position
            var h = this.height;
            if (!move.Equals(this.Killers1[h]))
            {
                if (move.Equals(this.Killers2[h]))
                {
                    this.Killers2[h] = this.Killers1[h];
                    this.Killers1[h] = move;
                }
                else
                {
                    if (move.Equals(this.Killers3[h]))
                    {
                        this.Killers3[h] = this.Killers2[h];
                        this.Killers2[h] = this.Killers1[h];
                        this.Killers1[h] = move;
                    }
                    else
                    {
                        this.Killers4[h] = this.Killers4[h];
                        this.Killers3[h] = this.Killers2[h];
                        this.Killers2[h] = this.Killers1[h];
                        this.Killers1[h] = move;
                    }
                }
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 16; j++)
                    sb.Append(this.Data[i * 16 + j]).Append(" ");
                sb.Append("\n");
            }
            return sb.ToString();
        }
        public bool IsDraw()
        {
            var i = this.height + this.age;
            var key = this.CodesStack[i];
            var t = 2;
            for (var k = i - 2; k >= 0; k -= 2)
                if (this.CodesStack[k] == key)
                    if (--t == 0)
                        return true;
            return false;

        }

        bool TestFields(int from, int to, int d)
        {
            for (var i = from + d; i != to; i += d)
                if (this.Data[i] != 0)
                    return false;
            return true;
        }
        public bool IsMoveLegal(Move move)
        {
            if (move.Moved != this.Data[move.From])
                return false;
            if (move.Captured != this.Data[move.To])
                return move.To == this.Enpassant && (move.Moved == PieceType.WP && move.Captured == PieceType.BP || move.Moved == PieceType.BP && move.Captured == PieceType.WP);
            var d = move.To - move.From;
            switch (move.Moved)
            {
                case PieceType.WP:
                    if (move.To - move.From == 32)
                        return this.Data[move.To] == 0 && this.Data[move.To - 16] == 0;
                    else if (move.To - move.From == 16)
                        return this.Data[move.To] == 0;
                    else
                        return true;
                case PieceType.BP:
                    if (move.To - move.From == -32)
                        return this.Data[move.To] == 0 && this.Data[move.To + 16] == 0;
                    else if (move.To - move.From == -16)
                        return this.Data[move.To] == 0;
                    else
                        return true;
                case PieceType.WR:
                case PieceType.BR:
                    if (d > 0)
                    {
                        if (d % 16 == 0)
                            return TestFields(move.From, move.To, 16);
                        else
                            return TestFields(move.From, move.To, 1);
                    }
                    else
                    {
                        if (d % 16 == 0)
                            return TestFields(move.From, move.To, -16);
                        else
                            return TestFields(move.From, move.To, -1);
                    }
                case PieceType.WB:
                case PieceType.BB:
                    if (d > 0)
                    {
                        if (d % 17 == 0)
                            return TestFields(move.From, move.To, 17);
                        else
                            return TestFields(move.From, move.To, 15);
                    }
                    else
                    {
                        if (d % 17 == 0)
                            return TestFields(move.From, move.To, -17);
                        else
                            return TestFields(move.From, move.To, -15);
                    }
                case PieceType.WQ:
                case PieceType.BQ:
                    if (d > 0)
                    {
                        if (d % 17 == 0)
                            return TestFields(move.From, move.To, 17);
                        else if(d % 15 == 0)
                            return TestFields(move.From, move.To, 15);
                        else if (d % 16 == 0)
                            return TestFields(move.From, move.To, 16);
                        else
                            return TestFields(move.From, move.To, 1);
                    }
                    else
                    {
                        if (d % 17 == 0)
                            return TestFields(move.From, move.To, -17);
                        else if (d % 15 == 0)
                            return TestFields(move.From, move.To, -15);
                        else if (d % 16 == 0)
                            return TestFields(move.From, move.To, -16);
                        else
                            return TestFields(move.From, move.To, -1);
                    }
                case PieceType.WK:
                    if (d == 2)
                        return this.WCR && this.Data[5] == 0 && this.Data[6] == 0 && !this.Attacked(4, false) && !this.Attacked(5, false) && !this.Attacked(6, false);
                    else if (d == -2)
                        return this.WCL && this.Data[3] == 0 && this.Data[2] == 0 && this.Data[1] == 0 && !this.Attacked(4, false) && !this.Attacked(3, false) && !this.Attacked(2, false);
                    else
                        return !this.Attacked(move.To, false);
                case PieceType.BK:
                    if (d == 2)
                        return this.BCR && this.Data[117] == 0 && this.Data[118] == 0 && !this.Attacked(116, true) && !this.Attacked(117, true) && !this.Attacked(118, true);
                    else if (d == -2)
                        return this.BCL && this.Data[115] == 0 && this.Data[114] == 0 && this.Data[113] == 0 && !this.Attacked(116, true) && !this.Attacked(115, true) && !this.Attacked(114, true);
                    else
                        return !this.Attacked(move.To, true);
                default:
                    return true;
            }
        }
        public Move GetKiller1()
        {
            return this.Killers1[this.height];
        }
        public Move GetKiller2()
        {
            return this.Killers2[this.height];
        }
        public Move GetKiller3()
        {
            return this.Killers3[this.height];
        }
        public Move GetKiller4()
        {
            return this.Killers4[this.height];
        }
        public Move GetMove(string moveString)
        {
            int length = moveString.Length;
            if (length != 4 && length != 5)
                throw new Exception("Invalid move string length");
            string from = moveString.Substring(0, 2);
            string to = moveString.Substring(2, 2);
            if (!Move.string2int.ContainsKey(from) || !Move.string2int.ContainsKey(to))
                throw new Exception("Unrecognized squere name");
            int From = Move.string2int[from];
            int To = Move.string2int[to];

            if (this.Data[From] == 0)
                throw new Exception("No piece to move.");

            PieceType Prom = PieceType.OO;
            if (moveString.Length == 5)
            {
                if (To < 8)
                {
                    switch (moveString[4])
                    {
                        case 'q': Prom = PieceType.BQ; break;
                        case 'r': Prom = PieceType.BR; break;
                        case 'b': Prom = PieceType.BB; break;
                        case 'n': Prom = PieceType.BN; break;
                        default: throw new Exception("Unrecognized promotion");
                    }
                }
                else if (To >= 112)
                {
                    switch (moveString[4])
                    {
                        case 'q': Prom = PieceType.WQ; break;
                        case 'r': Prom = PieceType.WR; break;
                        case 'b': Prom = PieceType.WB; break;
                        case 'n': Prom = PieceType.WN; break;
                        default: throw new Exception("Unrecognized promotion");
                    }
                }
            }
            return new Move(From, To, Prom, this.Data[From], To == this.Enpassant ? (this.WM ? PieceType.BP : PieceType.WP) : this.Data[To]);
        }
        public void PushMoveInfo()
        {
            this.stack[this.height] = new MoveInfo(this);
        }

        public void Make(Move move, bool ageBoard)
        {
            var To = move.To;
            var From = move.From;
            var moved = move.Moved;
            var Captured = move.Captured;
            var Prom = move.Prom;
#if DEBUG
            if ((From & 0x88) != 0 || (To & 0x88) != 0)
                throw new Exception("Illegel position");
            if (moved == 0)
                throw new Exception("No piece to move");
            var colorMask = (int)(this.WM ? Color.W : Color.B);
            if (((int)moved & colorMask) == 0)
                throw new Exception("Moving opposite color");
            //if (moved != move.Moved || (target != move.Captured && move.To != this.Enpassant))
            //    throw new Exception("Pieces does not match");
            if (((int)moved & (int)Captured & 0xF0) != 0)
                throw new Exception("Capturing own color");
            if (((int)Captured & 0x0F) == (int)Piece.K)
                throw new Exception("King capture");
#endif

            ++this.M50;
            this.WM = !this.WM;
            this.Data[To] = moved;
            this.Data[From] = 0;
            var OldEnpassant = this.Enpassant;
            this.Enpassant = 128;
            var Hash = this.Hash ^ hWM ^ hashCodes[(int)moved][From] ^ hashCodes[(int)moved][To] ^ (OldEnpassant == 128 ? 0 : hEnpassant[OldEnpassant % 16]);
            
            
            if (Captured != 0)
            {
                this.material -= Values.PIECEVALUES(Captured);
                this.M50 = 0;
                Hash ^= hashCodes[(int)Captured][To];
                if (Captured == PieceType.WR)
                {
                    if (To == 0 && this.WCL)
                    {
                        this.WCL = false;
                        Hash ^= hWLC;
                    }
                    else if (To == 7 && this.WCR)
                    {
                        this.WCR = false;
                        Hash ^= hWRC;
                    }
                }
                else if (Captured == PieceType.BR)
                {
                    if (To == 112 && this.BCL)
                    {
                        this.BCL = false;
                        Hash ^= hBLC;
                    }
                    else if (To == 119 && this.BCR)
                    {
                        this.BCR = false;
                        Hash ^= hBRC;
                    }
                }
            }

            switch (moved)
            {
                case PieceType.WP:
                    if (To == OldEnpassant)
                    {
                        var l = To - 16;
                        this.Data[l] = 0;
                        Hash ^= hashCodes[(int)PieceType.BP][To] ^ hashCodes[(int)PieceType.BP][l];
                    }
                    else if (To - From == 32)
                    {
                        var l = To - 16;
                        this.Enpassant = l;
                        Hash ^= hEnpassant[l % 16];
                    }
                    else if (Prom != 0)
                    {
                        this.Data[To] = Prom;
                        this.material += Values.PIECEVALUES(Prom) - Values.PIECEVALUES(PieceType.WP);
                        this.M50 = 0;
                        Hash ^= hashCodes[(int)Prom][To] ^ hashCodes[(int)PieceType.WP][To];
                    }
                    break;
                case PieceType.BP:
                    if (To == OldEnpassant)
                    {
                        var l = To + 16;
                        this.Data[l] = 0;
                        Hash ^= hashCodes[(int)PieceType.WP][To] ^ hashCodes[(int)PieceType.WP][l];
                    }
                    if (To - From == -32)
                    {
                        var l = To + 16;
                        this.Enpassant = l;
                        Hash ^= hEnpassant[l % 16];
                    }
                    else if (Prom != 0)
                    {
                        this.Data[To] = Prom;
                        this.material += Values.PIECEVALUES(Prom) - Values.PIECEVALUES(PieceType.BP);
                        this.M50 = 0;
                        Hash ^= hashCodes[(int)Prom][To] ^ hashCodes[(int)PieceType.BP][To];
                    }
                    break;
                case PieceType.WK:
                    this.WKSquare = To;
                    if (To - From == 2)
                    {
                        this.Data[5] = PieceType.WR;
                        this.Data[7] = 0;
                        Hash ^= hashCodes[(int)PieceType.WR][5] ^ hashCodes[(int)PieceType.WR][7];
                    }
                    else if (To - From == -2)
                    {
                        this.Data[3] = PieceType.WR;
                        this.Data[0] = 0;
                        Hash ^= hashCodes[(int)PieceType.WR][3] ^ hashCodes[(int)PieceType.WR][0];
                    }
                    if (this.WCL)
                    {
                        Hash ^= hWLC;
                        this.WCL = false;
                    }
                    if (this.WCR)
                    {
                        Hash ^= hWRC;
                        this.WCR = false;
                    }                    
                    break;
                case PieceType.BK:
                    this.BKSquare = To;
                    if (To - From == 2)
                    {
                        this.Data[117] = PieceType.BR;
                        this.Data[119] = 0;
                        Hash ^= hashCodes[(int)PieceType.BR][117] ^ hashCodes[(int)PieceType.BR][119];
                    }
                    else if (To - From == -2)
                    {
                        this.Data[115] = PieceType.BR;
                        this.Data[112] = 0;
                        Hash ^= hashCodes[(int)PieceType.BR][115] ^ hashCodes[(int)PieceType.BR][112];
                    }
                    if (this.BCL)
                    {
                        Hash ^= hBLC;
                        this.BCL = false;
                    }
                    if (this.BCR)
                    {
                        Hash ^= hBRC;
                        this.BCR = false;
                    }
                    break;
                case PieceType.WR:
                    if (From == 0 && this.WCL)
                    {
                        Hash ^= hWLC;
                        this.WCL = false;
                    }
                    else if (From == 7 && this.WCR)
                    {
                        Hash ^= hWRC;
                        this.WCR = false;
                    }
                    break;
                case PieceType.BR:
                    if (From == 112 && this.BCL)
                    {
                        Hash ^= hBLC;
                        this.BCL = false;
                    }
                    else if (From == 119 && this.BCR)
                    {
                        Hash ^= hBRC;
                        this.BCR = false;
                    }
                    break;
                default:
                    break;
            }
            if (ageBoard)
                ++this.age;
            else
                ++this.height;
            this.CodesStack[this.height + this.age] = this.Hash = Hash;

#if DEBUG
            if (this.Data[this.WKSquare] != PieceType.WK || this.Data[this.BKSquare] != PieceType.BK)
                throw new Exception("Error in king make move");
            this.CalculateHash();
            if (this.Hash != Hash)
                throw new Exception("Error in hash");
#endif
        }
        public void Unmake(Move move)//366429 14383658
        {
            var To = move.To;
            var From = move.From;
            var moved = move.Moved;
            var Captured = move.Captured;
            var Prom = move.Prom;
#if DEBUG
            if ((From & 0x88) != 0 || (To & 0x88) != 0)
                throw new Exception("Illegel position");
            if (moved == 0)
                throw new Exception("No piece to move");
            var colorMask = (int)(this.WM ? Color.B : Color.W);
            if (((int)moved & colorMask) == 0)
                throw new Exception("Moving opposite color");
            //if (moved != move.Moved && moved != move.Prom || (target != move.Captured/* && move.To != this.Enpassant*/))
             //   throw new Exception("Pieces does not match");
            if (((int)moved & (int)Captured & 0xF0) != 0)
                throw new Exception("Capturing own color");
            if (((int)Captured & 0x0F) == (int)Piece.K)
                throw new Exception("King capture");
#endif
            var info = this.stack[--this.height];
            var OldEnpassant = info.Enpassant;
            var Enpassant = this.Enpassant;
            var Hash = this.Hash ^ hWM ^ hashCodes[(int)moved][From] ^ hashCodes[(int)moved][To] ^ (OldEnpassant == 128 ? 0 : hEnpassant[OldEnpassant % 16]) ^ (Enpassant == 128 ? 0 : hEnpassant[Enpassant % 16]);
            if (Captured != 0)
            {
                this.material += Values.PIECEVALUES(Captured);
                Hash ^= hashCodes[(int)Captured][To];
            }
            

            
            this.Enpassant = OldEnpassant;
            this.M50 = info.M50;
            if (this.WCL != info.WCL)
                Hash ^= hWLC;
            if (this.WCR != info.WCR)
                Hash ^= hWRC;
            if (this.BCL != info.BCL)
                Hash ^= hBLC;
            if (this.BCR != info.BCR)
                Hash ^= hBRC;
            this.WCL = info.WCL;
            this.WCR = info.WCR;
            this.BCL = info.BCL;
            this.BCR = info.BCR;
            this.WM = !this.WM;
            //var orgPiece = moved;//move.Prom != 0 ? (((int)move.Prom & (int)Color.W) != 0 ? PieceType.WP : PieceType.BP) : moved;
            this.Data[From] = moved;// orgPiece;
            this.Data[To] = Captured;
            switch (moved)
            {
                case PieceType.WP:
                    if (To == OldEnpassant)
                    {
                        var l = To - 16;
                        this.Data[l] = PieceType.BP;
                        this.Data[To] = 0;
                        Hash ^= hashCodes[(int)PieceType.BP][l] ^ hashCodes[(int)PieceType.BP][To];
                    }
                    else if (Prom != 0)
                    {
                        this.material -= Values.PIECEVALUES(Prom) - Values.PIECEVALUES(PieceType.WP);
                        Hash ^= hashCodes[(int)Prom][To] ^ hashCodes[(int)PieceType.WP][To];
                    }
                    break;
                case PieceType.BP:
                    if (To == OldEnpassant)
                    {
                        var l = To + 16;
                        this.Data[l] = PieceType.WP;
                        this.Data[To] = 0;
                        Hash ^= hashCodes[(int)PieceType.WP][l] ^ hashCodes[(int)PieceType.WP][To];
                    }
                    else if (Prom != 0)
                    {
                        this.material -= Values.PIECEVALUES(Prom) - Values.PIECEVALUES(PieceType.BP);
                        Hash ^= hashCodes[(int)Prom][To] ^ hashCodes[(int)PieceType.BP][To];
                    }
                    break;
                case PieceType.WK:
                    this.WKSquare = From;
                    if (To - From == 2)
                    {
                        this.Data[5] = 0;
                        this.Data[7] = PieceType.WR;
                        Hash ^= hashCodes[(int)PieceType.WR][5] ^ hashCodes[(int)PieceType.WR][7];
                    }
                    else if (To - From == -2)
                    {
                        this.Data[3] = 0;
                        this.Data[0] = PieceType.WR;
                        Hash ^= hashCodes[(int)PieceType.WR][3] ^ hashCodes[(int)PieceType.WR][0];
                    }
                    break;
                case PieceType.BK:
                    this.BKSquare = From;
                    if (To - From == 2)
                    {
                        this.Data[117] = 0;
                        this.Data[119] = PieceType.BR;
                        Hash ^= hashCodes[(int)PieceType.BR][117] ^ hashCodes[(int)PieceType.BR][119];
                    }
                    else if (To - From == -2)
                    {
                        this.Data[115] = 0;
                        this.Data[112] = PieceType.BR;
                        Hash ^= hashCodes[(int)PieceType.BR][115] ^ hashCodes[(int)PieceType.BR][112];
                    }
                    break;
                default:
                    break;
            }
            this.Hash = Hash;
#if DEBUG
            if (this.Data[this.WKSquare] != PieceType.WK || this.Data[this.BKSquare] != PieceType.BK)
                throw new Exception("Error in king unmake move");
            this.CalculateHash();
            if (this.Hash != Hash)
                throw new Exception("Error in hash");
#endif
        }



        bool AttackedBySlide(int square, PieceType attackingPieces, int[] dirs)
        {
            var v = dirs.Length;
            do
            {
                var d = dirs[--v];
                for (var j = square + d; (j & 0x88) == 0; j += d)
                {
                    if (this.Data[j] != 0)
                    {
                        if (this.Data[j] == attackingPieces)
                            return true;
                        break;
                    }
                }
            }
            while (v != 0);
            return false;
        }
        bool AttackedByJump(int square, PieceType attackingPieces, int[] dirs)
        {
            var v = dirs.Length;
            do
            {
                var j = square - dirs[--v];
                if ((j & 0x88) == 0 && this.Data[j] == attackingPieces)
                    return true;
            }
            while (v != 0);
            return false;
        }

        bool Attacked(int square, bool whiteAttacks)
        {
            if (whiteAttacks)
            {
                return this.AttackedByJump(square, PieceType.WP, MovegenDirs.WPC)
                    || this.AttackedBySlide(square, PieceType.WB, MovegenDirs.BDirs)
                    || this.AttackedBySlide(square, PieceType.WR, MovegenDirs.RDirs)
                    || this.AttackedBySlide(square, PieceType.WQ, MovegenDirs.QDirs)
                    || this.AttackedByJump(square, PieceType.WN, MovegenDirs.NDirs)
                    || this.AttackedByJump(square, PieceType.WK, MovegenDirs.QDirs);
            }
            else
            {
                return this.AttackedByJump(square, PieceType.BP, MovegenDirs.BPC)
                    || this.AttackedBySlide(square, PieceType.BB, MovegenDirs.BDirs)
                    || this.AttackedBySlide(square, PieceType.BR, MovegenDirs.RDirs)
                    || this.AttackedBySlide(square, PieceType.BQ, MovegenDirs.QDirs)
                    || this.AttackedByJump(square, PieceType.BN, MovegenDirs.NDirs)
                    || this.AttackedByJump(square, PieceType.BK, MovegenDirs.QDirs);
            }
        }
        public bool IsCheck()
        {
            var check = this.inCheck[this.height + this.age];
            if (check != CheckState.NotChecked)
                return (check & CheckState.Check) != 0;
            else
            {
                if (this.WM)
                {
                    if (this.Attacked(this.WKSquare, false))
                    {
                        this.inCheck[this.height + this.age] = CheckState.White;
                        return true;
                    }
                    else
                    {
                        this.inCheck[this.height + this.age] = CheckState.No;
                        return false;
                    }
                }
                else
                {
                    if (this.Attacked(this.BKSquare, true))
                    {
                        this.inCheck[this.height + this.age] = CheckState.Black;
                        return true;
                    }
                    else
                    {
                        this.inCheck[this.height + this.age] = CheckState.No;
                        return false;
                    }
                }
            }
        }

        public int Evaluate()
        {
            var value = this.material;
            return this.WM ? value : -value;
        }

        public object Clone()
        {
            var b = new Board();
            b.Reset();
            for (int i = 0; i < 128; i++)
                b.Data[i] = this.Data[i];
            b.WCL = this.WCL;
            b.WCR = this.WCR;
            b.BCL = this.BCL;
            b.BCR = this.BCR;
            b.M50 = this.M50;
            b.Enpassant = this.Enpassant;
            b.WM = this.WM;
            b.WKSquare = this.WKSquare;
            b.BKSquare = this.BKSquare;
            return b;
        }

        public bool Equals(Board b)
        {
            for (int i = 0; i < 128; i++)
                if (b.Data[i] != this.Data[i])
                    return false;
            if (b.WCL != this.WCL)
                return false;
            if (b.WCR != this.WCR)
                return false;
            if (b.BCL != this.BCL)
                return false;
            if (b.BCR != this.BCR)
                return false;
            if (b.M50 != this.M50)
                return false;
            if (b.Enpassant != this.Enpassant)
                return false;
            if (b.WM != this.WM)
                return false;
            if (b.WKSquare != this.WKSquare)
                return false;
            if (b.BKSquare != this.BKSquare)
                return false;
            return true;
        }
        void CalculateHash()
        {
            var h = 0;
            for (var i = 0; i < 128; i++)
            {
                if (this.Data[i] != 0)
                    h ^= hashCodes[(int)this.Data[i]][i];
            }
            if (this.Enpassant != 128)
                h ^= hEnpassant[this.Enpassant % 16];
            if (this.WCL)
                h ^= hWLC;
            if (this.WCR)
                h ^= hWRC;
            if (this.BCL)
                h ^= hBLC;
            if (this.BCR)
                h ^= hBRC;
            if (this.WM)
                h ^= hWM;
            this.Hash = h;
        }

        static int[][] hashCodes;
        static int hBLC;
        static int hBRC;
        static int hWLC;
        static int hWRC;
        static int hWM;
        static int[] hEnpassant;
        static Board()
        {
            var r = new Random();
            hashCodes = new int[0x2D][];
            foreach (PieceType s in Enum.GetValues(typeof(PieceType)))
            {
                var d = new int[128];
                for (var i = 0; i < 128; i++)
                    d[i] = r.Next();
                hashCodes[(int)s] = d;
            }
            hBLC = r.Next();
            hBRC = r.Next();
            hWLC = r.Next();
            hWRC = r.Next();
            hWM = r.Next();
            hEnpassant = new int[8];
            for (var i = 0; i < 8; i++)
                hEnpassant[i] = r.Next();
        }

        
            }
}
