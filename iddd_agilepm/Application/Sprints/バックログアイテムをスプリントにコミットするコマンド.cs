﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.AgilePM.Application.Sprints
{
    public class バックログアイテムをスプリントにコミットするコマンド
    {
        public バックログアイテムをスプリントにコミットするコマンド()
        {
        }

        public バックログアイテムをスプリントにコミットするコマンド(string tenantId, string backlogItemId, string sprintId)
        {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.SprintId = sprintId;
        }

        public string TenantId { get; set; }
        public string BacklogItemId { get; set; }
        public string SprintId { get; set;}
    }
}
