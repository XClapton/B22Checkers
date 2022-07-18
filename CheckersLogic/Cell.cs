using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckersLogic.enums;

namespace CheckersLogic
{
    public class Cell
    {
        private eEntity m_Entity;
        private bool m_IsKing;

        public Cell(eEntity i_Entity)
        {
            m_Entity = i_Entity;
            m_IsKing = false;
        }

        public eEntity Entity
        {
            get { return m_Entity; }
            set { m_Entity = value; }
        }

        public bool IsKing
        {
            get { return m_IsKing; }
            set { m_IsKing = value; }
        }
    }
}
