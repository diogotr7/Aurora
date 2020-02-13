using Aurora.Utils;
using Aurora.Profiles.Fallout4.GSI;
using PipBoy;

namespace Aurora.Profiles.Fallout4
{
    public class GameEvent_Fallout4 : LightEvent {

        public override void ResetGameState()
        {
            _game_state = new GameState_Fallout4();
        }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnStop()
        {
            base.OnStart();
        }

        public override void UpdateTick()
        {
            base.UpdateTick();
        }
    }
}
