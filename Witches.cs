using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terra
{
    internal class Witches : Character
    {
        protected bool strongholdRightUsable = false;
        public Witches(int playerNO) : base(playerNO)
        {

        }
        public override void StartingSet()
        {
            name = "女巫";
            worker = 3;
            coin = 15;
            occupyingTable = [3, 0, 2, 3, 1, 2, 1];
            priestsBurning[3] = 2;
            color = Color.DarkOliveGreen;
        }
        public override void StartingRight()
        {
            score += 5;
        }
        public new void StrongholdLevelUp()
        {
            base.StrongholdLevelUp();
            strongholdRightUsable = true;
        }
        public override void StrongholdRight()
        {

        }
        public override void ShowMessage()
        {
            MessageBox.Show("Witches");
        }
    }
}
