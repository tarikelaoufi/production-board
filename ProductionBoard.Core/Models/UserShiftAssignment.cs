using System;

namespace ProductionBoard.Core.Models
{
    public class UserShiftAssignment
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string TeamName { get; set; }

        public string ShiftName { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public bool IsActive { get; set; }
    }
}