using System;
using System.Collections.Generic;
using System.Linq;
using Dimesoft.CoachAssistant.Domain.Models;

namespace Dimesoft.CoachAssistant.Domain.Repositories
{
    public interface IEventRepository
    {
        void Save(EventDto eventDto);
        void Save(TeamDto teamDto);
        void Save(LocationDto locationDto);

        IList<TeamDto> Teams();

        IList<LocationDto> Locations();

        IList<EventDto> All();
        IList<EventDto> Open();
        IList<EventDto> Completed();
        EventDto PracticeEvent(int eventId);
    }

    public class EventRepository : IEventRepository
    {
        public void SetupInitialData()
        {
            var results = from drill in DB.Database.Query<EventDto, int>().ToList()
                          select drill.LazyValue.Value;

            if (!results.Any())
            {

                DB.Database.Save<EventDto>(new EventDto
                                               {
                                                   Date = new DateTime(2012, 8, 20, 17, 30, 0),
                                                   EventTypeId = (int) EventType.Practice,
                                                   SportTypeId = (int)SportType.Soccer,
                                                   Completed = false,
                                                   Team = new TeamDto {Id = 1},
                                                   Location = new LocationDto {Id = 1},
                                                   PracticeDrills = new List<PracticeDrillDto>()
                                               });

                DB.Database.Save<EventDto>(new EventDto
                                                       {
                                                           Date = new DateTime(2012, 8, 25, 13, 15, 0),
                                                           EventTypeId = (int) EventType.Game,
                                                           SportTypeId = (int) SportType.Soccer,
                                                           Completed = false,
                                                           Team = new TeamDto {Id = 1},
                                                           Opponent = new TeamDto { Id = 3 },
                                                           Location = new LocationDto {Id = 4},
                                                           PracticeDrills = new List<PracticeDrillDto>()
                                                       });

                DB.Database.Save<EventDto>(new EventDto
                                                       {
                                                           Date = new DateTime(2012, 8, 27, 17, 30, 0),
                                                           EventTypeId = (int) EventType.Practice,
                                                           SportTypeId = (int) SportType.Soccer,
                                                           Completed = false,
                                                           Team = new TeamDto {Id = 1},
                                                           Location = new LocationDto {Id = 1},
                                                           PracticeDrills = new List<PracticeDrillDto>()
                                                       });

                DB.Database.Save<EventDto>(new EventDto
                                                       {
                                                           Date = new DateTime(2012, 9, 3, 17, 30, 0),
                                                           EventTypeId = (int) EventType.Practice,
                                                           SportTypeId = (int) SportType.Soccer,
                                                           Completed = false,
                                                           Team = new TeamDto {Id = 1},
                                                           Location = new LocationDto {Id = 1},
                                                           PracticeDrills = new List<PracticeDrillDto>()
                                                       });

                DB.Database.Save<EventDto>(new EventDto
                                                       {
                                                           Date = new DateTime(2012, 9, 8, 13, 15, 0),
                                                           EventTypeId = (int) EventType.Game,
                                                           SportTypeId = (int) SportType.Soccer,
                                                           Completed = false,
                                                           Team = new TeamDto {Id = 1},
                                                           Opponent = new TeamDto { Id = 4 },
                                                           Location = new LocationDto {Id = 4},
                                                           PracticeDrills = new List<PracticeDrillDto>()
                                                       });

                DB.Database.Save<EventDto>(new EventDto
                                                       {
                                                           Date = new DateTime(2012, 9, 10, 17, 30, 0),
                                                           EventTypeId = (int) EventType.Practice,
                                                           SportTypeId = (int) SportType.Soccer,
                                                           Completed = false,
                                                           Team = new TeamDto {Id = 1},
                                                           Location = new LocationDto {Id = 1},
                                                           PracticeDrills = new List<PracticeDrillDto>()
                                                       });

                DB.Database.Save<EventDto>(new EventDto
                                                       {
                                                           Date = new DateTime(2012, 9, 15, 13, 15, 0),
                                                           EventTypeId = (int) EventType.Game,
                                                           SportTypeId = (int) SportType.Soccer,
                                                           Completed = false,
                                                           Team = new TeamDto {Id = 1},
                                                           Opponent = new TeamDto { Id = 5 },
                                                           Location = new LocationDto {Id = 4},
                                                           PracticeDrills = new List<PracticeDrillDto>()
                                                       });

                DB.Database.Save<EventDto>(new EventDto
                                                       {
                                                           Date = new DateTime(2012, 9, 17, 17, 30, 0),
                                                           EventTypeId = (int) EventType.Practice,
                                                           SportTypeId = (int) SportType.Soccer,
                                                           Completed = false,
                                                           Team = new TeamDto {Id = 1},
                                                           Location = new LocationDto {Id = 1},
                                                           PracticeDrills = new List<PracticeDrillDto>()
                                                       });


                DB.Database.Save<EventDto>(new EventDto
                                                       {
                                                           Date = new DateTime(2012, 9, 22, 12, 0, 0),
                                                           EventTypeId = (int) EventType.Game,
                                                           SportTypeId = (int) SportType.Soccer,
                                                           Completed = false,
                                                           Team = new TeamDto {Id = 1},
                                                           Opponent = new TeamDto { Id = 6 },
                                                           Location = new LocationDto {Id = 3},
                                                           PracticeDrills = new List<PracticeDrillDto>()
                                                       });

                DB.Database.Save<EventDto>(new EventDto
                                               {
                                                   Date = new DateTime(2012, 9, 24, 17, 30, 0),
                                                   EventTypeId = (int) EventType.Practice,
                                                   SportTypeId = (int) SportType.Soccer,
                                                   Completed = false,
                                                   Team = new TeamDto {Id = 1},
                                                   Location = new LocationDto {Id = 1},
                                                   PracticeDrills = new List<PracticeDrillDto>()
                                               });

                DB.Database.Save<EventDto>(new EventDto
                                               {
                                                   Date = new DateTime(2012, 9, 29, 13, 15, 0),
                                                   EventTypeId = (int) EventType.Game,
                                                   SportTypeId = (int) SportType.Soccer,
                                                   Completed = false,
                                                   Team = new TeamDto {Id = 1},
                                                   Opponent = new TeamDto {Id = 7},
                                                   Location = new LocationDto {Id = 3},
                                                   PracticeDrills = new List<PracticeDrillDto>()
                                               });

                DB.Database.Save<EventDto>(new EventDto
                                               {
                                                   Date = new DateTime(2012, 10, 1, 17, 30, 0),
                                                   EventTypeId = (int) EventType.Practice,
                                                   SportTypeId = (int) SportType.Soccer,
                                                   Completed = false,
                                                   Team = new TeamDto {Id = 1},
                                                   Location = new LocationDto {Id = 1},
                                                   PracticeDrills = new List<PracticeDrillDto>()
                                               });


                DB.Database.Save<EventDto>(new EventDto
                                               {
                                                   Date = new DateTime(2012, 10, 6, 12, 00, 0),
                                                   EventTypeId = (int) EventType.Game,
                                                   SportTypeId = (int) SportType.Soccer,
                                                   Completed = false,
                                                   Team = new TeamDto {Id = 1},
                                                   Opponent = new TeamDto {Id = 8},
                                                   Location = new LocationDto {Id = 3},
                                                   PracticeDrills = new List<PracticeDrillDto>()
                                               });

                DB.Database.Save<EventDto>(new EventDto
                                               {
                                                   Date = new DateTime(2012, 10, 8, 17, 30, 0),
                                                   EventTypeId = (int) EventType.Practice,
                                                   SportTypeId = (int) SportType.Soccer,
                                                   Completed = false,
                                                   Team = new TeamDto {Id = 1},
                                                   Location = new LocationDto {Id = 1},
                                                   PracticeDrills = new List<PracticeDrillDto>()
                                               });


                DB.Database.Save<EventDto>(new EventDto
                                               {
                                                   Date = new DateTime(2012, 10, 13, 13, 15, 0),
                                                   EventTypeId = (int) EventType.Game,
                                                   SportTypeId = (int) SportType.Soccer,
                                                   Completed = false,
                                                   Team = new TeamDto {Id = 1},
                                                   Opponent = new TeamDto {Id = 9},
                                                   Location = new LocationDto {Id = 4},
                                                   PracticeDrills = new List<PracticeDrillDto>()
                                               });

                DB.Database.Save<EventDto>(new EventDto
                                               {
                                                   Date = new DateTime(2012, 10, 15, 17, 30, 0),
                                                   EventTypeId = (int) EventType.Practice,
                                                   SportTypeId = (int) SportType.Soccer,
                                                   Completed = false,
                                                   Team = new TeamDto {Id = 1},
                                                   Location = new LocationDto {Id = 1},
                                                   PracticeDrills = new List<PracticeDrillDto>()
                                               });

                DB.Database.Save<EventDto>(new EventDto
                                               {
                                                   Date = new DateTime(2012, 10, 20, 13, 15, 0),
                                                   EventTypeId = (int) EventType.Game,
                                                   SportTypeId = (int) SportType.Soccer,
                                                   Completed = false,
                                                   Team = new TeamDto {Id = 1},
                                                   Opponent = new TeamDto {Id = 10},
                                                   Location = new LocationDto {Id = 3},
                                                   PracticeDrills = new List<PracticeDrillDto>()
                                               });

                DB.Database.Save<EventDto>(new EventDto
                                               {
                                                   Date = new DateTime(2012, 10, 22, 17, 30, 0),
                                                   EventTypeId = (int) EventType.Practice,
                                                   SportTypeId = (int) SportType.Soccer,
                                                   Completed = false,
                                                   Team = new TeamDto {Id = 1},
                                                   Location = new LocationDto {Id = 1},
                                                   PracticeDrills = new List<PracticeDrillDto>()
                                               });

                DB.Database.Save<EventDto>(new EventDto
                                               {
                                                   Date = new DateTime(2012, 10, 27, 13, 15, 0),
                                                   EventTypeId = (int) EventType.Game,
                                                   SportTypeId = (int) SportType.Soccer,
                                                   Completed = false,
                                                   Team = new TeamDto {Id = 1},
                                                   Opponent = new TeamDto {Id = 11},
                                                   Location = new LocationDto {Id = 3},
                                                   PracticeDrills = new List<PracticeDrillDto>()
                                               });

                //DB.Database.Save<EventDto>(new EventDto
                //                               {
                //                                   Date = DateTime.Now.AddDays(1),
                //                                   Time = DateTime.Now.AddHours(1),
                //                                   EventTypeId = (int)EventType.Practice,
                //                                   SportTypeId = (int)SportType.Baseball,
                //                                   Completed = false,
                //                                   Team = new TeamDto {Id = 2},
                //                                   Location = new LocationDto {Id = 3},
                //                                   PracticeDrills = new List<PracticeDrillDto>
                //                                                        {
                //                                                            new PracticeDrillDto
                //                                                                {
                //                                                                    Id = 1,
                //                                                                    Name = "Drill 1",
                //                                                                    SportTypeId = (int)SportType.Soccer,
                //                                                                    Sequence = 1
                //                                                                }
                //                                                        }
                //                               });

                DB.Database.Save<TeamDto>(new TeamDto { Id = 1, Name = "Sparks", SportTypeId = (int)SportType.Soccer });
                DB.Database.Save<TeamDto>(new TeamDto { Id = 2, Name = "Clash", SportTypeId = (int)SportType.Soccer });

                DB.Database.Save<TeamDto>(new TeamDto { Id = 3, Name = "Express", SportTypeId = (int)SportType.Soccer });
                DB.Database.Save<TeamDto>(new TeamDto { Id = 4, Name = "Stars", SportTypeId = (int)SportType.Soccer });
                DB.Database.Save<TeamDto>(new TeamDto { Id = 5, Name = "Falcons", SportTypeId = (int)SportType.Soccer });
                DB.Database.Save<TeamDto>(new TeamDto { Id = 6, Name = "Titans", SportTypeId = (int)SportType.Soccer });
                DB.Database.Save<TeamDto>(new TeamDto { Id = 7, Name = "Cobras", SportTypeId = (int)SportType.Soccer });
                DB.Database.Save<TeamDto>(new TeamDto { Id = 8, Name = "Blast", SportTypeId = (int)SportType.Soccer });
                DB.Database.Save<TeamDto>(new TeamDto { Id = 9, Name = "Blaze", SportTypeId = (int)SportType.Soccer });
                DB.Database.Save<TeamDto>(new TeamDto { Id = 10, Name = "Hawks", SportTypeId = (int)SportType.Soccer });
                DB.Database.Save<TeamDto>(new TeamDto { Id = 11, Name = "Leeds United", SportTypeId = (int)SportType.Soccer });

                DB.Database.Save<LocationDto>(new LocationDto { Id = 1, Name = "G. H. Field 1a" });
                DB.Database.Save<LocationDto>(new LocationDto { Id = 2, Name = "G. H. Field 1b" });
                DB.Database.Save<LocationDto>(new LocationDto { Id = 3, Name = "G. H. Field 2a" });
                DB.Database.Save<LocationDto>(new LocationDto { Id = 4, Name = "G. H. Field 2b" });

                DB.Database.Flush();
            }
        }

        public void Save(EventDto eventDto )
        {
            try
            {
                DB.Database.Save<EventDto>(eventDto);

                DB.Database.Flush();
            }
            catch (Exception e)
            {
                var x = e;
            }
        }

        public void Save(TeamDto teamDto )
        {
            DB.Database.Save<TeamDto>(teamDto);

            DB.Database.Flush();
        }

        public void Save(LocationDto locationDto)
        {
            DB.Database.Save<LocationDto>(locationDto);

            DB.Database.Flush();
        }

        public IList<EventDto> All()
        {
            var results = from drill in DB.Database.Query<EventDto, int>()
                          select drill.LazyValue.Value;


            return results.OrderBy(x => x.Date).ToList();
        }

        public IList<TeamDto> Teams()
        {
            var results = from team in DB.Database.Query<TeamDto, int>()
                          select team.LazyValue.Value;


            return results.ToList();
        }

        public IList<LocationDto> Locations()
        {
            var results = from field in DB.Database.Query<LocationDto, int>()
                          select field.LazyValue.Value;


            return results.ToList();
        } 

        public IList<EventDto> Open()
        {
            var events = All().Where(x => x.Completed == false);

            return events.ToList();
        }

        public IList<EventDto> Completed()
        {
            var events = All().Where(x => x.Completed);

            return events.ToList();
        }

        public EventDto PracticeEvent(int eventId)
        {
            var found = All().FirstOrDefault(x => x.Id == eventId);
            
            return found;
        }
    }
}
