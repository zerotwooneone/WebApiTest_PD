﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Api.DataSource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AppointmentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/Appointments
        [Route("ByMonth")]
        [HttpGet]
        public async Task<IActionResult> GetByMonth()
        {
            var result = new List<ByMonthModel>();

            await RowByRow(@"
                select 
	                a.RequestedDateTimeOffset,
	                count(a.AppointmentId) count
	            from Appointment a
                group by strftime('%m', datetime(a.RequestedDateTimeOffset, 'unixepoch'))",
                reader =>
                {
                    var rawDatetimeOffset = reader["RequestedDateTimeOffset"];
                    var rawCount = reader["count"];
                    result.Add(new ByMonthModel
                    {
                        Month = DateTimeOffset.FromUnixTimeSeconds((int) rawDatetimeOffset).ToString("MMMM"),
                        Count = (long) rawCount
                    });
                });

            return Ok(result.ToDictionary(m=>m.Month));
        }

        // GET: api/Appointments
        [Route("ByType")]
        [HttpGet]
        public async Task<IActionResult> GetByType()
        {
            var result = new List<ByTypeModel>();

            await RowByRow(@"
                WITH types(type, str, AppointmentId) AS (
                    SELECT 
		                '', 
		                a.AppointmentType||',',
		                a.AppointmentId 
	                FROM Appointment a
                    UNION ALL 
	                SELECT 
		                substr(str, 0, instr(str, ',')),
		                substr(str, instr(str, ',')+1),
		                AppointmentId
                    FROM types WHERE str!=''
                )
                SELECT 
	                type, 
	                count(AppointmentId) count
                FROM types 
                WHERE type!=''
                GROUP BY type",
                reader =>
                {
                    var rawType = reader["type"];
                    var rawCount = reader["count"];
                    result.Add(new ByTypeModel
                    {
                        Type = rawType.ToString(),
                        Count = (long) rawCount
                    });
                });

            return Ok(result.ToDictionary(m=>m.Type));
        }

        private async Task RowByRow(string query,
            Action<DbDataReader> onEachRow)
        {
            var dataSourceConfig = GetConfig(); //we get the config each time so that it is possible to update appsettings.json while the program is running

            using (var sqlConnection = new SQLiteConnection($"Data Source={dataSourceConfig.FileName};Version=3;"))
            {
                await sqlConnection.OpenAsync();

                using (var createTableCommand = new SQLiteCommand(query, sqlConnection))
                {
                    using (var reader = await createTableCommand.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            onEachRow(reader);
                        }
                    }
                }
            }
        }

        private DataSourceConfig GetConfig()
        {
            return _configuration.GetSection("DataSource").Get<DataSourceConfig>();
        }
    }

    public class ByTypeModel
    {
        public string Type { get; set; }
        public long Count { get; set; }
    }

    public class ByMonthModel
    {
        public string Month { get; set; }
        public long Count { get; set; }
    }
}
