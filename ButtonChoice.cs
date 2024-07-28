using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace terra
{
    internal class ButtonChoice : Form
    {
        public Type SelectedType { get; private set; }
        public int SelectedLevelUp;
        public int SelectedBonusCard;
        public int strongholdLevelUp = 1;
        public int templesLevelUp = 2;
        public int selectedFavor = 0;
        protected Button buttonNomads;
        protected Button buttonWitches;
        protected Button buttonAlchemists;
        protected Button buttonStronghold;
        protected Button buttonTemples;
        protected Button buttonBonusCard1;
        protected Button buttonBonusCard2;
        protected Button buttonBonusCard3;
        protected Button buttonBonusCard4;
        protected Button buttonBonusCard5;
        protected Button buttonBonusCard6;
        protected Button buttonBonusCard7;
        protected Button buttonBonusCard8;
        protected Button buttonBonusCard9;
        protected Button buttonFavor1;
        protected Button buttonFavor2;
        protected Button buttonFavor3;
        protected Button buttonFavor4;
        protected Button buttonFavor5;
        protected Button buttonFavor6;
        protected Button buttonFavor7;
        protected Button buttonFavor8;
        protected Button buttonFavor9;
        protected Button buttonFavor10;
        protected Button buttonFavor11;
        protected Button buttonFavor12;
        public ButtonChoice()
        {

        }
        public void CreateCharacter()
        {
            buttonNomads = new Button { Text = "Nomads", Location = new System.Drawing.Point(30, 30) };
            buttonNomads.Click += (sender, e) => { SelectedType = typeof(Nomads); 
            this.DialogResult = DialogResult.OK; };

            buttonWitches = new Button { Text = "Witches", Location = new System.Drawing.Point(30, 70) };
            buttonWitches.Click += (sender, e) => { SelectedType = typeof(Witches); 
            this.DialogResult = DialogResult.OK; };

            buttonAlchemists = new Button { Text = "Alchemists", Location = new System.Drawing.Point(30, 110) };
            buttonAlchemists.Click += (sender, e) => { SelectedType = typeof(Alchemists); this.DialogResult = DialogResult.OK; };
            Controls.Add(buttonNomads);
            Controls.Add(buttonWitches);
            Controls.Add(buttonAlchemists);
        }
        public void ChooseLevelUp()
        {
            buttonStronghold = new Button { Text = "要塞", Location = new System.Drawing.Point(30, 150) };
            buttonStronghold.Click += (sender, e) => { SelectedLevelUp = strongholdLevelUp; this.DialogResult = DialogResult.OK; };

            buttonTemples = new Button { Text = "圓房", Location = new System.Drawing.Point(30, 190) };
            buttonTemples.Click += (sender, e) => { SelectedLevelUp = templesLevelUp; this.DialogResult = DialogResult.OK; };
            Controls.Add(buttonStronghold);
            Controls.Add(buttonTemples);
        }
        public void ChooseBonusCard(int[] card, int[] coin)
        {
            int cardNum = 0;
            if (Array.IndexOf(card, 1) != -1)
            {
                buttonBonusCard1 = new Button { Text = $"1({coin[Array.IndexOf(card, 1)]})", Location = new System.Drawing.Point(30, 30 + cardNum * 40) };
                buttonBonusCard1.Click += (sender, e) => { SelectedBonusCard = 1; this.DialogResult = DialogResult.OK; };
                Controls.Add(buttonBonusCard1);
                cardNum++;
            }
            if (Array.IndexOf(card, 2) != -1)
            {
                buttonBonusCard2 = new Button { Text = $"2({coin[Array.IndexOf(card, 2)]})", Location = new System.Drawing.Point(30, 30 + cardNum * 40) };
                buttonBonusCard2.Click += (sender, e) => { SelectedBonusCard = 2; this.DialogResult = DialogResult.OK; };
                Controls.Add(buttonBonusCard2);
                cardNum++;
            }
            if (Array.IndexOf(card, 3) != -1)
            {
                buttonBonusCard3 = new Button { Text = $"3({coin[Array.IndexOf(card, 3)]})", Location = new System.Drawing.Point(30, 30 + cardNum * 40) };
                buttonBonusCard3.Click += (sender, e) => { SelectedBonusCard = 3; this.DialogResult = DialogResult.OK; };
                Controls.Add(buttonBonusCard3);
                cardNum++;
            }
            if (Array.IndexOf(card, 4) != -1)
            {
                buttonBonusCard4 = new Button { Text = $"4({coin[Array.IndexOf(card, 4)]})", Location = new System.Drawing.Point(30, 30 + cardNum * 40) };
                buttonBonusCard4.Click += (sender, e) => { SelectedBonusCard = 4; this.DialogResult = DialogResult.OK; };
                Controls.Add(buttonBonusCard4);
                cardNum++;
            }
            if (Array.IndexOf(card, 5) != -1)
            {
                buttonBonusCard5 = new Button { Text = $"5({coin[Array.IndexOf(card, 5)]})", Location = new System.Drawing.Point(30, 30 + cardNum * 40) };
                buttonBonusCard5.Click += (sender, e) => { SelectedBonusCard =5; this.DialogResult = DialogResult.OK; };
                Controls.Add(buttonBonusCard5);
                cardNum++;
            }
            if (Array.IndexOf(card, 6) != -1)
            {
                buttonBonusCard6 = new Button { Text = $"6({coin[Array.IndexOf(card, 6)]})", Location = new System.Drawing.Point(30, 30 + cardNum * 40) };
                buttonBonusCard6.Click += (sender, e) => { SelectedBonusCard = 6; this.DialogResult = DialogResult.OK; };
                Controls.Add(buttonBonusCard6);
                cardNum++;
            }
            if (Array.IndexOf(card, 7) != -1)
            {
                buttonBonusCard7 = new Button { Text = $"7({coin[Array.IndexOf(card, 7)]})", Location = new System.Drawing.Point(30, 30 + cardNum * 40) };
                buttonBonusCard7.Click += (sender, e) => { SelectedBonusCard = 7; this.DialogResult = DialogResult.OK; };
                Controls.Add(buttonBonusCard7);
                cardNum++;
            }
            if (Array.IndexOf(card, 8) != -1)
            {
                buttonBonusCard8 = new Button { Text = $"8({coin[Array.IndexOf(card, 8)]})", Location = new System.Drawing.Point(30, 30 + cardNum * 40) };
                buttonBonusCard8.Click += (sender, e) => { SelectedBonusCard = 8; this.DialogResult = DialogResult.OK; };
                Controls.Add(buttonBonusCard8);
                cardNum++;
            }
            if (Array.IndexOf(card, 9) != -1)
            {
                buttonBonusCard9 = new Button { Text = $"9({coin[Array.IndexOf(card, 9)]})", Location = new System.Drawing.Point(30, 30 + cardNum * 40) };
                buttonBonusCard9.Click += (sender, e) => { SelectedBonusCard = 9; this.DialogResult = DialogResult.OK; };
                Controls.Add(buttonBonusCard9);
                cardNum++;
            }

        }
        public void ChooseFavor(int[] favorList, Character chr)
        {
            int favorNum = 0;
            int[] thisFavor = chr.ShowFavor();
            for(int i = 0; i<favorList.Length;i++)
            {
                if (favorList[i] != 0 && thisFavor[i] != 1)
                {
                    string buttonName = "buttonFavor";
                    buttonName += Convert.ToString(i);
                    Button button = this.Controls[buttonName] as Button;
                    button = new Button { Text = $"{i+1}({favorList[i]})", Location = new System.Drawing.Point(30, 30 + favorNum * 40) };
                    button.Click += (sender, e) => { selectedFavor = i; favorList[i]--; this.DialogResult = DialogResult.OK; };
                    Controls.Add(button);
                    favorNum++;
                }
                
            }
            
        }
    }
}
