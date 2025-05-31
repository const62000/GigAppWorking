using FirebaseAdmin.Messaging;
using GigApp.Application.Interfaces.Notifications;
using GigApp.Domain.Entities;
using GigApp.Infrastructure.Database;
using GIgApp.Contracts.Enums;
using GIgApp.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using NotificationModel = GigApp.Domain.Entities.Notification;

namespace GigApp.Infrastructure.Repositories.Notifications
{
    internal class NotificationRepository(ApplicationDbContext _applicationDbContext) : INotificationRepository
    {
        public async Task<BaseResult> GetNotifications(string auth0Id,int page, int pageSize)
        {
            var notifications = await _applicationDbContext.Notifications.Where(x => x.User!.Auth0UserId == auth0Id)
                .Skip((page) * pageSize)
                .Take(pageSize).ToListAsync();
            return new BaseResult(notifications, true, string.Empty);
        }

        public async Task<bool> SendPaymentSuccessNotification(long userId)
        {
            try
            {
                var token = await GetTokenByUserId(userId);
                await SendNotification(0, token, NotificationType.Payment_Success);
            }
            catch (Exception )
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SendPaymentFailedNotification(long userId)
        {
            try
            {
                var token = await GetTokenByUserId(userId);
                await SendNotification(0, token, NotificationType.Payment_Failed);
            }
            catch (Exception )
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ReceivePaymentSuccessNotification(long userId)
        {
            try
            {
                var token = await GetTokenByUserId(userId);
                await SendNotification(0, token, NotificationType.Payment_Received);
            }
            catch (Exception )
            {
                return false;
            }
            return true;
        }

        public async Task<bool> JobHiredNotification(long jobId, long freelancerId, long managerId)
        {
            try
            {
                var token = await GetTokenByUserId(freelancerId);
                await SendNotification(jobId, token, NotificationType.Freelancer_Hired);
            }
            catch (Exception ) 
            {
                return false;
            }
            return true;
        }

        public async Task<bool> JobApplicationViewedNotification(long applicationId)
        {
            var idToToken = await GetTokenByJobApplication(applicationId);
            try
            {
                await SendNotification(0, idToToken,
                NotificationType.Job_Application_Viewed,
                applicationId);
            }
            catch (Exception )
            {
                return false;
            }
            return true;
        }

        public async Task<bool> JobAppliedNotification(long jobId, long applicationId, string auth0Id)
        {
            var idToToken = await GetToken(auth0Id);
            try
            {
                await SendNotification(jobId, idToToken,
                NotificationType.Job_Created,
                applicationId);
            }
            catch(Exception )
            {
                return false;
            }
            return true;
        }

        public async Task<bool> JobCreatedNotification(long jobId, string auth0Id)
        {
            var freelancerIds = await _applicationDbContext.Freelancers.Select(x=>x.UserId).ToListAsync();
            var idToTokens = await _applicationDbContext.UserDevices.Where(x=>freelancerIds.Contains(Convert.ToInt32(x.UserId))).Select(x=>new IdToToken { Token = x.FirebaseToken,Id=x.UserId}).ToListAsync();
            foreach (var idToToken in idToTokens)
            {
                try
                {
                    await SendNotification(jobId, idToToken, NotificationType.Job_Created);
                }
                catch (Exception )
                {
                    return false;
                }
            }
            return true;
        }
        private async Task<IdToToken> GetToken(string auth0Id)
        {
            var idToToken = await _applicationDbContext.UserDevices.Where(x => x.User!.Auth0UserId == auth0Id).Select(x=>new IdToToken { Id = x.UserId,Token = x.FirebaseToken}).FirstOrDefaultAsync()??new IdToToken();
            return idToToken;
        }

        private async Task<IdToToken> GetTokenByUserId(long userId)
        {
            var idToToken = await _applicationDbContext.UserDevices.Where(x => x.UserId == userId).Select(x => new IdToToken { Id = x.UserId, Token = x.FirebaseToken }).FirstOrDefaultAsync()?? new IdToToken();
            return idToToken;
        }

        private async Task<IdToToken> GetTokenByJobApplication(long applicationId)
        {
            var jobApplication = await _applicationDbContext.JobApplications.FindAsync(applicationId)??new JobApplication();
            return await GetTokenByUserId((long)(jobApplication.FreelancerUserId??0));
        }

        private async Task<bool> SendNotification(long id, IdToToken idToToken, NotificationType type,long applicationId=0)
        {
            try
            {
                var message = new Message();
                if (type == NotificationType.Job_Applied)
                {
                    await _applicationDbContext.Notifications.AddAsync(new NotificationModel
                    {
                        Description = "Great news! You have received a new application for your job posting",
                        Title = "Job Applied",
                        Type = "Job Applied",
                        UserId = idToToken.Id
                    });
                    await _applicationDbContext.SaveChangesAsync();
                    message = new Message()
                    {
                        Token = idToToken.Token,
                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Body = "Great news! You have received a new application for your job posting",
                            Title = "Job Applied"
                        },
                        Data = new Dictionary<string, string>()
                        {
                            ["topic"] = nameof(type),
                            ["job_id"] = $"{id}",
                            ["application_id"] = $"{applicationId}"
                        },
                    };
                }
                else if (type == NotificationType.Job_Application_Viewed)
                {
                    await _applicationDbContext.Notifications.AddAsync(new NotificationModel
                    {
                        Description = "Your Job Application is viewed!",
                        Title = "Application Viewed",
                        Type = "Application Viewed",
                        UserId = idToToken.Id
                    });
                    await _applicationDbContext.SaveChangesAsync();
                    message = new Message()
                    {
                        Token = idToToken.Token,
                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Body = "Your Job Application is viewed",
                            Title = "Application Viewed"
                        },
                        Data = new Dictionary<string, string>()
                        {
                            ["topic"] = nameof(type),
                            ["application_id"] = $"{applicationId}"
                        },
                    };
                }
                else if (type == NotificationType.Freelancer_Hired)
                {
                    await _applicationDbContext.Notifications.AddAsync(new NotificationModel
                    {
                        Description = "Great news! You got hired",
                        Title = "Job Hired",
                        Type = "Job Hired",
                        UserId = idToToken.Id
                    });
                    await _applicationDbContext.SaveChangesAsync();
                    message = new Message()
                    {
                        Token = idToToken.Token,
                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Body = "Great news! You got hired",
                            Title = "Job Hired"
                        },
                        Data = new Dictionary<string, string>()
                        {
                            ["topic"] = nameof(type),
                            ["job_id"] = $"{id}",
                            ["application_id"] = $"{applicationId}"
                        },
                    };
                }
                else if (type == NotificationType.Manager_Hired)
                {
                    await _applicationDbContext.Notifications.AddAsync(new NotificationModel
                    {
                        Description = "Great news! You are hiring someone",
                        Title = "Job Hired",
                        Type = "Job Hired",
                        UserId = idToToken.Id
                    });
                    await _applicationDbContext.SaveChangesAsync();
                    message = new Message()
                    {
                        Token = idToToken.Token,
                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Body = "Great news! You are hiring someone",
                            Title = "Job Hired"
                        },
                        Data = new Dictionary<string, string>()
                        {
                            ["topic"] = nameof(type),
                            ["job_id"] = $"{id}",
                            ["application_id"] = $"{applicationId}"
                        },
                    };
                }
                else if (type == NotificationType.Payment_Received)
                {
                    await _applicationDbContext.Notifications.AddAsync(new NotificationModel
                    {
                        Description = "Great news! You have received a payment",
                        Title = "Payment Received",
                        Type = "Payment Received",
                        UserId = idToToken.Id
                    });
                    await _applicationDbContext.SaveChangesAsync();
                    message = new Message()
                    {
                        Token = idToToken.Token,
                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Body = "Great news! You have received a payment",
                            Title = "Payment Received"
                        },
                    };
                }
                else if (type == NotificationType.Payment_Success)
                {
                    await _applicationDbContext.Notifications.AddAsync(new NotificationModel
                    {
                        Description = "Great news! You have received a payment",
                        Title = "Payment Success",
                        Type = "Payment Success",
                        UserId = idToToken.Id
                    });
                    await _applicationDbContext.SaveChangesAsync();
                    message = new Message()
                    {
                        Token = idToToken.Token,
                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Body = "Great news! You have received a payment",
                            Title = "Payment Success"
                        },
                    };
                }
                else if (type == NotificationType.Payment_Failed)
                {
                    await _applicationDbContext.Notifications.AddAsync(new NotificationModel
                    {
                        Description = "Payment Failed",
                        Title = "Payment Failed",
                        Type = "Payment Failed",
                        UserId = idToToken.Id
                    });
                    await _applicationDbContext.SaveChangesAsync();
                    message = new Message()
                    {
                        Token = idToToken.Token,
                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Body = "Payment Failed",
                            Title = "Payment Failed"
                        },
                    };
                }
                else
                {
                    await _applicationDbContext.Notifications.AddAsync(new NotificationModel
                    {
                        Description = "We have a new job opportunity that might interest you!",
                        Title = "Job Posted",
                        Type = "Job Posted",
                        UserId = idToToken.Id
                    });
                    await _applicationDbContext.SaveChangesAsync();
                    message = new Message()
                    {
                        Token = idToToken.Token,
                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Body = "We have a new job opportunity that might interest you!",
                            Title = "Job Posted"
                        },
                        Data = new Dictionary<string, string>()
                        {
                            ["topic"] = nameof(type),
                            ["id"] = $"{id}"
                        },
                    };
                }
                // Send a message to the device corresponding to the provided token.
                var messaging = FirebaseMessaging.DefaultInstance;
                var result = await messaging.SendAsync(message);
                // Response is a message ID string.
                return !string.IsNullOrEmpty(result);
            }
            catch (Exception ex)
            {
                return false;
            }  
        }

        private class IdToToken
        {
            public long Id { get; set; }
            public string Token {  get; set; } = string.Empty;
        }
    }
}
