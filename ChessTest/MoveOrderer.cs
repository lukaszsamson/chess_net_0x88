using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTest
{
    class MoveOrderer
    {
        Board Board;
        int phase, cnt, cntq, cntc;
        List<Move> moves, quiet, captures;
        int[] quietvals, capturesvals;
        Move k1, k2, k3, k4, t;
        public MoveOrderer(Board board, Move trans)
        {
            this.t = trans;
            this.Board = board;
            this.phase = 0;
            this.moves = null;
            this.quiet = null;
            this.captures = null;
            this.cnt = -1;
            this.cntq = -1;
            this.cntc = -1;
            board.PushMoveInfo();
            this.moves = board.movelists[board.height];
            this.captures = board.captureslists[board.height];
            this.quiet = board.quietlists[board.height];
            this.moves.Clear();
            this.quiet.Clear();
            this.captures.Clear();

        }
        public Move Next()
        {
            while (true)
                switch (this.phase)
                {
                    case 0:
                        ++this.phase;
                        k1 = this.Board.GetKiller1();
                        if (k1 != null)
                        {
                            if (this.Board.IsMoveLegal(k1))
                                return k1;
                            else
                                continue;
                        }
                        else
                        {
                            this.phase = 4;
                            continue;
                        }
                    case 1:
                        ++this.phase;
                        k2 = this.Board.GetKiller2();
                        if (k2 != null)
                        {
                            if (this.Board.IsMoveLegal(k2))
                                return k2;
                            else
                                continue;
                        }
                        else
                        {
                            this.phase = 4;
                            continue;
                        }
                    case 2:
                        ++this.phase;
                        k3 = this.Board.GetKiller3();
                        if (k3 != null)
                        {
                            if (this.Board.IsMoveLegal(k3))
                                return k3;
                            else
                                continue;
                        }
                        else
                        {
                            this.phase = 4;
                            continue;
                        }
                    case 3:
                        ++this.phase;
                        k4 = this.Board.GetKiller4();
                        if (k4 != null)
                        {
                            if (this.Board.IsMoveLegal(k4))
                                return k4;
                            else
                                continue;
                        }
                        else
                        {
                            this.phase = 4;
                            continue;
                        }
                    case 4:
                        this.Board.FindAll();
                        this.cnt = -1;
                        ++this.phase;
                        continue;
                    case 5:
                        if (++this.cnt < this.moves.Count)
                        {
                            var move = this.moves[cnt];
                            if (!move.Equals(k1) && !move.Equals(k2) && !move.Equals(k3) && !move.Equals(k4))
                                return move;
                            else
                                continue;
                        }
                        else
                        {
                            ++this.phase;
                            return null;
                        }
                    case 6:
                        return null;
                }
        }

        public Move NextQS()
        {
            while (true)
                switch (this.phase)
                {
                    case 0:
                        this.Board.FindCaptures();
                        this.cnt = -1;
                        ++this.phase;
                        continue;
                    case 1:
                        if (++this.cnt < this.moves.Count)
                        {
                            var move = this.moves[cnt];
                            if (!move.Equals(k1) && !move.Equals(k2))
                                return move;
                            else
                                continue;
                        }
                        else
                            //++this.phase;
                            return null;
                    // continue;
                    case 4:
                        return null;
                }
        }

        public Move NextAll()
        {
            while (true)
                switch (this.phase)
                {
                    case 0:
                        ++this.phase;
                        if (t != null && this.Board.IsMoveLegal(t))
                            return t;
                        else
                            continue;
                    case 1:
                        this.Board.FindCaptures();
                        var lengthc = this.captures.Count;
                        if (lengthc > 0)
                        {
                            this.capturesvals = new int[lengthc + 1];
                            var kc = lengthc;
                            do
                            {
                                --kc;
                                this.capturesvals[kc] = this.captures[kc].CaptureVale();
                            }
                            while (kc != 0);
                            this.capturesvals[lengthc] = -1024;

                            for (var i = lengthc - 2; i >= 0; --i)
                            {
                                var value = this.capturesvals[i];
                                var mo = this.captures[i];
                                var j = i;
                                while (this.capturesvals[j + 1] > value)
                                {
                                    var jj = j + 1;
                                    this.capturesvals[j] = this.capturesvals[jj];
                                    this.captures[j] = this.captures[jj];
                                    ++j;
                                }
                                this.capturesvals[j] = value;
                                this.captures[j] = mo;
                            }
                            this.cntc = -1;
                            ++this.phase;
                            continue;
                        }
                        else
                        {
                            this.phase = 3;
                            continue;
                        }
                    case 2:
                        if (++this.cntc < this.captures.Count)
                        {
                            var move = this.captures[cntc];
                            if (this.capturesvals[cntc] >= -100)
                            {
                                if (!move.Equals(t))
                                    return move;
                                else
                                    continue;
                            }
                            else
                            {
                                --this.cntc;
                                ++this.phase;
                                continue;
                            }

                        }
                        else
                        {
                            ++this.phase;
                            continue;
                        }
                    case 3:
                        ++this.phase;
                        k1 = this.Board.GetKiller1();
                        if (k1 != null && !k1.Equals(t) && this.Board.IsMoveLegal(k1))
                            return k1;
                        else
                            continue;
                    case 4:
                        ++this.phase;
                        k2 = this.Board.GetKiller2();
                        if (k2 != null && !k2.Equals(t) && this.Board.IsMoveLegal(k2))
                            return k2;
                        else
                            continue;
                    case 5:
                        this.Board.FindQuiet();
                        var lengthq = this.quiet.Count;
                        if (lengthq > 0)
                        {
                            this.quietvals = new int[lengthq + 1];
                            var kq = lengthq;
                            do
                            {
                                --kq;
                                this.quietvals[kq] = this.Board.history.Value(this.quiet[kq]);
                            }
                            while (kq != 0);
                            this.quietvals[lengthq] = -1024;

                            for (var i = lengthq - 2; i >= 0; --i)
                            {
                                var value = this.quietvals[i];
                                var mo = this.quiet[i];
                                var j = i;
                                while (this.quietvals[j + 1] > value)
                                {
                                    var jj = j + 1;
                                    this.quietvals[j] = this.quietvals[jj];
                                    this.quiet[j] = this.quiet[jj];
                                    ++j;
                                }
                                this.quietvals[j] = value;
                                this.quiet[j] = mo;
                            }

                            this.cntq = -1;
                            ++this.phase;
                            continue;
                        }
                        else
                        {
                            this.phase = 7;
                            continue;
                        }
                    case 6:
                        if (++this.cntq < this.quiet.Count)
                        {
                            var move = this.quiet[cntq];
                            if (!move.Equals(k1) && !move.Equals(k2) && !move.Equals(t))
                                return move;
                            else
                                continue;
                        }
                        else
                        {
                            ++this.phase;
                            continue;
                        }
                    case 7:
                        if (++this.cntc < this.captures.Count)
                        {
                            var move = this.captures[cntc];
                            if (!move.Equals(t))
                                return move;
                            else
                                continue;
                        }
                        else
                        {
                            ++this.phase;
                            return null;
                        }
                    case 8:
                        return null;
                }
        }
    }
}
