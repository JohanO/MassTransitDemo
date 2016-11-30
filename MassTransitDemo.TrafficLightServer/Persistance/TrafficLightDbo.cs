using System;
using System.Data.Linq.Mapping;
using System.Runtime.InteropServices;

using MassTransitDemo.TrafficLightServer.Domain;

namespace MassTransitDemo.TrafficLightServer.Persistance
{
    [Table(Name = "TrafficLight")]
    public class TrafficLightDbo
    {
        [Column(IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column]
        public string State { get; set; }

        public TrafficLight ToTrafficLight() => new TrafficLight
        {
            Id = Id,
            State = (State)Enum.Parse(typeof(State), State)
        };
    }
}