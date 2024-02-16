using GDP_API.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data;
using GDP_API.Data;

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
                }
                //10 min delay
                await Task.Delay(600000);
            }
        }
    }
}
