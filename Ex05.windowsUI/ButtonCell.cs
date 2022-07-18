using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using CheckersLogic;
using CheckersLogic.enums;

namespace Ex05.windowsUI
{
    internal class ButtonCell : Button
    {
        private readonly int r_Row;
        private readonly int r_Col;
        private readonly bool r_IsActive = false;

        public ButtonCell(int i_Row, int i_Col, eEntity i_Entity)
        {
            r_Row = i_Row;
            r_Col = i_Col;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            if(i_Entity == eEntity.Inaccessible)
            {
                this.BackColor = System.Drawing.Color.Sienna;
                this.Enabled = false;
            }
            else
            {
                this.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                r_IsActive = true;
            }
        }
       
        public bool Active
        {
            get { return r_IsActive; }
        }
       
        public int Row
        {
            get { return r_Row; }
        }

        public int Col
        {
            get { return r_Col; }
        }

        public void AddImageToButton(Cell i_Cell)
        {
            Image cellImage;

            if(i_Cell.Entity == eEntity.Empty || i_Cell.Entity == eEntity.Inaccessible)
            {
                cellImage = null;
            }
            else if (i_Cell.Entity == eEntity.Player1)
            {
                if (i_Cell.IsKing)
                {
                    cellImage = Ex05.windowsUI.Properties.Resources.blackKing;
                }
                else
                {
                    cellImage = Ex05.windowsUI.Properties.Resources.blackMan;
                }
            }
            else
            {
                if (i_Cell.IsKing)
                {
                    cellImage = Ex05.windowsUI.Properties.Resources.whiteKing;
                }
                else
                {
                    cellImage = Ex05.windowsUI.Properties.Resources.whiteMan;
                }
            }

            this.BackgroundImage = cellImage;
        }
    }
}
