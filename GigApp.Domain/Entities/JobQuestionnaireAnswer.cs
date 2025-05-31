using System;
using System.Collections.Generic;

namespace GigApp.Domain.Entities;

public partial class JobQuestionnaireAnswer
{
    public int Id { get; set; }

    public long? JobApplicationId { get; set; }

    public long? QuestionId { get; set; }

    public long? UserId { get; set; }

    public string Answer { get; set; } = null!;

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual JobApplication? JobApplication { get; set; }

    public virtual JobQuestionnaire? Question { get; set; }

    public virtual User? User { get; set; }
}
