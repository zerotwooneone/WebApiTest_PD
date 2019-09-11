using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Reload
{
    public class AppointmentModel
    {
        public long? AppointmentId { get; set; }
        public string AppointmentType { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public DateTimeOffset? RequestedDateTimeOffset { get; set; }
        public UserModel User { get; set; }
        public AnimalModel Animal { get; set; }
    }
}
