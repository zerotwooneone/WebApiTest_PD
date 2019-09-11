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
using Newtonsoft.Json;

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
                    //var response = await httpClient.GetAsync("https://sampledata.petdesk.com/api/appointments");
                    //if (!response.IsSuccessStatusCode)
                    //{
                    //    return StatusCode((int) HttpStatusCode.InternalServerError, $"server returned: {response.StatusCode}");
                    //}

                    appointments = JsonConvert.DeserializeObject<IEnumerable<AppointmentModel>>(
                        "[{\"AppointmentId\":\"290318\",\"AppointmentType\":\"Other\",\"CreateDateTime\":\"2018-11-28T22:57:33\",\"RequestedDateTimeOffset\":\"2018-12-01T08:00:00-08:00\",\"User\":{\"UserId\":\"115066\",\"FirstName\":\"Tracey\",\"LastName\":\"Polzin\"},\"Animal\":{\"AnimalId\":\"137900\",\"FirstName\":\"Jackson\",\"Species\":\"Dog\",\"Breed\":\"German Shepherd\"}},{\"AppointmentId\":\"238668\",\"AppointmentType\":\"Nail Trim, Overnight Boarding\",\"CreateDateTime\":\"2018-10-15T23:14:39\",\"RequestedDateTimeOffset\":\"2018-10-17T07:30:00-05:00\",\"User\":{\"UserId\":\"55574\",\"FirstName\":\"Sandra\",\"LastName\":\"Anya\"},\"Animal\":{\"AnimalId\":\"73197\",\"FirstName\":\"SuperEmailTest\",\"Species\":null,\"Breed\":null}},{\"AppointmentId\":\"238666\",\"AppointmentType\":\"Trim Cut\",\"CreateDateTime\":\"2018-10-15T23:10:10\",\"RequestedDateTimeOffset\":\"2018-10-18T09:30:00-05:00\",\"User\":{\"UserId\":\"55573\",\"FirstName\":\"Jessica\",\"LastName\":\"Craik\"},\"Animal\":{\"AnimalId\":\"73195\",\"FirstName\":\"EmailDog\",\"Species\":null,\"Breed\":null}},{\"AppointmentId\":\"276430\",\"AppointmentType\":\"Vaccinations\",\"CreateDateTime\":\"2018-08-24T21:15:28\",\"RequestedDateTimeOffset\":\"2018-08-26T08:00:00-07:00\",\"User\":{\"UserId\":\"107690\",\"FirstName\":\"Elisa\",\"LastName\":\"Rushworth\"},\"Animal\":{\"AnimalId\":\"124977\",\"FirstName\":\"Lola\",\"Species\":\"Dog\",\"Breed\":\"Lab X\"}},{\"AppointmentId\":\"276423\",\"AppointmentType\":\"Vaccinations\",\"CreateDateTime\":\"2018-08-15T23:32:53\",\"RequestedDateTimeOffset\":\"2018-08-18T08:00:00-05:00\",\"User\":{\"UserId\":\"107574\",\"FirstName\":\"Jana\",\"LastName\":\"Broadway\"},\"Animal\":{\"AnimalId\":\"124027\",\"FirstName\":\"La\",\"Species\":\"Dog\",\"Breed\":\"Poodle\"}},{\"AppointmentId\":\"276422\",\"AppointmentType\":\"Vaccinations\",\"CreateDateTime\":\"2018-08-15T23:32:20\",\"RequestedDateTimeOffset\":\"2018-08-16T09:00:00-07:00\",\"User\":{\"UserId\":\"114673\",\"FirstName\":\"Michael\",\"LastName\":\"Nelson\"},\"Animal\":{\"AnimalId\":\"136692\",\"FirstName\":\"Lemmy\",\"Species\":\"Dog\",\"Breed\":\"Boston Terrier\"}},{\"AppointmentId\":\"276405\",\"AppointmentType\":\"Vaccinations\",\"CreateDateTime\":\"2018-08-14T02:12:06\",\"RequestedDateTimeOffset\":\"2018-08-17T10:00:00-05:00\",\"User\":{\"UserId\":\"90265\",\"FirstName\":\"Ryan\",\"LastName\":\"Shields\"},\"Animal\":{\"AnimalId\":\"114613\",\"FirstName\":\"Cookie\",\"Species\":\"Dog\",\"Breed\":\"Yorki\"}},{\"AppointmentId\":\"240068\",\"AppointmentType\":\"Medication Refill\",\"CreateDateTime\":\"2018-06-26T17:23:52\",\"RequestedDateTimeOffset\":\"2018-06-29T18:00:00-07:00\",\"User\":{\"UserId\":\"54171\",\"FirstName\":\"Mike\",\"LastName\":\"Michelson\"},\"Animal\":{\"AnimalId\":\"71089\",\"FirstName\":\"Molly\",\"Species\":\"Dog\",\"Breed\":\"Shih Tzu\"}},{\"AppointmentId\":\"240067\",\"AppointmentType\":\"Sick Pet Exam\",\"CreateDateTime\":\"2018-06-25T21:01:47\",\"RequestedDateTimeOffset\":\"2018-06-26T13:00:00-07:00\",\"User\":{\"UserId\":\"54872\",\"FirstName\":\"Shona\",\"LastName\":\"Morgan\"},\"Animal\":{\"AnimalId\":\"113855\",\"FirstName\":\"Burdie\",\"Species\":\"Bird\",\"Breed\":\"Canary\"}},{\"AppointmentId\":\"240065\",\"AppointmentType\":\"Annual Physical Exam\",\"CreateDateTime\":\"2018-06-25T20:06:42\",\"RequestedDateTimeOffset\":\"2018-07-01T21:30:00-07:00\",\"User\":{\"UserId\":\"54831\",\"FirstName\":\"Walter\",\"LastName\":\"White\"},\"Animal\":{\"AnimalId\":\"72288\",\"FirstName\":\"Jake\",\"Species\":\"Dog\",\"Breed\":\"Terrier, Rat\"}},{\"AppointmentId\":\"240060\",\"AppointmentType\":\"Medication Refill\",\"CreateDateTime\":\"2018-06-25T15:15:19\",\"RequestedDateTimeOffset\":\"2018-06-26T08:30:00-05:00\",\"User\":{\"UserId\":\"54919\",\"FirstName\":\"Amy\",\"LastName\":\"Rathbone\"},\"Animal\":{\"AnimalId\":\"72403\",\"FirstName\":\"Auggie\",\"Species\":\"Dog\",\"Breed\":\"Havanese\"}},{\"AppointmentId\":\"240056\",\"AppointmentType\":\"Annual Physical Exam\",\"CreateDateTime\":\"2018-06-17T23:00:45\",\"RequestedDateTimeOffset\":\"2018-06-20T09:00:00-07:00\",\"User\":{\"UserId\":\"55520\",\"FirstName\":\"Shona\",\"LastName\":\"terry\"},\"Animal\":{\"AnimalId\":\"73054\",\"FirstName\":\"New\",\"Species\":\"Dog\",\"Breed\":\"Retriever, Golden\"}},{\"AppointmentId\":\"240055\",\"AppointmentType\":\"Other\",\"CreateDateTime\":\"2018-06-17T22:53:56\",\"RequestedDateTimeOffset\":\"2018-06-20T11:00:00-07:00\",\"User\":{\"UserId\":\"55517\",\"FirstName\":\"Shona\",\"LastName\":\"Amyer Smith\"},\"Animal\":{\"AnimalId\":\"73051\",\"FirstName\":\"Charlie\",\"Species\":\"Dog\",\"Breed\":\"Havanese\"}},{\"AppointmentId\":\"240053\",\"AppointmentType\":\"Medication Refill\",\"CreateDateTime\":\"2018-06-11T18:47:03\",\"RequestedDateTimeOffset\":\"2018-06-12T08:30:00-07:00\",\"User\":{\"UserId\":\"55528\",\"FirstName\":\"Drew\",\"LastName\":\"Davies\"},\"Animal\":{\"AnimalId\":\"73062\",\"FirstName\":\"Chico\",\"Species\":\"Dog\",\"Breed\":\"Chihuahua Mix\"}},{\"AppointmentId\":\"220811\",\"AppointmentType\":\"Surgery, Full Cut\",\"CreateDateTime\":\"2018-05-16T19:36:58\",\"RequestedDateTimeOffset\":\"2018-05-23T10:00:00-05:00\",\"User\":{\"UserId\":\"47008\",\"FirstName\":\"Jen\",\"LastName\":\"Lindsey\"},\"Animal\":{\"AnimalId\":\"58368\",\"FirstName\":\"Maggie\",\"Species\":\"Dog\",\"Breed\":\"Terrier, Yorkshire\"}},{\"AppointmentId\":\"220803\",\"AppointmentType\":\"Nail Trim\",\"CreateDateTime\":\"2018-04-30T03:06:51\",\"RequestedDateTimeOffset\":\"2018-04-30T07:30:00-05:00\",\"User\":{\"UserId\":\"46889\",\"FirstName\":\"Ken\",\"LastName\":\"Tsui4\"},\"Animal\":{\"AnimalId\":\"57222\",\"FirstName\":\"Sassy\",\"Species\":null,\"Breed\":null}},{\"AppointmentId\":\"273595\",\"AppointmentType\":\"Dental\",\"CreateDateTime\":\"2018-04-22T18:18:02\",\"RequestedDateTimeOffset\":\"2018-04-29T13:00:00-05:00\",\"User\":{\"UserId\":\"113851\",\"FirstName\":\"Teresa\",\"LastName\":\"Wassum\"},\"Animal\":{\"AnimalId\":\"135517\",\"FirstName\":\"Matrix\",\"Species\":null,\"Breed\":\"matty\"}},{\"AppointmentId\":\"273581\",\"AppointmentType\":\"New Client/Patient Visit\",\"CreateDateTime\":\"2018-04-15T15:58:29\",\"RequestedDateTimeOffset\":\"2018-04-27T12:00:00-05:00\",\"User\":{\"UserId\":\"113844\",\"FirstName\":\"Claudia\",\"LastName\":\"Ortiz\"},\"Animal\":{\"AnimalId\":\"135508\",\"FirstName\":\"Mjolnir\",\"Species\":null,\"Breed\":null}},{\"AppointmentId\":\"290355\",\"AppointmentType\":\"Dental\",\"CreateDateTime\":\"2018-02-14T19:58:47\",\"RequestedDateTimeOffset\":\"2018-02-16T08:00:00-08:00\",\"User\":{\"UserId\":\"106799\",\"FirstName\":\"Cynthia\",\"LastName\":\"Kirchner\"},\"Animal\":{\"AnimalId\":\"124323\",\"FirstName\":\"Zuffy2\",\"Species\":\"Dog\",\"Breed\":\"Akita\"}},{\"AppointmentId\":\"4735\",\"AppointmentType\":\"Bath\",\"CreateDateTime\":\"2018-01-28T19:03:35\",\"RequestedDateTimeOffset\":\"2018-01-31T07:30:00-06:00\",\"User\":{\"UserId\":\"40524\",\"FirstName\":\"Leigh\",\"LastName\":\"Crump\"},\"Animal\":{\"AnimalId\":\"46783\",\"FirstName\":\"Gracie\",\"Species\":\"Dog\",\"Breed\":null}}]"); 
                    //(await response.Content.ReadAsAsync<IEnumerable<AppointmentModel>>()).ToArray(); //This materializes the values in memory so that we are sure we wont be re-enumerating results.Instead of this, we might want to handle 'pages' of appointments at  time. 

                }
                catch (HttpRequestException e)
                {
                    return StatusCode((int) HttpStatusCode.InternalServerError, e.ToString());
                }
            }

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
                    Insert("Appointment",
                        "AppointmentId, AppointmentType,CreateDateTime,RequestedDateTimeOffset,UserId,AnimalId",
                        $"{appointment.AppointmentId.Value},'{appointment.AppointmentType}',{appointment.CreateDateTime.Value.Ticks},{appointment.RequestedDateTimeOffset.Value.ToUnixTimeSeconds()},{appointment.User?.UserId.Value}, {appointment.Animal?.AnimalId.Value}",
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
            return Ok("something");
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
