using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
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

        // GET: api/Appointments/ByMonth
        [Route("ByMonth")]
        [HttpGet]
        public async Task<IActionResult> GetByMonth()
        {
            var result = new Dictionary<string, StatisticsModel>();

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
                    var requestedDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((int) rawDatetimeOffset);
                    var month = requestedDateTimeOffset.ToString("MMMM");
                    result.Add(month, 
                        new StatisticsModel
                        {
                            Count = (long) rawCount
                        }
                    );
                });

            return Ok(result);
        }

        // GET: api/Appointments/ByType
        [Route("ByType")]
        [HttpGet]
        public async Task<IActionResult> GetByType()
        {
            var result = new Dictionary<string, StatisticsModel>();

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
                    result.Add(rawType.ToString(), 
                        new StatisticsModel
                        {
                            Count = (long) rawCount
                        });
                });

            return Ok(result);
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
}
