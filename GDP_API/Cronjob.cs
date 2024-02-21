using GDP_API.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data;
using GDP_API.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;

namespace GDP_API
{
    public class Cronjob : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingtoken)
        {
            while (!stoppingtoken.IsCancellationRequested)
            {
                //Code to repeat
               using (var context = new DataContext())
                {
                    //Query for the activity status table, I'm using a temporary table to save the needed data, probably there is a more efficient way.
                    var query = from userhasactivity in context.UserHasActivities
                                join activity in context.Activities on userhasactivity.ActivityId equals activity.Id
                                join user in context.Users on userhasactivity.UserId equals user.Id
                                group activity by new { user.Id } into g
                                select new
                                {
                                    UserId = g.Key.Id,
                                    ActivitiesStatus = g.Select(c=>c.Status),
                                    ActivitiesDeadline = g.Select(c => c.EndDate)

                                };
                    //Datatable to make the operations over it instead of making more SQL queries, I did it this way because to many SQL can be detrimental for the server
                    DataTable table = new DataTable();
                    table.Columns.Add("UserID", typeof(int));
                    table.Columns.Add("Status", typeof(string));
                    table.Columns.Add("Deadline", typeof(DateTime));

                    foreach (var item in query)
                    {
                        table.Rows.Add(item.UserId, item.ActivitiesStatus, item.ActivitiesDeadline);
                    }
                    //User total task status
                    var q1 = from row in table.AsEnumerable()
                             group row by new
                             {
                                 user = row.Field<string>("UserID"),
                                 status = row.Field<string>("Status")
                             }into grouped
                             select new 
                             {
                                 users = grouped.Key.user,
                                 ts = grouped.Key.status,
                                 qs = grouped.Count()
                             };
                    //Show results in console (change this when the front-end is ready)
                    foreach (var group in q1)
                    {
                        Console.WriteLine($"IdUsuario: {group.users}, Tarea status: {group.ts}, Numero de tareas: {group.qs}");
                    }
                    //User tasks comparing deadline with the current date to know if a task is on time or overdue
                    DateTime currentDate= DateTime.Now.Date;
                    var q2 = table.AsEnumerable().GroupBy(row => row.Field<string>("UserID"));
                    foreach (var group in q2)
                    {
                        int onTime = 0;
                        int overDue = 0;
                        //Write in console user id delete after conected with front-end
                        Console.WriteLine($"UserID: {group.Key}");
                        foreach (var item in group)
                        {
                            DateTime dateInRow = (DateTime)item["Deadline"];
                            if (dateInRow <= currentDate) 
                            {
                                onTime++;
                            }
                            else
                            {
                                overDue++;
                            }
                        }
                        //Write in console on time and overdue task delete after conect with front-end
                        Console.WriteLine("Tiene: ", onTime + overDue, " tareas totales y tiene: ", overDue," tareas atrasadas");
                    }
                }
                
                //10 min delay
                await Task.Delay(600000);
            }
        }
    }
}
