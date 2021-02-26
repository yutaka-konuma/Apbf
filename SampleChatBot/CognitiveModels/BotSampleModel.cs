// <copyright file="BotSampleModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Microsoft.BotBuilderSamples
{
    using System.Collections.Generic;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.AI.Luis;
    using Newtonsoft.Json;

    public partial class BotSampleModel : IRecognizerConvert
    {
        public enum Intent
        {
            SearchOrder,
            None,
        }

        public Dictionary<Intent, IntentScore> Intents;

        public class BotSampleEntities
        {
            public string[] OrderNumber;

            // Lists
            public string[][] SelectItem;

            // Instance
            public class BitSampleInstance
            {
                public InstanceData[] OrderNumber;
                public InstanceData[] SelectItem;
            }

            [JsonProperty("$instance")]
            public BitSampleInstance bitSampleInstance;
        }

        public BotSampleEntities Entities;

        [JsonExtensionData(ReadData = true, WriteData = true)]
        public IDictionary<string, object> Properties { get; set; }

        public void Convert(dynamic result)
        {
            var app = JsonConvert.DeserializeObject<BotSampleModel>(
                JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            this.Intents = app.Intents;
            this.Entities = app.Entities;
        }

        public (Intent intent, double score) TopIntent()
        {
            Intent maxIntent = Intent.None;
            var max = 0.0;
            foreach (var entry in this.Intents)
            {
                if (entry.Value.Score > max)
                {
                    maxIntent = entry.Key;
                    max = entry.Value.Score.Value;
                }
            }

            return (maxIntent, max);
        }
    }
}
