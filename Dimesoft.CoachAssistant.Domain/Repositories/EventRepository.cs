﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dimesoft.CoachAssistant.Domain.Models;

namespace Dimesoft.CoachAssistant.Domain.Repositories
{
    public interface IEventRepository
    {
        void Save(PracticeEventDto eventDto);

        IList<TeamDto> Teams();

        IList<PracticeEventDto> All();
        IList<PracticeEventDto> Open();
        IList<PracticeEventDto> Completed();
        PracticeEventDto PracticeEvent(int eventId);
    }

    public class EventRepository : IEventRepository
    {
        public void SetupInitialData()
        {
            var results = from drill in DB.Database.Query<PracticeEventDto, int>().ToList()
                          select drill.LazyValue.Value;

            if (!results.Any())
            {
                DB.Database.Save<PracticeEventDto>(new PracticeEventDto
                                               {
                                                   Id = 1,
                                                   Date = DateTime.Now,
                                                   Time = DateTime.Now.AddHours(1),
                                                   EventTypeId = (int) EventType.Practice,
                                                   SportTypeId = (int)SportType.Soccer,
                                                   Completed = false,
                                                   Team = new TeamDto {Id = 1},
                                                   Location = new LocationDto {Id = 1, Name = "Practice Field 1"},
                                                   PracticeDrills = new List<PracticeDrillDto>()
                                               });
                DB.Database.Save<PracticeEventDto>(new PracticeEventDto
                                               {
                                                   Id = 2,
                                                   Date = DateTime.Now.AddDays(1),
                                                   Time = DateTime.Now.AddHours(1),
                                                   EventTypeId = (int)EventType.Practice,
                                                   SportTypeId = (int)SportType.Baseball,
                                                   Completed = false,
                                                   Team = new TeamDto {Id = 2},
                                                   Location = new LocationDto {Id = 1, Name = "Practice Field 2"},
                                                   PracticeDrills = new List<PracticeDrillDto>
                                                                        {
                                                                            new PracticeDrillDto
                                                                                {
                                                                                    Id = 1,
                                                                                    Name = "Drill 1",
                                                                                    SportTypeId = (int)SportType.Soccer,
                                                                                    Sequence = 1
                                                                                }
                                                                        }
                                               });

                DB.Database.Save<TeamDto>(new TeamDto { Id = 1, Name = "Sparks" });
                DB.Database.Save<TeamDto>(new TeamDto { Id = 2, Name = "Braves" });

                DB.Database.Flush();
            }
        }

        public void Save(PracticeEventDto eventDto )
        {
            try
            {
                DB.Database.Save<PracticeEventDto>(eventDto);

                DB.Database.Flush();
            }
            catch (Exception e)
            {
                var x = e;
            }
        }

        public IList<PracticeEventDto> All()
        {
            var results = from drill in DB.Database.Query<PracticeEventDto, int>()
                          select drill.LazyValue.Value;


            return results.ToList();
        }

        public IList<TeamDto> Teams()
        {
            var results = from team in DB.Database.Query<TeamDto, int>()
                          select team.LazyValue.Value;


            return results.ToList();
        } 

        public IList<PracticeEventDto> Open()
        {
            var events = All().Where(x => x.Completed == false);

            return events.ToList();
        }

        public IList<PracticeEventDto> Completed()
        {
            var events = All().Where(x => x.Completed);

            return events.ToList();
        }

        public PracticeEventDto PracticeEvent(int eventId)
        {
            var found = All().FirstOrDefault(x => x.Id == eventId);
            
            return found;
        }
    }
}