using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using Servers.Models.Interfaces;

namespace Servers.Models.Factories
{
    public class QuestFactory : IFactory
    {
        public NpcFactory NpcFactory { get; protected set; }
        public Dictionary<int, Quest> Quests { get; set; }
        protected ILogger Log = LogManager.GetCurrentClassLogger();


        public QuestFactory(NpcFactory npcFactory, IEnumerable<Quest> quests)
        {
            NpcFactory = npcFactory;

            Quests = new Dictionary<int, Quest>();

            foreach (Quest quest in quests)
            {
                if (!Quests.ContainsKey(quest.Id))
                {
                    Quests.Add(quest.Id, quest);
                    Log.DebugFormat("Quest '{0}' registered successfully.", quest.Name);
                }
                else
                {
                    Log.WarnFormat("Quest '{0}' could not be registered. Id {1} allready exists", quest.Name,
                        quest.Id);
                }
            }
        }

    
    }
}
