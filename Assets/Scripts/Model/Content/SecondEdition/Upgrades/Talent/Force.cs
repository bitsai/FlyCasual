﻿using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class Force : GenericUpgrade
    {
        public Force() : base()
        {
            IsHidden = !DebugManager.FreeMode;

            UpgradeInfo = new UpgradeCardInfo(
				"Force",
                UpgradeType.Talent,
                cost: 0,
                addForce: 1
            );

            //TODO add image
            //ImageUrl = "https://i.imgur.com/nvHEwLO.png";
        }
    }
}