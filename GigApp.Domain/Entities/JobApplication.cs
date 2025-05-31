using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class JobApplication
{
    public long Id { get; set; }

    public long? JobId { get; set; }

    public long? FreelancerUserId { get; set; }

    public string JobApplicationStatus { get; set; } = null!;

    public string? WithdrawalStatus { get; set; }

    public string? WithdrawalReason { get; set; }

    public DateTime? WithdrawalDate { get; set; } = DateTime.UtcNow;

    public string Proposal { get; set; } = null!;

    public decimal? ProposalHourlyRate { get; set; }

    public bool? Viewed { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User? FreelancerUser { get; set; }

    public virtual Job? Job { get; set; }

    public virtual ICollection<JobQuestionnaireAnswer> JobQuestionnaireAnswers { get; set; } = new List<JobQuestionnaireAnswer>();
}
