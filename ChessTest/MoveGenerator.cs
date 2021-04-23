using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTest
{
    partial class Board
    {
        #region AllMoves

        #region WhitePawnMoves

        void WhitePawnMoves(List<Move> moves, int square)
        {
            var promote = square >= 96;
            var notMoved = square < 32;

            var j = square + 15;
            if ((j & 0x88) == 0)
            {
                var target = this.Data[j];
                if (((int)target & (int)Color.B) != 0)
                {
                    if (promote)
                    {
                        moves.Add(new Move(square, j, PieceType.WQ, PieceType.WP, target));
                        moves.Add(new Move(square, j, PieceType.WN, PieceType.WP, target));
                        moves.Add(new Move(square, j, PieceType.WB, PieceType.WP, target));
                        moves.Add(new Move(square, j, PieceType.WR, PieceType.WP, target));
                    }
                    else
                        moves.Add(new Move(square, j, 0, PieceType.WP, target));
                }
                else if (this.Enpassant == j)
                    moves.Add(new Move(square, j, 0, PieceType.WP, PieceType.BP));
            }

            j = square + 17;
            if ((j & 0x88) == 0)
            {
                var target = this.Data[j];
                if (((int)target & (int)Color.B) != 0)
                {
                    if (promote)
                    {
                        moves.Add(new Move(square, j, PieceType.WQ, PieceType.WP, target));
                        moves.Add(new Move(square, j, PieceType.WN, PieceType.WP, target));
                        moves.Add(new Move(square, j, PieceType.WB, PieceType.WP, target));
                        moves.Add(new Move(square, j, PieceType.WR, PieceType.WP, target));
                    }
                    else
                        moves.Add(new Move(square, j, 0, PieceType.WP, target));
                }
                else if (this.Enpassant == j)
                    moves.Add(new Move(square, j, 0, PieceType.WP, PieceType.BP));
            }

            j = square + 16;
            if (this.Data[j] == 0)
            {
                if (promote)
                {
                    moves.Add(new Move(square, j, PieceType.WQ, PieceType.WP, 0));
                    moves.Add(new Move(square, j, PieceType.WN, PieceType.WP, 0));
                    moves.Add(new Move(square, j, PieceType.WB, PieceType.WP, 0));
                    moves.Add(new Move(square, j, PieceType.WR, PieceType.WP, 0));
                }
                else
                {
                    moves.Add(new Move(square, j, 0, PieceType.WP, 0));
                    if (notMoved)
                    {
                        j += 16;
                        if (this.Data[j] == 0)
                            moves.Add(new Move(square, j, 0, PieceType.WP, 0));
                    }
                }
            }
        }

        #endregion

        #region BlackPawnMoves

        void BlackPawnMoves(List<Move> moves, int square)
        {
            var promote = square < 32;
            var notMoved = square >= 96;

            var j = square - 15;
            if ((j & 0x88) == 0)
            {
                var target = this.Data[j];
                if (((int)target & (int)Color.W) != 0)
                {
                    if (promote)
                    {
                        moves.Add(new Move(square, j, PieceType.BQ, PieceType.BP, target));
                        moves.Add(new Move(square, j, PieceType.BN, PieceType.BP, target));
                        moves.Add(new Move(square, j, PieceType.BB, PieceType.BP, target));
                        moves.Add(new Move(square, j, PieceType.BR, PieceType.BP, target));
                    }
                    else
                        moves.Add(new Move(square, j, 0, PieceType.BP, target));
                }
                else if (this.Enpassant == j)
                    moves.Add(new Move(square, j, 0, PieceType.BP, PieceType.WP));
            }

            j = square - 17;
            if ((j & 0x88) == 0)
            {
                var target = this.Data[j];
                if (((int)target & (int)Color.W) != 0)
                {
                    if (promote)
                    {
                        moves.Add(new Move(square, j, PieceType.BQ, PieceType.BP, target));
                        moves.Add(new Move(square, j, PieceType.BN, PieceType.BP, target));
                        moves.Add(new Move(square, j, PieceType.BB, PieceType.BP, target));
                        moves.Add(new Move(square, j, PieceType.BR, PieceType.BP, target));
                    }
                    else
                        moves.Add(new Move(square, j, 0, PieceType.BP, target));
                }
                else if (this.Enpassant == j)
                    moves.Add(new Move(square, j, 0, PieceType.BP, PieceType.WP));
            }

            j = square - 16;
            if (this.Data[j] == 0)
            {
                if (promote)
                {
                    moves.Add(new Move(square, j, PieceType.BQ, PieceType.BP, 0));
                    moves.Add(new Move(square, j, PieceType.BN, PieceType.BP, 0));
                    moves.Add(new Move(square, j, PieceType.BB, PieceType.BP, 0));
                    moves.Add(new Move(square, j, PieceType.BR, PieceType.BP, 0));
                }
                else
                {
                    moves.Add(new Move(square, j, 0, PieceType.BP, 0));
                    if (notMoved)
                    {
                        j -= 16;
                        if (this.Data[j] == 0)
                            moves.Add(new Move(square, j, 0, PieceType.BP, 0));
                    }
                }
            }
        }

        #endregion

        #region KnightMoves

        void KnightMoves(List<Move> moves, int square, Color pieceColor, PieceType orgPieceType)
        {
            var j = square + 14;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square + 31;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square + 33;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square + 18;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 14;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 31;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 33;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 18;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }
        }

        #endregion

        #region BishopMoves

        void BishopMoves(List<Move> moves, int square, Color pieceColor, PieceType orgPieceType)
        {
            var d = -15;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = 15;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = -17;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = 17;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

        }

        #endregion

        #region RookMoves

        void RookMoves(List<Move> moves, int square, Color pieceColor, PieceType orgPieceType)
        {
            var d = -1;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }


            d = 1;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = -16;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = 16;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

        }

        #endregion

        #region QueenMoves

        void QueenMoves(List<Move> moves, int square, Color pieceColor, PieceType orgPieceType)
        {
            var d = -15;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = 15;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = -17;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = 17;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = -1;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = 1;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = -16;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = 16;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, 0));
                else
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

        }

        #endregion

        #region KingMoves

        void KingMoves(List<Move> moves, int square, Color pieceColor, PieceType orgPieceType)
        {
            //this.Data[square] = 0;
            var j = square + 1;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square + 17;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square + 16;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square + 15;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 1;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 17;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 16;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 15;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)pieceColor) == 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            //this.Data[square] = orgPieceType;
        }

        #endregion

        #region CastleMoves

        void CastleMoves(List<Move> moves)
        {
#warning no islegal
            if (this.WM)
            {
                if (this.WCL && this.Data[3] == 0 && this.Data[2] == 0 && this.Data[1] == 0 && !this.Attacked2(4, false) && !this.Attacked2(3, false)/* && !this.Attacked(2, false)*/)
                    moves.Add(new Move(4, 2, 0, PieceType.WK, 0));
                if (this.WCR && this.Data[5] == 0 && this.Data[6] == 0 && !this.Attacked2(4, false) && !this.Attacked2(5, false)/* && !this.Attacked(6, false)*/)
                    moves.Add(new Move(4, 6, 0, PieceType.WK, 0));
            }
            else
            {
                if (this.BCL && this.Data[115] == 0 && this.Data[114] == 0 && this.Data[113] == 0 && !this.Attacked2(116, true) && !this.Attacked2(115, true)/* && !this.Attacked(114, true)*/)
                    moves.Add(new Move(116, 114, 0, PieceType.BK, 0));
                if (this.BCR && this.Data[117] == 0 && this.Data[118] == 0 && !this.Attacked2(116, true) && !this.Attacked2(117, true)/* && !this.Attacked(118, true)*/)
                    moves.Add(new Move(116, 118, 0, PieceType.BK, 0));
            }
        }

        #endregion

        #endregion

        #region Captures

        #region WhitePawnCaptures

        void WhitePawnCaptures(List<Move> moves, int square)
        {
            var promote = square >= 96;

            var j = square + 15;
            if ((j & 0x88) == 0)
            {
                var target = this.Data[j];
                if (((int)target & (int)Color.B) != 0)
                {
                    if (promote)
                    {
                        moves.Add(new Move(square, j, PieceType.WQ, PieceType.WP, target));
                        moves.Add(new Move(square, j, PieceType.WN, PieceType.WP, target));
                        moves.Add(new Move(square, j, PieceType.WB, PieceType.WP, target));
                        moves.Add(new Move(square, j, PieceType.WR, PieceType.WP, target));
                    }
                    else
                        moves.Add(new Move(square, j, 0, PieceType.WP, target));
                }
                else if (this.Enpassant == j)
                    moves.Add(new Move(square, j, 0, PieceType.WP, PieceType.BP));
            }

            j = square + 17;
            if ((j & 0x88) == 0)
            {
                var target = this.Data[j];
                if (((int)target & (int)Color.B) != 0)
                {
                    if (promote)
                    {
                        moves.Add(new Move(square, j, PieceType.WQ, PieceType.WP, target));
                        moves.Add(new Move(square, j, PieceType.WN, PieceType.WP, target));
                        moves.Add(new Move(square, j, PieceType.WB, PieceType.WP, target));
                        moves.Add(new Move(square, j, PieceType.WR, PieceType.WP, target));
                    }
                    else
                        moves.Add(new Move(square, j, 0, PieceType.WP, target));
                }
                else if (this.Enpassant == j)
                    moves.Add(new Move(square, j, 0, PieceType.WP, PieceType.BP));
            }

            if (promote)
            {
                j = square + 16;
                if (this.Data[j] == 0)
                {

                    moves.Add(new Move(square, j, PieceType.WQ, PieceType.WP, 0));
                    moves.Add(new Move(square, j, PieceType.WN, PieceType.WP, 0));
                    moves.Add(new Move(square, j, PieceType.WB, PieceType.WP, 0));
                    moves.Add(new Move(square, j, PieceType.WR, PieceType.WP, 0));

                }
            }
        }

        #endregion

        #region BlackPawnCaptures

        void BlackPawnCaptures(List<Move> moves, int square)
        {
            var promote = square < 32;

            var j = square - 15;
            if ((j & 0x88) == 0)
            {
                var target = this.Data[j];
                if (((int)target & (int)Color.W) != 0)
                {
                    if (promote)
                    {
                        moves.Add(new Move(square, j, PieceType.BQ, PieceType.BP, target));
                        moves.Add(new Move(square, j, PieceType.BN, PieceType.BP, target));
                        moves.Add(new Move(square, j, PieceType.BB, PieceType.BP, target));
                        moves.Add(new Move(square, j, PieceType.BR, PieceType.BP, target));
                    }
                    else
                        moves.Add(new Move(square, j, 0, PieceType.BP, target));
                }
                else if (this.Enpassant == j)
                    moves.Add(new Move(square, j, 0, PieceType.BP, PieceType.WP));
            }

            j = square - 17;
            if ((j & 0x88) == 0)
            {
                var target = this.Data[j];
                if (((int)target & (int)Color.W) != 0)
                {
                    if (promote)
                    {
                        moves.Add(new Move(square, j, PieceType.BQ, PieceType.BP, target));
                        moves.Add(new Move(square, j, PieceType.BN, PieceType.BP, target));
                        moves.Add(new Move(square, j, PieceType.BB, PieceType.BP, target));
                        moves.Add(new Move(square, j, PieceType.BR, PieceType.BP, target));
                    }
                    else
                        moves.Add(new Move(square, j, 0, PieceType.BP, target));
                }
                else if (this.Enpassant == j)
                    moves.Add(new Move(square, j, 0, PieceType.BP, PieceType.WP));
            }

            if (promote)
            {
                j = square - 16;
                if (this.Data[j] == 0)
                {


                    moves.Add(new Move(square, j, PieceType.BQ, PieceType.BP, 0));
                    moves.Add(new Move(square, j, PieceType.BN, PieceType.BP, 0));
                    moves.Add(new Move(square, j, PieceType.BB, PieceType.BP, 0));
                    moves.Add(new Move(square, j, PieceType.BR, PieceType.BP, 0));
                }
            }
        }

        #endregion

        #region KnightCaptures

        void KnightCaptures(List<Move> moves, int square, Color opposingColor, PieceType orgPieceType)
        {
            var j = square + 14;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square + 31;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square + 33;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square + 18;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 14;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 31;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 33;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 18;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }
        }

        #endregion

        #region BishopCaptures

        void BishopCaptures(List<Move> moves, int square, Color opposingColor, PieceType orgPieceType)
        {
            var d = -15;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = 15;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = -17;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = 17;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

        }

        #endregion

        #region RookCaptures

        void RookCaptures(List<Move> moves, int square, Color opposingColor, PieceType orgPieceType)
        {
            var d = -1;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }


            d = 1;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = -16;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = 16;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

        }

        #endregion

        #region QueenCaptures

        void QueenCaptures(List<Move> moves, int square, Color opposingColor, PieceType orgPieceType)
        {
            var d = -15;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = 15;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = -17;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = 17;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = -1;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = 1;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = -16;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

            d = 16;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)opposingColor) != 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                    break;
                }
            }

        }

        #endregion

        #region KingCaptures

        void KingCaptures(List<Move> moves, int square, Color opposingColor, PieceType orgPieceType)
        {
            //this.Data[square] = 0;
            var j = square + 1;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square + 17;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square + 16;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square + 15;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 1;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 17;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 16;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            j = square - 15;
            if ((j & 0x88) == 0)
            {
                var piecetype = this.Data[j];
                if (((int)piecetype & (int)opposingColor) != 0)
                    moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
            }

            //this.Data[square] = orgPieceType;
        }

        #endregion

        #endregion

        #region Quiet

        #region WhitePawnQuiet

        void WhitePawnQuiet(List<Move> moves, int square)
        {
            var notMoved = square < 32;

            var j = square + 16;
            if (this.Data[j] == 0)
            {
                moves.Add(new Move(square, j, 0, PieceType.WP, 0));
                if (notMoved)
                {
                    j += 16;
                    if (this.Data[j] == 0)
                        moves.Add(new Move(square, j, 0, PieceType.WP, 0));
                }
            }
        }

        #endregion

        #region BlackPawnQuiet

        void BlackPawnQuiet(List<Move> moves, int square)
        {
            var notMoved = square >= 96;

            var j = square - 16;
            if (this.Data[j] == 0)
            {
                moves.Add(new Move(square, j, 0, PieceType.BP, 0));
                if (notMoved)
                {
                    j -= 16;
                    if (this.Data[j] == 0)
                        moves.Add(new Move(square, j, 0, PieceType.BP, 0));
                }
            }
        }

        #endregion

        #region KnightQuiet

        void KnightQuiet(List<Move> moves, int square, PieceType orgPieceType)
        {
            var j = square + 14;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            j = square + 31;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            j = square + 33;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            j = square + 18;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            j = square - 14;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            j = square - 31;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            j = square - 33;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            j = square - 18;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));
        }

        #endregion

        #region BishopQuiet

        void BishopQuiet(List<Move> moves, int square, PieceType orgPieceType)
        {
            var d = -15;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            d = 15;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            d = -17;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            d = 17;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

        }

        #endregion

        #region RookQuiet

        void RookQuiet(List<Move> moves, int square, PieceType orgPieceType)
        {
            var d = -1;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            d = 1;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            d = -16;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            d = 16;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

        }

        #endregion

        #region QueenQuiet

        void QueenQuiet(List<Move> moves, int square, PieceType orgPieceType)
        {
            var d = -15;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            d = 15;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            d = -17;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            d = 17;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            d = -1;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            d = 1;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            d = -16;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            d = 16;
            for (var j = square + d; (j & 0x88) == 0 && this.Data[j] == 0; j += d)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

        }

        #endregion

        #region KingQuiet

        void KingQuiet(List<Move> moves, int square, PieceType orgPieceType)
        {
            //this.Data[square] = 0;
            var j = square + 1;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));


            j = square + 17;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            j = square + 16;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            j = square + 15;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            j = square - 1;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            j = square - 17;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            j = square - 16;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            j = square - 15;
            if ((j & 0x88) == 0 && this.Data[j] == 0)
                moves.Add(new Move(square, j, 0, orgPieceType, 0));

            //this.Data[square] = orgPieceType;
        }

        #endregion

        #endregion

        #region FindAll

        public void FindAll()
        {
            var moves = this.movelists[this.height];
            var kingSquare = 0;
            var pieceColor = Color.O;
            if (this.WM)
            {
                pieceColor = Color.W;
                kingSquare = this.WKSquare;
            }
            else
            {
                pieceColor = Color.B;
                kingSquare = this.BKSquare;
            }

            //FindPins(kingSquare, pieceColor);

            CastleMoves(moves);

            var i = 128;
            do
            {
                var piece = this.Data[--i];
                if (((int)piece & (int)pieceColor) != 0)
                {
                    switch (piece)
                    {
                        case PieceType.WP:
                            this.WhitePawnMoves(moves, i);
                            break;
                        case PieceType.BP:
                            this.BlackPawnMoves(moves, i);
                            break;
                        case PieceType.WN:
                            this.KnightMoves(moves, i, Color.W, PieceType.WN);
                            break;
                        case PieceType.BN:
                            this.KnightMoves(moves, i, Color.B, PieceType.BN);
                            break;
                        case PieceType.WB:
                            this.BishopMoves(moves, i, Color.W, PieceType.WB);
                            break;
                        case PieceType.BB:
                            this.BishopMoves(moves, i, Color.B, PieceType.BB);
                            break;
                        case PieceType.WR:
                            this.RookMoves(moves, i, Color.W, PieceType.WR);
                            break;
                        case PieceType.BR:
                            this.RookMoves(moves, i, Color.B, PieceType.BR);
                            break;
                        case PieceType.WQ:
                            this.QueenMoves(moves, i, Color.W, PieceType.WQ);
                            break;
                        case PieceType.BQ:
                            this.QueenMoves(moves, i, Color.B, PieceType.BQ);
                            break;
                        case PieceType.WK:
                            this.KingMoves(moves, i, Color.W, PieceType.WK);
                            break;
                        case PieceType.BK:
                            this.KingMoves(moves, i, Color.B, PieceType.BK);
                            break;

                        default:
                            throw new Exception("Wrong piece type");
                    }
                }
            }
            while (i != 0);
        }

        #endregion

        #region FindCaptures

        public void FindCaptures()
        {
            var moves = this.captureslists[this.height];
            var pieceColor = this.WM ? Color.W : Color.B;
            var opposingColor = this.WM ? Color.B : Color.W;

            //FindPins(kingSquare, pieceColor);

            var i = 128;
            do
            {
                var piece = this.Data[--i];
                if (((int)piece & (int)pieceColor) != 0)
                {
                    switch (piece)
                    {
                        case PieceType.WP:
                            this.WhitePawnCaptures(moves, i);
                            break;
                        case PieceType.BP:
                            this.BlackPawnCaptures(moves, i);
                            break;
                        case PieceType.WN:
                            this.KnightCaptures(moves, i, Color.B, PieceType.WN);
                            break;
                        case PieceType.BN:
                            this.KnightCaptures(moves, i, Color.W, PieceType.BN);
                            break;
                        case PieceType.WB:
                            this.BishopCaptures(moves, i, Color.B, PieceType.WB);
                            break;
                        case PieceType.BB:
                            this.BishopCaptures(moves, i, Color.W, PieceType.BB);
                            break;
                        case PieceType.WR:
                            this.RookCaptures(moves, i, Color.B, PieceType.WR);
                            break;
                        case PieceType.BR:
                            this.RookCaptures(moves, i, Color.W, PieceType.BR);
                            break;
                        case PieceType.WQ:
                            this.QueenCaptures(moves, i, Color.B, PieceType.WQ);
                            break;
                        case PieceType.BQ:
                            this.QueenCaptures(moves, i, Color.W, PieceType.BQ);
                            break;
                        case PieceType.WK:
                            this.KingCaptures(moves, i, Color.B, PieceType.WK);
                            break;
                        case PieceType.BK:
                            this.KingCaptures(moves, i, Color.W, PieceType.BK);
                            break;

                        default:
                            throw new Exception("Wrong piece type");
                    }
                }
            }
            while (i != 0);
        }

        #endregion

        #region FindQuiet

        public void FindQuiet()
        {
            var moves = this.quietlists[this.height];
            var kingSquare = this.WM ? this.WKSquare : this.BKSquare;
            var pieceColor = this.WM ? Color.W : Color.B;

            //FindPins(kingSquare, pieceColor);

            CastleMoves(moves);

            var i = 128;
            do
            {
                var piece = this.Data[--i];
                if (((int)piece & (int)pieceColor) != 0)
                {
                    switch (piece)
                    {
                        case PieceType.WP:
                            this.WhitePawnQuiet(moves, i);
                            break;
                        case PieceType.BP:
                            this.BlackPawnQuiet(moves, i);
                            break;
                        case PieceType.WN:
                            this.KnightQuiet(moves, i, PieceType.WN);
                            break;
                        case PieceType.BN:
                            this.KnightQuiet(moves, i, PieceType.BN);
                            break;
                        case PieceType.WB:
                            this.BishopQuiet(moves, i, PieceType.WB);
                            break;
                        case PieceType.BB:
                            this.BishopQuiet(moves, i, PieceType.BB);
                            break;
                        case PieceType.WR:
                            this.RookQuiet(moves, i, PieceType.WR);
                            break;
                        case PieceType.BR:
                            this.RookQuiet(moves, i, PieceType.BR);
                            break;
                        case PieceType.WQ:
                            this.QueenQuiet(moves, i, PieceType.WQ);
                            break;
                        case PieceType.BQ:
                            this.QueenQuiet(moves, i, PieceType.BQ);
                            break;
                        case PieceType.WK:
                            this.KingQuiet(moves, i, PieceType.WK);
                            break;
                        case PieceType.BK:
                            this.KingQuiet(moves, i, PieceType.BK);
                            break;

                        default:
                            throw new Exception("Wrong piece type");
                    }
                }
            }
            while (i != 0);
        }

        #endregion

        public enum Dirs
        {
            OO = 0x00,
            N = 0xFE,
            S = 0xFD,
            NS = 0xFC,
            W = 0xFB,
            E = 0xF7,
            WE = 0xF3,
            NE = 0xEF,
            SW = 0xDF,
            NESW = 0xCF,
            NW = 0xBF,
            SE = 0x7F,
            NWSE = 0x3F,
            ALL = 0xFF
        }
        #region FindPins
        void FindPins(int square, Color pieceColor)
        {
            var d = 16;
            var foundCandidate = false;
            var candidate = 128;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                    {
                        if (foundCandidate && ((int)piecetype & (int)Piece.R) != 0)
                        {
                            this.pins[candidate] = Dirs.NS;
                            break;
                        }
                        else
                            break;
                    }
                    else
                    {
                        if (foundCandidate)
                            break;
                        else
                        {
                            candidate = j;
                            foundCandidate = true;
                        }
                    }
                }
            }

            d = -16;
            foundCandidate = false;
            candidate = 128;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                    {
                        if (foundCandidate && ((int)piecetype & (int)Piece.R) != 0)
                        {
                            this.pins[candidate] = Dirs.NS;
                            break;
                        }
                        else
                            break;
                    }
                    else
                    {
                        if (foundCandidate)
                            break;
                        else
                        {
                            candidate = j;
                            foundCandidate = true;
                        }
                    }
                }
            }

            d = 1;
            foundCandidate = false;
            candidate = 128;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                    {
                        if (foundCandidate && ((int)piecetype & (int)Piece.R) != 0)
                        {
                            this.pins[candidate] = Dirs.WE;
                            break;
                        }
                        else
                            break;
                    }
                    else
                    {
                        if (foundCandidate)
                            break;
                        else
                        {
                            candidate = j;
                            foundCandidate = true;
                        }
                    }
                }
            }

            d = -1;
            foundCandidate = false;
            candidate = 128;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                    {
                        if (foundCandidate && ((int)piecetype & (int)Piece.R) != 0)
                        {
                            this.pins[candidate] = Dirs.WE;
                            break;
                        }
                        else
                            break;
                    }
                    else
                    {
                        if (foundCandidate)
                            break;
                        else
                        {
                            candidate = j;
                            foundCandidate = true;
                        }
                    }
                }
            }

            d = 15;
            foundCandidate = false;
            candidate = 128;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                    {
                        if (foundCandidate && ((int)piecetype & (int)Piece.B) != 0)
                        {
                            this.pins[candidate] = Dirs.NESW;
                            break;
                        }
                        else
                            break;
                    }
                    else
                    {
                        if (foundCandidate)
                            break;
                        else
                        {
                            candidate = j;
                            foundCandidate = true;
                        }
                    }
                }
            }

            d = -15;
            foundCandidate = false;
            candidate = 128;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                    {
                        if (foundCandidate && ((int)piecetype & (int)Piece.B) != 0)
                        {
                            this.pins[candidate] = Dirs.NESW;
                            break;
                        }
                        else
                            break;
                    }
                    else
                    {
                        if (foundCandidate)
                            break;
                        else
                        {
                            candidate = j;
                            foundCandidate = true;
                        }
                    }
                }
            }

            d = 17;
            foundCandidate = false;
            candidate = 128;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                    {
                        if (foundCandidate && ((int)piecetype & (int)Piece.B) != 0)
                        {
                            this.pins[candidate] = Dirs.NWSE;
                            break;
                        }
                        else
                            break;
                    }
                    else
                    {
                        if (foundCandidate)
                            break;
                        else
                        {
                            candidate = j;
                            foundCandidate = true;
                        }
                    }
                }
            }

            d = -17;
            foundCandidate = false;
            candidate = 128;
            for (var j = square + d; (j & 0x88) == 0; j += d)
            {
                var piecetype = this.Data[j];
                if (piecetype != 0)
                {
                    if (((int)piecetype & (int)pieceColor) == 0)
                    {
                        if (foundCandidate && ((int)piecetype & (int)Piece.B) != 0)
                        {
                            this.pins[candidate] = Dirs.NWSE;
                            break;
                        }
                        else
                            break;
                    }
                    else
                    {
                        if (foundCandidate)
                            break;
                        else
                        {
                            candidate = j;
                            foundCandidate = true;
                        }
                    }
                }
            }
        }
        #endregion

        #region SlideMoves

        void SlideMoves(List<Move> moves, int square, int[] dirs, Color pieceColor, PieceType orgPieceType)
        {
            var v = dirs.Length;
            do
            {
                var d = dirs[--v];
                for (var j = square + d; (j & 0x88) == 0; j += d)
                {
                    var piecetype = this.Data[j];
                    if (piecetype == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, 0));
                    else
                    {
                        if (((int)piecetype & (int)pieceColor) == 0)
                            moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                        break;
                    }
                }
            }
            while (v != 0);
        }

        #endregion

        void KingMoves(List<Move> moves, int square, int[] dirs, Color pieceColor)
        {
            var v = dirs.Length;
            do
            {
                var d = dirs[--v];
                var j = square + d;
                if ((j & 0x88) == 0 && ((int)this.Data[j] & (int)pieceColor) == 0 && !this.Attacked(square, pieceColor == Color.B))
                    moves.Add(new Move(square, j, 0, this.Data[square], this.Data[j]));
            }
            while (v != 0);
        }

        #region JumpMoves

        void JumpMoves(List<Move> moves, int square, int[] dirs, Color pieceColor, PieceType orgPieceType)
        {
            var v = dirs.Length;
            do
            {
                var d = dirs[--v];
                var j = square + d;
                if ((j & 0x88) == 0)
                {
                    var piecetype = this.Data[j];
                    if (((int)piecetype & (int)pieceColor) == 0)
                        moves.Add(new Move(square, j, 0, orgPieceType, piecetype));
                }
            }
            while (v != 0);
        }

        #endregion

        #region PMoves

        void PMoves(List<Move> moves, int square, Color target, bool white, int[] adirs, int mdir, bool notMoved, bool promote)
        {
            var v = adirs.Length;
            var j = 0;
            do
            {
                j = square + adirs[--v];
                if ((j & 0x88) == 0 && (((int)this.Data[j] & (int)target) != 0 || this.Enpassant == j))
                {
                    if (promote)
                    {
                        moves.Add(new Move(square, j, white ? PieceType.WQ : PieceType.BQ, this.Data[square], this.Data[j]));
                        moves.Add(new Move(square, j, white ? PieceType.WN : PieceType.BN, this.Data[square], this.Data[j]));
                        moves.Add(new Move(square, j, white ? PieceType.WB : PieceType.BB, this.Data[square], this.Data[j]));
                        moves.Add(new Move(square, j, white ? PieceType.WR : PieceType.BR, this.Data[square], this.Data[j]));
                    }
                    else
                    {
                        moves.Add(new Move(square, j, 0, this.Data[square], this.Enpassant == j ? (white ? PieceType.BP : PieceType.WP) : this.Data[j]));
                    }
                }
            }
            while (v != 0);

            j = square + mdir;
            if (this.Data[j] == 0)
            {
                if (promote)
                {
                    moves.Add(new Move(square, j, white ? PieceType.WQ : PieceType.BQ, this.Data[square], this.Data[j]));
                    moves.Add(new Move(square, j, white ? PieceType.WN : PieceType.BN, this.Data[square], this.Data[j]));
                    moves.Add(new Move(square, j, white ? PieceType.WB : PieceType.BB, this.Data[square], this.Data[j]));
                    moves.Add(new Move(square, j, white ? PieceType.WR : PieceType.BR, this.Data[square], this.Data[j]));
                }
                else
                {
                    moves.Add(new Move(square, j, 0, this.Data[square], this.Data[j]));
                    if (notMoved)
                    {
                        j += mdir;
                        if (this.Data[j] == 0)
                            moves.Add(new Move(square, j, 0, this.Data[square], this.Data[j]));
                    }
                }
            }
        }

        #endregion




        public List<Move> CheckResponses()
        {
            var moves = new List<Move>();
            if (this.WM)
            {
                this.KingMoves(moves, this.WKSquare, MovegenDirs.QDirs, Color.W);

            }
            else
            {
                this.KingMoves(moves, this.BKSquare, MovegenDirs.QDirs, Color.B);
            }
            return moves;
        }


        public List<Move> FindPseudoLegal()
        {
            var moves = this.movelists[this.height];
            var pieceColor = this.WM ? Color.W : Color.B;

            CastleMoves(moves);

            var i = 128;
            do
            {
                var piece = this.Data[--i];
                if (((int)piece & (int)pieceColor) != 0)
                {
                    switch (piece)
                    {
                        case PieceType.WQ:
                            this.SlideMoves(moves, i, MovegenDirs.QDirs, Color.W, PieceType.WQ);
                            break;
                        case PieceType.BQ:
                            this.SlideMoves(moves, i, MovegenDirs.QDirs, Color.B, PieceType.BQ);
                            break;
                        case PieceType.WB:
                            this.SlideMoves(moves, i, MovegenDirs.BDirs, Color.W, PieceType.WB);
                            break;
                        case PieceType.BB:
                            this.SlideMoves(moves, i, MovegenDirs.BDirs, Color.B, PieceType.BB);
                            break;
                        case PieceType.WR:
                            this.SlideMoves(moves, i, MovegenDirs.RDirs, Color.W, PieceType.WR);
                            break;
                        case PieceType.BR:
                            this.SlideMoves(moves, i, MovegenDirs.RDirs, Color.B, PieceType.BR);
                            break;
                        case PieceType.WN:
                            this.JumpMoves(moves, i, MovegenDirs.NDirs, Color.W, PieceType.WN);
                            break;
                        case PieceType.BN:
                            this.JumpMoves(moves, i, MovegenDirs.NDirs, Color.B, PieceType.BN);
                            break;
                        case PieceType.WK:
                            this.JumpMoves(moves, i, MovegenDirs.QDirs, Color.W, PieceType.WK);
                            break;
                        case PieceType.BK:
                            this.JumpMoves(moves, i, MovegenDirs.QDirs, Color.B, PieceType.BK);
                            break;
                        case PieceType.WP:
                            this.PMoves(moves, i, Color.B, true, MovegenDirs.WPC, MovegenDirs.WPM, i < 32, i >= 96);
                            break;
                        case PieceType.BP:
                            this.PMoves(moves, i, Color.W, false, MovegenDirs.BPC, MovegenDirs.BPM, i >= 96, i < 32);
                            break;
                        default:
                            throw new Exception("Wrong piece type");
                    }
                }
            }
            while (i != 0);

            return moves;
        }
        #region Attacked
        bool Attacked2(int square, bool whiteAttack)
        {
            if (whiteAttack)
            {
                var d = square - 17;
                if ((d & 0x88) == 0)
                {
                    var t = this.Data[d];
                    if (t == PieceType.WP || t == PieceType.WK)
                        return true;
                }
                d = square - 15;
                if ((d & 0x88) == 0)
                {
                    var t = this.Data[d];
                    if (t == PieceType.WP || t == PieceType.WK)
                        return true;
                }

                d = square + 16;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WK)
                    return true;
                d = square - 16;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WK)
                    return true;
                d = square + 1;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WK)
                    return true;
                d = square - 1;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WK)
                    return true;
                d = square + 15;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WK)
                    return true;
                d = square + 17;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WK)
                    return true;

                d = square + 33;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                d = square + 31;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                d = square - 33;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                d = square - 31;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                d = square + 18;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                d = square + 14;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                d = square - 18;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                d = square - 14;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                var r = 16;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WR || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -16;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WR || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                r = 1;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WR || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -1;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WR || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                r = 15;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WB || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -15;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WB || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                r = 17;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WB || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -17;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WB || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                return false;
            }
            else
            {
                var d = square + 17;
                if ((d & 0x88) == 0)
                {
                    var t = this.Data[d];
                    if (t == PieceType.BP || t == PieceType.BK)
                        return true;
                }
                d = square + 15;
                if ((d & 0x88) == 0)
                {
                    var t = this.Data[d];
                    if (t == PieceType.BP || t == PieceType.BK)
                        return true;
                }

                d = square + 16;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BK)
                    return true;
                d = square - 16;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BK)
                    return true;
                d = square + 1;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BK)
                    return true;
                d = square - 1;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BK)
                    return true;
                d = square - 15;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BK)
                    return true;
                d = square - 17;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BK)
                    return true;

                d = square + 33;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                d = square + 31;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                d = square - 33;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                d = square - 31;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                d = square + 18;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                d = square + 14;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                d = square - 18;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                d = square - 14;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                var r = 16;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BR || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -16;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BR || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                r = 1;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BR || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -1;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BR || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                r = 15;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BB || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -15;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BB || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                r = 17;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BB || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -17;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BB || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                return false;
            }
        }
        #endregion

        public bool IsIllegal()
        {
            if (this.WM)
            {
                var square = this.BKSquare;
                var d = square - 17;
                if ((d & 0x88) == 0)
                {
                    var t = this.Data[d];
                    if (t == PieceType.WP || t == PieceType.WK)
                        return true;
                }
                d = square - 15;
                if ((d & 0x88) == 0)
                {
                    var t = this.Data[d];
                    if (t == PieceType.WP || t == PieceType.WK)
                        return true;
                }

                d = square + 16;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WK)
                    return true;
                d = square - 16;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WK)
                    return true;
                d = square + 1;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WK)
                    return true;
                d = square - 1;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WK)
                    return true;
                d = square + 15;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WK)
                    return true;
                d = square + 17;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WK)
                    return true;

                d = square + 33;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                d = square + 31;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                d = square - 33;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                d = square - 31;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                d = square + 18;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                d = square + 14;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                d = square - 18;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                d = square - 14;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.WN)
                    return true;
                var r = 16;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WR || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -16;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WR || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                r = 1;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WR || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -1;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WR || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                r = 15;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WB || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -15;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WB || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                r = 17;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WB || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -17;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.WB || t == PieceType.WQ)
                            return true;
                        else
                            break;
                    }
                }
                return false;
            }
            else
            {
                var square = this.WKSquare;
                var d = square + 17;
                if ((d & 0x88) == 0)
                {
                    var t = this.Data[d];
                    if (t == PieceType.BP || t == PieceType.BK)
                        return true;
                }
                d = square + 15;
                if ((d & 0x88) == 0)
                {
                    var t = this.Data[d];
                    if (t == PieceType.BP || t == PieceType.BK)
                        return true;
                }

                d = square + 16;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BK)
                    return true;
                d = square - 16;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BK)
                    return true;
                d = square + 1;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BK)
                    return true;
                d = square - 1;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BK)
                    return true;
                d = square - 15;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BK)
                    return true;
                d = square - 17;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BK)
                    return true;

                d = square + 33;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                d = square + 31;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                d = square - 33;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                d = square - 31;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                d = square + 18;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                d = square + 14;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                d = square - 18;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                d = square - 14;
                if ((d & 0x88) == 0 && this.Data[d] == PieceType.BN)
                    return true;
                var r = 16;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BR || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -16;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BR || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                r = 1;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BR || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -1;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BR || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                r = 15;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BB || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -15;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BB || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                r = 17;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BB || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                r = -17;
                for (d = square + r; (d & 0x88) == 0; d += r)
                {
                    var t = this.Data[d];
                    if (t != 0)
                    {
                        if (t == PieceType.BB || t == PieceType.BQ)
                            return true;
                        else
                            break;
                    }
                }
                return false;
            }

        }
    }
}
