using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ChessTest
{
    class Searcher
    {
        public int nodes;
        public int lastnodes;
        public int time;
        public int lasttime;
        public Move bestMove;
        public int bestMoveValue;
        public Board board;
        public bool stop;
        public int maxTime;
        TimeSpan ts;
        public delegate void WriteBestMove(Move bestMove);
        public delegate void WritePly(int depth, int score, int time, int nodes, int nps, Move[] pv);
        public delegate void WriteCurrMove(Move move, int number, int time, int nodes, int nps);
        WriteBestMove wbm;
        WritePly wp;
        WriteCurrMove wcm;
        Thread thread;
        Move[] foundPV;
        HashTable table;
        void UpdateTime()
        {
            this.time = (int)Math.Round(DateTime.Now.TimeOfDay.Subtract(ts).TotalMilliseconds);
            if (this.time > this.maxTime)
                this.stop = true;
        }
        public Searcher(WriteBestMove wbm, WritePly wp, WriteCurrMove wcm)
        {
            this.wbm = wbm;
            this.wp = wp;
            this.wcm = wcm;
            table = new HashTable(1024 * 128);
        }
        public void Go(Board board, int maxDepth, int maxTime)
        {
            this.board = board;
            this.stop = false;
            this.maxTime = maxTime;
            this.foundPV = new Move[100];
            this.nodes = 0;
            this.lastnodes = 0;
            this.time = 0;
            this.lasttime = 0;
            this.ts = DateTime.Now.TimeOfDay;
            this.thread = new Thread(new ParameterizedThreadStart(this.ID));
            thread.Start(maxDepth);
        }
        void ID(object maxDepth)
        {
            var md = (int)maxDepth;
            this.bestMoveValue = Values.NONE;
            for (var d = 1; d <= md; ++d)
            {
                var value = this.RootSearch(d, this.bestMoveValue);
                if (!this.stop)
                {
                    this.bestMoveValue = value;
                    wp(d, this.bestMoveValue, this.time, this.nodes, 1000 * (this.nodes - this.lastnodes) / (this.time - this.lasttime + 1), this.foundPV);
                    this.lastnodes = this.nodes;
                    this.lasttime = this.time;
                }
            }
            wbm(this.bestMove);
        }
        int RootSearch(int depth, int lastValue)
        {
            if (this.stop)
                return Values.NONE;
            //this.nodes = 0;
            //this.lastnodes = 0;
            int a, b;
            if (depth < 3)
            {
                a = -Values.INF;
                b = Values.INF;
            }
            else
            {
                //draw
                if (lastValue == Values.DRAW)
                {
                    a = lastValue - 1;
                    b = lastValue + 1;
                }
                else
                {
                    a = lastValue - Values.WINDOW;
                    b = lastValue + Values.WINDOW;
                }
            }
            return this.RootAlphaBetaSimple(a, b, depth);
        }
        int RootAlphaBeta(int alpha, int beta, int depth)
        {
            if (this.stop)
                return Values.NONE;
            if (this.board.IsDraw())
                return Values.DRAW;
            if (depth <= 0)
                return this.board.Evaluate();
            ++this.nodes;
            var bestValue = Values.NONE;
            bestMove = null;
            Move move = null;
            var newPV = new Move[100];
            this.foundPV[0] = null;
            var currmove = 0;
            this.board.height = 0;
            var gen = new MoveOrderer(this.board, null);
            while ((move = gen.NextAll()) != null)
            {
                var value = Values.NONE;
#if DEBUG
                var clone = (Board)this.board.Clone();
#endif
                this.board.Make(move, false);
                if (!this.board.IsIllegal())
                {
                    currmove++;
                    if (!this.stop && this.time > 3000)
                    {
                        wcm(move, currmove, time, nodes, 1000 * (nodes - lastnodes) / (time - lasttime + 1));
                        lastnodes = nodes;
                        lasttime = time;
                    }
                    if (bestValue == Values.NONE)
                    {
                        value = -this.AlphaBeta(-beta, -alpha, depth - 1, NodeType.PV, newPV);
                        if (value <= alpha)
                        {
                            value = -this.AlphaBeta(-alpha, Values.INF, depth - 1, NodeType.PV, newPV);
                            alpha = -Values.INF;
                        }
                        else if (value >= beta)
                        {
                            value = -this.AlphaBeta(-Values.INF, beta, depth - 1, NodeType.PV, newPV);
                            beta = Values.INF;
                        }
                    }
                    else
                    {
                        value = -this.AlphaBeta(-alpha - 1, -alpha, depth - 1, NodeType.CUT, newPV);
                        if (value > alpha)
                        {
                            value = -this.AlphaBeta(-Values.INF, -alpha, depth - 1, NodeType.PV, newPV);
                            if (value >= beta)
                                beta = Values.INF;
                        }
                    }
                }
                this.board.Unmake(move);
#if DEBUG
                if (!clone.Equals(this.board))
                    throw new Exception("Unmake Fails");
#endif
                if (!this.stop && value > bestValue && (bestValue == Values.NONE || value > alpha))
                {
                    bestValue = value;
                    bestMove = move;
                    if (value > alpha)
                    {
                        alpha = value;
                        this.PVCat(this.foundPV, newPV, move);
                    }
                    if ((bestMove.Captured | bestMove.Prom) == 0)
                        this.board.AddKiller(move);
                }
            }
            return bestValue;
        }
        int AlphaBeta(int alpha, int beta, int depth, NodeType nodeType, Move[] PV)
        {
            
            if (this.stop)
                return Values.NONE;
            if (this.board.IsDraw())
                return Values.DRAW;
            if (depth <= 0)
                return this.board.Evaluate();
            if (++this.nodes % 1000 == 0)
                this.UpdateTime();
            Move transmove = null;
            if (false && depth >= 1)
            {
                var enty = table.Read(this.board.Hash);
                if (enty != null)
                {
                    transmove = enty.move;
                    if (nodeType != NodeType.PV && enty.depth >= depth)
                    {
                        var tv = enty.value;
                        var flag = enty.flag;
                        if (flag == HashTable.Flag.Exact || flag == HashTable.Flag.Lower && tv >= beta || flag == HashTable.Flag.Upper && tv <= alpha)
                            return tv;
                    }                    
                }
            }
            var newPV = new Move[100];
            PV[0] = null;
            var bestValue = Values.NONE;
            var oldAlpha = alpha;
            Move bestMove = null;
            Move move = null;
            var gen = new MoveOrderer(this.board, transmove);
            var inCheck = this.board.IsCheck();
            while ((move = gen.NextAll()) != null)
            {
                var value = Values.NONE;
#if DEBUG
                var clone = (Board)this.board.Clone();
#endif
                this.board.Make(move, false);
                if (!this.board.IsIllegal())
                {
                    if (nodeType != NodeType.PV || bestValue == Values.NONE)
                    {
                        value = -this.AlphaBeta(-beta, -alpha, depth - 1, nodeType == NodeType.PV ? NodeType.PV : NodeType.ALL, newPV);
                    }
                    else
                    {
                        value = -this.AlphaBeta(-alpha - 1, -alpha, depth - 1, NodeType.CUT, newPV);
                        if (value > alpha)
                            value = -this.AlphaBeta(-beta, -alpha, depth - 1, NodeType.PV, newPV);
                    }
                }
                this.board.Unmake(move);
#if DEBUG
                if (!clone.Equals(this.board))
                    throw new Exception("Unmake Fails");
#endif
                if (value > bestValue)
                {
                    bestValue = value;
                    bestMove = move;
                    if (value > alpha)
                    {
                        alpha = value;
                        if (value >= beta)
                        {
                            if ((bestMove.Captured | bestMove.Prom) == 0)
                                this.board.AddKiller(move);
                            break;
                        }
                        else
                        {
                            this.PVCat(PV, newPV, move);
                        }
                    }
                }
            }
            if (bestValue == Values.NONE)
            {
                if (inCheck)
                    return -Values.MATE + this.board.height;
                else
                    return Values.DRAW;
            }
            else
            {
                this.board.GoodMove(bestMove, depth);

            }
            if (depth >= 1)
            {
                var flag = HashTable.Flag.Unknown;
                if (bestValue > oldAlpha)
                    flag |= HashTable.Flag.Lower;
                if (bestValue < beta)
                    flag |= HashTable.Flag.Upper;
                this.table.Store(this.board.Hash, bestMove, bestValue, depth, this.board.age, flag);
            }
            return bestValue;
        }
        private void PVCat(Move[] pv, Move[] newpv, Move move)
        {
            var i = 0;
            pv[i++] = move;
            while ((pv[i] = newpv[i - 1]) != null) ++i;
        }

        int RootAlphaBetaSimple(int alpha, int beta, int depth)
        {
            if (this.stop)
                return Values.NONE;
            if (this.board.IsDraw())
                return Values.DRAW;
            if (depth <= 0)
                return this.board.Evaluate();
            ++this.nodes;
            var bestValue = Values.NONE;
            bestMove = null;
            Move move = null;
            var newPV = new Move[100];
            this.foundPV[0] = null;
            var currmove = 0;
            this.board.height = 0;
            var gen = new MoveOrderer(this.board, null);
            while ((move = gen.Next()) != null)
            {
                var value = Values.NONE;
#if DEBUG
                var clone = (Board)this.board.Clone();
#endif
                this.board.Make(move, false);
                if (!this.board.IsIllegal())
                {
                    currmove++;
                    if (!this.stop && this.time > 3000)
                    {
                        wcm(move, currmove, time, nodes, 1000 * (nodes - lastnodes) / (time - lasttime + 1));
                        lastnodes = nodes;
                        lasttime = time;
                    }
                    if (bestValue == Values.NONE)
                    {
                        value = -this.AlphaBetaSimple(-beta, -alpha, depth - 1, NodeType.PV, newPV);
                        if (value <= alpha)
                        {
                            value = -this.AlphaBetaSimple(-alpha, Values.INF, depth - 1, NodeType.PV, newPV);
                            alpha = -Values.INF;
                        }
                        else if (value >= beta)
                        {
                            value = -this.AlphaBetaSimple(-Values.INF, beta, depth - 1, NodeType.PV, newPV);
                            beta = Values.INF;
                        }
                    }
                    else
                    {
                        value = -this.AlphaBetaSimple(-alpha - 1, -alpha, depth - 1, NodeType.CUT, newPV);
                        if (value > alpha)
                        {
                            value = -this.AlphaBetaSimple(-Values.INF, -alpha, depth - 1, NodeType.PV, newPV);
                            if (value >= beta)
                                beta = Values.INF;
                        }
                    }
                }
                this.board.Unmake(move);
#if DEBUG
                if (!clone.Equals(this.board))
                    throw new Exception("Unmake Fails");
#endif
                if (!this.stop && value > bestValue && (bestValue == Values.NONE || value > alpha))
                {
                    bestValue = value;
                    bestMove = move;
                    if (value > alpha)
                    {
                        alpha = value;
                        this.PVCat(this.foundPV, newPV, move);
                    }
                    //pvstore
                    this.board.AddKiller(move);
                }
            }
            return bestValue;
        }
        int AlphaBetaSimple(int alpha, int beta, int depth, NodeType nodeType, Move[] PV)
        {

            if (this.stop)
                return Values.NONE;
            if (this.board.IsDraw())
                return Values.DRAW;
            if (depth <= 0)
                return this.board.Evaluate();
            if (++this.nodes % 1000 == 0)
                this.UpdateTime();
            Move transmove = null;

            var newPV = new Move[100];
            PV[0] = null;
            var bestValue = Values.NONE;
            var oldAlpha = alpha;
            Move bestMove = null;
            Move move = null;
            var gen = new MoveOrderer(this.board, transmove);
            var inCheck = this.board.IsCheck();
            while ((move = gen.Next()) != null)
            {
                var value = Values.NONE;
#if DEBUG
                var clone = (Board)this.board.Clone();
#endif
                this.board.Make(move, false);
                if (!this.board.IsIllegal())
                {
                    if (nodeType != NodeType.PV || bestValue == Values.NONE)
                    {
                        value = -this.AlphaBetaSimple(-beta, -alpha, depth - 1, nodeType == NodeType.PV ? NodeType.PV : NodeType.ALL, newPV);
                    }
                    else
                    {
                        value = -this.AlphaBetaSimple(-alpha - 1, -alpha, depth - 1, NodeType.CUT, newPV);
                        if (value > alpha)
                            value = -this.AlphaBetaSimple(-beta, -alpha, depth - 1, NodeType.PV, newPV);
                    }
                }
                this.board.Unmake(move);
#if DEBUG
                if (!clone.Equals(this.board))
                    throw new Exception("Unmake Fails");
#endif
                if (value > bestValue)
                {
                    bestValue = value;
                    bestMove = move;
                    if (value > alpha)
                    {
                        alpha = value;
                        if (value >= beta)
                        {
                            this.board.AddKiller(move);
                            break;
                        }
                        else
                        {
                            this.PVCat(PV, newPV, move);
                        }
                    }
                }
            }
            if (bestValue == Values.NONE)
            {
                if (inCheck)
                    return -Values.MATE + this.board.height;
                else
                    return Values.DRAW;
            }

            return bestValue;
        }
    }
}
