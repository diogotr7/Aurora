using Aurora.Profiles.Fallout4.GSI.Nodes;
using System;

namespace Aurora.Profiles.Fallout4.GSI
{
    /// <summary>
    /// A class representing various information retaining to Game State Integration of Fallout 4
    /// </summary>
    public class GameState_Fallout4 : GameState<GameState_Fallout4>
    {
        private PlayerNode player;
        private StatusNode status;
        private SPECIALNode special;
        private StatsNode stats;
        private PipBoyColorNode pipBoyColor;

        public PlayerNode PlayerInfo => player ?? (player = new PlayerNode());

        public StatusNode Status => status ?? (status = new StatusNode());

        public SPECIALNode Special => special ?? (special = new SPECIALNode());

        public StatsNode Stats => stats ?? (stats = new StatsNode());

        public PipBoyColorNode PipBoyColor => pipBoyColor ?? (pipBoyColor = new PipBoyColorNode());

        /// <summary>
        /// Creates a default GameState_Fallout4 instance.
        /// </summary>
        public GameState_Fallout4() : base()
        {
        }

        /// <summary>
        /// Creates a GameState instance based on the passed json data.
        /// </summary>
        /// <param name="json_data">The passed json data</param>
        public GameState_Fallout4(string json_data) : base(json_data)
        {
        }

        /// <summary>
        /// A copy constructor, creates a GameState_Fallout4 instance based on the data from the passed GameState instance.
        /// </summary>
        /// <param name="other_state">The passed GameState</param>
        public GameState_Fallout4(IGameState other_state) : base(other_state)
        {
        }
    }
}
