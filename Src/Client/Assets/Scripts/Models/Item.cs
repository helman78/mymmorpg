﻿using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    class Item
    {
        public int Id;
        public int Count;
        public ItemDefine Define;

        public Item(NItemInfo item) : this(item.Id, item.Count) { }
        public Item(int id,int count)
        {
            this.Id = id;
            this.Count = count;
            this.Define = DataManager.Instance.Items[this.Id];
        }
        public override string ToString()
        {
            return string.Format("Id:{0},Count:{1}", this.Id, this.Count);
        }
        public void Add(int count)
        {
            this.Count += count;
        }
        public void Remove(int count)
        {
            this.Count -= count;
        }
    }
}
