using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Profiles.Fallout4.GSI.Nodes
{
    /// <summary>
    /// Class representing game status information
    /// </summary>
    public class StatusNode : Node<StatusNode>
    {
        public bool IsLoading = false;
        public bool IsPlayerDead = false;
        public bool IsInVatsPlayback = false;
        public bool IsDataUnavailable = false;
        public bool IsMenuOpen = false;
        public bool IsPlayerMovementLocked = false;
        public bool IsInAutoVanity = false;
        public bool IsPlayerPipboyLocked = false;
        public bool IsPlayerInDialogue = false;
        public bool IsInVats = false;
        public bool IsInAnimation = false;
        public bool IsPipboyNotEquipped = false;
    }
}
