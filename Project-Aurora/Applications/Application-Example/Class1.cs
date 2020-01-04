using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Profiles;

namespace Application_Example
{
    public class Example : Application
    {
        public Example()
            : base(
                  new LightEventConfig {
                      Name = "Example",
                      ID = "example",
                      ProcessNames = new[] { "chrome.exe" },
                      ProfileType = typeof(WrapperProfile),
                      OverviewControlType = typeof(Aurora.Profiles.Desktop.Control_Desktop),
                      GameStateType = typeof(GameState),
                      Event = new GameEvent_Generic(),
                      IconURI = $"pack://application:,,,/{typeof(Example).Assembly.GetName().Name};component/Resources/icon.png"
                  })
        {        }
    }
}
