using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTest
{
    public class UCIInterface
    {
        private Board board;
        private Searcher searcher;
        public bool Exit;
        private int age;
        public UCIInterface()
        {
            board = new Board();
            board.Reset();
            searcher = new Searcher(this.WriteBestMove, this.WritePly, this.WriteCurrMove);
        }
        void WriteBestMove(Move bestMove)
        {
            Console.WriteLine("bestmove " + bestMove);
        }
        void WritePly(int depth, int score, int time, int nodes, int nps, Move[] pv)
        {
            var sb = new StringBuilder();
            sb.Append("info depth ").Append(depth).Append(" score ");
            if (score >= Values.MATE)
                ;
            else if (score <= -Values.MATE)
                ;
            else
                sb.Append("cp ").Append(score);
            sb.Append(" time ").Append(time).Append(" nodes ").Append(nodes).Append(" nps ").Append(nps);
            if (pv != null)
            {
                if (pv.Length > 0 && pv[0] != null)
                {
                    sb.Append(" pv");
                    for (var i = 0; pv[i] != null; i++)
                        sb.Append(" ").Append(pv[i]);
                }
            }
            Console.WriteLine(sb.ToString());
        }
        void WriteCurrMove(Move move, int number, int time, int nodes, int nps)
        {
            Console.WriteLine("info currmove " + move + " currmovenumber " + number + " time " + time + " nodes " + nodes + " nps " + nps);
        }
        public void ProcessCommand(string com)
        {
            if (com.IndexOf("go") == 0)
            {
                if (com.IndexOf("movetime") == 3)
                    searcher.Go(board, 100, int.Parse(com.Substring(12)));
//#warning cheat
                if (com.IndexOf("depth") == 3)
                    searcher.Go(board, int.Parse(com.Substring(9)), 60 * 60 * 1000);

                if (com.IndexOf("infinite") == 3)
                    searcher.Go(board, 200, 24 * 60 * 60 * 1000);
                //if(!strcmp(param, "wtime")) {

            }

            else if (com.IndexOf("position") == 0)
            {
                int index = 0;
                if (com.IndexOf("startpos") == 9)
                {
                    board.Reset();
                    index = 18;
                }
                //else if(com.IndexOf("fen") == 9)
                //{
                //    index = 13;
                //}               
                if (index < com.Length && com.IndexOf("moves", index) == index)
                {
                    string[] moves = com.Substring(index + 6).Split(' ');
                    int movesCount = moves.Length;
                    for (int i = 0; i < movesCount; i++)
                    {
                        if (moves[i] == "0000")
                            ;//board.MakeNullMove();//dupa
                        else
                        {
                            board.Make(board.GetMove(moves[i]), true);
                        }
                    }
                }
            }

            else if (com == "uci")
            {
                Console.WriteLine("id name szachi");
                Console.WriteLine("id author thor the powerhead");
                Console.WriteLine("uciok");
            }

            else if (com == "isready")
                Console.WriteLine("readyok");

            else if (com == "stop")
                searcher.stop = true;

            else if (com == "quit")
            {
                searcher.stop = true;
                Exit = true;
            }
        }
    }
}
