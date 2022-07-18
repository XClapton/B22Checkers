using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckersLogic;

namespace Ex05.windowsUI
{
    public class CheckersWindowsGame
    {
        private CheckersForm m_CheckersForm;

        public void StartGame()
        {
            m_CheckersForm = new CheckersForm();
            m_CheckersForm.ShowDialog();
        }
    }
}