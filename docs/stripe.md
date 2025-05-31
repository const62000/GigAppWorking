# Key Requirements

- A Payment Method (credit card or bank account) must be collected from Job Providers before posting a job. This must be stored on Stripe's platform so that the marketplace platform does not store the the credit card or bank account info.
- The Job Provider may post multiple jobs and hire multiple freelancers and would pay them from the same payment method.
- Job Providers may post jobs that take several weeks to complete. Freelancers submit time sheets, and upon approval by the job provider, payment is released to the freelancer. There will be serval time sheet submissions, approvals and payments per job.

# Stripe Integration Guide

Below is the **comprehensive guide**, tailored for a **C#** backend (using the official [Stripe .NET library](https://github.com/stripe/stripe-dotnet)) and a **Flutter** front-end. It details how to:

1. Enable Stripe Connect (Standard)
2. Onboard freelancers
3. Create a Stripe Customer (Job Provider)
4. Store a payment method using **Stripe Checkout in Setup mode**
5. Charge with **PaymentIntents** when timesheets are approved
6. Handle automatic payouts to freelancers
7. Collect marketplace fees
8. Set up a webhook in C#

It also clarifies that it’s perfectly fine to collect a payment method with Checkout (in setup mode) and then use PaymentIntents to charge later.

---

# 1. Enable Stripe Connect (Standard)

1. **Create or configure** your main Stripe account:
   - [Sign up for Stripe](https://dashboard.stripe.com/register) if you don’t have one.
   - In the [Stripe Dashboard](https://dashboard.stripe.com/), go to **Settings > Connect** and fill out your platform details.
2. **Set up** the required business details for your platform (brand settings, public info, etc.).

**References**

- [Getting Started with Connect](https://stripe.com/docs/connect/quickstart)

---

# 2. Onboard Freelancers (Standard Accounts)
## 2.1. Need to provide URLS for the backend to create the account link
Since you want the least development overhead, use **Standard** accounts. In C#, you might do something like:

```csharp
using Stripe;

var accountService = new AccountService();

// 1) Create a Stripe Connect Standard account
var accountOptions = new AccountCreateOptions
{
    Type = "standard",
    // Optionally set country, capabilities, etc.
};
var account = accountService.Create(accountOptions);

// 2) Create an AccountLink for onboarding
var accountLinkService = new AccountLinkService();
var accountLinkOptions = new AccountLinkCreateOptions
{
    Account = account.Id,
    RefreshUrl = "https://your-website.com/connect/onboarding/refresh",
    ReturnUrl = "https://your-website.com/connect/onboarding/return",
    Type = "account_onboarding",
};

var accountLink = accountLinkService.Create(accountLinkOptions);

// 3) Redirect the freelancer to accountLink.Url
// They complete onboarding on Stripe's hosted page.
```

- **Store `account.Id`** in your database to reference the freelancer’s Stripe account.
- Once the freelancer completes onboarding, they have a fully functional **Standard** account for payouts.

**References**

- [Standard Accounts Onboarding](https://stripe.com/docs/connect/standard-accounts#onboarding)
- [Account Links API](https://stripe.com/docs/api/account_links/create)

---

# 3. Create a Stripe Customer for Each Job Provider
- #3 and #4 are called by the backend when a Job Provider adds a credit card payment method (before job posting)

Create a **Stripe Customer** to associate multiple jobs/charges with a single Job Provider. In C#:

```csharp
var customerService = new CustomerService();
var customerOptions = new CustomerCreateOptions
{
    Email = "job.provider@example.com",
    Name = "Example Job Provider",
    // Optional fields: Phone, metadata, etc.
};

var customer = customerService.Create(customerOptions);
// Store customer.Id in your DB for future references
```

---

# 4. Storing the Payment Method Using **Checkout in Setup Mode** (Option B)

You want to collect and store a payment method off-session so you can charge the Job Provider later. **Stripe Checkout** in “setup” mode handles this.

## 4.1. Backend: Create a Checkout Session (C#)

```csharp
using Stripe.Checkout;

var sessionService = new SessionService();

var sessionOptions = new SessionCreateOptions
{
    Customer = customer.Id, // The ID of the Stripe Customer you created
    Mode = "setup",         // <-- This is crucial for Setup Mode
    PaymentMethodTypes = new List<string> { "card" },
    // If you want bank info, e.g. "us_bank_account"

    SuccessUrl = "https://your-website.com/payment-setup/success",
    CancelUrl = "https://your-website.com/payment-setup/cancel",
};

var checkoutSession = sessionService.Create(sessionOptions);

// checkoutSession.Url -> redirect the Job Provider to this URL
```

The **Session** will allow the Job Provider to enter payment details and attach a `PaymentMethod` to the `Customer` for future use.

## 4.2. Front-End (Flutter): Redirect to Checkout

In Flutter, open the returned `checkoutSession.Url` in a browser or WebView, e.g. using [url_launcher](https://pub.dev/packages/url_launcher):

```dart
import 'package:url_launcher/url_launcher.dart';

Future<void> redirectToCheckout(String checkoutUrl) async {
  final Uri checkoutUri = Uri.parse(checkoutUrl);

  if (!await launchUrl(checkoutUri, mode: LaunchMode.externalApplication)) {
    throw 'Could not launch $checkoutUrl';
  }
}
```

- After the user finishes entering payment details, Stripe attaches the payment method to the `Customer`.
- Stripe then redirects them to your `SuccessUrl` or `CancelUrl`.

## 4.3. Listen for `checkout.session.completed` Webhook

When the session completes, you’ll receive a `checkout.session.completed` event containing a `setup_intent` ID. You can retrieve that `SetupIntent` to see which `PaymentMethod` was stored. Alternatively, you can fetch the Customer’s saved PaymentMethods later.

**Mixing Checkout for Setup & PaymentIntents Later**  
This is perfectly fine. You can collect/stash a payment method with Checkout (in setup mode), then use **PaymentIntents** for off-session charges as needed.

---

# 5. Multiple Timesheets & Charging with PaymentIntents

## 5.1. Timesheet Scenario

- A job has a budget or hourly rate.
- The freelancer logs hours → timesheets → pending approval.
- Once approved, you create a **PaymentIntent** to charge the saved payment method.

## 5.2. PaymentIntent Creation (C#)

Assuming you have:

- `jobProviderCustomerId`: The Stripe Customer ID
- `savedPaymentMethodId`: The PaymentMethod ID stored earlier
- `freelancerConnectedAccountId`: The freelancer’s Standard Connect `account.Id`
- `timesheetAmount`: The amount to charge in the smallest currency unit (e.g., cents for USD)
- `applicationFeeAmount`: Your marketplace fee in cents

```csharp
using Stripe;

var paymentIntentService = new PaymentIntentService();

var paymentIntentOptions = new PaymentIntentCreateOptions
{
    Amount = timesheetAmount,      // e.g., 5000 for $50.00
    Currency = "usd",
    Customer = jobProviderCustomerId,
    PaymentMethod = savedPaymentMethodId,
    OffSession = true,          // Charging when the user isn't present
    Confirm = true,             // Attempt immediate charge
    PaymentMethodTypes = new List<string> { "card" },

    ApplicationFeeAmount = applicationFeeAmount,  // e.g., 500 for a $5 fee
    TransferData = new PaymentIntentTransferDataOptions
    {
        Destination = freelancerConnectedAccountId
    },
};

try
{
    var paymentIntent = paymentIntentService.Create(paymentIntentOptions);
    // If successful, the funds route to the freelancer minus your fee
}
catch (StripeException e)
{
    // Handle declines, insufficient funds, etc.
    // Prompt the Job Provider to update their payment info if needed
}
```

- `OffSession = true` + `Confirm = true` → Charges the saved payment method without user interaction. If 3D Secure or other authentication is required, you’ll get an error.
- For more robust 3D Secure handling, set `Confirm = false` and follow the recommended flow. But for simplicity, we assume minimal friction.

---

# 6. Automatic Payouts to Freelancers (Standard)

With **Standard** accounts:

- Stripe automatically **pays out** to the freelancer’s bank account per their **payout schedule** (daily, weekly, monthly, etc.).
- No extra development is needed to transfer funds from your platform to the freelancer.

**References**

- [Standard Accounts: Payouts](https://stripe.com/docs/connect/standard-accounts#managing-funds)

---

# 7. Collecting Your Marketplace Fee

Your fee is set via the `ApplicationFeeAmount` or `ApplicationFeePercent` when creating the **PaymentIntent**. That portion goes to your Stripe balance. Stripe deducts its processing fees, then sends the net to the freelancer’s account.

**References**

- [Collecting Fees with Connect](https://stripe.com/docs/connect/collecting-fees)

---

# 8. Setting Up a Webhook in C#

**Webhooks** let Stripe notify your server about events (e.g., `checkout.session.completed`, `payment_intent.succeeded`, etc.).

## 8.1. Configure the Webhook Endpoint in Stripe Dashboard

1. Go to **Developers > Webhooks** in the Stripe Dashboard.
2. **Add endpoint**: enter `https://your-domain.com/api/stripe/webhook`.
3. Pick the events you need (`checkout.session.completed`, `payment_intent.succeeded`, `payment_intent.payment_failed`, etc.).
4. Copy the **signing secret** for verifying signatures.

## 8.2. C# Webhook Endpoint Example

Below is a minimal ASP.NET Core example:

```csharp
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.IO;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/stripe")]
    public class StripeWebhookController : ControllerBase
    {
        private readonly string _endpointSecret = "whsec_12345..."; // From Stripe Dashboard

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            // 1) Read the request body as text
            using var reader = new StreamReader(HttpContext.Request.Body);
            var json = await reader.ReadToEndAsync();

            // 2) Grab the signature from Stripe
            var signatureHeader = Request.Headers["Stripe-Signature"];

            Event stripeEvent;
            try
            {
                // 3) Verify the event's signature
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signatureHeader,
                    _endpointSecret
                );
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }

            // 4) Handle the event
            switch (stripeEvent.Type)
            {
                case Events.CheckoutSessionCompleted:
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    // Access session.SetupIntentId or session.PaymentIntentId
                    // Handle storing PaymentMethod, marking as complete, etc.
                    break;

                case Events.PaymentIntentSucceeded:
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    // Mark the timesheet as "paid" in your DB
                    break;

                case Events.PaymentIntentPaymentFailed:
                    var failedIntent = stripeEvent.Data.Object as PaymentIntent;
                    // Notify Job Provider to update payment info
                    break;

                default:
                    // Handle other event types as needed
                    break;
            }

            // 5) Return 200 to acknowledge receipt of the event
            return Ok();
        }
    }
}
```

**References**

- [Stripe Webhooks](https://stripe.com/docs/webhooks)
- [Stripe .NET Webhook Docs](https://stripe.com/docs/development/quickstart/webhooks#signatures)

---

## Putting It All Together

1. **Enable Connect (Standard)** in your Stripe account.
2. **Onboard Freelancers** with the `AccountLink` flow; store their `account.Id`.
3. **Create a Customer** object for the Job Provider.
4. **Collect Payment Method** using **Checkout in setup mode**.
   - Flutter front-end → open `checkoutSession.Url`
   - On success, a PaymentMethod is attached to the Customer for future use.
5. **Timesheet Approvals** → **PaymentIntent** (C#):
   - Reference the saved PaymentMethod.
   - `OffSession = true, Confirm = true, TransferData.Destination = freelancerId, ApplicationFeeAmount = yourFee`.
6. **Automatic Payout** to the freelancer.
7. **Webhook**:
   - Listen for `checkout.session.completed`, `payment_intent.succeeded`, etc.
   - Update your database, notify relevant parties.

---

# Final References

- **Stripe Connect Documentation**  
  [https://stripe.com/docs/connect](https://stripe.com/docs/connect)

- **Stripe Checkout in Setup Mode**  
  [https://stripe.com/docs/payments/checkout/set-up-a-subscription#collect-payment-details-for-future-use](https://stripe.com/docs/payments/checkout/set-up-a-subscription#collect-payment-details-for-future-use)

- **PaymentIntents Off-Session**  
  [https://stripe.com/docs/payments/payment-intents/off-session](https://stripe.com/docs/payments/payment-intents/off-session)

- **Collecting Fees with Connect**  
  [https://stripe.com/docs/connect/collecting-fees](https://stripe.com/docs/connect/collecting-fees)

By following this approach, you’ll:

- Maintain minimal PCI compliance overhead (Stripe handles sensitive data).
- Let Stripe handle freelancer onboarding and payouts (Standard accounts).
- Store payment methods for seamless off-session charges (timesheet approvals).
- Collect marketplace fees and automate distribution of funds.
