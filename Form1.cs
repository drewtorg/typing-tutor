using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace cs2412proj1
{
    public partial class Form1 : Form
    {
        //Many parallel arrays which hold the information for all the goes on with a single prompt
        //I probably could put this in an object that would synchronize the data, but that's too easy
        private List<string> Prompts { get; set; }
        private List<string> UserInput { get; set; }
        private List<int> Incorrect { get; set; }
        private List<int> Correct{ get; set; }
        private List<int> CurrentChar { get; set; }

        private int currentPrompt;
        private const int MAX_PROMPTS = 9;
        private Dictionary<char, int> MissedKeys;
        System.Media.SoundPlayer player;

        public Form1()
        {
            InitializeComponent();

            this.colorComboBox.Text = "Default";

            player = new System.Media.SoundPlayer();
            player.SoundLocation = "Mummyf---er.wav";
            player.PlayLooping();

            UserInput = new List<string>();
            Incorrect = new List<int>();
            Correct = new List<int>();
            CurrentChar = new List<int>();
            currentPrompt = 0;

            for (int i = 0; i < 5; i++)
                missedListBox.Items.Add("");

            //read the prompts into the prompts list
            Prompts = new List<string>();
            TextReader reader = File.OpenText("prompts.txt");

            //initialize all our lists
            for (int i = 0; i < MAX_PROMPTS; i++)
            {
                Prompts.Add(reader.ReadLine());
                UserInput.Add("");
                Incorrect.Add(0);
                Correct.Add(0);
                CurrentChar.Add(0);
            }

            reader.Close();

            Prompts.Add("");
            UserInput.Add("");
            Incorrect.Add(0);
            Correct.Add(0);
            CurrentChar.Add(0);

            //initialize the MissedKeys Dictionary
            MissedKeys = new Dictionary<char, int>();
            promptTextBox.Text = Prompts[0];
            leftButton.Enabled = false;
        }

        //when we press a key while focused on the input text box
        //check to see if the correct character is typed, if so, increment the correct, if not increment the incorrect
        private void inputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //do this only if the current character we are looking at is within the prompt length
            if (CurrentChar[currentPrompt] < Prompts[currentPrompt].Length)
            {
                char current = Prompts[currentPrompt][CurrentChar[currentPrompt]];

                //if user input matches the character in the prompt
                if (e.KeyChar == current)
                    Correct[currentPrompt]++;
                else
                {
                    Incorrect[currentPrompt]++;
                    int numMissed;
                    //add the missed char to the dictionary or increment it if it is already there
                    if (MissedKeys.TryGetValue(current, out numMissed))
                    {
                        numMissed++;
                        MissedKeys.Remove(current);
                        MissedKeys.Add(current, numMissed);
                    }
                    else
                        MissedKeys.Add(current, 1);

                    UpdateMostMissed();
                }

                UpdateStats(currentPrompt);

                CurrentChar[currentPrompt]++;


            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            //on key down, make button azure

            if (Control.ModifierKeys == Keys.Shift)
            {
                lshiftButton.BackColor = Color.Azure;
                rshiftButton.BackColor = Color.Azure;
            }

            ChangeKeyColor(e.KeyCode, Color.Azure);
        }

        private void OnKeyRelease(object sender, KeyEventArgs e)
        {
            //on key up, make button back color again

            if (Control.ModifierKeys != Keys.Shift)
            {
                lshiftButton.BackColor = this.BackColor;
                rshiftButton.BackColor = this.BackColor;
            }

            ChangeKeyColor(e.KeyCode, this.BackColor);

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //basically intercepts the tab key and changes it's color
            if (keyData == Keys.Tab)
            {
                if (tabButton.BackColor == Color.Azure)
                    ChangeKeyColor(Keys.Tab, this.BackColor);
                else
                    ChangeKeyColor(Keys.Tab, Color.Azure);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ChangeKeyColor(Keys key, Color color)
        {
            //worst switch statement ever

            switch (key)
            {
                //letters

                case Keys.Q: qButton.BackColor = color; break;
                case Keys.W: wButton.BackColor = color; break;
                case Keys.E: eButton.BackColor = color; break;
                case Keys.R: rButton.BackColor = color; break;
                case Keys.T: tButton.BackColor = color; break;
                case Keys.Y: yButton.BackColor = color; break;
                case Keys.U: uButton.BackColor = color; break;
                case Keys.I: iButton.BackColor = color; break;
                case Keys.O: oButton.BackColor = color; break;
                case Keys.P: pButton.BackColor = color; break;
                case Keys.A: aButton.BackColor = color; break;
                case Keys.S: sButton.BackColor = color; break;
                case Keys.D: dButton.BackColor = color; break;
                case Keys.F: fButton.BackColor = color; break;
                case Keys.G: gButton.BackColor = color; break;
                case Keys.H: hButton.BackColor = color; break;
                case Keys.J: jButton.BackColor = color; break;
                case Keys.K: kButton.BackColor = color; break;
                case Keys.L: lButton.BackColor = color; break;
                case Keys.Z: zButton.BackColor = color; break;
                case Keys.X: xButton.BackColor = color; break;
                case Keys.C: cButton.BackColor = color; break;
                case Keys.V: vButton.BackColor = color; break;
                case Keys.B: bButton.BackColor = color; break;
                case Keys.N: nButton.BackColor = color; break;
                case Keys.M: mButton.BackColor = color; break;

                //numbers

                case Keys.D1: button1.BackColor = color; break;
                case Keys.D2: button2.BackColor = color; break;
                case Keys.D3: button3.BackColor = color; break;
                case Keys.D4: button4.BackColor = color; break;
                case Keys.D5: button5.BackColor = color; break;
                case Keys.D6: button6.BackColor = color; break;
                case Keys.D7: button7.BackColor = color; break;
                case Keys.D8: button8.BackColor = color; break;
                case Keys.D9: button9.BackColor = color; break;
                case Keys.D0: button0.BackColor = color; break;

                //symbols

                case Keys.Oemtilde: tildeButton.BackColor = color; break;
                case Keys.Oemcomma: commaButton.BackColor = color; break;
                case Keys.OemPeriod: periodButton.BackColor = color; break;
                case Keys.OemQuestion: forwardslashButton.BackColor = color; break;
                case Keys.OemSemicolon: colonButton.BackColor = color; break;
                case Keys.OemQuotes: quoteButton.BackColor = color; break;
                case Keys.OemOpenBrackets: lbraceButton.BackColor = color; break;
                case Keys.OemCloseBrackets: rbraceButton.BackColor = color; break;
                case Keys.OemMinus:
                    {
                        if (Control.ModifierKeys == Keys.Shift)
                            { underscoreButton.BackColor = color; break; }
                        minusButton.BackColor = color; break;
                    }
                case Keys.Oemplus:
                    {
                        if (Control.ModifierKeys == Keys.Shift) 
                            { plusButton.BackColor = color; break; }
                        equalsButton.BackColor = color; break;
                    }

                //other buttons

                case Keys.Tab: tabButton.BackColor = color; break;
                case Keys.Back: backspaceButton.BackColor = color; break;
                case Keys.CapsLock: capsButton.BackColor = color; break;
                case Keys.Space: spacebarButton.BackColor = color; break;
                case Keys.Enter: enterButton.BackColor = color; break;
            }
        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            //prep for the next prompt
            if (currentPrompt < MAX_PROMPTS - 1)
                ShowNextPrompt();

            //print the results screen
            else
                ShowFinalScreen();
        }

        private void leftButton_Click(object sender, EventArgs e)
        {
            ShowPrevPrompt();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            RestartPrompt();
        }

        private void RestartPrompt()
        {
            //reinitialize the user input and the textbox
            UserInput[currentPrompt] = "";
            inputTextBox.Text = "";
            inputTextBox.Enabled = true;
            Incorrect[currentPrompt] = 0;
            Correct[currentPrompt] = 0;
            CurrentChar[currentPrompt] = 0;

            UpdateStats(currentPrompt);
        }

        private void UpdateStats(int index)
        {
            //change all the labels at the top to show the right stuff

            incorrectNumLabel.Text = Incorrect[index].ToString();
            correctNumLabel.Text = Correct[index].ToString();
            double accuracy = (double)Correct[index] / (double)Prompts[index].Length;
            accuracyNumLabel.Text = accuracy.ToString("P");
            currNumLabel.Text = (currentPrompt + 1).ToString();
        }

        private void UpdateFinalStats()
        {
            //chnge the labels to reflect that we are at the end

            int totalIncorrect = 0;
            int totalCorrect = 0;
            int totalLength = 0;
            double totalAccuracy = 0;

            for(int i = 0; i < MAX_PROMPTS; i++)
            {
                totalIncorrect += Incorrect[i];
                totalCorrect += Correct[i];
                totalLength += Prompts[i].Length;
            }

            totalAccuracy = (double)totalCorrect / (double)totalLength;

            incorrectNumLabel.Text = totalIncorrect.ToString();
            correctNumLabel.Text = totalCorrect.ToString();
            accuracyNumLabel.Text = totalAccuracy.ToString("P");
        }

        //opens the prompt text box to user edits so they can ake custom prompts
        private void editPromptButton_Click(object sender, EventArgs e)
        {
            if(editPromptButton.Text == "Edit Prompt")
            {
                promptTextBox.Enabled = true;
                inputTextBox.Enabled = false;
                restartButton.Enabled = false;
                editPromptButton.Text = "Save Prompt";
                
            }
            else
            {
                promptTextBox.Enabled = false;
                inputTextBox.Enabled = true;
                restartButton.Enabled = true;
                editPromptButton.Text = "Edit Prompt";
                Prompts[currentPrompt] = promptTextBox.Text;
                RestartPrompt();
            }
        }

        private void UpdateMostMissed()
        {
            //get the top 5 most mist in the dictionary and print to the listbox
            int prevMaxVal = int.MaxValue;
            for(int i = 0; i < 5; i++)
            {
                int currMaxVal = -1;
                string currMaxKey = "";
                foreach(KeyValuePair<char, int> missedKey in MissedKeys)
                {
                    if (missedKey.Value > currMaxVal && missedKey.Value < prevMaxVal)
                    {
                        currMaxVal = missedKey.Value;
                        currMaxKey = missedKey.Key.ToString();
                    }
                }
                if (currMaxVal != -1)
                {
                    if (currMaxKey == " ")
                        currMaxKey = "Space";
                    this.missedListBox.Items[i] = (i + 1) + ". " + currMaxKey + " : " + currMaxVal + " times";
                }
                prevMaxVal = currMaxVal;
            }
        }

        private void ShowNextPrompt()
        {
            //save off the current user input
            UserInput[currentPrompt] = inputTextBox.Text;

            leftButton.Enabled = true;
            inputTextBox.Enabled = true;

            inputTextBox.Text = UserInput[++currentPrompt];
            promptTextBox.Text = Prompts[currentPrompt];

            UpdateStats(currentPrompt);
        }

        private void ShowPrevPrompt()
        {
            //allows the user to look at and redo past prompts
            if (currentPrompt > 1)
            {
                incorrectLabel.Text = "Incorrect: ";
                correctLabel.Text = "Correct: ";
                accuracyLabel.Text = "Accuracy: ";

                //save off current user input
                UserInput[currentPrompt] = inputTextBox.Text;

                inputTextBox.Enabled = true;
                rightButton.Enabled = true;
                restartButton.Enabled = true;
                editPromptButton.Enabled = true;
                rickLinkLabel.Visible = false;

                inputTextBox.Text = UserInput[--currentPrompt];
                promptTextBox.Text = Prompts[currentPrompt];


                UpdateStats(currentPrompt);
            }
            else
            {
                leftButton.Enabled = false;

                //save off current user input
                UserInput[currentPrompt] = inputTextBox.Text;

                inputTextBox.Text = UserInput[--currentPrompt];
                promptTextBox.Text = Prompts[currentPrompt];

                UpdateStats(currentPrompt);
            }
        }

        private void ShowFinalScreen()
        {
            incorrectLabel.Text = "Total Incorrect: ";
            correctLabel.Text = "Total Correct: ";
            accuracyLabel.Text = "Total Accuracy: ";

            //save off the current user input
            UserInput[currentPrompt] = inputTextBox.Text;
            inputTextBox.Text = UserInput[++currentPrompt];
            promptTextBox.Text = Prompts[currentPrompt];

            inputTextBox.Enabled = false;
            rightButton.Enabled = false;
            restartButton.Enabled = false;
            editPromptButton.Enabled = false;
            rickLinkLabel.Visible = true;

            UpdateFinalStats();
        }

        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {

            //when we hit the end of a prompt, switch to the next prompt
            if (CurrentChar[currentPrompt] == Prompts[currentPrompt].Length)
            {
                CurrentChar[currentPrompt]++;

                if (currentPrompt < MAX_PROMPTS - 1)
                    ShowNextPrompt();
                else if(currentPrompt == MAX_PROMPTS - 1)
                    ShowFinalScreen();
            }
            else if(CurrentChar[currentPrompt] > Prompts[currentPrompt].Length)
                this.inputTextBox.Enabled = false;
        }

        private void rickLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //rick roll the poor saps
            this.rickLinkLabel.LinkVisited = true;
            player.Stop();
            System.Diagnostics.Process.Start("http://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        private void colorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //change the colors to be more (or less) beautiful
            Color color = Color.Salmon;

            switch(colorComboBox.SelectedIndex)
            {
                case 0:
                    color = DefaultBackColor;
                    break;
                case 1:
                    color = Color.Linen;
                    break;
                case 2:
                    color = Color.MistyRose;
                    break;
                case 3:
                    color = Color.PaleGoldenrod;
                    break;
                case 4:
                    color = Color.PapayaWhip;
                    break;
                case 5:
                    color = Color.Silver;
                    break;
                case 6:
                    color = Color.PowderBlue;
                    break;
            }

            //keep all the button representing the keyboard the normal back color
            //change the rest of the things to be the new color
            this.BackColor = color;
            foreach (Control c in this.Controls)
            {
                if(c is Button && !c.Enabled)
                    c.BackColor = color;
            }

            //keep these certain buttons the same color, it looks best this way
            this.leftButton.BackColor = DefaultBackColor;
            this.rightButton.BackColor = DefaultBackColor;
            this.restartButton.BackColor = DefaultBackColor;
            this.editPromptButton.BackColor = DefaultBackColor;
        }
    }
}
