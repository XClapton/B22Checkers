using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public class Move
    {
        private readonly int r_CurrRow;
        private readonly int r_CurrCol;
        private readonly int r_DestRow;
        private readonly int r_DestCol;

        public Move(int i_CurrCol, int i_CurrRow, int i_DestCol, int i_DestRow)
        {
            r_CurrCol = i_CurrCol;
            r_CurrRow = i_CurrRow;
            r_DestCol = i_DestCol;
            r_DestRow = i_DestRow;
        }

        public int CurrRow
        {
            get { return r_CurrRow; }
        }

        public int CurrCol
        {
            get { return r_CurrCol; }
        }

        public int DestRow
        {
            get { return r_DestRow; }
        }

        public int DestCol
        {
            get { return r_DestCol; }
        }
    }
}