using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using MassTransitDemo.TrafficLightServer.Domain;

namespace MassTransitDemo.TrafficLightServer.Persistance
{
    public class Repository : IRepository
    {
        private readonly DataContext _db;

        public Repository()
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filepath = Path.Combine(directory, "Persistance", "Database.mdf");
            _db = new DataContext(filepath);
        }

        public bool Exists(int id) => TrafficLightTable().Any(x => x.Id == id);

        public TrafficLight FindById(int id) => TrafficLightTable().SingleOrDefault(x => x.Id == id)?.ToTrafficLight();

        public void Save(TrafficLight trafficLight)
        {
            var table = _db.GetTable<TrafficLightDbo>();
            var dbo = table.SingleOrDefault(x => x.Id == trafficLight.Id);
            if (dbo == null)
            {
                dbo = new TrafficLightDbo { Id = trafficLight.Id, State = Enum.GetName(typeof(State), trafficLight.State) };
                table.InsertOnSubmit(dbo);
            }
            else
            {
                dbo.State = Enum.GetName(typeof(State), trafficLight.State);
            }

            _db.SubmitChanges();
        }

        public IEnumerable<TrafficLight> FindAll() => TrafficLightTable().Select(x => x.ToTrafficLight());

        private Table<TrafficLightDbo> TrafficLightTable() => _db.GetTable<TrafficLightDbo>();
    }
}