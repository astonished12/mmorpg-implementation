using Photon.MmoDemo.Common;

namespace GameCommon.SerializedObjects
{
    public class NpcCharacter
    {
        public NpcTemplate NpcTemplate { get; set; }
        public Vector Position { get; set; }
        public Vector StartPosition { get; set; }

    }
}
