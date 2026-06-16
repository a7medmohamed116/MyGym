using MyGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class Membership : BaseEntity
    {
        public Plan Plan { get; set; }
        public  Member Member { get; set; }

        public int PlanId { get; set; }
        public int MemberId { get; set; }

        //startdate == createdat of base entity

        public DateTime EndDate { get; set; }


        //Readonly properties doesn't transfer in database
        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
        public bool IsActive => EndDate > DateTime.Now;

    }
}
