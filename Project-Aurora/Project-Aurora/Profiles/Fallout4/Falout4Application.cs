using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Profiles.Fallout4
{
    public class Falout4 : Application
    {
        public Falout4() : base(new LightEventConfig()
        {
            Name = "Fallout 4",
            ID = "fallout4",
            AppID = "377160",
            ProcessNames = new[] { "fallout4.exe" },
            ProfileType = typeof(Fallout4Profile),
            OverviewControlType = typeof(Control_Fallout4),
            GameStateType = typeof(GSI.GameState_Fallout4),
            Event = new GameEvent_Fallout4(),
            IconURI = "Resources/fallout4_256x256.png"
        })
        {

        }
    }
}
