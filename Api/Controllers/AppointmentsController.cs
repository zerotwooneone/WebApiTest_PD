using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
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
        public async Task<IActionResult> Get()
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

    public class ByMonthModel
    {
        public string Month { get; set; }
        public long Count { get; set; }
    }
}
