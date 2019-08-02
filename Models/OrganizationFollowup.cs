using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class OrganizationFollowup
    {
        public int OrganizationFollowupId { get; set; }
        public LookupAgencyStatus AgencyStatusCode { get; set; }
        public int AgencyStatusCodeId { get; set; }
        public DateTime? AgencyStatusCodeDate { get; set; }
        public string AgencyStatusCodeBy { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToNR { get; set; }
        public int DQReady { get; set; }
        public DateTime? DQReadyDate { get; set; }
        public int MaxAttempts { get; set;}
        public int NRMaxAttempts { get; set; }
        public int? OrganizationId { get; set; }
        public int? OrganizationYear { get; set; }
        public int SummaryStatusCode { get; set; }
        public DateTime? SummaryStatusCodeDate { get; set; }
        public string SummaryStatusCodeBy { get; set; }
        public int ASJStatusCode { get; set; }
        public int ASJQualityCode { get; set; }
        public int? TextReview { get; set; }
        public string TextReviewNotes { get; set; }
        public int SummaryQualityCode { get; set; }
        public DateTime? SummaryQualityCodeDate { get; set; }
        public int PriorTextReview { get; set; }
        public DateTime? FormCreatedDate { get; set; }
        public DateTime? FormModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        [ForeignKey("OrganizationId,OrganizationYear")]
        public virtual Organization Organization { get; set; }
        public bool SupervisorAlert { get; set; }
        public DateTime? DateReceived { get; set; }
        public string SubmissionMode { get; set; }

    }
}
