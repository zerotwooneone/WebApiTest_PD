using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Reload;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReloadController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReloadController(IHttpClientFactory httpClientFactory,
            IHostingEnvironment hostingEnvironment)
        {
            _httpClientFactory = httpClientFactory;
        }
        // POST: api/Reload
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            IEnumerable<AppointmentModel> appointments;
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                try
                {
                    var response = await httpClient.GetAsync("https://sampledata.petdesk.com/api/appointments");
                    if (!response.IsSuccessStatusCode)
                    {
                        //we should log request/response info here
                        return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong connecting to a third party");
                    }

                    appointments = (await response.Content.ReadAsAsync<IEnumerable<AppointmentModel>>()).ToArray(); //This materializes the values in memory so that we are sure we wont be re-enumerating results.Instead of this, we might want to handle 'pages' of appointments at  time. 

                }
                catch (HttpRequestException e)
                {
                    //we should log this exception
                    return StatusCode((int) HttpStatusCode.InternalServerError, "Something went wrong connecting to a third party");
                }
            }

            //note to bring up: This may be appropriate for this sample data, but I think I would do this differently if I were to write it again.
            // I would not do appointments.ToArray() but rather do appointments.Select() and return an object that contains collections of valid
            // appointments, users, and animals as well as doing the logging of invalid input in one pass. The reason being, we could change the
            // above to retrieve pages of results at a time through a custom IEnumerable, and not have to change the validation or logic downstream.

            if (appointments.Any(a =>
                a.User?.UserId == null 
                || a.Animal?.AnimalId == null
                || string.IsNullOrEmpty(a.AppointmentType)
                || a.CreateDateTime == null
                || a.RequestedDateTimeOffset == null))
            {
                //probably want to log these, rather than store invalid values
            }

            appointments = appointments.Where(a =>
                a.User?.UserId != null 
                || a.Animal?.AnimalId != null
                || string.IsNullOrEmpty(a.AppointmentType)
                || a.CreateDateTime == null
                || a.RequestedDateTimeOffset == null); 

            var usersToAdd = GetDistinct(appointments,
                a => a.User,
                u => u.UserId);

            var animalsToAdd = GetDistinct(appointments,
                a => a.Animal,
                u => u.AnimalId);

            var appointmentsToAdd = GetDistinct(appointments,
                a => a,
                u => u.AppointmentId);
            
            const string databaseFileName = "apiStorage.sqlite";

            SQLiteConnection.CreateFile(databaseFileName); //this completely replaces the file on disk!

            using (var sqlConnection = new SQLiteConnection($"Data Source={databaseFileName};Version=3;"))
            {
                await sqlConnection.OpenAsync();

                CreateTable("Appointment",
                    @"AppointmentId INT PRIMARY KEY, 
                            AppointmentType VARCHAR(200), 
                            CreateDateTime INT, 
                            RequestedDateTimeOffset INT, 
                            UserId INT, 
                            AnimalId INT",
                    sqlConnection);
                CreateTable("User",
                    @"UserId INT PRIMARY KEY, 
                            FirstName VARCHAR(200), 
                            LastName VARCHAR(200)",
                    sqlConnection);
                CreateTable("Animal",
                    @"AnimalId INT PRIMARY KEY, 
                            FirstName VARCHAR(200), 
                            Species VARCHAR(200), 
                            Breed VARCHAR(200)",
                    sqlConnection);

                //might want to consider batching these inserts for performance
                foreach (var appointment in appointmentsToAdd)
                {
                    var appointmentType = SanitizeAppointmentType(appointment.AppointmentType);
                    Insert("Appointment",
                        "AppointmentId, AppointmentType,CreateDateTime,RequestedDateTimeOffset,UserId,AnimalId",
                        $"{appointment.AppointmentId.Value},'{appointmentType}',{appointment.CreateDateTime.Value.Ticks},{appointment.RequestedDateTimeOffset.Value.ToUnixTimeSeconds()},{appointment.User?.UserId.Value}, {appointment.Animal?.AnimalId.Value}",
                        sqlConnection);
                }

                foreach (var user in usersToAdd)
                {
                    Insert("User",
                        "UserId, FirstName,LastName",
                        $"{user.UserId.Value},'{user.FirstName}','{user.LastName}'",
                        sqlConnection);
                }

                foreach (var animal in animalsToAdd)
                {
                    Insert("Animal",
                        "AnimalId, FirstName,Species, Breed",
                        $"{animal.AnimalId.Value},'{animal.FirstName}','{animal.Species}','{animal.Breed}'",
                        sqlConnection);
                }
            }
            return Ok("success");
        }

        private string SanitizeAppointmentType(string appointmentType)
        {
            return appointmentType.Replace(", ", ",");
        }

        private static IEnumerable<T> GetDistinct<T>(IEnumerable<AppointmentModel> appointments,
            Func<AppointmentModel, T> modelSelector,
            Func<T, long?> idSelector)
        {
            var rawModels = appointments
                .Select(modelSelector);
            var modelArray =
                rawModels as T[] ??
                rawModels.ToArray(); //prevent multiple enumeration, explicitly materialize into memory now

            if (modelArray
                .Any(m=>m==null))
            {
                //api didn't give us a model. log this somewhere
            }

            if (modelArray
                .Where(m=>m !=null)
                .Select(idSelector)
                .Any(l => !l.HasValue))
            {
                //api gave us a model without an id. log this somewhere
            }

            var modelsById = modelArray
                .Where(m=>m !=null && idSelector(m).HasValue)
                .ToLookup(m => idSelector(m).Value);

            if (modelsById.Any(g => g.Count() > 1))
            {
                //the same model has multiple appointments. If the fields dont match, then we might need to consolidate or log as unresolvable
            }

            var modelsToAdd = modelsById.Select(g => g.First()); //arbitrarily select the first in case of duplicates
            return modelsToAdd;
        }

        private static void CreateTable(string tableName, string columns, SQLiteConnection sqlConnection)
        {
            using (var createTableCommand =
                new SQLiteCommand($"create table {tableName} ({columns})", sqlConnection))
            {
                createTableCommand.ExecuteNonQuery();
            }
        }

        private static void Insert(string tableName, string columns, string values, SQLiteConnection sqlConnection)
        {
            using (var createTableCommand =
                new SQLiteCommand($"insert into {tableName} ({columns}) values ({values})", sqlConnection))
            {
                createTableCommand.ExecuteNonQuery();
            }
        }
    }
}
