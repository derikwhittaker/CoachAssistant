using System.Collections.Generic;
using Dimesoft.CoachAssistant.Domain.Models;
using Wintellect.Sterling;
using Wintellect.Sterling.Database;
using Wintellect.Sterling.IsolatedStorage;
using Wintellect.Sterling.Serialization;

namespace Dimesoft.CoachAssistant.Domain.Repositories
{
    public class DB
    {

        private static SterlingEngine _engine;
        private static ISterlingDatabaseInstance _database;
        private static SterlingDefaultLogger _logger = null;

        public void ActivateEngine()
        {
            
            _engine = new SterlingEngine();
            _engine.SterlingDatabase.RegisterSerializer<DefaultSerializer>();
            _logger = new SterlingDefaultLogger(SterlingLogLevel.Information);
            _engine.Activate();

            _database = _engine.SterlingDatabase.RegisterDatabase<CoachesDatabase>(new IsolatedStorageDriver());

            _database.RegisterTrigger(new IdentityTrigger<EventDto>(_database));
            _database.RegisterTrigger(new IdentityTrigger<PracticeDrillDto>(_database));
            _database.RegisterTrigger(new IdentityTrigger<TeamDto>(_database));
            _database.RegisterTrigger(new IdentityTrigger<LocationDto>(_database));
            _database.RegisterTrigger(new IdentityTrigger<PlayerDto>(_database));
            _database.RegisterTrigger(new IdentityTrigger<SportDto>(_database));
            
        }

        public void DeactivateEngine()
        {
            _logger.Detach();
            _engine.Dispose();
            _database = null;
            _engine = null;
        }
        
        public static ISterlingDatabaseInstance Database
        {
            get { return _database; }
        }
    }

    public class CoachesDatabase : BaseDatabaseInstance
    {
        public const string SportTypeDataIndex = "SportTypeIndex";
        public const string EventTypeDataIndex = "EventTypeIndex";
        
        public override string Name
        {
            get { return "CoachesAssistantDatabase"; }
        }

        protected override List<ITableDefinition> RegisterTables()
        {            
            return new List<ITableDefinition>
                       {

                           CreateTableDefinition<EventDto, int>(model => model.Id)
                                .WithIndex<EventDto, int, int>(SportTypeDataIndex, t => t.SportTypeId)
                                .WithIndex<EventDto, int, int>(EventTypeDataIndex, t => t.EventTypeId),
                               
                           CreateTableDefinition<PracticeDrillDto, int>(testModel => testModel.Id)
                               .WithIndex<PracticeDrillDto, int, int>(SportTypeDataIndex, t => t.SportTypeId),
                           
                           CreateTableDefinition<TeamDto, int>(testModel => testModel.Id),

                           CreateTableDefinition<LocationDto, int>(testModel => testModel.Id),

                           CreateTableDefinition<PlayerDto, int>(testModel => testModel.Id),

                           CreateTableDefinition<SportDto, int>(testModel => testModel.Id),
                           
                       };
        }
    }
    
}
