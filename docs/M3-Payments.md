---
title: Milestone 3 API's
Abdo status: Completed
Akshay status: Pending Testing
---

# Milestone 3 API's

## Payment Module

### Tables (Payment, PaymentDetail)

```sql
-- 1. Enhanced payment_methods table
-- Rules:
-- 1. A user can have multiple payment methods.
-- 2. A user can have only one default payment method.
CREATE TABLE payment_methods (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id BIGINT REFERENCES users(id) NOT NULL,
    company_id BIGINT REFERENCES companies(id) NOT NULL,
    card_last_four VARCHAR(4) NOT NULL,
    card_brand VARCHAR(50) NOT NULL,             -- 'visa', 'mastercard', etc.
    exp_month INTEGER NOT NULL,
    exp_year INTEGER NOT NULL,
    cardholder_name VARCHAR(255) NOT NULL,
    stripe_payment_method_id VARCHAR(255) UNIQUE NOT NULL,
    stripe_card_id VARCHAR(255) UNIQUE NOT NULL,
    is_default BOOLEAN DEFAULT FALSE,
    status VARCHAR(20) DEFAULT 'active', -- 'active', 'expired', 'deleted'
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- 2. Enhanced payments table
-- Rules:
-- 1. A payment can have multiple payment details.
-- 2. Send notification to the manager and freelancer when a payment is created or updated.
CREATE TABLE payments (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    job_id INTEGER REFERENCES jobs(id),
    payment_method_id BIGINT REFERENCES payment_methods(id),
    amount DECIMAL(10, 2) NOT NULL,
    status VARCHAR(20) NOT NULL,         -- 'pending', 'processing', 'completed', 'failed'
    payment_type VARCHAR(20) NOT NULL,   -- 'contract', 'pay_as_you_go', 'escrow'
    escrow_status VARCHAR(20),           -- 'held', 'released', 'refunded'
    stripe_payment_intent_id VARCHAR(255) UNIQUE,
    stripe_transfer_id VARCHAR(255) UNIQUE,
    description TEXT,
    failure_reason TEXT,
    paid_by_user_id BIGINT REFERENCES users(id),
    paid_to_user_id BIGINT REFERENCES users(id),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

### API's

- [ ] AddPaymentMethod (POST)
  - { accessToken: string, paymentMethod: {
    cardLastFour: string,
    cardBrand: string,
    expMonth: number,
    expYear: number,
    cardholderName: string,
    stripePaymentMethodId: string,
    stripeCardId: string,
    isDefault: boolean,
  } }
  - Returns PaymentMethodId (number)
  - Action:
    - 1. Create PaymentMethod row
    - 2. Return PaymentMethodId
- [ ] DeletePaymentMethod (DELETE)
  - { accessToken: string, paymentMethodId: number }
  - Returns Message (string)
  - Action:
    - 1. Delete PaymentMethod row
    - 2. Return Message
- [ ] GetPaymentMethods (GET)
  - { accessToken: string }
  - Returns List of PaymentMethods (array of objects)
  - Action:
    - 1. Get list of PaymentMethods by UserId
    - 2. Return List of PaymentMethods
