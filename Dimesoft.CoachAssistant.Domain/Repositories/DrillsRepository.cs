using System;
using System.Linq;
using System.Collections.Generic;
using Dimesoft.CoachAssistant.Domain.Models;
using Wintellect.Sterling.Keys;

namespace Dimesoft.CoachAssistant.Domain.Repositories
{
    public interface IDrillsRepository
    {
        void Save(PracticeDrillDto drill);
        IList<PracticeDrillDto> ForSport(SportType sportType);
    }

    public class DrillsRepository : IDrillsRepository
    {
        
        public void SetupInitialData()
        {
            var results = from drill in DB.Database.Query<PracticeDrillDto, int>().ToList()
                          select drill.LazyValue.Value;

            if (!results.Any())
            {
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { SportTypeId = (int)SportType.Soccer, Name = "1 on 1 Passing" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { SportTypeId = (int)SportType.Soccer, Name = "3 Person Passing" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { SportTypeId = (int)SportType.Soccer, Name = "4 Corner Passing" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { SportTypeId = (int)SportType.Soccer, Name = "Cone Dribbling" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { SportTypeId = (int)SportType.Soccer, Name = "Corner Kicks" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { SportTypeId = (int)SportType.Soccer, Name = "Throw Ins" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { SportTypeId = (int)SportType.Soccer, Name = "Goal Kicks" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { SportTypeId = (int)SportType.Soccer, Name = "Player Positioning" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { SportTypeId = (int)SportType.Soccer, Name = "Two-Foot Dribbling" });



                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 20, SportTypeId = (int)SportType.Baseball, Name = "Drill 10" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 21, SportTypeId = (int)SportType.Baseball, Name = "Drill 11" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 22, SportTypeId = (int)SportType.Baseball, Name = "Drill 12" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 23, SportTypeId = (int)SportType.Baseball, Name = "Drill 13" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 24, SportTypeId = (int)SportType.Baseball, Name = "Drill 14" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 25, SportTypeId = (int)SportType.Baseball, Name = "Drill 15" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 26, SportTypeId = (int)SportType.Baseball, Name = "Drill 16" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 27, SportTypeId = (int)SportType.Baseball, Name = "Drill 17" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 28, SportTypeId = (int)SportType.Baseball, Name = "Drill 18" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 29, SportTypeId = (int)SportType.Baseball, Name = "Drill 19" });

                DB.Database.Flush();
            }
        }

        public void Save(PracticeDrillDto drill)
        {
            try
            {
                DB.Database.Save<PracticeDrillDto>(drill);

                DB.Database.Flush();
            }
            catch (Exception e)
            {
                var x = e;
            }
            
        }

        public IList<PracticeDrillDto> ForSport(SportType sportType)
        {
            var results = from drill in DB.Database.Query<PracticeDrillDto, int, int>(CoachesDatabase.SportTypeDataIndex)
                          where drill.Index == (int)sportType
                          select drill.LazyValue.Value;
            
            return results.ToList();
        } 
    }
}