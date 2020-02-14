using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Profiles.Fallout4.GSI.Nodes
{
    /// <summary>
    /// Class representing game stats information
    /// </summary>
    public class StatsNode : Node<StatsNode>
    {    
        public uint RadawayCount = 0;
        public uint StimpakCount = 0;     
        public float HeadCondition = 0;
        public float LArmCondition = 0;
        public float RArmCondition = 0;
        public float TorsoCondition = 0;
        public float LLegCondition = 0;
        public float RLegCondition = 0;
    }
}
