# Questions

## Note on how these questions are answered (for MVP)
- For MVP, we are not implementing escrow.
- We are not implementing platform fees.
- We are not implementing minimum balance requirement.
- We are not implementing refund policies.

### 1. Payment Release Timing
- For hourly jobs, should payments be released:
  - At the time of timesheet approval? (Yes)
  - Or only when the job is marked as completed? (No)
- For fixed contracts, payments will be released when the job is marked as completed. Is this acceptable? (There are no fixed contracts)

### 2. Platform Fees and Deductions
- How should platform fees, contract fees, or other charges be applied? (no platform fees in MVP)
  - Should they be deducted from the escrow before payment release? (No. Escrow is not a requirement)
  - Should they be displayed transparently to both job providers and freelancers? (No)

### 3. Minimum Balance Requirement
- Should there be a minimum balance on hold for every contract? (For MVP, no)
  - For hourly contracts, we currently hold `HourlyRate * MaximumHoursInWeek`. Is this acceptable?
  - Should we revise this calculation or add additional buffers?

### 4. Refund Policies (for MVP, no refunds)
- For hourly jobs, if fewer hours are worked than escrowed:
  - Should unused funds be refunded immediately?
  - Or should they remain in escrow for future adjustments?

### 5. Payment Splits (For MVP, no payment splits. Each timesheet is paid upon approval)
- In cases of partial completion, how should payments be split?
  - Should it be proportional to the work completed, or based on specific milestones?

### 6. Additional Escrow Features (For MVP, no additional escrow features)
- Should we allow job providers to top up the escrow balance mid-contract?
  - For example, if the freelancer exceeds the agreed hours, should job providers add additional funds?

### 7. Compliance and Regulations (For MVP, no compliance and regulations)
- Are there specific compliance or regulatory requirements we need to adhere to for payment processing?
  - For example, restrictions based on location, bank account types, or transaction limits?

### 8. Notifications
- Should we send notifications for:
  - Escrow balances falling below the required amount? (N/A)
  - Payment releases or approvals? (Yes)
  - Platform fee deductions? (N/A)
