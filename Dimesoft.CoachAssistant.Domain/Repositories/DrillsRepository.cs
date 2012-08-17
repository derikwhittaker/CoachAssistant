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
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 1, SportTypeId = (int)SportType.Soccer, Name = "1 on 1 Passing" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 2, SportTypeId = (int)SportType.Soccer, Name = "3 Person Passing" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 3, SportTypeId = (int)SportType.Soccer, Name = "4 Corner Passing" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 4, SportTypeId = (int)SportType.Soccer, Name = "Cone Dribbling" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 5, SportTypeId = (int)SportType.Soccer, Name = "Corner Kicks" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 6, SportTypeId = (int)SportType.Soccer, Name = "Throw Ins" });



                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 10, SportTypeId = (int)SportType.Baseball, Name = "Drill 10" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 11, SportTypeId = (int)SportType.Baseball, Name = "Drill 11" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 12, SportTypeId = (int)SportType.Baseball, Name = "Drill 12" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 13, SportTypeId = (int)SportType.Baseball, Name = "Drill 13" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 14, SportTypeId = (int)SportType.Baseball, Name = "Drill 14" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 15, SportTypeId = (int)SportType.Baseball, Name = "Drill 15" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 16, SportTypeId = (int)SportType.Baseball, Name = "Drill 16" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 17, SportTypeId = (int)SportType.Baseball, Name = "Drill 17" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 18, SportTypeId = (int)SportType.Baseball, Name = "Drill 18" });
                DB.Database.Save<PracticeDrillDto>(new PracticeDrillDto { Id = 19, SportTypeId = (int)SportType.Baseball, Name = "Drill 19" });

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