using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;

namespace Managers
{
    class NPCManager : Singleton<NPCManager>
    {
        public delegate bool NPCActionHandler(NPCDefine npc);
        Dictionary<NpcFunction, NPCActionHandler> eventMap = new Dictionary<NpcFunction, NPCActionHandler>();

        public void RegisterNPCEvent(NpcFunction function, NPCActionHandler action)
        {
            if (!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else eventMap[function] += action;
        }
        public NPCDefine GetNPCDefine(int npcId)
        {
            return DataManager.Instance.NPCs[npcId];
        }
        public bool Interactive(int npcId)
        {
            if (DataManager.Instance.NPCs.ContainsKey(npcId))
            {
                var npc = DataManager.Instance.NPCs[npcId];
                return Interactive(npc);
            }
            return false;
        }
        public bool Interactive(NPCDefine npc)
        {
            if (DoTaskInteractive(npc))
            {
                return true;
            }
            else if (npc.Type == NpcType.Functional)
            {
                return DoFunctionInteractive(npc);
            }
            return false;
        }
        private bool DoTaskInteractive(NPCDefine npc)
        {
            var status = QuestManager.Instance.GetQuestStatusByNpc(npc.ID);
            if (status == NPCQuestStatus.None)
                return false;
            return QuestManager.Instance.OpenNpcQuest(npc.ID);
        }
        private bool DoFunctionInteractive(NPCDefine npc)
        {
            if (npc.Type != NpcType.Functional)
                return false;
            if (!eventMap.ContainsKey(npc.Function))
                return false;
            return eventMap[npc.Function](npc);
        }
    }
}
