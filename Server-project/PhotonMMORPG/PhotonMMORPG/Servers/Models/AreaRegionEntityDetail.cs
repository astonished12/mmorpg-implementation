﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;

namespace Servers.Models
{
    public class AreaRegionEntityDetail
    {
        public Vector Position { get; set; }
        public List<NpcCharacter> NpcCharacters { get; set; }
    }
}