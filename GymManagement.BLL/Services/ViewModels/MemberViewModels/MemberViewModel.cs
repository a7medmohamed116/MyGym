using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.ViewModels.MemberViewModels
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string? Photo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        // put the member details here to show in details page , to save not make another view  model with almost prop here
        public string? Address{ get; set; }
        public string? DateOfBirth { get; set; }
        public string? PlanName { get; set; }
        public string? MemberShipStartDate{ get; set; }
        public string? MemberShipEndDate { get; set; }



    }
}
