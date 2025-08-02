using Common.Data;
using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Models
{
    class User : Singleton<User>
    {
        SkillBridge.Message.NUserInfo userInfo;


        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }


        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }
        public MapDefine CurrentMapData { get; set; }
        public GameObject CurrentCharacterObject { get; set; }
    
        public void AddGold(int gold)
        {
            this.CurrentCharacter.Gold += gold;
        }
        //public void AddItem(int itemId, int count)
        //{
        //    Item item = null;
        //    if(User.Instance.CurrentCharacter.itemItemManager.Instance.Items.TryGetValue(itemId, out item))
        //    {
        //        item.Add(count);
        //    }
        //    else
        //    {
        //        //添加新道具逻辑
        //    }
        //}
    }
}
