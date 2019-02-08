﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCommon.SerializedObjects
{
    [Serializable]
    public class Character
    {
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int Level { get; set; }
        public decimal ExperiencePoints { get; set; }


    }
}
