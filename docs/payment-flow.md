# Payment Flow

## As JobProvider

### JobPosting

- Job post creation: (When a `JobProvider` starts creating a job post)
    - [x] Capture credit card information using the flutter_stripe plugin, generate a paymentMethodId and store it in the Flutter app for subsequent use.
- Job post submission: (When a `JobProvider` submits a job post)
    Frontend:
        - [x] Send the `paymentMethodId` to the backend.
    Backend:
        - [x] Create or update the job post with the `paymentMethodId`.
        - [x] Store the `paymentMethodId` in the database with user, so, it can be used for future payments.
        - [ ] Hold the payment in the scrow account.
- Freelancer application and completion: (When a `Freelancer` applies for a job post and completes the job)
    Frontend:
        - [x] Apply for the job post, do the work and submit for review. (Nothing for payment flow)
- Job Completion Confirmation: (When a `JobProvider` confirms the job completion)
    Frontend:
        - [x] Mark the job as completed.
    Backend:
        - [ ] Release the payment from the scrow account to `freelancer` account.

### As Freelancer

- Bank Account:
    Fronend:
        - [x] Add US bank account as a recevied payment method, pass bankAccountToken to the backend.
        - [x] verify the bank account, by using the `microDepositAmounts` and `bankAccountId`.
    Backend:
        - [x] Add bank account api for freelancer, to pass the bank account token and other details.
        - [x] Verify the bank account, by using the `microDepositAmounts` and `bankAccountId`.