using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAA.DataAccess.Model
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool CanViewUpperClass { get; set; }
        public bool CanViewPremiumEconomy { get; set; }
        public bool CanViewEconomy { get; set; }
        public bool CanEditUpperClass { get; set; }
        public bool CanEditPremiumEconomy { get; set; }
        public bool CanEditEconomy { get; set; }
        public bool CanDeleteUpperClass { get; set; }
        public bool CanDeletePremiumEconomy { get; set; }
        public bool CanDeleteEconomy { get; set; }
        public bool CanApproveUpperClass { get; set; }
        public bool CanApprovePremiumEconomy { get; set; }
        public bool CanApproveEconomy { get; set; }

    }
}
