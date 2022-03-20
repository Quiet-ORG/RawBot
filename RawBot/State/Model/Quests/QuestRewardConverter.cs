﻿using Newtonsoft.Json;
using RawBot.State.Model.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RawBot.State.Model.Quests
{
    public class QuestRewardConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<ItemBase>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<Dictionary<string, Dictionary<int, ItemBase>>>(reader)?.Values.SelectMany(x => x.Values).ToList();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
