using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CheckersLogic;
using CheckersLogic.enums;

namespace Ex05.windowsUI
{
    public partial class SettingsForm : Form
    {
        private const int k_SmallBoardSize = 6;
        private const int k_MediumBoardSize = 8;
        private const int k_BigBoardSize = 10;

        public SettingsForm()
        {
            InitializeComponent();
        }

        public int BoardSize
        {
            get
            {
                int boardSize = 0;

                if(radioButton6x6.Checked)
                {
                    boardSize = k_SmallBoardSize;
                }
                else if(radioButton8x8.Checked)
                {
                    boardSize = k_MediumBoardSize;
                }
                else if(radioButton10x10.Checked)
                {
                    boardSize = k_BigBoardSize;
                }

                return boardSize;
            }
        }

        public string FirstPlayerName
        {
            get { return textBoxPlayer1.Text; }
        }

        public string SecondPlayerName
        {
            get { return textBoxPlayer2.Text; }
        }

        public ePlayerType SecondPlayerType
        {
            get { return checkBoxPlayer2.Checked ? ePlayerType.Human : ePlayerType.Computer; }
        }

        private void checkBoxPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPlayer2.Enabled = !textBoxPlayer2.Enabled;
            if(textBoxPlayer2.Enabled)
            {
                textBoxPlayer2.Text = string.Empty;
            }
            else
            {
                textBoxPlayer2.Text = "Computer";
            }
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            if (isFormFulfilled())
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid input.");
            }
        }

        private bool isFormFulfilled()
        {
            return (textBoxPlayer1.Text != string.Empty && textBoxPlayer2.Text != string.Empty) ? true : false;
        }
    }
}
