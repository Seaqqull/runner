using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace firstForms
{
    public partial class Form1 : Form
    {
        RunnerGame RunGame;
        FormSensor GameState, GameReset, GameUp, GameDown;
        public Form1()
        {
            InitializeComponent();
            RunGame = new RunnerGame(this, toolStripStatusLabel1, toolStripStatusLabel2);

            GameReset = new FormSensor();
            GameReset.SetShift(Width / 2 - GameReset.Width / 2, GameReset.Height + 25);
            GameReset.Owner = this;
            GameReset.BackColor = Color.Black;
            GameReset.FormBorderStyle = FormBorderStyle.None;
            GameReset.AllowTransparency = true;
            GameReset.TransparencyKey = GameReset.BackColor;
            GameReset.pictureBack.Image = new Bitmap(@"E:\KNU\3_Kurs\Projects\Runner c#\firstForms\Images\Reset.png");
            GameReset.pictureBack.Click += new System.EventHandler(this.NewGameToolStripMenuItem_Click);

            GameState = new FormSensor();
            GameState.SetShift(Width / 2 - GameState.Width / 2, Height - GameState.Height - 50);
            GameState.Owner = this;
            GameState.BackColor = Color.Black;
            GameState.FormBorderStyle = FormBorderStyle.None;
            GameState.AllowTransparency = true;
            GameState.TransparencyKey = GameState.BackColor;
            GameState.pictureBack.Image = new Bitmap(@"E:\KNU\3_Kurs\Projects\Runner c#\firstForms\Images\Jump.png", true);
            GameState.pictureBack.Click += new System.EventHandler(this.SpaceGame);

            GameUp = new FormSensor();
            GameUp.SetShift(-40 , (GameUp.Height + 350) / 2);
            GameUp.Owner = this;
            GameUp.BackColor = Color.White;
            GameUp.FormBorderStyle = FormBorderStyle.None;
            GameUp.AllowTransparency = true;
            GameUp.TransparencyKey = GameUp.BackColor;
            GameUp.pictureBack.Image = new Bitmap(@"E:\KNU\3_Kurs\Projects\Runner c#\firstForms\Images\Up.png");
            GameUp.pictureBack.Click += new System.EventHandler(this.MoveUp);

            GameDown = new FormSensor();
            GameDown.SetShift(this.Size.Width - 95, (GameDown.Height + 350) / 2);
            GameDown.Owner = this;
            GameDown.BackColor = Color.White;
            GameDown.FormBorderStyle = FormBorderStyle.None;
            GameDown.AllowTransparency = true;
            GameDown.TransparencyKey = GameDown.BackColor;
            GameDown.pictureBack.Image = new Bitmap(@"E:\KNU\3_Kurs\Projects\Runner c#\firstForms\Images\Down.png");
            GameDown.pictureBack.Click += new System.EventHandler(this.MoveDown);

            GameState.Show();
            GameReset.Show();
            GameUp.Show();
            GameDown.Show();
        }

        private void ResumeButton_Click(object sender, EventArgs e)
        {
            if (!RunGame.timer.Enabled)
            {
                RunGame.records = false;
                RunGame.StartGame();
            }
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (RunGame.timer.Enabled)
            {
                RunGame.records = false;
                RunGame.PauseGame();
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            RunGame.records = false;
            RunGame.ResetGame();
        }

        private void NewGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunGame.records = false;
            RunGame.ResetGame();
        }

        private void PauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RunGame.timer.Enabled)
            {
                PauseToolStripMenuItem.Text = "Продолжить";
                RunGame.records = false;
                RunGame.PauseGame();
            }
            else {
                PauseToolStripMenuItem.Text = "Пауза";
                RunGame.records = false;
                RunGame.StartGame();
            }
        }

        private void MoveUp(object sender, EventArgs e) {
            if (RunGame.timer.Enabled)
                RunGame.Move(RunGame.Shift, -RunGame.Size_cell_height);
        }
        private void MoveDown(object sender, EventArgs e) {
            if (RunGame.timer.Enabled)
                RunGame.Move(RunGame.Shift, RunGame.Size_cell_height);
        }
        private void SpaceGame(object sender, EventArgs e)
        {       
            if (RunGame.timer.Enabled && !RunGame.Is_fly)
            RunGame.Is_fly = true;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) {
                if (RunGame.timer.Enabled)
                    RunGame.Move(RunGame.Shift, -RunGame.Size_cell_height);                
            }
            else if (e.KeyCode == Keys.Down) {
                if (RunGame.timer.Enabled)
                    RunGame.Move(RunGame.Shift, RunGame.Size_cell_height);
            }
            else if (e.KeyCode == Keys.Space) {
                if (RunGame.timer.Enabled && !RunGame.Is_fly)
                    RunGame.Is_fly = true;
            }
        }

        private void RecordsButton_Click(object sender, EventArgs e)
        {
            if (!RunGame.records)
            {
                RecordsButton.Text = "Игра";
                RunGame.records = true;
                RunGame.PauseGame();
                RunGame.Show_Records();
            }
            else {
                RecordsButton.Text = "Рекорды";
                RunGame.records = false;
                RunGame.DrawField();
            }
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            GameState.Refresh();
            GameReset.Refresh();
            GameUp.Refresh();
            GameDown.Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!RunGame.timer.Enabled && RunGame.records) RunGame.Show_Records();            
        }
    }
}
