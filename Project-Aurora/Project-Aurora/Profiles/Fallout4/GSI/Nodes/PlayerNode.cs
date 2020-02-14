namespace Aurora.Profiles.Fallout4.GSI.Nodes
{
    /// <summary>
    /// Class representing player information
    /// </summary>
    public class PlayerNode : Node<PlayerNode>
    {
        public string PlayerName = "";
        public int XPLevel = 0;
        public float XPProgressPct = 0.0f;
        public float MaxHP = 0.0f;
        public float CurrHP = 0.0f;
        public float CurrentHPGain = 0.0f;
        public float MaxAP = 0.0f;
        public float CurrAP = 0.0f;
        public float MaxWeight = 0.0f;
        public float CurrentWeight = 0.0f;
        public uint PerkPoints = 0;
        public int Caps = 0;
        public uint DateYear = 0;
        public uint DateMonth = 0;
        public byte DateDay = 0;
        public float TimeHour = 0.0f;
    }
}