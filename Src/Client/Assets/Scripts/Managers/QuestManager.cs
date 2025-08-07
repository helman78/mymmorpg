using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace Managers
{
    public enum NPCQuestStatus
    {
        None = 0,//无任务
        Complete,//拥有已完成可提交任务
        Available,//拥有可接受任务
        Incomplete,//拥有未完成任务
    }
    class QuestManager:Singleton<QuestManager>
    {
        //所有有效任务
        public List<NQuestInfo> questInfos;
        public Dictionary<int, Quest> allQuests = new Dictionary<int, Quest>();
        public Dictionary<int, Dictionary<NPCQuestStatus, List<Quest>>> npcQuests = new Dictionary<int, Dictionary<NPCQuestStatus, List<Quest>>>();

        public UnityAction onQuestStatusChanged;

        public void Init(List<NQuestInfo> quests)
        {
            this.questInfos = quests;
            allQuests.Clear();
            this.npcQuests.Clear();
            InitQuests();
        }
        void InitQuests()
        {
            //初始化已有任务
            foreach(var info in this.questInfos)
            {
                Quest quest = new Quest(info);
                this.allQuests[quest.Info.QuestId] = quest;
            }
            //初始化可用任务
            this.CheckAvailableQuests();
            foreach(var kv in this.allQuests){
                this.AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                this.AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
            }
        }
        void CheckAvailableQuests()
        {
            //初始化可用任务
            foreach (var kv in DataManager.Instance.Quests)
            {
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != User.Instance.CurrentCharacter.Class)
                    continue;//不符合职业
                if (kv.Value.LimitLevel > User.Instance.CurrentCharacter.Level)
                    continue;//不符合等级
                if (this.allQuests.ContainsKey(kv.Key))
                    continue;//任务已经存在
                if (kv.Value.PreQuest > 0)
                {
                    Quest preQuest;
                    if (this.allQuests.TryGetValue(kv.Value.PreQuest, out preQuest))//获取前置任务
                    {
                        if (preQuest.Info == null)
                            continue;//前置任务未接取
                        if (preQuest.Info.Status != QuestStatus.Finished)
                            continue;//前置任务未完成
                    }
                    else continue;//前置任务还没接
                }
                Quest quest = new Quest(kv.Value);
                this.allQuests[quest.Define.ID] = quest;
            }
        }
        void AddNpcQuest(int npcId, Quest quest)
        {
            if (!this.npcQuests.ContainsKey(npcId))
                this.npcQuests[npcId] = new Dictionary<NPCQuestStatus, List<Quest>>();
            List<Quest> availables;
            List<Quest> complates;
            List<Quest> incomplates;
            if(!this.npcQuests[npcId].TryGetValue(NPCQuestStatus.Available, out availables))
            {
                availables = new List<Quest>();
                this.npcQuests[npcId][NPCQuestStatus.Available] = availables;
            }
            if(!this.npcQuests[npcId].TryGetValue(NPCQuestStatus.Complete, out complates))
            {
                complates = new List<Quest>();
                this.npcQuests[npcId][NPCQuestStatus.Complete] = complates;
            }
            if(!this.npcQuests[npcId].TryGetValue(NPCQuestStatus.Incomplete, out incomplates))
            {
                incomplates = new List<Quest>();
                this.npcQuests[npcId][NPCQuestStatus.Incomplete] = incomplates;
            }
            if(quest.Info == null)
            {
                if(npcId == quest.Define.AcceptNPC && !this.npcQuests[npcId][NPCQuestStatus.Available].Contains(quest))
                {
                    this.npcQuests[npcId][NPCQuestStatus.Available].Add(quest);
                }
            }
            else
            {
                if(quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.Complated)
                {
                    if (!this.npcQuests[npcId][NPCQuestStatus.Complete].Contains(quest))
                    {
                        this.npcQuests[npcId][NPCQuestStatus.Complete].Add(quest);
                    }
                }
                if(quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.InProgress)
                {
                    if (!this.npcQuests[npcId][NPCQuestStatus.Incomplete].Contains(quest))
                    {
                        this.npcQuests[npcId][NPCQuestStatus.Incomplete].Add(quest);
                    }
                }
            }
        }
        public NPCQuestStatus GetQuestStatusByNpc(int npcId)
        {
            Dictionary<NPCQuestStatus, List<Quest>> status = new Dictionary<NPCQuestStatus, List<Quest>>();
            if(this.npcQuests.TryGetValue(npcId, out status))//获取NPC任务
            {
                if (status[NPCQuestStatus.Complete].Count > 0)
                    return NPCQuestStatus.Complete;
                if (status[NPCQuestStatus.Available].Count > 0)
                    return NPCQuestStatus.Available;
                if (status[NPCQuestStatus.Incomplete].Count > 0)
                    return NPCQuestStatus.Incomplete;
            }
            return NPCQuestStatus.None;
        }
        public bool OpenNpcQuest(int npcId)
        {
            Dictionary<NPCQuestStatus, List<Quest>> status = new Dictionary<NPCQuestStatus, List<Quest>>();
            if(this.npcQuests.TryGetValue(npcId, out status))//获取NPC任务
            {
                if (status[NPCQuestStatus.Complete].Count > 0)
                    return ShowQuestDialog(status[NPCQuestStatus.Complete].First());
                if (status[NPCQuestStatus.Available].Count > 0)
                    return ShowQuestDialog(status[NPCQuestStatus.Available].First());
                if (status[NPCQuestStatus.Incomplete].Count > 0)
                    return ShowQuestDialog(status[NPCQuestStatus.Incomplete].First());
            }
            return false;
        }
        bool ShowQuestDialog(Quest quest)
        {
            if(quest.Info == null || quest.Info.Status == QuestStatus.Complated)
            {
                UIQuestDialog dlg = UIManager.Instance.Show<UIQuestDialog>();
                dlg.SetQuest(quest);
                dlg.OnClose += OnQuestDialogClose;
                return true;
            }
            if(quest.Info != null || quest.Info.Status == QuestStatus.Complated)
            {
                if (!string.IsNullOrEmpty(quest.Define.DialogIncomplete))
                    MessageBox.Show(quest.Define.DialogIncomplete);
            }
            return true;
        }
        void OnQuestDialogClose(UIWindow sender, UIWindow.WindowResult result)
        {
            UIQuestDialog dlg = (UIQuestDialog)sender;
            if(result == UIWindow.WindowResult.Yes)
            {
                if (dlg.quest.Info == null)
                    QuestService.Instance.SendQuestAccept(dlg.quest);
                else if (dlg.quest.Info.Status == QuestStatus.Complated)
                    QuestService.Instance.SendQuestSubmit(dlg.quest);
            }
            else if(result == UIWindow.WindowResult.No)
            {
                MessageBox.Show(dlg.quest.Define.DialogDeny);
            }
        }
        Quest RefreshQuestStatus(NQuestInfo quest)
        {
            this.npcQuests.Clear();
            Quest result;
            if (this.allQuests.ContainsKey(quest.QuestId))
            {
                //更新新的任务状态
                this.allQuests[quest.QuestId].Info = quest;
                result = this.allQuests[quest.QuestId];
            }
            else
            {
                result = new Quest(quest);
                this.allQuests[quest.QuestId] = result;
            }
            CheckAvailableQuests();
            foreach(var kv in this.allQuests){
                this.AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                this.AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
            }
            if (onQuestStatusChanged != null)
                onQuestStatusChanged();
            return result;
        }
        public void OnQuestAccepted(NQuestInfo info)
        {
            var quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogAccept);
        }
        public void OnQuestSubmited(NQuestInfo info)
        {
            var quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogFinish);
        }
    }
}
