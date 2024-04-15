using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GymManagement.Contracts.Subscriptions
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubscriptionType
    {
        Free,
        Starter,
        Pro
    }
}
