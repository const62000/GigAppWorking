using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class JobQuestionnaire
{
    public long Id { get; set; }

    public long? JobId { get; set; }

    public string Question { get; set; } = null!;

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Job? Job { get; set; }

    public virtual ICollection<JobQuestionnaireAnswer> JobQuestionnaireAnswers { get; set; } = new List<JobQuestionnaireAnswer>();
}
